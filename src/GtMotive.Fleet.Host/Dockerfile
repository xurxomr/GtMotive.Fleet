# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Build context is expected to be the repository root.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Solution-level build configuration (needed before restore for MSBuild props/targets).
COPY Directory.Build.props Directory.Build.targets .editorconfig global.json NuGet.config ./

# Project files first, to leverage Docker layer caching on restore.
COPY src/GtMotive.Fleet.Host/GtMotive.Fleet.Host.csproj src/GtMotive.Fleet.Host/
COPY src/GtMotive.Fleet.Api/GtMotive.Fleet.Api.csproj src/GtMotive.Fleet.Api/
COPY src/GtMotive.Fleet.ApplicationCore/GtMotive.Fleet.ApplicationCore.csproj src/GtMotive.Fleet.ApplicationCore/
COPY src/GtMotive.Fleet.Domain/GtMotive.Fleet.Domain.csproj src/GtMotive.Fleet.Domain/
COPY src/GtMotive.Fleet.Infrastructure/GtMotive.Fleet.Infrastructure.csproj src/GtMotive.Fleet.Infrastructure/
RUN dotnet restore src/GtMotive.Fleet.Host/GtMotive.Fleet.Host.csproj

# Copy the rest of the source and publish.
COPY src/ src/
RUN dotnet publish src/GtMotive.Fleet.Host/GtMotive.Fleet.Host.csproj -c Release --no-restore -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GtMotive.Fleet.Host.dll"]
