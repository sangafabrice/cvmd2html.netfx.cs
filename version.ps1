<#PSScriptInfo .VERSION 0.0.1.5#>

[CmdletBinding()]
Param ()

& {
  $infoFilePath = "$PSScriptRoot\src\AssemblyInfo.cs"
  Get-ChildItem "$PSScriptRoot\*.cs" -Recurse | ForEach-Object {
    try {
      [version] (@(Get-Content $_.FullName).where({[string]::IsNullOrWhiteSpace($_)}, 'until').SubString(4) |
      Out-String | ForEach-Object { [xml] "<header>$_</header>" }).header.version
    } catch { }
  } | Sort-Object | Where-Object Revision -GT 0 | Measure-Object -Property Revision -Sum | ForEach-Object {
    $version = $_.Sum
    $infoContent = ((Get-Content $infoFilePath).where({ $_ -like '*version*' }, 'until') -join [Environment]::NewLine) + [Environment]::NewLine + @"
[assembly: AssemblyInformationalVersion("0.0.1.$version")]
[assembly: AssemblyVersion("0.0.1.$version")]
"@
    Set-Content $infoFilePath $infoContent -NoNewline
  }
}