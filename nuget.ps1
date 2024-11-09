<#PSScriptInfo .VERSION 0.0.1.1#>

[CmdletBinding()]
Param ()

& {
  $LibDir = "$PSScriptRoot\lib"
  Remove-Item $LibDir -Recurse -ErrorAction SilentlyContinue -Force
  & "$PSScriptRoot\nuget.exe" install Markdig -Version 0.37.0 -Framework net4.8.1 -DependencyVersion Highest -Outputdirectory $LibDir -verbosity quiet
  Get-ChildItem "$PSScriptRoot\lib\net46*" -Directory -Recurse |
  Where-Object Parent -Like *\lib |
  ForEach-Object {
    Copy-Item (Get-ChildItem $_ -File -Filter *.dll).FullName .\lib\
  }
  Get-ChildItem "$LibDir\*" -Directory | Remove-Item -Recurse -Force
}