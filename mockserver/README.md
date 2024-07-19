##

```
docker run --rm --name=mockserver -p 1080:1080 \
  -v $(pwd)/expectations.json:/config/expectations.json \
  -e MOCKSERVER_INITIALIZATION_JSON_PATH=/config/expectations.json \
  mockserver/mockserver \
  -serverPort 1080 \
  -logLevel INFO
```


docker run --rm --name=mockserver -p 1080:1080 \
  -v $(pwd)/expectations.json:/config/expectations.json \
  -e MOCKSERVER_INITIALIZATION_JSON_PATH=/config/expectations.json \
  mockserver/mockserver \
  -serverPort 1080 \
  -logLevel INFO
