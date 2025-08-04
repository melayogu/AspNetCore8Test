using System;

class Program
{
    static void Main(string[] args)
    {
        var tests = new[]
        {
            "特殊字符測試ABC123",
            "Product 產品",
            "😀",
            "🎉",
            "Mixed測試123"
        };

        foreach (var test in tests)
        {
            Console.WriteLine($"'{test}' = {test.Length} characters");
        }
    }
}
