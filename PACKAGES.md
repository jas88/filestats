# NuGet Packages

This document lists all NuGet packages used in the FileStats project, their purpose, and risk assessment.

## Production Dependencies

### Microsoft.Data.Sqlite 9.0.1
- **Purpose**: SQLite database access for caching directory statistics
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official Microsoft Entity Framework Core SQLite provider. Actively maintained as part of .NET.
- **Repository**: https://github.com/dotnet/efcore

### System.CommandLine 2.0.0-beta5.25277.114
- **Purpose**: Command-line argument parsing
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official Microsoft library for building command-line applications. Provides modern API with shell completions support and async/await integration.
- **Repository**: https://github.com/dotnet/command-line-api

### System.IO.Abstractions 21.2.1
- **Purpose**: File system abstraction layer for testability
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Well-established library enabling dependency injection of file system operations. Actively maintained.
- **Repository**: https://github.com/TestableIO/System.IO.Abstractions

## Test Dependencies

### coverlet.collector 6.0.4
- **Purpose**: Code coverage collection during test execution
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official .NET Foundation project for code coverage. Test-time only.
- **Repository**: https://github.com/coverlet-coverage/coverlet

### Microsoft.NET.Test.Sdk 17.12.0
- **Purpose**: Test platform for running NUnit tests
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official Microsoft test SDK. Required for running tests via dotnet test.
- **Repository**: https://github.com/microsoft/vstest

### NUnit 4.3.2
- **Purpose**: Unit testing framework
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Mature, widely-used testing framework. Actively maintained with regular updates.
- **Repository**: https://github.com/nunit/nunit

### NUnit.Analyzers 4.6.0
- **Purpose**: Roslyn analyzers for NUnit best practices
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official NUnit analyzers. Build-time only, helps enforce testing best practices.
- **Repository**: https://github.com/nunit/nunit.analyzers

### NUnit3TestAdapter 4.6.0
- **Purpose**: Test adapter for running NUnit tests in Visual Studio / dotnet test
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official NUnit test adapter. Required for test discovery and execution.
- **Repository**: https://github.com/nunit/nunit3-vs-adapter

### System.IO.Abstractions.TestingHelpers 21.2.1
- **Purpose**: Mock file system implementation for testing
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Companion package to System.IO.Abstractions, provides in-memory file system for unit tests.
- **Repository**: https://github.com/TestableIO/System.IO.Abstractions

## Risk Assessment Summary

- **Total Packages**: 10 (3 production, 7 test)
- **High Risk**: 0
- **Medium Risk**: 0
- **Low Risk**: 10

All packages are from reputable sources (Microsoft official or established open-source projects) with MIT licensing and active maintenance.

## Update Policy

- Production dependencies: Review quarterly or when security advisories are issued
- Test dependencies: Update as needed to maintain compatibility with latest .NET SDK
- All packages use Dependabot for automated update PRs
