REM https://www.nuget.org/packages/AppSoftware.LicenceEngine.KeyGenerator/
REM https://www.nuget.org/packages/AppSoftware.LicenceEngine.KeyVerification/

REM Replace version and API key in nuget_push.bat (VCS ignored)

dotnet build AppSoftware.LicenceEngine.KeyGenerator/AppSoftware.LicenceEngine.KeyGenerator.csproj --configuration Release
dotnet build AppSoftware.LicenceEngine.KeyVerification/AppSoftware.LicenceEngine.KeyVerification.csproj --configuration Release

REM TODO: Replace with equivalent dotnet nuget commands

nuget pack AppSoftware.LicenceEngine.KeyGenerator/AppSoftware.LicenceEngine.KeyGenerator.csproj -OutputDirectory packages -Properties Configuration=Release
nuget pack AppSoftware.LicenceEngine.KeyVerification/AppSoftware.LicenceEngine.KeyVerification.csproj -OutputDirectory packages -Properties Configuration=Release

nuget push packages/AppSoftware.LicenceEngine.KeyGenerator.9.9.9.nupkg -Source https://api.nuget.org/v3/index.json -SkipDuplicate -ApiKey xxxxxxxxxxxxxxxxxxxxx
nuget push packages/AppSoftware.LicenceEngine.KeyVerification.9.9.9.nupkg -Source https://api.nuget.org/v3/index.json -SkipDuplicate -ApiKey xxxxxxxxxxxxxxxxxxxxx

PAUSE