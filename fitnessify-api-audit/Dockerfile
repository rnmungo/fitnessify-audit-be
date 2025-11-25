#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080/
EXPOSE 8080
EXPOSE 8443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["fitnessify-api-audit/fitnessify-api-audit.csproj", "fitnessify-api-audit/"]
RUN dotnet restore "fitnessify-api-audit/fitnessify-api-audit.csproj"
COPY . .
WORKDIR "/src/fitnessify-api-audit"
RUN dotnet build "fitnessify-api-audit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "fitnessify-api-audit.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "fitnessify-api-audit.dll"]