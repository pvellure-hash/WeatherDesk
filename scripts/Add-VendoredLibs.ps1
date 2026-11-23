<#
 Downloads the two third-party JavaScript libraries the page uses into wwwroot\lib.
 They are checked in on purpose (no package manifest names them), at older releases.
 Run once from anywhere, then commit the two files:
   powershell -ExecutionPolicy Bypass -File scripts\Add-VendoredLibs.ps1
#>
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$lib  = Join-Path $root 'src\WeatherDesk.Web\wwwroot\lib'

$files = @(
  @{ Url = 'https://code.jquery.com/jquery-3.4.1.min.js';                                         Path = 'jquery\jquery.min.js' },
  @{ Url = 'https://cdn.jsdelivr.net/npm/jquery-validation@1.22.1/dist/jquery.validate.js';       Path = 'jquery-validate\jquery.validate.js' }
)

foreach ($f in $files) {
  $dest = Join-Path $lib $f.Path
  New-Item -ItemType Directory -Force -Path (Split-Path -Parent $dest) | Out-Null
  Invoke-WebRequest -Uri $f.Url -OutFile $dest -UseBasicParsing
  $banner = (Get-Content -Path $dest -TotalCount 2) -join ' '
  Write-Host ("Saved {0}  ->  {1}" -f $f.Path, $banner.Substring(0, [Math]::Min(90, $banner.Length)))
}
Write-Host 'Done. Reload the page.'
