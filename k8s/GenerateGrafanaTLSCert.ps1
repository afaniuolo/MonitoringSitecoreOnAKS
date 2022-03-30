$mkcert = ".\mkcert.exe"
if (-not (Test-Path $mkcert)) {
    Invoke-WebRequest https://github.com/FiloSottile/mkcert/releases/download/v1.4.1/mkcert-v1.4.1-windows-amd64.exe -UseBasicParsing -OutFile mkcert.exe
}
Write-Host "Creating TLS/HTTPS certificates..."
& $mkcert -install
& $mkcert -cert-file grafana-secrets\tls\global-grafana\tls.crt -key-file grafana-secrets\tls\global-grafana\tls.key "grafana.globalhost"
