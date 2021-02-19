$progressPreference = 'silentlyContinue'
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

Write-Host -Text "Downloading Nuget..."
Invoke-WebRequest -OutFile nuget.exe -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
if (-Not (Test-Path nuget.exe)) {
	Write-Host -Text 'nuget.exe not downloaded'
	Throw [System.IO.FileNotFoundException] "nuget.exe not downloaded."
}

.\nuget install Microsoft.TestPlatform -Version 15.9.2

$vsc = '.\Microsoft.TestPlatform.15.9.2\tools\net451\Common7\IDE\Extensions\TestPlatform\vstest.console.exe'
Start-Process "$vsc" -ArgumentList "/TestCaseFilter:Category=Integration .\netdemo-Server.exe /Logger:trx" -NoNewWindow -Wait

Copy-Item .\TestResults\*.trx -Destination $env:TEST_LOGS_DIR/test.xml -ErrorAction SilentlyContinue
