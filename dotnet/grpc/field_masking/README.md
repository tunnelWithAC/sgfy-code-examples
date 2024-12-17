Based on [gRPC API Performance Improvement Through Protobuf FieldMask In .NET](https://www.azilen.com/blog/grpc-api-performance-improvement-through-protobuf-fieldmask-in-net/)

## Troubleshooting

You can use the command below to fix `Unhandled exception. System.Net.Http.HttpRequestException: The SSL connection could not be established, see inner exception` when running locally.

```
dotnet dev-certs https --trust
```