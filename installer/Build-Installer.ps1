$ErrorActionPreference = "Stop"

$repositoryRoot = Split-Path -Parent $PSScriptRoot
$projectPath = Join-Path $repositoryRoot "Sortexa\Sortexa.csproj"
$publishPath = Join-Path $repositoryRoot "Sortexa\bin\installer-publish"
$innoCompilerPath = (Get-Command "iscc.exe" -ErrorAction SilentlyContinue).Source

$knownCompilerPaths = @(
    (Join-Path ${env:ProgramFiles(x86)} "Inno Setup 6\ISCC.exe"),
    (Join-Path ${env:ProgramFiles} "Inno Setup 6\ISCC.exe"),
    (Join-Path ${env:LOCALAPPDATA} "Programs\Inno Setup 6\ISCC.exe")
)
if ([string]::IsNullOrWhiteSpace($innoCompilerPath)) {
    $innoCompilerPath = $knownCompilerPaths |
        Where-Object { Test-Path $_ } |
        Select-Object -First 1
}

if ([string]::IsNullOrWhiteSpace($innoCompilerPath)) {
    throw "Inno Setup was not found. Install it from https://jrsoftware.org/isinfo.php, then run this script again."
}

dotnet publish $projectPath `
    --configuration Release `
    --runtime win-x64 `
    --self-contained true `
    --property:PublishSingleFile=true `
    --property:EnableCompressionInSingleFile=true `
    --property:IncludeNativeLibrariesForSelfExtract=true `
    --output $publishPath

& $innoCompilerPath (Join-Path $PSScriptRoot "Sortexa.iss")
if ($LASTEXITCODE -ne 0) {
    throw "Inno Setup failed with exit code $LASTEXITCODE."
}

Write-Output "Installer created in Sortexa\bin\installer."
