#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5002
ENV ASPNETCORE_URLS=http://Api:5002

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Sentinel/FakeDataApi/FakeDataApi.csproj", "Sentinel/FakeDataApi/"]
RUN dotnet restore "Sentinel/FakeDataApi/FakeDataApi.csproj"
COPY . .
WORKDIR "/src/Sentinel/FakeDataApi"
RUN dotnet build "FakeDataApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FakeDataApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FakeDataApi.dll"]