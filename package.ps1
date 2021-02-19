$basename = "netdemo-server"
$archiveName = ".\$basename.zip"

nuget restore
msbuild.exe /t:Rebuild /p:Configuration=Release

Remove-Item $archiveName -ErrorAction Ignore

Write-Host "Creating Archive: $archiveName"

$Command=" & 'C:\Program Files\7-Zip\7z.exe' a $archiveName .\$basename\bin\Release\*"
Write-Host "Running: $Command"
$Command | Invoke-Expression

if(-Not(Test-Path $archiveName)){
    Write-Host "WARNING: $archiveName was not created"
}
