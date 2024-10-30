FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.API/ToDo.Microservices.Entries.API.csproj", "ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.API/"]
COPY ["ToDo.Domain/ToDo.Domain.csproj", "ToDo.Domain/"]
COPY ["ToDo.Extensions/ToDo.Extensions.csproj", "ToDo.Extensions/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Middleware/ToDo.Microservices.Middleware.csproj", "ToDo.Microservices/ToDo.Microservices.Middleware/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.Database/ToDo.Microservices.Entries.Database.csproj", "ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.Database/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.Domain/ToDo.Microservices.Entries.Domain.csproj", "ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.Domain/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.Infrastructure/ToDo.Microservices.Entries.Infrastructure.csproj", "ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.Infrastructure/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Cache/ToDo.Microservices.Cache.csproj", "ToDo.Microservices/ToDo.Microservices.Cache/"]
COPY ["ToDo.Cache/ToDo.Cache.Abstractions/ToDo.Cache.Abstractions.csproj", "ToDo.Cache/ToDo.Cache.Abstractions/"]
COPY ["ToDo.Microservices/ToDo.Microservices.MQ/ToDo.Microservices.MQ.csproj", "ToDo.Microservices/ToDo.Microservices.MQ/"]
COPY ["ToDo.MQ/ToDo.MQ.Abstractions/ToDo.MQ.Abstractions.csproj", "ToDo.MQ/ToDo.MQ.Abstractions/"]
COPY ["ToDo.MQ/ToDo.MQ.RabbitMQ/ToDo.MQ.RabbitMQ.csproj", "ToDo.MQ/ToDo.MQ.RabbitMQ/"]
COPY ["ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.UseCases/ToDo.Microservices.Entries.UseCases.csproj", "ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.UseCases/"]
RUN dotnet restore "./ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.API/./ToDo.Microservices.Entries.API.csproj"
COPY . .
WORKDIR "/src/ToDo.Microservices/ToDo.Microservices.Entries/ToDo.Microservices.Entries.API"
RUN dotnet build "./ToDo.Microservices.Entries.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ToDo.Microservices.Entries.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDo.Microservices.Entries.API.dll"]