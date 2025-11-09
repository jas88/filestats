# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial release of FileStats utility
- Directory scanning with caching support
- SQLite-based cache for performance
- Command-line options for debug mode, retry logic, and custom cache paths
- Size statistics with human-readable formatting (B, KB, MB, GB, TB, PB, EB)
- Native AOT compilation support for faster startup and smaller binaries
- Multi-platform release binaries (linux-x64, linux-arm64, win-x64, win-arm64, osx-x64, osx-arm64)

### Changed
- Consolidated GitHub Actions workflows into single CI/CD pipeline
- Optimized for speed with IlcOptimizationPreference setting
- Native binaries now published as release assets with SHA256 checksums

### Deprecated

### Removed

### Fixed

### Security

[Unreleased]: https://github.com/jas88/filestats/compare/v1.0.0...HEAD
