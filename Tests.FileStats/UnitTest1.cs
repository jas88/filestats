using NUnit.Framework;
using System.IO;
using System;

namespace Tests.FileStats;

public class Tests
{
    [Test]
    public void Test1()
    {
        using var stdout = new StringWriter();
        using var stderr = new StringWriter();
        Console.SetOut(stdout);
        Console.SetError(stderr);
        fileStats.FileStats.Main(new[] { "--bad" });
        Assert.That(stderr.ToString().Contains("Option 'bad' is unknown"));
    }
}