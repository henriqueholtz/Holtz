# Holtz.DotnetCounters

Just a simple example on how to track your own metrics with .NET counters.

Step by step to see:

- `dotnet run --project ./Holtz.DotnetCounters.Api/Holtz.DotnetCounters.Api.csproj`: Run the project.
- `dotnet counters ps`: Show the dotnet process running in your machine.
- `dotnet counters monitor -p <pid> --counters Holtz.RequestProcessTimeEventSource`: Monitor the dotnet process and track the `Holtz.RequestProcessTimeEventSource`.
- `dotnet counters collect -p 22336 --counters Holtz.RequestProcessTimeEventSource --format json -o collect.json`: Collect the metrics in a json file.
