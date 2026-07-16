# .NET Upgrade Assessment: ContosoUniversity → net10.0

**Source:** .NET Framework 4.8 (ASP.NET MVC 5)  
**Target:** .NET 10.0 (ASP.NET Core)  
**Assessment Date:** 2026-07-16

## Summary

| Severity | Count |
|----------|-------|
| Mandatory | 8 |
| Potential | 3 |
| Optional | 1 |
| **Total** | **12** |

## Mandatory Issues (Blockers)

| Issue | Effort (hrs) | Affected Files |
|-------|-------------|----------------|
| ASP.NET MVC 5 (System.Web) requires migration to ASP.NET Core MVC | 40 | 6 controllers |
| System.Web namespace not available in .NET 10 | 20 | CoursesController, StudentsController, Global.asax.cs |
| Global.asax application lifecycle must be migrated | 5 | Global.asax, Global.asax.cs |
| System.Configuration.ConfigurationManager not available | 5 | SchoolContextFactory, Global.asax.cs, NotificationService |
| System.Web.Optimization (bundling) not supported | 3 | BundleConfig.cs |
| System.Messaging (MSMQ) not available in .NET 10 | 13 | NotificationService.cs |
| Entity Framework Core 3.1 must be upgraded to EF Core 9+ | 8 | ContosoUniversity.csproj |
| Legacy .csproj format (non-SDK style) must be converted | 5 | ContosoUniversity.csproj |

## Potential Issues

| Issue | Effort (hrs) | Affected Files |
|-------|-------------|----------------|
| Razor views need migration from MVC 5 Razor to ASP.NET Core Razor | 13 | Views/**/*.cshtml |
| packages.config must be migrated to PackageReference | 3 | packages.config |
| Microsoft.Data.SqlClient upgrade required | 2 | packages.config |

## Optional Issues

| Issue | Effort (hrs) | Affected Files |
|-------|-------------|----------------|
| Newtonsoft.Json can be replaced with System.Text.Json | 5 | packages.config, NotificationService.cs |

## Estimated Total Migration Effort

| Category | Hours |
|----------|-------|
| Mandatory blockers | ~99 |
| Potential issues | ~18 |
| Optional improvements | ~5 |
| **Total** | **~122** |

## Recommended Migration Path

1. **Convert to SDK-style project** using the `sdk-style-conversion` scenario
2. **Migrate to ASP.NET Core** — rewrite controllers, views, and startup using ASP.NET Core 10 patterns
3. **Replace System.Web types** — `HttpPostedFileBase` → `IFormFile`, `Server.MapPath` → `IWebHostEnvironment.WebRootPath`, `HttpContext.Current` → injected `IHttpContextAccessor`
4. **Replace ConfigurationManager** with `IConfiguration` and `appsettings.json`
5. **Upgrade EF Core** from 3.1 to 9.0+ — update DbContext, migrations, and provider packages
6. **Replace MSMQ** with Azure Service Bus or Azure Queue Storage
7. **Migrate bundles** to libman or npm for static assets
8. **Update packages.config** to PackageReference format
