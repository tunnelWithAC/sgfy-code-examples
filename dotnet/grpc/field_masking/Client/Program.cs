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

// Create request
FieldMask fieldMask = new FieldMask();
fieldMask.Paths.AddRange(new string[] { "id", "amount" });
var request = new GetDiscountRequest();
request.ProductName = "SampleProduct";
request.FieldMask = fieldMask;

// RPC
var getDiscountReply = await client.GetDiscountAsync(request);

// Log output
Console.WriteLine($"Id: {getDiscountReply.Id}");
Console.WriteLine($"ProductName: {getDiscountReply.ProductName}"); // This will be empty
Console.WriteLine($"Description: {getDiscountReply.Description}"); // This will be empty
Console.WriteLine($"Amount: {getDiscountReply.Amount}");
