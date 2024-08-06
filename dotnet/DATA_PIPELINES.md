Creating performant data pipelines in .NET involves several steps and considerations. Here's a general approach:

1. Use the right data structures: .NET provides a variety of data structures like Lists, Queues, Stacks, HashSets, etc. Choose the one that best fits your needs.

2. Use async/await for I/O operations: If your pipeline involves I/O operations like reading from a file or making a network request, use async/await to avoid blocking the main thread.

3. Use Parallel Programming: .NET provides the Task Parallel Library (TPL) and PLINQ (Parallel LINQ) to perform CPU-bound operations in parallel.

4. Use Dataflow (System.Threading.Tasks.Dataflow): The Dataflow library provides in-process message passing for concurrent and asynchronous operations. It's great for creating a pipeline where each step is a block that can be composed with others to create a pipeline.