FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BudgetPlanner.Server/BudgetPlanner.Server.csproj", "BudgetPlanner.Server/"]
RUN dotnet restore "BudgetPlanner.Server/BudgetPlanner.Server.csproj"
COPY . .
WORKDIR "/src/BudgetPlanner.Server"
RUN dotnet build "BudgetPlanner.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BudgetPlanner.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetPlanner.Server.dll"]
