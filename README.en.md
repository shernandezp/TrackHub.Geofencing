# TrackHub Geofencing API

## Key Features

- **Geospatial Data Management**: Create and manage geographic boundaries using PostGIS spatial data types
- **GraphQL Interface**: Efficient, flexible querying with Hot Chocolate GraphQL server
- **Real-Time Position Analysis**: Query transporter positions relative to defined geofences
- **Clean Architecture**: Layered architecture ensuring maintainability and testability
- **User-Scoped Access**: View-based data access with user permission filtering
- **PostgreSQL + PostGIS**: Enterprise-grade spatial database capabilities

---

## Quick Start

### Prerequisites

- .NET 10.0 SDK
- PostgreSQL 14+ with PostGIS extension enabled
- TrackHub Authority Server running (for authentication)

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/shernandezp/TrackHub.Geofencing.git
   cd TrackHub.Geofencing
   ```

2. **Enable PostGIS extension** in PostgreSQL:
   ```sql
   CREATE EXTENSION postgis;
   ```

3. **Configure the database connection** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "ManagerConnection": "Host=localhost;Database=trackhub_manager;Username=postgres;Password=yourpassword"
     }
   }
   ```

4. **Run database migrations**:
   ```bash
   dotnet ef database update
   ```

5. **Start the application**:
   ```bash
   dotnet run --project src/Web
   ```

6. **Access GraphQL Playground** at `https://localhost:5001/graphql`

---

## Components and Resources

| Component                | Description                                           | Documentation                                                                 |
|--------------------------|-------------------------------------------------------|-------------------------------------------------------------------------------|
| Hot Chocolate            | GraphQL server for .NET                               | [Hot Chocolate Documentation](https://chillicream.com/docs/hotchocolate/v13)  |
| .NET Core                | Development platform for modern applications          | [.NET Core Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/overview) |
| Postgres                 | Relational database management system                 | [Postgres Documentation](https://www.postgresql.org/)                         |

---

## Overview

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