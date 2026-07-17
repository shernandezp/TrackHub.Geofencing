using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace TrackHub.Geofencing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GeofenceAlertsCirclesAndEventAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "alertonentry",
                schema: "geofencing",
                table: "geofences",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "alertonexit",
                schema: "geofencing",
                table: "geofences",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Point>(
                name: "circlecenter",
                schema: "geofencing",
                table: "geofences",
                type: "geometry (Point, 4326)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "circleradiusmeters",
                schema: "geofencing",
                table: "geofences",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "dwellthresholdminutes",
                schema: "geofencing",
                table: "geofences",
                type: "integer",
                nullable: true);

            // AccountId lands nullable, is backfilled from the owning geofence in one bounded
            // UPDATE ... FROM join, and only then becomes NOT NULL (spec 08 §15).
            migrationBuilder.AddColumn<Guid>(
                name: "accountid",
                schema: "geofencing",
                table: "geofenceevents",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE geofencing.geofenceevents e
                SET accountid = g.accountid
                FROM geofencing.geofences g
                WHERE g.id = e.geofenceid AND e.accountid IS NULL;
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "accountid",
                schema: "geofencing",
                table: "geofenceevents",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "dwellalertedat",
                schema: "geofencing",
                table: "geofenceevents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_geofenceevent_accountid_datetime",
                schema: "geofencing",
                table: "geofenceevents",
                columns: new[] { "accountid", "datetime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_geofenceevent_accountid_datetime",
                schema: "geofencing",
                table: "geofenceevents");

            migrationBuilder.DropColumn(
                name: "alertonentry",
                schema: "geofencing",
                table: "geofences");

            migrationBuilder.DropColumn(
                name: "alertonexit",
                schema: "geofencing",
                table: "geofences");

            migrationBuilder.DropColumn(
                name: "circlecenter",
                schema: "geofencing",
                table: "geofences");

            migrationBuilder.DropColumn(
                name: "circleradiusmeters",
                schema: "geofencing",
                table: "geofences");

            migrationBuilder.DropColumn(
                name: "dwellthresholdminutes",
                schema: "geofencing",
                table: "geofences");

            migrationBuilder.DropColumn(
                name: "accountid",
                schema: "geofencing",
                table: "geofenceevents");

            migrationBuilder.DropColumn(
                name: "dwellalertedat",
                schema: "geofencing",
                table: "geofenceevents");
        }
    }
}
