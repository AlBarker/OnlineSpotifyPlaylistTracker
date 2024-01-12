#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OnlineSpotifyPlaylistTracker.API/OnlineSpotifyPlaylistTracker.API.csproj", "OnlineSpotifyPlaylistTracker.API/"]
COPY ["OnlineSpotifyPlaylistTracker/OnlineSpotifyPlaylistTracker.csproj", "OnlineSpotifyPlaylistTracker/"]
COPY ["OnlineSpotifyPlaylistTracker.Domain/OnlineSpotifyPlaylistTracker.Domain.csproj", "OnlineSpotifyPlaylistTracker.Domain/"]
RUN dotnet restore "OnlineSpotifyPlaylistTracker.API/OnlineSpotifyPlaylistTracker.API.csproj"
COPY . .
WORKDIR "/src/OnlineSpotifyPlaylistTracker.API"
RUN dotnet build "OnlineSpotifyPlaylistTracker.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineSpotifyPlaylistTracker.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineSpotifyPlaylistTracker.API.dll"]