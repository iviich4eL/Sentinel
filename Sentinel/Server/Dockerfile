#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Sentinel/Server/Sentinel.Server.csproj", "Sentinel/Server/"]
COPY ["Sentinel/Client/Sentinel.Client.csproj", "Sentinel/Client/"]
COPY ["Sentinel/Shared/Sentinel.Shared.csproj", "Sentinel/Shared/"]
RUN dotnet restore "Sentinel/Server/Sentinel.Server.csproj"
COPY . .
WORKDIR "/src/Sentinel/Server"
RUN dotnet build "Sentinel.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sentinel.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sentinel.Server.dll"]

#COPY ["Sentinel.Server.csproj", "Sentinel/Server/"]