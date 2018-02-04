#!/bin/bash

mkdir -p bin
cd src
echo "Building libraries...."
gcc -fPIC -D__UNIX__ -shared -o ../bin/libtclwrapper.so tclwrapper.c -ltcl8.6 -ltk8.6

cd TickleSharp
dotnet build --configuration Release
cp bin/Release/netstandard2.0/TickleSharp.dll ../../bin/TickleSharp.dll
