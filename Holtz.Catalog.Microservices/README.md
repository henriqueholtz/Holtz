# Holtz.Catalog.Microservices

- .NET 8
- MongoDB
- Docker + Docker Compose

### How to run

Based on https://www.youtube.com/watch?v=ubCvfws1m4A

- `docker run -d -p 27017:27017 --name catalog-mongo mongo`

### Docker Compose

In the `docker-compose.override.yml` the environment variable `MongoDb__ConnectionString` turns into `MongoDb:ConnectionString` to override the `appsettings.json`.

**Note:** The `__` turns out as `:`
