using AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;
using MongoDB.Driver;
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
        ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().AddDebug());
        var logger = _loggerFactory.CreateLogger<Program>();

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
            var mongoSettings = builder.Configuration.GetSection("MongoDB");
            var settings = MongoClientSettings.FromConnectionString(mongoSettings["ConnectionString"]);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            builder.Services.AddSingleton<IMongoClient>(new MongoClient(settings));
            #endregion

            #region Configure Repositories
            builder.Services.AddSingleton<IRepository<AuditModel, Guid>>(sp =>
                new MongoRepository<AuditModel, Guid>(sp.GetRequiredService<IMongoClient>(), mongoSettings["DatabaseName"]));
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

            app.Run();
            logger.LogInformation("Application ");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error starting the application");
        }
    }
}