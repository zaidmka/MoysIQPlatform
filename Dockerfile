# -------- Stage 1: Build --------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore
COPY *.sln .
COPY MoysIQPlatform/*.csproj ./MoysIQPlatform/
RUN dotnet restore

# Copy everything and publish
COPY . .
WORKDIR /src/MoysIQPlatform
RUN dotnet publish -c Release -o /app/publish

# -------- Stage 2: Runtime --------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MoysIQPlatform.dll"]
