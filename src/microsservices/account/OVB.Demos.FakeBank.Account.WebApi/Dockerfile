FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/microsservices/account/OVB.Demos.FakeBank.Account.WebApi/OVB.Demos.FakeBank.Account.WebApi.csproj", "src/microsservices/account/OVB.Demos.FakeBank.Account.WebApi/"]
RUN dotnet restore "./src/microsservices/account/OVB.Demos.FakeBank.Account.WebApi/OVB.Demos.FakeBank.Account.WebApi.csproj"
COPY . .
WORKDIR "/src/src/microsservices/account/OVB.Demos.FakeBank.Account.WebApi"
RUN dotnet build "./OVB.Demos.FakeBank.Account.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OVB.Demos.FakeBank.Account.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OVB.Demos.FakeBank.Account.WebApi.dll"]