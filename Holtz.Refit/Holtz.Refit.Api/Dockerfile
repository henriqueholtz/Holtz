#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Holtz.Refit.Api/Holtz.Refit.Api.csproj", "Holtz.Refit.Api/"]
RUN dotnet restore "./Holtz.Refit.Api/./Holtz.Refit.Api.csproj"
COPY . .
WORKDIR "/src/Holtz.Refit.Api"
RUN dotnet build "./Holtz.Refit.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Holtz.Refit.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Holtz.Refit.Api.dll"]

# docker build -f Holtz.Refit.Api/Dockerfile -t holtz.refit.api:latest .
# docker run --rm -it -p 8085:8080 --name holtz.refit.api holtz.refit.api

# DockerHub: build and push am64 + arm64
# docker buildx build --push --platform linux/amd64,linux/arm64 -f Holtz.Refit.Api/Dockerfile -t henriqueholtz/holtz.refit.api:multi-arch .