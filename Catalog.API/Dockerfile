# Use .NET SDK image for building (ARM64)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files individually to improve caching
COPY ["CatalogWebAPI/Catalog.API/Catalog.API.csproj", "Catalog.API/"]
COPY ["CatalogWebAPI/Catalog.Core/Catalog.Core.csproj", "Catalog.Core/"]
COPY ["CatalogWebAPI/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Catalog.Infrastructure/"]
RUN dotnet restore "Catalog.API/Catalog.API.csproj"

# Copy the entire project for building
COPY CatalogWebAPI .

# Set the working directory to Merchant.API and build the project
WORKDIR /src/Catalog.API
RUN dotnet build "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish project
FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# Use ASP.NET runtime image for deployment (ARM64)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Check if dotnet exists in the expected location and create symlink if necessary
RUN if [ ! -e /usr/share/dotnet/dotnet ]; then ln -s /usr/local/share/dotnet /usr/share/dotnet; fi

# Copy published output
COPY --from=publish /app/publish .

# Expose port (optional, adjust if necessary)
EXPOSE 8083

# Run the application
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
