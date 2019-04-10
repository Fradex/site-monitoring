FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_8.x | bash - && \
    apt-get install -y build-essential nodejs
RUN npm install @angular/cli -g
# End setup

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["SiteMonitoing/SiteMonitoring.csproj", "SiteMonitoing/"]
COPY ["SiteMonitoring.Model/SiteMonitoring.Model.csproj", "SiteMonitoring.Model/"]
RUN dotnet restore "SiteMonitoing/SiteMonitoring.csproj"
COPY . .
WORKDIR "/src/SiteMonitoring"
RUN dotnet build "SiteMonitoring.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SiteMonitoring.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
WORKDIR /app/ClientApp
RUN npm install
RUN npm rebuild node-sass
WORKDIR /app

COPY missing-cert.crt /usr/share/ca-certificates
RUN echo missing-cert.crt >> /etc/ca-certificates.conf
RUN update-ca-certificates

ENTRYPOINT ["dotnet", "SiteMonitoring.dll"]