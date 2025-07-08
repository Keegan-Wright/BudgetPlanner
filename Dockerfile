FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8083

ENV ASPNETCORE_URLS=http://+:8083

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["./BudgetPlanner.Server/BudgetPlanner.Server.csproj", "/"]
RUN dotnet restore "/BudgetPlanner.Server.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "./BudgetPlanner.Server/BudgetPlanner.Server.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "./BudgetPlanner.Server/BudgetPlanner.Server.csproj" -c $configuration -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetPlanner.Server.dll"]
