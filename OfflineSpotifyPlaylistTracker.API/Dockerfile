#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OfflineSpotifyPlaylistTracker.API/OfflineSpotifyPlaylistTracker.API.csproj", "OfflineSpotifyPlaylistTracker.API/"]
COPY ["OfflineSpotifyPlaylistTracker/OfflineSpotifyPlaylistTracker.csproj", "OfflineSpotifyPlaylistTracker/"]
COPY ["OfflineSpotifyPlaylistTracker.Domain/OfflineSpotifyPlaylistTracker.Domain.csproj", "OfflineSpotifyPlaylistTracker.Domain/"]
RUN dotnet restore "OfflineSpotifyPlaylistTracker.API/OfflineSpotifyPlaylistTracker.API.csproj"
COPY . .
WORKDIR "/src/OfflineSpotifyPlaylistTracker.API"
RUN dotnet build "OfflineSpotifyPlaylistTracker.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OfflineSpotifyPlaylistTracker.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OfflineSpotifyPlaylistTracker.API.dll"]