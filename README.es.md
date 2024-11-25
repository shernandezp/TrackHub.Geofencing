﻿## Componentes y Recursos Utilizados

| Componente                | Descripción                                             | Documentación                                                                 |
|---------------------------|---------------------------------------------------------|-------------------------------------------------------------------------------|
| Hot Chocolate             | Servidor GraphQL para .Net        | [Documentación Hot Chocolate](https://chillicream.com/docs/hotchocolate/v13)                           |
| .NET Core                 | Development platform for modern applications          | [.NET Core Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/overview) |
| Postgres                  | Sistema de gestión de bases de datos relacional         | [Documentación Postgres](https://www.postgresql.org/)                         |

---

## API de Geocercado de TrackHub

La **API de Geofencing de TrackHub** proporciona servicios para gestionar las características de geocercado de TrackHub. Sigue los principios de la **Arquitectura Limpia** del proyecto, aprovechando **GraphQL** para las interacciones de la API y **Postgres** para la gestión de la base de datos.

### Características Principales

La API ofrece las siguientes funcionalidades:
- Gestión de límites geográficos, conocidos como geocercas.
- Generación de informes basados en datos de geocercas.

---

## Entidades

### Geofencing

- **Geofence**: Representa las geocercas utilizando la extensión PostGIS de Postgres para manejar datos espaciales.
- **VwUser**: Una vista de la tabla de usuarios dentro del esquema de geofencing, que ofrece una interfaz simplificada para acceder a datos relacionados con usuarios.
- **VwTransporterPosition**: Una vista que combina datos de transportadores con sus respectivas posiciones en formato geométrico para facilitar consultas geoespaciales.

---

### ¿Por qué GraphQL?

El uso de **GraphQL** permite consultas eficientes y personalizables, permitiendo a los clientes solicitar solo los datos que necesitan para minimizar el ancho de banda y mejorar el rendimiento de la aplicación. Con GraphQL, las aplicaciones pueden recuperar detalles específicos sobre usuarios, transportadores o dispositivos, optimizando tanto la eficiencia operativa como la experiencia del usuario.

## Licencia

Este proyecto está bajo la Licencia Apache 2.0. Consulta el archivo [LICENSE](https://www.apache.org/licenses/LICENSE-2.0) para más información.