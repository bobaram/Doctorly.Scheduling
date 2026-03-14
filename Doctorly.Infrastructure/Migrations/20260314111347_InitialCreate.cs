using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctorly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "calendar_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    duration_start = table.Column<DateTime>(type: "TEXT", nullable: false),
                    duration_end = table.Column<DateTime>(type: "TEXT", nullable: false),
                    row_version = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_calendar_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "attendees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    is_attending = table.Column<bool>(type: "INTEGER", nullable: false),
                    calendar_event_id = table.Column<Guid>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attendees", x => x.id);
                    table.ForeignKey(
                        name: "fk_attendees_calendar_events_calendar_event_id",
                        column: x => x.calendar_event_id,
                        principalTable: "calendar_events",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_attendees_calendar_event_id",
                table: "attendees",
                column: "calendar_event_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attendees");

            migrationBuilder.DropTable(
                name: "calendar_events");
        }
    }
}
