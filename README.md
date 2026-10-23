# Sortexa

Sortexa is a local-first Windows Forms utility for reviewing and safely organizing files.

## Requirements

- Windows
- .NET SDK 10.0.402 or newer
- Windows Desktop runtime for .NET 10

## Build and test

From the repository root:

```powershell
dotnet restore Sortexa.slnx
dotnet build Sortexa.slnx --no-restore
dotnet test
```

The test suite uses temporary directories and synthetic files. It does not operate on user folders.

## Run locally

```powershell
dotnet run --project Sortexa\Sortexa.csproj
```

The application does not require an account or network connection. Its local data is stored under:

```text
%LOCALAPPDATA%\Sortexa\
```

Current files include:

- `settings.json` — user preferences
- `operations.json` — move history used for undo
- `window.state` — window position and size
- `logs\activity.json` — bounded activity history

## Publish a Windows build

For a self-contained 64-bit Windows build:

```powershell
dotnet publish Sortexa\Sortexa.csproj `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  --output Sortexa\bin\publish
```

The publish directory is ignored by Git. Review the output and test it on a clean Windows machine before distributing it.

## Safety behavior

- Only files directly inside the selected folder are scanned by default.
- Existing subfolders are not traversed or reorganized.
- Planned destinations use `Category\yyyy-MM-dd\filename`, based on the file's last-modified date.
- Organization is previewed before execution.
- Existing destination files are treated as conflicts.
- Planned duplicate destinations are rejected.
- Undo checks that the moved file still exists and that the original path is free.
- Partial failures and skipped files are reported.
- File contents are not copied into the operation history.

## Current scope

The current implementation provides scanning, categorization, organization planning, batch rename planning, operation history, undo, settings, activity logs, and an About page. Installer creation, automatic updates, telemetry, cloud synchronization, and network services are intentionally not included.
