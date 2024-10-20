FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EsamiOnline/EsamiOnline.csproj", "EsamiOnline/"]
RUN dotnet restore "EsamiOnline/EsamiOnline.csproj"
COPY . .
WORKDIR "/src/EsamiOnline"
RUN dotnet build "EsamiOnline.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS test
WORKDIR /src
COPY ["EsamiOnline.Tests/EsamiOnline.Tests.csproj", "EsamiOnline.Tests/"]
RUN dotnet restore "EsamiOnline.Tests/EsamiOnline.Tests.csproj"
COPY . .
WORKDIR "/src/EsamiOnline.Tests"
RUN dotnet test "EsamiOnline.Tests.csproj" --logger:trx

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EsamiOnline.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EsamiOnline.dll"]
