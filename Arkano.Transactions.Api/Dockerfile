FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Arkano.Transactions.Api/Arkano.Transactions.Api.csproj Arkano.Transactions.Api/
COPY Arkano.Transactions.Aplication/Arkano.Transactions.Aplication.csproj Arkano.Transactions.Aplication/
COPY Arkano.Transactions.Domain/Arkano.Transactions.Domain.csproj Arkano.Transactions.Domain/
COPY Arkano.Transactions.Infraestructure/Arkano.Transactions.Infraestructure.csproj Arkano.Transactions.Infraestructure/

RUN dotnet restore Arkano.Transactions.Api/Arkano.Transactions.Api.csproj

COPY . .

WORKDIR /src/Arkano.Transactions.Api
RUN dotnet build Arkano.Transactions.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Arkano.Transactions.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arkano.Transactions.Api.dll"]