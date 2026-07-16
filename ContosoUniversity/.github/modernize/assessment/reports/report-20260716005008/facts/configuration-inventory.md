# Configuration & Externalized Settings Inventory

This inventory summarizes configuration sources and externalized settings for ContosoUniversity, including web configuration, build/runtime settings, and sensitive configuration references.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web.config | XML app/runtime config | `ContosoUniversity/Web.config` | Primary runtime config including connection string, app settings, and assembly bindings |
| packages.config | NuGet dependency config | `ContosoUniversity/packages.config` | Declared package dependencies and versions |
| ContosoUniversity.csproj | MSBuild project config | `ContosoUniversity/ContosoUniversity.csproj` | Build targets, references, and framework target |
| Global.asax.cs | Startup initialization config in code | `ContosoUniversity/Global.asax.cs` | Startup registration and database initialization call |
| RouteConfig.cs | Routing config in code | `ContosoUniversity/App_Start/RouteConfig.cs` | Defines default route template |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | Default local development build | Includes debug symbols and development behavior | `compilation debug=true`, IIS Express enabled |
| Release | Manual or CI build configuration | Optimized production build | Standard MSBuild Release pipeline |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default | IIS/IIS Express application startup | `Web.config` | Uses `DefaultConnection`, queue path, upload/request limits |

## Properties Inventory

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `connectionStrings:DefaultConnection` | `Data Source=(LocalDb)\\MSSQLLocalDB;...` | Default | `Web.config` |
| `webpages:Version` | `3.0.0.0` | Default | `Web.config` |
| `webpages:Enabled` | `false` | Default | `Web.config` |
| `ClientValidationEnabled` | `true` | Default | `Web.config` |
| `UnobtrusiveJavaScriptEnabled` | `true` | Default | `Web.config` |
| `NotificationQueuePath` | `.\\Private$\\ContosoUniversityNotifications` | Default | `Web.config` |
| `httpRuntime.maxRequestLength` | `10240` | Default | `Web.config` |
| `requestLimits.maxAllowedContentLength` | `10485760` | Default | `Web.config` |
| `httpRuntime.executionTimeout` | `3600` | Default | `Web.config` |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| ContosoUniversity.Web | .NET Framework 4.8 in IIS/IIS Express | Not explicitly configured in repository | Not specified |

## Startup Dependency Chain

1. IIS/IIS Express starts ContosoUniversity web application.
2. `Application_Start` registers MVC routes, filters, and bundles.
3. Application initializes `SchoolContext` using `DefaultConnection`.
4. `DbInitializer.Initialize` ensures database creation and seed data.
5. Controllers instantiate `NotificationService`, which creates/opens MSMQ queue on first use.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `connectionStrings:DefaultConnection` | Database connection string | `Web.config` (integrated security; no password literal) |
| `NotificationQueuePath` | Infrastructure endpoint path | `Web.config` |

### Secrets Provisioning Workflow

Configuration is read from local `Web.config` at runtime using `ConfigurationManager`. Database connectivity currently relies on integrated security and local machine SQL Server LocalDB. No external secret manager integration (Key Vault, Vault, or cloud secret injection pipeline) is configured in repository files.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None detected | N/A | N/A |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| .NET Framework | 4.8 | `ContosoUniversity.csproj` target framework |
| ASP.NET MVC | 5.2.9 | `packages.config` |
| Razor/WebPages | 3.2.9 | `packages.config` |
| Entity Framework Core | 3.1.32 | `packages.config` |
| Microsoft.Data.SqlClient | 2.1.4 | `packages.config` |
| Newtonsoft.Json | 13.0.3 | `packages.config` |
| jQuery | 3.7.1 | `packages.config` |
| Bootstrap | 5.3.3 | `packages.config` |
