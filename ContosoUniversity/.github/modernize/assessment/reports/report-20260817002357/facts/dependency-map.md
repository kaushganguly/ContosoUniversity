# Dependency Map

ContosoUniversity declares 45 NuGet package dependencies in `packages.config`, plus framework assembly references in the legacy project file. Runtime dependencies center on ASP.NET MVC, EF Core, SQL Server connectivity, client UI assets, and JSON serialization.

## Dependencies

```mermaid
flowchart LR
    App["ContosoUniversity"]

    subgraph Web["Web Frameworks"]
        Mvc["Microsoft.AspNet.Mvc 5.2.9"]
        Razor["Microsoft.AspNet.Razor 3.2.9"]
        WebPages["Microsoft.AspNet.WebPages 3.2.9"]
        Optimization["Web Optimization 1.1.3"]
    end
    subgraph Db["Database / ORM"]
        EfCore["EntityFrameworkCore 3.1.32"]
        EfRel["EF Core Relational 3.1.32"]
        EfSql["EF Core SqlServer 3.1.32"]
        SqlClient["Microsoft.Data.SqlClient 2.1.4"]
    end
    subgraph Messaging["Messaging"]
        Msmq["System.Messaging framework assembly"]
    end
    subgraph Cache["Caching"]
        CacheAbs["Extensions Caching Abstractions 3.1.32"]
        CacheMem["Extensions Caching Memory 3.1.32"]
    end
    subgraph Log["Logging"]
        LogExt["Extensions Logging 3.1.32"]
        Diagnostics["DiagnosticSource 4.7.1"]
    end
    subgraph Sec["Security"]
        Msal["Microsoft.Identity.Client 4.21.1"]
    end
    subgraph Client["Client UI"]
        Bootstrap["bootstrap 5.3.3"]
        JQuery["jQuery 3.7.1"]
        JQueryVal["jQuery.Validation 1.21.0"]
        Unobtrusive["Unobtrusive Validation 4.0.0"]
        Modernizr["Modernizr 2.6.2"]
    end
    subgraph Util["Utilities"]
        Newtonsoft["Newtonsoft.Json 13.0.3"]
        DI["Extensions DependencyInjection 3.1.32"]
        Config["Extensions Configuration 3.1.32"]
        Compiler["CodeDom Providers 2.0.1"]
        NetStd["NETStandard.Library 2.0.3"]
        RuntimeUtils["System runtime support packages"]
    end

    App -->|"web"| Web
    App -->|"persistence"| Db
    App -->|"messaging"| Messaging
    App -->|"caching"| Cache
    App -->|"logging"| Log
    App -->|"security library"| Sec
    App -->|"client assets"| Client
    App -->|"utilities"| Util
    EfSql -.->|"uses"| SqlClient
    Mvc -.->|"views"| Razor
    Mvc -.->|"helpers"| WebPages
    Optimization -.->|"minification"| WebGrease["WebGrease 1.5.2"]
```

### Dependency Summary

| Category | Count | Key Libraries | Notes |
|---|---:|---|---|
| Web Frameworks | 5 | ASP.NET MVC 5.2.9, Razor 3.2.9, WebPages 3.2.9 | Legacy ASP.NET MVC stack for .NET Framework |
| Database / ORM | 7 | EF Core 3.1.32, EF Core SqlServer 3.1.32, Microsoft.Data.SqlClient 2.1.4 | EF Core 3.1 is out of support and SQL Server-specific |
| Messaging | 1 | System.Messaging | Framework assembly for MSMQ private queues |
| Caching | 2 | Microsoft.Extensions.Caching.Abstractions, Memory | Referenced but no active cache usage detected in source |
| Logging | 2 | Microsoft.Extensions.Logging, DiagnosticSource | Framework logging packages referenced; source uses Trace and Debug directly |
| Security | 1 | Microsoft.Identity.Client 4.21.1 | Dependency is declared, but MVC authorization is disabled in global filters |
| Client UI | 5 | Bootstrap, jQuery, jQuery Validation, Unobtrusive Validation, Modernizr | Supports Razor form validation and UI styling |
| Utilities | 22 | Newtonsoft.Json, DI, Configuration, CodeDom, NETStandard, runtime support packages | Mostly compatibility and runtime support dependencies |

### Version & Compatibility Risks

The project targets .NET Framework 4.8 and uses ASP.NET MVC 5, both of which require a migration path to ASP.NET Core for modern .NET targets. EF Core 3.1 and several Microsoft.Extensions 3.1 packages are out of support, while `System.Messaging` depends on MSMQ and is not supported cross-platform in modern .NET.

### Notable Observations

- The project mixes legacy ASP.NET MVC 5 with EF Core 3.1 packages, creating migration concerns for both the web and data layers.
- `Microsoft.Identity.Client` is declared but no active authentication or authorization enforcement was detected in the MVC configuration.
- Client-side packages are restored through NuGet rather than npm, reflecting an older ASP.NET asset-management model.
- No test-scoped NuGet dependencies are declared.

## Test Dependencies

| Framework | Version | Notes |
|---|---:|---|
| None detected | N/A | No unit test, integration test, or mocking packages are declared in `packages.config`. |

Total test-scope dependencies: 0
No test dependencies detected. This limits automated validation for modernization changes.
