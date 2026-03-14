# Doctorly Scheduling API

A Senior-level .NET technical assessment implementing a Doctor Practice Scheduling System.

## Architecture & Design Choices

### 1. Clean Architecture
The solution is organized into four distinct layers to ensure separation of concerns and testability:
- **Domain:** Core business logic, entities, and value objects. No external dependencies.
- **Application:** Use cases (Commands/Queries) and DTOs. Orchestrates domain logic.
- **Infrastructure:** Persistence (EF Core/SQLite) and Messaging stubs.
- **API:** HTTP entry points and configuration.

### 2. Domain-Driven Design (DDD)
- **Aggregate Roots:** `CalendarEvent` acts as the root, protecting its internal `Attendees` collection.
- **Value Objects:** `TimeRange` ensures invariant logic (e.g., start time < end time).
- **Domain Events:** Side effects like notifications are decoupled using a domain event pattern (`CalendarEventCreated`, `AttendeeStatusChanged`).
- **Encapsulation:** Entities use private setters and rich domain methods to ensure state integrity.

### 3. Key Technical Features
- **Optimistic Concurrency:** Implemented using a manual `RowVersion` token (compatible with SQLite) to handle concurrent updates ("Could" requirement).
- **SQLite Persistence:** Used for zero-configuration portability, ensuring the solution runs immediately upon cloning.
- **OpenAPI/Swagger:** Fully documented API endpoints with typed responses for auto-generated client support.
- **Global Error Handling:** Middleware to handle concurrency conflicts (`DbUpdateConcurrencyException`) and provide clean `409 Conflict` responses.

## Requirements Coverage

| Requirement | Status | Implementation Detail |
|---|---|---|
| Create/Update/Delete Event | ✅ Done | Full CRUD support in Application layer. |
| Attendee Management | ✅ Done | Name, Email, and Attending status included. |
| Notifications | ✅ Done | `INotificationService` abstraction with Domain Event integration. |
| List & Search | ✅ Done | Filters by date-range and keyword search. |
| Concurrency | ✅ Done | Optimistic locking with middleware conflict resolution. |
| Testing (Unit) | ✅ Done | xUnit + NSubstitute tests for Domain and Application layers. |
| Testing (Integration) | ✅ Done | Full API-to-DB testing using `WebApplicationFactory`. |
| Accept/Reject | ✅ Done | `UpdateAttendeeStatus` rich domain method. |

## Getting Started

### Prerequisites
- .NET 8 SDK

### Running the API
1. Navigate to the API project and run:
   ```bash
   dotnet run --project Doctorly.Api
   ```
2. The API will be available at:
   - **Swagger UI:** `http://localhost:5555/swagger`
   - **API Endpoints:** `http://localhost:5555/api/events`

### Running Tests
```bash
dotnet test
```

## Future Improvements (Out of Scope for 4h)
- **Validation:** Integrate `FluentValidation` for stricter API request validation.
- **Integration Events:** Move from internal domain events to a message broker (RabbitMQ/Azure Service Bus) for distributed notifications.
- **Audit Logging:** Implement a generic interceptor to track all changes to event data.
- **Availability Check:** Add logic to prevent overlapping appointments for the same doctor/resource.

---
*Developed as part of a technical assessment for Doctorly GmbH.*
