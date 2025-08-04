using Xunit;

namespace AspNetCore8Test.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(2, 3, 5)]
    [InlineData(-1, 1, 0)]
    [InlineData(0, 0, 0)]
    [InlineData(10, -5, 5)]
    public void Addition_ShouldReturnCorrectSum(int a, int b, int expected)
    {
        // Act
        var result = a + b;
        
        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5, 3, 2)]
    [InlineData(10, 10, 0)]
    [InlineData(1, 5, -4)]
    [InlineData(0, 0, 0)]
    public void Subtraction_ShouldReturnCorrectDifference(int a, int b, int expected)
    {
        // Act
        var result = a - b;
        
        // Assert
        Assert.Equal(expected, result);
    }
}
