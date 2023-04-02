FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /
COPY ["nuget.config", "/"]
# Copy csproj and restore as distinct layers
COPY ["src/Host/Host.csproj", "src/Host/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

RUN dotnet restore "src/Host/Host.csproj" --disable-parallel

# Copy everything else and build
COPY . .
WORKDIR "/src/Host"
RUN dotnet publish "Host.csproj" -c Release -o /app/publish

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

COPY --from=build /app/publish .

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

ENV ASPNETCORE_URLS=https://+:6050;http://+:6060
EXPOSE 6050
EXPOSE 6060

ENTRYPOINT ["dotnet", "Minima.Gateway.dll"]