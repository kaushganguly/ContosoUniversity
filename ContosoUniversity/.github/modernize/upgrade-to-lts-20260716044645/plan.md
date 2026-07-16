# Upgrade Plan: ContosoUniversity — .NET Framework 4.8 → .NET 10

## Overview

This plan upgrades the **ContosoUniversity** ASP.NET MVC application from **.NET Framework 4.8** to **.NET 10.0** (latest LTS), the latest Long-Term Support release of modern .NET.

## Project Analysis

| Property | Value |
|----------|-------|
| **Solution** | ContosoUniversity.sln |
| **Project** | ContosoUniversity/ContosoUniversity.csproj |
| **Source Framework** | .NET Framework 4.8 (`v4.8`) |
| **Target Framework** | .NET 10.0 (`net10.0`) |
| **Project Type** | ASP.NET MVC 5 (non-SDK-style .csproj) |

## Upgrade Scope

The migration from .NET Framework 4.8 to .NET 10.0 involves the following major areas:

### 1. SDK-Style Project File Conversion
The project uses the legacy non-SDK `.csproj` format. It must be converted to the modern SDK-style format (`<Project Sdk="Microsoft.NET.Sdk.Web">`), removing explicit file references and legacy MSBuild imports.

### 2. Target Framework Moniker Update
Change `<TargetFrameworkVersion>v4.8</TargetFrameworkVersion>` to `<TargetFramework>net10.0</TargetFramework>`.

### 3. ASP.NET MVC → ASP.NET Core MVC Migration
- Replace `System.Web.Mvc` (ASP.NET MVC 5) with ASP.NET Core MVC
- Replace `Global.asax` / `Application_Start` with `Program.cs` / `Startup.cs`
- Replace `Web.config` with `appsettings.json`
- Migrate route configuration, filters, and bundle configuration
- Update Razor views to ASP.NET Core Razor syntax

### 4. NuGet Package Updates
- Replace legacy `packages.config`-based packages with PackageReference format
- Upgrade/replace:
  - `Microsoft.AspNet.Mvc` → `Microsoft.AspNetCore.Mvc` (included in ASP.NET Core)
  - `Microsoft.EntityFrameworkCore` 3.1.x → latest compatible with .NET 10
  - `Newtonsoft.Json` → `System.Text.Json` (or update Newtonsoft.Json)
  - Remove packages not compatible with .NET 10 (WebGrease, Antlr, etc.)

### 5. API Compatibility Fixes
- Replace `System.Web` dependencies with ASP.NET Core equivalents
- Update `HttpContext`, `HttpRequest`, `HttpResponse` usage
- Replace `System.Messaging` with modern alternatives if needed
- Update `IISExpress` hosting model to Kestrel-based hosting

### 6. Entity Framework Core Updates
- Update EF Core from 3.1.x to the version compatible with .NET 10
- Update `DbContext` configuration to use the new dependency injection pattern
- Migrate connection string configuration to `appsettings.json`

## Tasks

A single upgrade task handles the full migration. See `.metadata/tasks.json` for details.
