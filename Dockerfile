# -------- Stage 1: Build --------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and all project files (Server, Client, Shared)
COPY MoysIQPlatform.sln ./
COPY Server/MoysIQPlatform.Server.csproj ./Server/
COPY Client/MoysIQPlatform.Client.csproj ./Client/
COPY Shared/MoysIQPlatform.Shared.csproj ./Shared/

# Restore dependencies
RUN dotnet restore

# Copy the full source code
COPY . .

# Publish
WORKDIR /src/Server
RUN dotnet publish -c Release -o /app/publish

# -------- Stage 2: Runtime --------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MoysIQPlatform.Server.dll"]
