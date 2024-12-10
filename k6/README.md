## K6

Docs: https://grafana.com/docs/k6/latest/using-k6

## K6 Browser

Docs: https://grafana.com/docs/k6/latest/using-k6-browser/
Docker Image Source: https://hub.docker.com/r/grafana/k6/tags

```
docker run --rm --name grafana-k6 -e K6_BROWSER_ENABLED=true -i grafana/k6:master-with-browser run - <script.js
```
