<#PSScriptInfo .VERSION 0.0.1.9#>

using namespace System.Management.Automation
[CmdletBinding()]
Param ()

& {
  $HostColorArgs = @{
    ForegroundColor = 'Black'
    BackgroundColor = 'Green'
    NoNewline = $True
  }

  Try {
    Remove-Item ($BinDir = "$PSScriptRoot\bin") -Recurse -ErrorAction Stop
  } Catch [ItemNotFoundException] {
    Write-Host $_.Exception.Message @HostColorArgs
    Write-Host
  } Catch {
    $HostColorArgs.BackgroundColor = 'Red'
    Write-Host $_.Exception.Message @HostColorArgs
    Write-Host
    Return
  }
  New-Item $BinDir -ItemType Directory -ErrorAction SilentlyContinue | Out-Null
  Copy-Item "$PSScriptRoot\App.config" -Destination "$BinDir\cvmd2html.exe.config"
  Copy-Item "$PSScriptRoot\lib\*" -Destination $BinDir

  # Compile the source code with csc.exe.
  $EnvPath = $Env:Path
  $Env:Path = "$(($NetFxPath = "$Env:windir\Microsoft.NET\Framework$(If ([Environment]::Is64BitOperatingSystem) { '64' })\v4.0.30319"))\;$Env:Path"
  csc.exe /nologo /target:$($DebugPreference -eq 'Continue' ? 'exe':'winexe') /win32icon:"$PSScriptRoot\menu.ico" /optimize /reference:System.Numerics.dll /reference:"$BinDir\System.Runtime.CompilerServices.Unsafe.dll" /reference:"$BinDir\System.Numerics.Vectors.dll" /reference:"$BinDir\System.Memory.dll" /reference:"$BinDir\System.Buffers.dll" /reference:"$BinDir\Markdig.dll" /reference:"$(($WpfPath = "$NetFxPath\WPF"))\PresentationFramework.dll" /reference:"$WpfPath\PresentationCore.dll" /reference:"$WpfPath\WindowsBase.dll" /reference:System.Xaml.dll /out:$(($ConvertExe = "$BinDir\cvmd2html.exe")) "$(($SrcDir = "$PSScriptRoot\src"))\AssemblyInfo.cs" "$SrcDir\Converter.cs" "$SrcDir\MessageBox.cs" "$SrcDir\Parameters.cs" "$PSScriptRoot\Program.cs" "$SrcDir\Setup.cs"
  $Env:Path = $EnvPath

  If ($LASTEXITCODE -eq 0) {
    Write-Host "Output file $ConvertExe written." @HostColorArgs
    (Get-Item $ConvertExe).VersionInfo | Format-List * -Force
  }
}