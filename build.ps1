<#PSScriptInfo .VERSION 0.0.1.11#>

[CmdletBinding()]
Param ()

Start-Job {
  $ScriptRoot = $using:PSScriptRoot

  $HostColorArgs = @{
    ForegroundColor = 'Black'
    BackgroundColor = 'Green'
    NoNewline = $True
  }

  Try {
    Remove-Item ($BinDir = "$ScriptRoot\bin") -Recurse -ErrorAction Stop
  } Catch [System.Management.Automation.ItemNotFoundException] {
    Write-Host $_.Exception.Message @HostColorArgs
    Write-Host
  } Catch {
    $HostColorArgs.BackgroundColor = 'Red'
    Write-Host $_.Exception.Message @HostColorArgs
    Write-Host
    Return
  }
  New-Item $BinDir -ItemType Directory -ErrorAction SilentlyContinue | Out-Null
  Copy-Item "$ScriptRoot\App.config" -Destination "$BinDir\cvmd2html.exe.config" -Recurse
  Copy-Item "$(($LibDir = "$ScriptRoot\lib"))\*" -Destination $BinDir -Recurse

  # Compile the source code with jsc.
  $CompilerParams = [System.CodeDom.Compiler.CompilerParameters] @{
    OutputAssembly = ($ConvertExe = "$BinDir\cvmd2html.exe")
    GenerateInMemory = $False
    GenerateExecutable = $True
    CompilerOptions = "/target:$(If ($args[0].Value -eq 'Continue') { 'exe' } Else { 'winexe' }) /win32icon:`"$ScriptRoot\menu.ico`" /optimize"
  }
  $CompilerParams.ReferencedAssemblies.AddRange(@(
    Get-ChildItem @(
      "$LibDir\*"
      'PresentationFramework','WindowsBase','PresentationCore' |
      ForEach-Object { "$Env:windir\Microsoft.NET\Framework$(If ([Environment]::Is64BitOperatingSystem) { '64' })\v4.0.30319\WPF\${_}.dll" }
    ) | ForEach-Object { $_.FullName }
    'System.Xaml.dll','System.Numerics.dll'
    'Microsoft.CSharp.dll','System.dll','System.Core.dll'
  ))
  $Results = [Microsoft.CSharp.CSharpCodeProvider]::new().CompileAssemblyFromFile($CompilerParams, [string[]](Get-Item "$ScriptRoot\src\*.cs","$ScriptRoot\Program.cs" -Exclude 'Resource.cs').FullName)

  If ($Results.Errors.Count -eq 0 -and 0 -eq $Error.Count) {
    Write-Host "Output file $ConvertExe written." @HostColorArgs
    (Get-Item $ConvertExe).VersionInfo | Format-List * -Force
  } ElseIf ($Results.Errors.Count -ne 0) {
    $HostColorArgs.BackgroundColor = 'Red'
    ForEach ($ErrResult in $Results.Errors) {
      Write-Host ('Error {1}: {2}' -f $ErrResult.ErrorNumber,$ErrResult.ErrorText) @HostColorArgs
    }
  }
} -ArgumentList $DebugPreference -PSVersion 5.1 | Receive-Job -Wait -AutoRemoveJob