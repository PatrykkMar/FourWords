FROM node:20 AS frontend-build
WORKDIR /app/client

COPY fourwords.client/package*.json ./
RUN npm install

COPY fourwords.client/ ./
RUN npm run build


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

RUN apt-get update && apt-get install -y curl \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/*

COPY ["FourWords.Server/FourWords.Server.csproj", "FourWords.Server/"]
RUN dotnet restore "FourWords.Server/FourWords.Server.csproj"

COPY . .
WORKDIR /src/FourWords.Server

COPY --from=frontend-build /app/client/dist /src/FourWords.Server/wwwroot

RUN dotnet publish "FourWords.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FourWords.Server.dll"]