#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Holtz.CQRS/Holtz.CQRS.Api.csproj", "Holtz.CQRS/"]
RUN dotnet restore "Holtz.CQRS/Holtz.CQRS.Api.csproj"
COPY . .
WORKDIR "/src/Holtz.CQRS"
RUN dotnet build "Holtz.CQRS.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Holtz.CQRS.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Holtz.CQRS.Api.dll"]

################### Build and run ###################

# docker build -t holtz-cqrs-api:dev2 -f ./Holtz.CQRS.API/Dockerfile --build-arg BUILD_CONFIGURATION=Debug ..
# docker run -p 5000:5000 --name holtz-cqrs-api -e ASPNETCORE_ENVIRONMENT=Development holtz-cqrs-api:dev


################### Build and run as arm64 ####################

# docker buildx build --platform=linux/arm64 -t holtz-cqrs-api:dev-arm64 -f ./Holtz.CQRS.API/Dockerfile ..