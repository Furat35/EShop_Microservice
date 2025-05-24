#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 10004

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/server/Services/CatalogService/CatalogService.Api/CatalogService.Api.csproj", "src/server/Services/CatalogService/CatalogService.Api/"]
COPY ["src/server/BuildingBlocks/CommonLibrary/CommonLibrary.csproj", "src/server/BuildingBlocks/CommonLibrary/"]
RUN dotnet restore "./src/server/Services/CatalogService/CatalogService.Api/CatalogService.Api.csproj"
COPY . .
WORKDIR "/src/src/server/Services/CatalogService/CatalogService.Api"
RUN dotnet build "./CatalogService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CatalogService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatalogService.Api.dll"]