using System;
using System.IO;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace Tests.FileStats;

/// <summary>
/// Runs the scanner end to end, with a temporary directory tree as the current directory and a cache
/// database outside that tree.
/// </summary>
[NonParallelizable] // changes the process-wide current directory and console
internal sealed class ScanTests
{
    private string _baseDirectory = string.Empty;
    private string _previousDirectory = string.Empty;
    private TextWriter _previousOut = TextWriter.Null;
    private TextWriter _previousError = TextWriter.Null;

    private string TreeDirectory => Path.Join(_baseDirectory, "tree");

    private string CachePath => Path.Join(_baseDirectory, "cache.db");

    [SetUp]
    public void SetUp()
    {
        // NUnit reuses the fixture instance, so each test gets its own tree and cache here.
        _baseDirectory = Directory.CreateTempSubdirectory("filestats-tests-").FullName;

        // a/ holds 300 bytes in 2 files; b/ holds 1074 bytes in 2 files, one of them in b/nested/.
        WriteFile("a/one.bin", 100);
        WriteFile("a/two.bin", 200);
        WriteFile("b/three.bin", 1024);
        WriteFile("b/nested/four.bin", 50);
        WriteFile("top-level.bin", 5000); // only subdirectories of the current directory are counted

        _previousDirectory = Directory.GetCurrentDirectory();
        _previousOut = Console.Out;
        _previousError = Console.Error;
        Directory.SetCurrentDirectory(TreeDirectory);
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetOut(_previousOut);
        Console.SetError(_previousError);
        Directory.SetCurrentDirectory(_previousDirectory);

        // Pooled connections keep the database file open, which stops Windows deleting it.
        SqliteConnection.ClearAllPools();
        Directory.Delete(_baseDirectory, recursive: true);
    }

    [Test]
    public void CountsTheFilesAndBytesUnderEachSubdirectory()
    {
        var (exitCode, output) = Run("--cachePath", CachePath);

        Assert.Multiple(() =>
        {
            Assert.That(exitCode, Is.Zero);
            Assert.That(output, Does.Contain("Found 2 directories containing 4 files, total 1374 bytes (1.3KB)"));
        });
    }

    [Test]
    public void ReusesCachedTotalsAndScansOnlyNewDirectories()
    {
        Run("--cachePath", CachePath);
        WriteFile("a/added-later.bin", 1000); // a/ is in the cache now, so this file is not counted
        WriteFile("c/new.bin", 10);           // c/ is not, so it is scanned

        var (exitCode, output) = Run("--cachePath", CachePath);

        Assert.Multiple(() =>
        {
            Assert.That(exitCode, Is.Zero);
            Assert.That(output, Does.Contain("Found 3 directories containing 5 files, total 1384 bytes (1.4KB)"));
        });
    }

    [Test]
    public void DebugReportsTheCacheLocationAndExpiredEntries()
    {
        var (exitCode, output) = Run("--debug", "--cachePath", CachePath);

        Assert.Multiple(() =>
        {
            Assert.That(exitCode, Is.Zero);
            Assert.That(output, Does.Contain($"Caching in '{CachePath}'"));
            Assert.That(output, Does.Contain("Expired 0 old cache entries"));
        });
    }

    private (int ExitCode, string Output) Run(params string[] args)
    {
        using var output = new StringWriter();
        Console.SetOut(output);
        Console.SetError(output);
        try
        {
            return (fileStats.FileStats.Main(args), output.ToString());
        }
        finally
        {
            Console.SetOut(_previousOut);
            Console.SetError(_previousError);
        }
    }

    private void WriteFile(string relativePath, int length)
    {
        var path = Path.Join(TreeDirectory, relativePath);
        Directory.CreateDirectory(Path.Join(TreeDirectory, Path.GetDirectoryName(relativePath)));
        File.WriteAllBytes(path, new byte[length]);
    }
}
