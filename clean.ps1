<#PSScriptInfo .VERSION 0.0.1.4#>

[CmdletBinding()]
Param ()

& {
  Get-ChildItem "$PSScriptRoot\*.cs","$PSScriptRoot\*.html","$PSScriptRoot\*.ps*1","$PSScriptRoot\.gitignore" -Recurse | ForEach-Object {
    $content = @(Get-Content $_.FullName).TrimEnd() -join [Environment]::NewLine
    Set-Content $_.FullName $content -NoNewLine
  }
}