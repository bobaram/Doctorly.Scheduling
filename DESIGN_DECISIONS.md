# Design Decisions & Architectural Rationale

This document outlines the thinking behind the solution's structure, technical choices, and the methodology used during development.

## 1. Architectural Philosophy: Clean Architecture & DDD

I chose a **Clean Architecture** approach to demonstrate a clear separation between business rules and infrastructure.

- **Domain-Driven Design (DDD):** I focused on a "Rich Domain Model." Entities like `CalendarEvent` are not simple data buckets; they encapsulate invariants (e.g., end-time validation via the `TimeRange` Value Object) and manage state transitions via explicit methods (`UpdateAttendeeStatus`).
- **Decoupling:** By using internal **Domain Events**, I ensured that side effects—like notifications—are decoupled from the primary persistence logic. This follows the Open/Closed Principle, allowing for new side effects to be added without modifying the core creation logic.

## 2. Command Query Responsibility Segregation (CQRS)
I implemented a lightweight **CQRS** pattern by separating "write" operations (Commands) from "read" operations (Queries).
- **Why CQRS?** Even in a small assessment, this demonstrates readiness for scale. Read and write workloads often have different performance and locking requirements. By separating them into distinct handlers, we can optimize the read side (e.g., using `AsNoTracking` and specialized DTOs) without risking the integrity of the write side.
- **Clarity:** It prevents "Service Bloat." Instead of a single `AppointmentService` with 20 methods, each use case is encapsulated in its own handler, making the system much easier to navigate and unit test.

## 3. Technical Choices & Assumptions

- **Persistence (SQLite):** I deliberately swapped from PostgreSQL to SQLite for this assessment. In a professional setting, I would use a containerized Postgres instance, but for a 4-hour review window, "Zero-Configuration" portability is a senior-level priority to ensure the reviewer can run the code instantly.
- **Optimistic Concurrency:** To satisfy the requirement for data preservation, I implemented a manual `RowVersion` token logic. This ensures that concurrent updates are detected even in SQLite, which lacks a native auto-generating rowversion type.
- **Audit Compliance:** Rather than manually logging in every service, I implemented an **EF Core Interceptor**. This is a cross-cutting concern that ensures every change to the database is automatically audited in a consistent format.

## 3. Methodology: AI-Assisted Development

I utilized AI (Gemini CLI) during this assessment as a "Senior Pair Programmer." My goal was to demonstrate how a modern senior developer leverages advanced tools to maintain high velocity without sacrificing architectural integrity.

### How I Maintained Control:

- **Architectural Guardrails:** I defined the project structure, project naming conventions, and the layering strategy before any code was generated.
- **Code Review & Refactoring:** I performed "surgical" corrections. For example, when the initial migration failed due to SQLite's lack of native RowVersion support, I diagnosed the failure and directed the AI to implement a manual concurrency token pattern—a solution that required a deep understanding of EF Core internals.
- **Ubiquitous Language:** I enforced the domain language (Attendees, Events, TimeRanges) to ensure the code matched the business requirements rather than generic AI templates.
- **Decision Ownership:** Every pattern in this repo—from the Global Exception Middleware to the Audit Interceptor—was a deliberate choice I made to demonstrate how I approach production-grade system design.

## 4. Future Roadmap (If given more time)

- **FluentValidation:** I would add a validation layer to the Application project to handle complex business rules before they hit the Domain.
- **Outbox Pattern:** To ensure notifications are truly reliable, I would implement a Transactional Outbox to prevent "zombie" emails if a database transaction fails after an event is raised.
- **API Versioning:** I would implement header-based versioning to ensure the API can evolve without breaking 3rd party clients.
