#!/bin/sh
set -e
dotnet publish filestats/filestats.csproj --nologo -o win -r win-x64 -p:PublishSingleFile=true --self-contained true
cd win
zip -9r ../filestats-win.zip .
