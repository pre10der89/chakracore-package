.\clean_all.ps1

Set-Alias DEVENV "$env:VS140COMNTOOLS\..\IDE\devenv"

DEVENV  /build "Debug|AnyCPU" Chakra.Package.Tester.sln
DEVENV  /build "Release|AnyCPU" Chakra.Package.Tester.sln

DEVENV  /build "Debug|x86" Chakra.Package.Tester.sln
DEVENV  /build "Release|x86" Chakra.Package.Tester.sln

DEVENV  /build "Debug|x64" Chakra.Package.Tester.sln
DEVENV  /build "Release|x64" Chakra.Package.Tester.sln

DEVENV  /build "Debug|ARM" Chakra.Package.Tester.sln
DEVENV  /build "Release|ARM" Chakra.Package.Tester.sln
