using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackHub.Manager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GeofenceEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "geofenceevents",
                schema: "geofencing",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    transporterid = table.Column<Guid>(type: "uuid", nullable: false),
                    geofenceid = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    departuretimestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_geofenceevents", x => x.id);
                    table.ForeignKey(
                        name: "FK_geofenceevents_geofences_geofenceid",
                        column: x => x.geofenceid,
                        principalSchema: "geofencing",
                        principalTable: "geofences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_geofenceevent_geofenceid",
                schema: "geofencing",
                table: "geofenceevents",
                column: "geofenceid");

            migrationBuilder.CreateIndex(
                name: "ix_geofenceevent_open_events",
                schema: "geofencing",
                table: "geofenceevents",
                columns: new[] { "transporterid", "geofenceid", "departuretimestamp" },
                filter: "departuretimestamp IS NULL");

            migrationBuilder.CreateIndex(
                name: "ix_geofenceevent_transporterid",
                schema: "geofencing",
                table: "geofenceevents",
                column: "transporterid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "geofenceevents",
                schema: "geofencing");
        }
    }
}
