# .NET Upgrade Plan: ContosoUniversity

## Overview

Upgrade **ContosoUniversity** from **.NET Framework 4.8** to **.NET 10.0 (LTS)**.

The project currently uses the legacy non-SDK-style project format targeting .NET Framework 4.8 with ASP.NET MVC 5. The upgrade requires converting the project to the modern SDK-style format, migrating from ASP.NET MVC 5 to ASP.NET Core MVC, updating all NuGet dependencies, and addressing any breaking API changes.

---

## Source and Target Versions

| | Version |
|---|---|
| **Source** | .NET Framework 4.8 |
| **Target** | .NET 10.0 (`net10.0`) |

---

## Projects in Solution

| Project | Path | Current TFM |
|---------|------|-------------|
| ContosoUniversity | `ContosoUniversity/ContosoUniversity.csproj` | `net48` (.NET Framework 4.8) |

---

## Upgrade Scope

1. **SDK-style project conversion** — Convert the legacy `.csproj` (non-SDK-style) to the modern SDK-style format. Remove explicit `<Reference>` entries in favour of `<PackageReference>` items.

2. **Target Framework Moniker (TFM) update** — Change `<TargetFrameworkVersion>v4.8</TargetFrameworkVersion>` to `<TargetFramework>net10.0</TargetFramework>`.

3. **ASP.NET MVC 5 → ASP.NET Core MVC migration** — Replace `System.Web.Mvc`, `System.Web`, `Global.asax`, and related MVC 5 infrastructure with ASP.NET Core MVC equivalents (`Program.cs`, `Startup`-style configuration, middleware pipeline).

4. **NuGet package updates** — Update all packages to .NET 10-compatible versions:
   - `Microsoft.EntityFrameworkCore.*` 3.1 → latest compatible with net10.0
   - `Microsoft.Extensions.*` packages to current versions
   - Remove packages that are superseded by the .NET 10 BCL (e.g., `Microsoft.Bcl.AsyncInterfaces`)

5. **API compatibility fixes** — Address any breaking changes introduced across .NET 5–10 (e.g., `System.Web` removal, `HttpContext` usage, bundling/minification replacements).

6. **Web.config → appsettings.json** — Migrate configuration from `Web.config` to `appsettings.json` / environment variables using the `Microsoft.Extensions.Configuration` system.

---

## Tasks

See `.metadata/tasks.json` for the structured task list.
