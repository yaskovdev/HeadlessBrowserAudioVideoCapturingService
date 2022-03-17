# HeadlessBrowserAudioVideoCapturingService

```shell
docker build -f HeadlessBrowserAudioVideoCapturingService/Dockerfile -t yaskovdev/headless-capturing-server .
docker run -p 8080:80 -d yaskovdev/headless-capturing-server
```

```shell
curl -v http://localhost:8080/status
```

```shell
curl -v http://localhost:8080/start \
   -H 'Content-Type: application/json' \
   -d '{ "captureDurationMs": "10000", "saveCapturedStreams": false }'
```
