$port = "8080"
if ($null -ne $env:DEMOSERVER.PORT) {
    $port = $env:DEMOSERVER.PORT
}

$ipa = (Get-NetIPConfiguration | Where-Object {$_.IPv4DefaultGateway -ne $null -and $_.NetAdapter.Status -ne "Disconnected"}).IPv4Address.IPAddress

$r = Invoke-WebRequest -Uri "http://${ipa}:${port}/" -UseBasicParsing
if (200 -ne $r.StatusCode) {
    throw "Integration test failed"
}
Write-Host "Integration tests passed!"
