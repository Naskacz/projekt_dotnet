# Używamy SDK, żeby działał dotnet watch
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dev
WORKDIR /app

# Kopiujemy tylko pliki projektu dla szybkiego restore
COPY *.csproj ./
RUN dotnet restore

# Kopiujemy resztę plików
COPY . ./

# Ustawiamy port do developmentu
ENV ASPNETCORE_URLS=http://+:8080

# Domyślny punkt wejścia – dotnet watch
CMD ["dotnet", "watch", "run", "--urls", "http://+:8080"]

EXPOSE 8080
