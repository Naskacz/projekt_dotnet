FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
# Certificate will be mounted at runtime via docker-compose volume
ENV ASPNETCORE_URLS=https://+:5001;http://+:5000
ENTRYPOINT ["dotnet", "Projekt_dotnet.dll"]

EXPOSE 5001

# Skopiuj certyfikat do katalogu w kontenerze
COPY certs/aspnetapp.pfx /root/.aspnet/https/aspnetapp.pfx

# Ustaw zmienne środowiskowe dla Kestrel
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/root/.aspnet/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=password