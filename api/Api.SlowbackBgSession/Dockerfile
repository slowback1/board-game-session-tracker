FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Api.SlowbackBgSession/Api.SlowbackBgSession.csproj", "Api.SlowbackBgSession/"]
RUN dotnet restore "Api.SlowbackBgSession/Api.SlowbackBgSession.csproj"
COPY . .
WORKDIR "/src/Api.SlowbackBgSession"
RUN dotnet build "Api.SlowbackBgSession.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.SlowbackBgSession.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.SlowbackBgSession.dll"]
