FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dev
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

ENV ASPNETCORE_URLS=http://+:8080

CMD ["dotnet", "watch", "run", "--urls", "http://+:8080"]

EXPOSE 8080
