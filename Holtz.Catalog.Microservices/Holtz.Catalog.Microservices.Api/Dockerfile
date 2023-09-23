#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Holtz.Catalog.Microservices.Api/Holtz.Catalog.Microservices.Api.csproj", "Holtz.Catalog.Microservices.Api/"]
RUN dotnet restore "Holtz.Catalog.Microservices.Api/Holtz.Catalog.Microservices.Api.csproj"
COPY . .
WORKDIR "/src/Holtz.Catalog.Microservices.Api"
RUN dotnet build "Holtz.Catalog.Microservices.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Holtz.Catalog.Microservices.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Holtz.Catalog.Microservices.Api.dll"]