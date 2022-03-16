# HeadlessBrowserAudioVideoCapturingService

```shell
docker build -f HeadlessBrowserAudioVideoCapturingService/Dockerfile -t yaskovdev/headless-capturing-server .
docker run -p 5000:5000 -d yaskovdev/headless-capturing-server
```

```shell
curl -v http://localhost:5000/start \
   -H 'Content-Type: application/json' \
   -d '{ "captureDurationMs": "10000", "saveCapturedStreams": false }'
```