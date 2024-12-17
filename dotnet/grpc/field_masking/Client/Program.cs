#region Copyright notice and license

// Copyright 2019 The gRPC Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using Greet;
using Grpc.Net.Client;
using Google.Protobuf.WellKnownTypes;
using System.Reflection;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Greeter.GreeterClient(channel);

// var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
// Console.WriteLine("Greeting: " + reply.Message);
FieldMask fieldMask = new FieldMask();
fieldMask.Paths.AddRange(new string[] { "id", "amount" });
var request = new GetDiscountRequest();
request.ProductName = "SampleProduct";
request.FieldMask = fieldMask;

var getDiscountReply = await client.GetDiscountAsync(request);
// Console.WriteLine("GetDiscount: " + getDiscountReply.Id);

Console.WriteLine($"Id: {getDiscountReply.Id}");
Console.WriteLine($"ProductName: {getDiscountReply.ProductName}"); // This will be empty
Console.WriteLine($"Description: {getDiscountReply.Description}"); // This will be empty
Console.WriteLine($"Amount: {getDiscountReply.Amount}");

// System.Type type = typeof(CouponModel); // Replace with your generated class
// PropertyInfo[] properties = type.GetProperties();

// Console.WriteLine("Fields in GetDiscountRequest:");
// foreach (var property in properties)
// {
//     Console.WriteLine(property.Name);
// }

Console.WriteLine("Shutting down");
// Console.WriteLine("Press any key to exit...");
// Console.ReadKey();
