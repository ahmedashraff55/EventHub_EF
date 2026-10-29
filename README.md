# EventHub — EF Core Console Application

This project implements the EventHub case study using Entity Framework Core and SQL Server.

## Entities
- **Organizer**: name, optional company name, verified status, profile, and events.
- **OrganizerProfile**: biography, optional website/logo URLs, and organizer relationship.
- **Event**: title, description, start date, optional end date, maximum attendees, and organizer.
- **Attendee**: full name, email, street, city, country, postal code, computed `FullAddress`, and optional badge.
- **Badge**: unique badge number, issue date, tier (`Standard` or `VIP`), and attendee.

## Requirements covered
- Organizer–OrganizerProfile: one-to-one.
- Organizer–Event: one-to-many.
- Attendee–Badge: one-to-one, enforced by a unique index on `Badge.AttendeeId`.
- Organizer and Attendee use Data Annotations for the requested validation/column rules.
- Organizer table name is `EventOrganizers`.
- Attendee postal code is stored in `ZipCode`.
- `FullAddress` is read-only and excluded from the database with `[NotMapped]`.
- OrganizerProfile and Badge are configured directly in `OnModelCreating`.
- Event is configured in a separate `IEntityTypeConfiguration<Event>` class.
- Sample data is inserted and counts/relationships are printed to verify the database.

## Run
Requires .NET 8 SDK and SQL Server LocalDB (or another SQL Server instance).

```bash
dotnet restore
dotnet run
```

Default connection string in `EventHubContext.cs`:
`Server=(localdb)\MSSQLLocalDB;Database=EventHubDb;Trusted_Connection=True;TrustServerCertificate=True;`

Change it if your SQL Server instance differs.

`EnsureCreated()` creates a fresh database and tables for this demo. It does not apply migrations or update an existing schema after model changes.
