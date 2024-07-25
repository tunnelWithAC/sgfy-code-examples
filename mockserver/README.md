##

```
docker run --rm --name=mockserver -p 1080:1080 \
  -v $(pwd)/expectations.json:/config/expectations.json \
  -e MOCKSERVER_INITIALIZATION_JSON_PATH=/config/expectations.json \
  mockserver/mockserver \
  -serverPort 1080 \
  -logLevel INFO
```

## Notes

`RemainingTimes` is a method that specifies how many times an expectation (a combination of a request matcher and a response action) should be used before it becomes inactive.

For example, if you set remainingTimes(5) for a certain expectation, MockServer will return the specified response for the next 5 times that expectation is matched. After that, the expectation will not be used anymore, even if the same request is received.

Here's an example:

```
mockServerClient
    .when(
        request()
            .withMethod("GET")
            .withPath("/some/path"),
        exactly(5)  // This expectation will be used exactly 5 times
    )
    .respond(
        response()
            .withStatusCode(200)
            .withBody("{ message: 'Hello, world!' }")
    );
```

```
{
  "httpRequest": {
    "path": "/some/path"
  },
  "httpResponse": {
    "body": "some_response_body"
  },
  "times": {
    "remainingTimes": 5,
    "unlimited": false
  }
}
```

In this example, the first 5 GET requests to "/some/path" will receive a 200 response with the body "{ message: 'Hello, world!' }". Any subsequent GET requests to "/some/path" will not match this expectation, and will need to be handled by another expectation or will receive a default response.

`Unlimited()` is a method that specifies an expectation should be used an unlimited number of times. This means that the expectation will always be active and will be used every time a matching request is received, regardless of how many times it has been used before.

Here's an example:

```
// code
mockServerClient
    .when(
        request()
            .withMethod("GET")
            .withPath("/some/path"),
        unlimited()  // This expectation will be used an unlimited number of times
    )
    .respond(
        response()
            .withStatusCode(200)
            .withBody("{ message: 'Hello, world!' }")
    );
```

```
// json
{
  "httpRequest": {
    "path": "/some/path"
  },
  "httpResponse": {
    "body": "some_response_body"
  },
  "times": {
    "remainingTimes": 1,
    "unlimited": false
  },
  "timeToLive": {
    "timeUnit": "SECONDS",
    "timeToLive": 60,
    "unlimited": false
  }
}
```

In this example, every GET request to "/some/path" will receive a 200 response with the body "{ message: 'Hello, world!' }", no matter how many such requests are received.

3.
In MockServer, the `remainingTimes` and `unlimited` fields are used to control how many times an expectation can be matched.

* `remainingTimes` specifies the exact number of times an expectation can be matched.

* `unlimited` when set to true, specifies that an expectation can be matched an unlimited number of times.

However, using `remainingTimes` and `unlimited` together in the same expectation can lead to confusion, as they serve opposing purposes. If `unlimited` is set to `true`, it should override `remainingTimes` regardless of its value, meaning the expectation can be matched an unlimited number of times.

In the example above, `remainingTimes` is set to `1` and `unlimited` is set to `false`. This means the expectation will only be matched once. After the first match, the expectation will not be used anymore, even if the same request is received again.

The `timeToLive` field is used to specify how long the expectation should exist before it is automatically removed. In your example, it is set to 60 seconds. After 60 seconds, the expectation will be removed, even if it has not been matched the specified number of times.