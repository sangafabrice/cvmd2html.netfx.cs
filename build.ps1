<#PSScriptInfo .VERSION 0.0.1.12#>

[CmdletBinding()]
Param ()

Start-Process "$Env:windir\Microsoft.NET\Framework$(If ([Environment]::Is64BitOperatingSystem) { '64' })\v4.0.30319\msbuild.exe" -ArgumentList @(
  '-nologo -toolsversion:4.0 "{0}"' -f "$PSScriptRoot\cvmd2html.csproj"
  If ($DebugPreference -eq 'Continue') { "-property:OutputType=exe" }
) -NoNewWindow -Wait