# Holtz.FusionCache

This is a simple web api using [FusionCache](https://github.com/ZiggyCreatures/FusionCache).

- .NET 8
- FusionCache with distributed cache L2 (redis)
- OpenTelemetry (FusionCache fully integrated)
- .NET Aspire

### How to run redis as standalone

To came up with the application running correctly you just need to run the redis as a container using the following command:

```
docker run --name redis -d -p 6379:6379 redis
```
