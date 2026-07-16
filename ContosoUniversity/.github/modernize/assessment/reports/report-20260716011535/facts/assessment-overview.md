# Assessment Overview

This directory contains supplementary architectural analysis documents generated as part of the application assessment for **ContosoUniversity**. These documents provide deep insight into the application's structure, dependencies, data model, configuration, and business workflows to support cloud migration planning.

## Supplementary Documents

1. **[Architecture Diagram](architecture-diagram.md)** — High-level application architecture and component relationship diagrams showing layers, data flow, and technology stack.

2. **[Dependency Map](dependency-map.md)** — Visual map of all external NuGet dependencies grouped by functional category, with version and compatibility risk analysis.

3. **[API & Service Contracts](api-service-contracts.md)** — Inventory of all MVC action endpoints, JSON API endpoints, communication patterns, and security posture.

4. **[Data Architecture](data-architecture.md)** — Entity model (ER diagram), database configuration, repository access patterns, caching strategy, and data sensitivity classification.

5. **[Configuration Inventory](configuration-inventory.md)** — Comprehensive inventory of all configuration sources, build profiles, runtime properties, secrets workflow, and framework versions.

6. **[Business Workflows](business-workflows.md)** — End-to-end business processes, domain entity descriptions, validation rules, and decision logic documentation.

## Key Findings Summary

- **Legacy stack**: ASP.NET MVC 5 on .NET Framework 4.8 — requires full migration to ASP.NET Core
- **Windows-only dependencies**: MSMQ (`System.Messaging`) prevents containerization on Linux
- **No authentication**: All endpoints are publicly accessible — must be addressed before cloud deployment
- **No cloud configuration**: All config in `Web.config` plaintext — must migrate to environment variables / Key Vault
- **PII unprotected**: Student names, grades, and enrollment dates stored without encryption or masking
- **EF Core 3.1 EOL**: Entity Framework Core 3.1 reached end-of-life December 2022
