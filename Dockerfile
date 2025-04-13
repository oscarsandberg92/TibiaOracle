# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY TibiaOracle.Api/*.csproj ./TibiaOracle.Api/
RUN dotnet restore

# Copy the rest of the files and publish
COPY . .
WORKDIR /app/TibiaOracle.Api
RUN dotnet publish -c Release -o out

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/TibiaOracle.Api/out ./

# Expose port (adjust if your app uses a different one)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TibiaOracle.Api.dll"]
