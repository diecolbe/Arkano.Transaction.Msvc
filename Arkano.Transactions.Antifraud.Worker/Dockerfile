FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Arkano.Transactions.Antifraud.Worker/Arkano.Transactions.Antifraud.Worker.csproj Arkano.Transactions.Antifraud.Worker/
COPY Arkano.Transactions.Domain/Arkano.Transactions.Domain.csproj Arkano.Transactions.Domain/
COPY Arkano.Transactions.Infraestructure/Arkano.Transactions.Infraestructure.csproj Arkano.Transactions.Infraestructure/
COPY Arkano.Transactions.Aplication/Arkano.Transactions.Aplication.csproj Arkano.Transactions.Aplication/

RUN dotnet restore Arkano.Transactions.Antifraud.Worker/Arkano.Transactions.Antifraud.Worker.csproj

COPY . .

WORKDIR /src/Arkano.Transactions.Antifraud.Worker
RUN dotnet build Arkano.Transactions.Antifraud.Worker.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Arkano.Transactions.Antifraud.Worker.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arkano.Transactions.Antifraud.Worker.dll"]