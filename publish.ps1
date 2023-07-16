#/usr/bin/pwsh
dotnet publish ./IsbnConverter/IsbnConverter.csproj -c Release -r win-x64 -p:PublishTrimmed=true -p:PublishAot=True 