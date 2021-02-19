$healthprocess = Get-Process netdemo-Server -ErrorAction SilentlyContinue
if ($healthprocess -and !$healthprocess.HasExited) {
	$healthprocess | Stop-Process -Force
}