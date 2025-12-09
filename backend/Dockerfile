FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dev
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

RUN dotnet tool install --global dotnet-ef --version 10.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

COPY . ./

ENV ASPNETCORE_URLS=http://+:8080

CMD ["dotnet", "watch", "run", "--urls", "http://+:8080"]

EXPOSE 8080
