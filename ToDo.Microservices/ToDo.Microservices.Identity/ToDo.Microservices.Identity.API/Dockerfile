FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["ToDo.Domain/ToDo.Domain.csproj", "ToDo.Domain/"]
COPY ["ToDo.Extensions/ToDo.Extensions.csproj", "ToDo.Extensions/"]
COPY ["ToDo.MQ/ToDo.MQ.RabbitMQ/ToDo.MQ.RabbitMQ.csproj", "ToDo.MQ/ToDo.MQ.RabbitMQ/"]
COPY ["ToDo.MQ/ToDo.MQ.Abstractions/ToDo.MQ.Abstractions.csproj", "ToDo.MQ/ToDo.MQ.Abstractions/"]
COPY ["ToDo.Microservices/ToDo.Microservices.MQ/ToDo.Microservices.MQ.csproj", "ToDo.Microservices/ToDo.Microservices.MQ/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Middleware/ToDo.Microservices.Middleware.csproj", "ToDo.Microservices/ToDo.Microservices.Middleware/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.Domain/ToDo.Microservices.Identity.Domain.csproj", "ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.Domain/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.UseCases/ToDo.Microservices.Identity.UseCases.csproj", "ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.UseCases/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.Infrastructure/ToDo.Microservices.Identity.Infrastructure.csproj", "ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.Infrastructure/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.Database/ToDo.Microservices.Identity.Database.csproj", "ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.Database/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.API/ToDo.Microservices.Identity.API.csproj", "ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.API/"]
RUN dotnet restore "./ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.API/./ToDo.Microservices.Identity.API.csproj"
COPY . .
WORKDIR "/src/ToDo.Microservices/ToDo.Microservices.Identity/ToDo.Microservices.Identity.API"
RUN dotnet build "./ToDo.Microservices.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ToDo.Microservices.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDo.Microservices.Identity.API.dll"]