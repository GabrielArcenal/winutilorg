# Sortexa

Sortexa is a lightweight Windows desktop app I am building to make file organization safer and easier.

It scans a folder, shows exactly what it found, creates a preview of the proposed organization, and only moves files after the user confirms the plan. It is designed for people who want a cleaner Downloads or workspace folder without accidentally touching files that are already arranged in subfolders.

> **Project status: WIP, but working**
>
> The main organize-and-undo workflow is implemented and tested. The app can be run locally or published as a standalone Windows executable. The project is still under active development, so the interface and feature set may continue to improve.

## What Sortexa does

The current workflow is:

1. **Select a source folder** — for example, `Downloads`.
2. **Scan the folder** — Sortexa lists the files directly inside it.
3. **Review the file information** — name, extension, size, modified date, and category.
4. **Select a destination folder** — for example, a new `Sorted` folder.
5. **Preview the changes** — Sortexa shows the exact destination path for every file.
6. **Organize the files** — after the plan is reviewed, Sortexa creates the required folders and moves the files.
7. **Undo when possible** — the last operation can be restored if the original paths are still available and no conflicts block the restore.

## Example

Suppose the selected source folder contains:

```text
Downloads\
├── report.pdf
├── vacation.jpg
├── song.mp3
└── AlreadyOrganized\
    └── old-report.pdf
```

If the destination is `Downloads\Sorted`, the preview can show:

```text
Sorted\Documents\2026-10-23\report.pdf
Sorted\Images\2026-10-22\vacation.jpg
Sorted\Audio\2026-10-20\song.mp3
```

Sortexa creates those destination folders automatically when the operation runs. The date folder uses each file's last-modified date.

`AlreadyOrganized\old-report.pdf` is not included because Sortexa scans only files directly inside the selected folder. Existing subfolders are left alone.

## What the preview tells you

The file list shows:

- File name
- Extension
- File size
- Last modified date
- Detected category

The organization preview shows:

- Original file name
- Category
- Exact destination path
- `Ready` or the reason a file cannot be moved

This means the app does not hide where files will go. Users can inspect the plan before any files are changed.

## Categories

The current built-in categories are:

- **Documents** — `.doc`, `.docx`, `.pdf`, `.txt`, `.rtf`
- **Images** — `.bmp`, `.gif`, `.jpeg`, `.jpg`, `.png`, `.webp`
- **Audio** — `.aac`, `.flac`, `.mp3`, `.wav`
- **Video** — `.avi`, `.mkv`, `.mov`, `.mp4`, `.webm`
- **Other** — extensions that do not match the configured categories

Category rules are being made configurable through the Settings area as the project continues to develop.

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

## Use the published app

To create a standalone 64-bit Windows executable:

```powershell
dotnet publish Sortexa\Sortexa.csproj `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  --property:PublishSingleFile=true `
  --property:EnableCompressionInSingleFile=true `
  --property:IncludeNativeLibrariesForSelfExtract=true `
  --output Sortexa\bin\standalone
```

Then launch:

```text
Sortexa\bin\standalone\Sortexa.exe
```

The published executable includes the .NET runtime, so a separate .NET installation is not required on the target Windows computer.

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
- Existing destination files are treated as conflicts and are never overwritten automatically.
- Planned duplicate destinations are rejected.
- Undo checks that the moved file still exists and that the original path is free.
- Partial failures and skipped files are reported.
- File contents are not copied into the operation history.

## Current working features

- Native WinForms desktop interface
- Dashboard, Organizer, Activity Logs, Settings, and About pages
- Responsive layout for resizing and Windows display scaling
- First-launch welcome dialog with a “Don't show this again” option
- Folder scanning and file metadata display
- Category-based and date-based organization
- Full organization preview before moving files
- Conflict detection and no-overwrite-by-default behavior
- Cancellation and partial-failure reporting
- Operation history and conflict-aware undo
- Bounded activity logs
- Persistent settings and window state
- Automated tests for scanner, organization, settings, logging, and undo behavior

## Current WIP areas

Sortexa is usable for its core organization workflow, but it is not presented as a finished commercial product yet. Current work includes:

- Refining the UI and making the workflow even easier for first-time users
- Making categories and organization rules more configurable
- Improving batch renaming from preview through execution
- Adding more detailed progress and completion summaries
- Expanding test coverage for unusual Windows file-system cases
- Improving packaging and release instructions

The following are intentionally not included:

- Automatic updates
- Telemetry or analytics
- Cloud synchronization
- Account sign-in
- Network services
- An installer

Sortexa operates locally by default and does not need an internet connection for its core functionality.
