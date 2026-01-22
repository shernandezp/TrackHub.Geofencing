using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGeofenceEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timestamp",
                schema: "geofencing",
                table: "geofenceevents",
                newName: "datetime");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "departureoffset",
                schema: "geofencing",
                table: "geofenceevents",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "offset",
                schema: "geofencing",
                table: "geofenceevents",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "departureoffset",
                schema: "geofencing",
                table: "geofenceevents");

            migrationBuilder.DropColumn(
                name: "offset",
                schema: "geofencing",
                table: "geofenceevents");

            migrationBuilder.RenameColumn(
                name: "datetime",
                schema: "geofencing",
                table: "geofenceevents",
                newName: "timestamp");
        }
    }
}
