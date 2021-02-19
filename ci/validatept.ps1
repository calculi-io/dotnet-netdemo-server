$limit = 10.0
if ($null -ne $env:PERFORMANCE_LIMIT) {
	$limit = [single]$env:PERFORMANCE_LIMIT
}

$resultsfile = "results.jtl"
if ($null -ne $env:TEST_LOGS_DIR) {
	$resultsfile = "$env:TEST_LOGS_DIR/$resultsfile"
}

$pt = Import-Csv -Path $resultsfile
$avg = ($pt | Measure-Object -Property 'elapsed' -Average).Average
Write-Host "Average response time: $avg"

if ($limit -le $avg) {
	throw "Performance Test exceeded $limit"
}
Write-Host "Performance under $limit."
