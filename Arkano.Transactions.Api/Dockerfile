FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 443

FROM base AS final
WORKDIR /app

COPY . .

RUN groupadd -r appuser && useradd -r -g appuser appuser && \
    chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Arkano.Transactions.Api.dll"]