FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

# Install Python
RUN apt-get update && apt-get install -y python3

RUN dotnet workload install wasm-tools

# Kopiere die CSPROJ Datei und restore dependencies
COPY ["BlazorMSEstatesUIAdmin/BlazorMSEstatesUIAdmin.csproj", "BlazorMSEstatesUIAdmin/"]
COPY ["MSEstatesAppLibrary/MSEstatesAppLibrary.csproj", "MSEstatesAppLibrary/"]
RUN dotnet restore "./BlazorMSEstatesUIAdmin/BlazorMSEstatesUIAdmin.csproj"
COPY . .

WORKDIR "/app/BlazorMSEstatesUIAdmin"
RUN dotnet publish -c Release -o out

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/BlazorMSEstatesUIAdmin/out/wwwroot .
COPY BlazorMSEstatesUIAdmin/nginx.conf /etc/nginx/nginx.conf
#ENV ASPNETCORE_URLS http://*:8080
EXPOSE 8080

