using System;
using System.IO;
using NUnit.Framework;

namespace Tests.FileStats;

file sealed class Tests
{
    [Test]
    public void Test1()
    {
        using var stdout = new StringWriter();
        using var stderr = new StringWriter();
        Console.SetOut(stdout);
        Console.SetError(stderr);
        fileStats.FileStats.Main(["--bad"]);
        Assert.That(stderr.ToString(), Does.Contain("Unrecognized command or argument"));
    }
}