# -------- Stage 1: Build --------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project file
COPY MoysIQPlatform.sln ./
COPY Server/MoysIQPlatform.Server.csproj ./Server/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the files
COPY . .

# Publish from Server folder
WORKDIR /src/Server
RUN dotnet publish -c Release -o /app/publish

# -------- Stage 2: Runtime --------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MoysIQPlatform.Server.dll"]
