---

Set up a mock API in 2 minutes
In this article, I'll explain how you can quickly transform a JSON file into a mock API only using Docker. 
I recently implemented a mock API using Mockserver. While it is a powerful tool that can be run in multiple programming languages the documentation does contain a straightforward guide on how to get set up. This article is primarily an aggregation of the official docs that will let you get setup quickly.
tldr;
Step 1: Create an expactations.json file that will contain the endpoints you want to mock and the expected response
Step 2: Passing the expectations.json file to the official Mockserver Docker image when running it using the following command:
docker run --rm --name=mockserver -p 1080:1080 \
  -v $(pwd)/expectations.json:/config/expectations.json \
  -e MOCKSERVER_INITIALIZATION_JSON_PATH=/config/expectations.json \
  mockserver/mockserver \
  -serverPort 1080 \
  -logLevel INFO
Using Mockserver in more detail
The typical sequence for using MockServer is as follows:
Setup Expectations
Run Your Test Scenarios
Verify Requests

I wanted to follow a simple and generic implementation so I ran Mockserver using Docker and configured expectations using JSON files. 

---

https://www.mock-server.com/mock_server/running_mock_server.html#docker_container
MockServer can be run using docker-compose by adding the container as a service.
version: "2.4"
services:
  mockServer:
    image: mockserver/mockserver:5.14.0
    command: -logLevel DEBUG -serverPort 1090 -proxyRemotePort 80 -proxyRemoteHost www.mock-server.com
    ports:
      - 1080:1090
It is also possible to configure the MockServer by mounting a volume containing a properties file or JSON expectation initializer, as follows:
version: "2.4"
services:
  mockServer:
    image: mockserver/mockserver:5.14.0
    ports:
      - 1080:1080
    environment:
      MOCKSERVER_PROPERTY_FILE: /config/mockserver.properties
      MOCKSERVER_INITIALIZATION_JSON_PATH: /config/initializerJson.json
    volumes:
      - type: bind
        source: .
        target: /config

---

mockserver/mockserver-examples/json_examples.md at master · mock-server/mockserver
MockServer enables easy mocking of any system you integrate with via HTTP or HTTPS with clients written in Java…github.com

---

To pass a JSON expectations file to the MockServer Docker image, you can use Docker's volume mounting feature. This allows you to mount a directory from your host machine to your Docker container, making the file available to the MockServer running inside the container.
Here's an example of how you can do it:
First, make sure your JSON expectations file is in a directory on your host machine. For example, you might have a directory /path/to/your/expectations and a file named expectations.json inside it.
Then, you can run the MockServer Docker image with the -v option to mount the directory:

docker run -d -p 1080:1080 -v /path/to/your/expectations:/mockserver \
 mockserver/mockserver

---

In this example, replace /path/to/your/expectations with the path to your directory on the host machine. The :/mockserver part of the -v option specifies the path inside the container where the directory will be mounted.
3. Your expectations.json file is now available at /mockserver/expectations.json inside the Docker container. You can load this file into MockServer using the MockServer client.
Please note that the MockServer Docker image does not automatically load expectations from a file. You need to use the MockServer client to load the expectations after the container has started.

---

To pass a JSON file to a MockServer running in a Docker container, you can use Docker's volume mounting feature. This allows you to make a file or directory on the host machine accessible inside the Docker container.
Here's an example of how you can do this:
First, create a JSON file with your MockServer expectations. Let's say the file is named expectations.json and its located in the current directory. You can find lots of examples of JSON expectations on Mockservers github repo.
Then run the MockServer Docker container with a command like this:

docker run --rm --name=mockserver -p 1080:1080 \
  -v $(pwd)/expectations.json:/config/expectations.json \
  -e MOCKSERVER_INITIALIZATION_JSON_PATH=/config/expectations.json \
  mockserver/mockserver \
  -serverPort 1080 \
  -logLevel INFO
In this command:
-d runs the container in detached mode.
--rm automatically removes the container when it exits.
--name=mockserver names the container "mockserver".
-p 1080:1080 maps port 1080 in the container to port 1080 on the host machine.
-v $(pwd)/expectations.json:/config/expectations.json mounts the expectations.json file from the current directory on the host machine to /config/expectations.json inside the container.
-e MOCKSERVER_INITIALIZATION_JSON_PATH=/config/expectations.json is needed for Mockserver to load the expectations automatically once it has been mounted to the container
mockserver/mockserver is the name of the Docker image to run.
-serverPort 1080 sets the MockServer to listen on port 1080.
-logLevel INFO sets the log level to INFO.

---

https://www.mock-server.com/mock_server/performance.html