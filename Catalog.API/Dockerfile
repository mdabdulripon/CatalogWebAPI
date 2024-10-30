FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files individually to improve caching
COPY ["CatalogWebAPI/Catalog.API/Catalog.API.csproj", "Catalog.API/"]
COPY ["CatalogWebAPI/Catalog.Core/Catalog.Core.csproj", "Catalog.Core/"]
COPY ["CatalogWebAPI/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Catalog.Infrastructure/"]

# Restore dependencies for Catalog.API
RUN dotnet restore "Catalog.API/Catalog.API.csproj"

# Copy the entire project for building
COPY CatalogWebAPI .

# Set the working directory to Merchant.API and build the project
WORKDIR "/src/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
