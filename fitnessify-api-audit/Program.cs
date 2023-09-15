using AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using Audits.Business.Contracts;
using Audits.Business.Mappers;
using Audits.Business.Services;
using Audits.Domain.Models;
using Audits.Infrastructure.BBDD.Contracts;
using Audits.Infrastructure.BBDD.Repositories;
using fitnessify_api_audit.Mappers;
using fitnessify_api_audit.MiddleWares;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Configure Logger
        Log.Logger = new LoggerConfiguration()
            .Filter.ByExcluding(logEvent =>
            {
                bool hasContext = logEvent.Properties.ContainsKey("SourceContext");

                if (hasContext)
                {
                    var sourceContext = (ScalarValue)logEvent.Properties["SourceContext"];
                    return sourceContext?.Value?.ToString() == "Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware";
                }
                return false;
            })
            .ReadFrom.Configuration(builder.Configuration, sectionName: "Serilog")
            .WriteTo.PostgreSQL(
                connectionString: builder.Configuration.GetConnectionString("PostgreSQL") ?? "",
                tableName: builder.Configuration["Serilog:PostgreSQL:TableName"],
                needAutoCreateTable: true,
                schemaName: builder.Configuration["Serilog:PostgreSQL:SchemaName"])
            .AuditTo.File("Logs.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
        builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
        #endregion

        try
        {
            #region Configure Basic Services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            #endregion

            #region Configure AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CoreMapper());
                mc.AddProfile(new UIMapper());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddSingleton(mapper);
            #endregion

            #region Configure DbContext
            var settings = MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("MongoDB"));
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            builder.Services.AddSingleton<IMongoClient>(new MongoClient(settings));
            #endregion

            #region Configure Repositories
            builder.Services.AddSingleton<IRepository<AuditModel, Guid>>(sp =>
                new MongoRepository<AuditModel, Guid>(sp.GetRequiredService<IMongoClient>(), builder.Configuration["MongoDB:DatabaseName"]));
            #endregion

            #region Configure Core Services
            builder.Services.AddHealthChecks();
            builder.Services.AddSingleton<IAuditService, AuditService>();
            #endregion

            #region Configure Workers
            // builder.Services.AddSingleton<IProcessMessageAsync<RequestPostAuditsDTO>, AuditProcessor>();
            // builder.Services.AddHostedService<BasicWorkerAsync<RequestPostAuditsDTO>>();
            #endregion

            var app = builder.Build();

            #region Configure Development
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fitnessify API Audit v1");
                    c.RoutePrefix = string.Empty;
                });
            }
            #endregion

            #region Configure Application
            app.UseStaticFiles();
            app.UseRouting();
            app.UseMiddleware<ExceptionMiddleWare>();
            app.UseHttpsRedirection();
            app.UseHsts();
            app.UseForwardedHeaders();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/api/health");
            });
            #endregion

            Log.Information("Application is ready");

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An error occurred while creating the DB");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}