FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/gateway/OVB.Demos.FakeBank.WebApi/OVB.Demos.FakeBank.WebApi.csproj", "src/gateway/OVB.Demos.FakeBank.WebApi/"]
RUN dotnet restore "./src/gateway/OVB.Demos.FakeBank.WebApi/OVB.Demos.FakeBank.WebApi.csproj"
COPY . .
WORKDIR "/src/src/gateway/OVB.Demos.FakeBank.WebApi"
RUN dotnet build "./OVB.Demos.FakeBank.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OVB.Demos.FakeBank.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OVB.Demos.FakeBank.WebApi.dll"]