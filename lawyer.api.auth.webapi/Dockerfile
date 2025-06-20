FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV DOTNET_ENVIRONMENT=Docker
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["lawyer.api.auth.webapi/lawyer.api.auth.webapi.csproj", "lawyer.api.auth.webapi/"]
COPY ["lawyer.api.auth.application/lawyer.api.auth.application.csproj", "lawyer.api.auth.application/"]
COPY ["lawyer.api.auth.datastore.mssql/lawyer.api.auth.datastore.mssql.csproj", "lawyer.api.auth.datastore.mssql/"]
RUN dotnet restore "lawyer.api.auth.webapi/lawyer.api.auth.webapi.csproj"
COPY . .
WORKDIR "/src/lawyer.api.auth.webapi"
RUN dotnet build "./lawyer.api.auth.webapi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./lawyer.api.auth.webapi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lawyer.api.auth.webapi.dll"]
