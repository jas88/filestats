# NuGet Packages

This document lists all NuGet packages used in the FileStats project, their purpose, and risk assessment.

## Production Dependencies

### Microsoft.Data.Sqlite 11.0.0

- **Purpose**: SQLite database access for caching directory statistics
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official Microsoft ADO.NET provider for SQLite, developed alongside Entity Framework Core. Actively maintained as part of .NET.
- **Repository**: <https://github.com/dotnet/efcore>

### SQLitePCLRaw.bundle_e_sqlite3 3.0.5

- **Purpose**: Native SQLite library for Microsoft.Data.Sqlite
- **License**: Apache-2.0
- **Risk Level**: LOW
- **Notes**: The SQLite build that Microsoft.Data.Sqlite uses by default. Referenced directly to pin a version that is not affected by GHSA-2m69-gcr7-jv3q (CVE-2025-6965), the vulnerable native SQLite in SQLitePCLRaw 2.1.11 and earlier.
- **Repository**: <https://github.com/ericsink/SQLitePCL.raw>

### System.CommandLine 3.0.0

- **Purpose**: Command-line argument parsing
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official Microsoft library for building command-line applications. Provides modern API with shell completions support and async/await integration.
- **Repository**: <https://github.com/dotnet/command-line-api>

### System.IO.Abstractions 23.0.0

- **Purpose**: File system abstraction layer for testability
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Well-established library enabling dependency injection of file system operations. Actively maintained.
- **Repository**: <https://github.com/TestableIO/System.IO.Abstractions>

## Build Dependencies

### DotNet.ReproducibleBuilds 1.2.39

- **Purpose**: Reproducible build settings
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: .NET Foundation package that turns on ContinuousIntegrationBuild when it detects a CI server and sets the other MSBuild properties reproducible builds need. Added to packable projects (filestats) by the shared build-standards; the version is set in Directory.Packages.props. Build-time only.
- **Repository**: <https://github.com/dotnet/reproducible-builds>

### MinVer 8.0.0

- **Purpose**: Versioning from Git tags
- **License**: Apache-2.0
- **Risk Level**: LOW
- **Notes**: Widely used package that sets the assembly and package versions from Git tags (prefix `v`) at build time. Added to packable projects (filestats) by the shared build-standards; the version is set in Directory.Packages.props. Build-time only.
- **Repository**: <https://github.com/adamralph/minver>

## Test Dependencies

### Microsoft.Extensions.DependencyModel 11.0.0

- **Purpose**: Dependency version pin for the test runner
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official Microsoft library for reading `.deps.json` files. Not used by the tests directly: it is referenced to fix a dependency version conflict with NUnit3TestAdapter. Test-time only.
- **Repository**: <https://github.com/dotnet/runtime>

### Microsoft.NET.Test.Sdk 18.12.0

- **Purpose**: VSTest host and MSBuild targets for test projects
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official Microsoft test SDK. dotnet test runs the tests on Microsoft Testing Platform through the NUnit runner, which does not need it; it provides the VSTest host for running the tests through VSTest.
- **Repository**: <https://github.com/microsoft/vstest>

### Microsoft.Testing.Extensions.CodeCoverage 18.11.0

- **Purpose**: Code coverage collection during test execution
- **License**: Microsoft Software License Terms (Microsoft .NET Library)
- **Risk Level**: LOW
- **Notes**: Official Microsoft code coverage extension for Microsoft Testing Platform. Not open source; the licence allows its use to develop and test applications. Added to test projects by the shared build-standards; the version is set in Directory.Packages.props. Test-time only.
- **Repository**: <https://github.com/microsoft/codecoverage>

### NUnit 5.0.0

- **Purpose**: Unit testing framework
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Mature, widely-used testing framework. Actively maintained with regular updates.
- **Repository**: <https://github.com/nunit/nunit>

### NUnit.Analyzers 4.15.0

- **Purpose**: Roslyn analyzers for NUnit best practices
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official NUnit analyzers. Build-time only, helps enforce testing best practices.
- **Repository**: <https://github.com/nunit/nunit.analyzers>

### NUnit3TestAdapter 6.3.1

- **Purpose**: Test adapter for running NUnit tests in Visual Studio / dotnet test
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Official NUnit test adapter. Required for test discovery and execution. With EnableNUnitRunner set, it also provides the Microsoft Testing Platform runner that dotnet test uses.
- **Repository**: <https://github.com/nunit/nunit3-vs-adapter>

### System.IO.Abstractions.TestingHelpers 22.3.0

- **Purpose**: Mock file system implementation for testing
- **License**: MIT
- **Risk Level**: LOW
- **Notes**: Companion package to System.IO.Abstractions, provides in-memory file system for unit tests.
- **Repository**: <https://github.com/TestableIO/System.IO.Abstractions>

## Risk Assessment Summary

- **Total Packages**: 13 (4 production, 2 build, 7 test)
- **High Risk**: 0
- **Medium Risk**: 0
- **Low Risk**: 13

All packages are from reputable sources (Microsoft official or established open-source projects) and actively maintained. Most use MIT licensing; MinVer and SQLitePCLRaw.bundle_e_sqlite3 use Apache-2.0, and Microsoft.Testing.Extensions.CodeCoverage uses Microsoft's own licence terms.

## Update Policy

- Production dependencies: Review quarterly or when security advisories are issued
- Test dependencies: Update as needed to maintain compatibility with latest .NET SDK
- All packages use Dependabot for automated update PRs
