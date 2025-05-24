#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 10002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/server/Services/OrderService/OrderService.Api/OrderService.Api.csproj", "src/server/Services/OrderService/OrderService.Api/"]
COPY ["src/server/BuildingBlocks/CommonLibrary/CommonLibrary.csproj", "src/server/BuildingBlocks/CommonLibrary/"]
COPY ["src/server/BuildingBlocks/EventBus/EventBus.Base/EventBus.Base.csproj", "src/server/BuildingBlocks/EventBus/EventBus.Base/"]
COPY ["src/server/BuildingBlocks/EventBus/EventBus.Factory/EventBus.Factory.csproj", "src/server/BuildingBlocks/EventBus/EventBus.Factory/"]
COPY ["src/server/BuildingBlocks/EventBus/EventBus.AzureServiceBus/EventBus.AzureServiceBus.csproj", "src/server/BuildingBlocks/EventBus/EventBus.AzureServiceBus/"]
COPY ["src/server/BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "src/server/BuildingBlocks/EventBus/EventBus.RabbitMQ/"]
COPY ["src/server/Services/OrderService/OrderService.Application/OrderService.Application.csproj", "src/server/Services/OrderService/OrderService.Application/"]
COPY ["src/server/Services/OrderService/OrderService.Domain/OrderService.Domain.csproj", "src/server/Services/OrderService/OrderService.Domain/"]
COPY ["src/server/Services/OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj", "src/server/Services/OrderService/OrderService.Infrastructure/"]
RUN dotnet restore "./src/server/Services/OrderService/OrderService.Api/OrderService.Api.csproj"
COPY . .
WORKDIR "/src/src/server/Services/OrderService/OrderService.Api"
RUN dotnet build "./OrderService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]