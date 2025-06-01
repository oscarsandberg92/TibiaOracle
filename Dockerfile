# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and all projects
COPY *.sln .
COPY Connector_TibiaData/ ./Connector_TibiaData/
COPY Connector_TibiaMarket/ ./Connector_TibiaMarket/
COPY TibiaOracle.Api/ ./TibiaOracle.Api/
COPY TibiaOracle.JobScheduler/ ./TibiaOracle.JobScheduler/
COPY TibiaOracle.Logic/ ./TibiaOracle.Logic/

# Restore dependencies for the solution
RUN dotnet restore

# Build and publish only the TibiaOracle.Api project
WORKDIR /src/TibiaOracle.Api
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TibiaOracle.Api.dll"]
