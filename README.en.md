## Components and Resources

| Component                | Description                                           | Documentation                                                                 |
|--------------------------|-------------------------------------------------------|-------------------------------------------------------------------------------|
| Hot Chocolate            | GraphQL server for .NET                               | [Hot Chocolate Documentation](https://chillicream.com/docs/hotchocolate/v13)  |
| .NET Core                | Development platform for modern applications          | [.NET Core Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/overview) |
| Postgres                 | Relational database management system                 | [Documentación Postgres](https://www.postgresql.org/)                         |

---

## API de Geofencing de TrackHub

The **TrackHub Geofencing API** provides services for managing the geofencing features of TrackHub. It adheres to the project's **Clean Architecture** principles, leveraging **GraphQL** for API interactions and **Postgres** for database management.

### Key Features

The API offers the following functionalities:
- Management of geographic boundaries, also known as geofences.
- Generating reports based on geofencing data.

---

## Entities

### Geofencing

- **Geofence**: Represents geographical boundaries using Postgres' PostGIS extension to handle spatial data.
- **VwUser**: A view of the user table within the geofencing schema, offering a simplified interface for accessing user-related data.
- **VwTransporterPosition**: A view that combines transporter data with their respective positions in geometry format for easy geospatial queries.

---


### Why GraphQL?

The use of **GraphQL** enables efficient, customizable queries, letting clients request only the data they need to minimize bandwidth and enhance app performance. With GraphQL, applications can retrieve specific details about users, transporters, or devices, optimizing both operational efficiency and user experience.

## License

This project is licensed under the Apache 2.0 License. See the [LICENSE file](https://www.apache.org/licenses/LICENSE-2.0) for more information.