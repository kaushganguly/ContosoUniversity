# Dependency Map

This document summarizes declared external dependencies for ContosoUniversity from `packages.config` and project references, grouped by functional category.

## Dependencies

```mermaid
flowchart LR
    App["ContosoUniversity"]

    subgraph Web["Web Frameworks"]
        AspMvc["Microsoft.AspNet.Mvc 5.2.9"]
        Razor["Microsoft.AspNet.Razor 3.2.9"]
        WebPages["Microsoft.AspNet.WebPages 3.2.9"]
        WebOpt["Microsoft.AspNet.Web.Optimization 1.1.3"]
        Bootstrap["bootstrap 5.3.3"]
        JQuery["jQuery 3.7.1"]
    end

    subgraph Db["Database / ORM"]
        EFCore["Microsoft.EntityFrameworkCore 3.1.32"]
        EFSql["Microsoft.EntityFrameworkCore.SqlServer 3.1.32"]
        SqlClient["Microsoft.Data.SqlClient 2.1.4"]
    end

    subgraph Messaging
        MsmqRef["System.Messaging assembly"]
    end

    subgraph Cache["Caching"]
        MemCache["Microsoft.Extensions.Caching.Memory 3.1.32"]
    end

    subgraph Log["Logging"]
        ExtLog["Microsoft.Extensions.Logging 3.1.32"]
    end

    subgraph Sec["Security"]
        Msal["Microsoft.Identity.Client 4.21.1"]
    end

    subgraph Util["Utilities"]
        JsonNet["Newtonsoft.Json 13.0.3"]
        Compiler["Microsoft.CodeDom.Providers.DotNetCompilerPlatform 2.0.1"]
        WebGrease["WebGrease 1.5.2"]
        Antlr["Antlr 3.4.1"]
    end

    App -->|"web"| Web
    App -->|"data"| Db
    App -->|"messaging"| Messaging
    App -->|"cache"| Cache
    App -->|"logging"| Log
    App -->|"security"| Sec
    App -->|"utilities"| Util
```

### Dependency Summary

| Category | Count | Key Libraries | Notes |
|---|---:|---|---|
| Web Frameworks | 6 | Microsoft.AspNet.Mvc, Razor, WebPages, bootstrap | Legacy ASP.NET MVC 5 server-rendered stack |
| Database / ORM | 3 | EF Core, EF Core SqlServer, SqlClient | EF Core 3.1 on .NET Framework 4.8 |
| Messaging | 1 | System.Messaging | MSMQ dependency requires Windows messaging infrastructure |
| Caching | 1 | Microsoft.Extensions.Caching.Memory | In-memory cache primitives available |
| Logging | 1 | Microsoft.Extensions.Logging | Basic logging abstractions present |
| Security | 1 | Microsoft.Identity.Client | Identity client package present |
| Utilities | 4 | Newtonsoft.Json, CodeDom provider, WebGrease, Antlr | Supporting runtime and build-time libraries |

### Version & Compatibility Risks

The project targets .NET Framework 4.8 and depends on ASP.NET MVC 5 and EF Core 3.1, which increases migration effort for newer .NET target frameworks. `System.Messaging` is Windows-specific and is a common blocker for Linux/container modernization scenarios.

### Notable Observations

- `packages.config` package management is still used instead of modern `PackageReference`.
- Both legacy ASP.NET MVC packages and newer `Microsoft.Extensions.*` packages coexist in the same web application.
- Front-end libraries are tied to static package versions rather than modern build pipeline dependency management.
- Messaging integration uses MSMQ, indicating infrastructure coupling to Windows-hosted environments.

## Test Dependencies

| Framework | Version | Notes |
|---|---|---|
| None detected | N/A | No test-scoped packages found in `packages.config` |

Total test-scope dependencies: 0

No test dependencies were detected from declared package metadata in this project.
