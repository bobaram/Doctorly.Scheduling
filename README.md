# Doctor Practice Scheduling API

A .NET 8 Web API for managing practice calendars, appointments, and attendees.

## Technical Approach

### Architecture
The project follows a standard 4-layer Clean Architecture:
- **Domain:** Pure business logic. Entities use private setters and rich methods to maintain state integrity.
- **Application:** Use-case orchestration. I chose to use individual Command/Query handlers over a monolithic service to keep the logic decoupled and testable.
- **Infrastructure:** Persistence and external concerns. SQLite was selected for this assessment to ensure the project runs immediately without requiring a local database installation.
- **Api:** Entry point and configuration.

### Data Integrity & Concurrency
- **Optimistic Concurrency:** Since data preservation is critical, I implemented a `RowVersion` token on the `CalendarEvent` aggregate. If two users edit the same event, the second one will receive a `409 Conflict`.
- **Audit Compliance:** I added an EF Core `SaveChangesInterceptor` that automatically logs all inserts, updates, and deletes to the console. This provides a transparent audit trail without polluting the business logic.

### Design Decisions
- **Domain Events:** Side effects (like notifications) are triggered via internal domain events. This ensures that the primary transaction (saving the event) isn't tightly coupled to the notification logic.
- **Value Objects:** `TimeRange` is used to enforce the invariant that an appointment cannot end before it starts.
- **Testing:** I implemented a mix of Unit tests for core logic and an Integration test using `WebApplicationFactory` to verify the full HTTP-to-DB path.

## Requirements Coverage
- [x] **Create/Update/Delete:** Full CRUD for calendar events.
- [x] **Attendees:** Track names, emails, and attending status.
- [x] **List & Search:** Filter by date range or keyword search.
- [x] **Notifications:** Integrated via `INotificationService` and domain events.
- [x] **Accept/Reject:** Endpoint to update attendee status.
- [x] **Concurrency:** Conflict detection middleware.
- [x] **Audit:** Automatic change logging.

## Setup & Running

### Run the API
```bash
dotnet run --project Doctorly.Api
```
The API is configured to run on:
- **Swagger:** http://localhost:5555/swagger
- **HTTPS:** https://localhost:7777/swagger

### Run Tests
```bash
dotnet test
```

---
*Assumptions: I assumed UTC for all timestamps to avoid timezone complexity in this initial version.*
