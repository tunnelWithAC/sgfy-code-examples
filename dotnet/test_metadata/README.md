In .NET, you can categorize tests using the TestCategory attribute. This attribute can be applied to test methods and allows you to group and filter tests based on the category.

Here's an example of how you can use it:

```
[TestClass]
public class UnitTest1
{
    [TestMethod]
    [TestCategory("CategoryA")]
    public void TestMethod1()
    {
        // Your test code here
    }

    [TestMethod]
    [TestCategory("CategoryB")]
    public void TestMethod2()
    {
        // Your test code here
    }
}
```

In the above example, `TestMethod1` is in `CategoryA` and `TestMethod2` is in `CategoryB`.

To run tests from a specific category, you can use the `/category` command-line option with `vstest.console.exe``:

`vstest.console.exe /category:"CategoryA" MyTestAssembly.dll`

This will only run tests that are in `CategoryA`.

In Visual Studio, you can also filter tests by category in the Test Explorer window by entering Trait:CategoryA in the search box.


-----

In xUnit, you can categorize tests using the Trait attribute. This attribute can be applied to test methods and allows you to group and filter tests based on the trait.

Here's an example of how you can use it:

```
public class UnitTest1
{
    [Fact]
    [Trait("Category", "CategoryA")]
    public void TestMethod1()
    {
        // Your test code here
    }

    [Fact]
    [Trait("Category", "CategoryB")]
    public void TestMethod2()
    {
        // Your test code here
    }
}
```

In the above example, TestMethod1 is in CategoryA and TestMethod2 is in CategoryB.

To run tests from a specific category, you can use the -trait command-line option with `dotnet test`:

```
dotnet test --filter Trait=CategoryA
```

To exclude a category, you can use the != operator:

```
dotnet test --filter Category!=CategoryA
```