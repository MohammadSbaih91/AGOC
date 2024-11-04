FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

COPY appsettings.json /app/appsettings.json

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Install required LDAP and Kerberos libraries for LDAP functionality
RUN apt-get update && apt-get install -y \
    libldap-2.5-0 \
    libkrb5-3 \
    ca-certificates && \
    rm -rf /var/lib/apt/lists/*

EXPOSE 8080

COPY --from=build /app/out ./

# Set environment variables to configure OpenShift
ENV ASPNETCORE_URLS=http://*:8080

ENTRYPOINT ["dotnet", "AGOC.dll"]