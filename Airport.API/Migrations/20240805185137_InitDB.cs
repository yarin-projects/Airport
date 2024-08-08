using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


namespace Airport.API.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        private static readonly string[] columns = ["LegId", "CrossingTime", "FlightId", "LegType"];
        private static readonly string[] columnsArray = ["LegId", "NextLegId"];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Number = table.Column<Guid>(type: "TEXT", nullable: false),
                    PassengersCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: true),
                    FlightStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDone = table.Column<bool>(type: "INTEGER", nullable: false),
                    TimeCompleted = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LegId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightId);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LegId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlightNumber = table.Column<string>(type: "TEXT", nullable: true),
                    IsWaitingList = table.Column<bool>(type: "INTEGER", nullable: false),
                    TimeEntered = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    TimeExit = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "WaitingFlights",
                columns: table => new
                {
                    WaitingFlightId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false),
                    LegIdToEnter = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<Guid>(type: "TEXT", nullable: false),
                    PassengersCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: true),
                    FlightStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitingFlights", x => x.WaitingFlightId);
                });

            migrationBuilder.CreateTable(
                name: "Legs",
                columns: table => new
                {
                    LegId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LegType = table.Column<int>(type: "INTEGER", nullable: false),
                    CrossingTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: true),
                    RowVersion = table.Column<string>(type: "TEXT", rowVersion: true, nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legs", x => x.LegId);
                    table.ForeignKey(
                        name: "FK_Legs_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "FlightId");
                });

            migrationBuilder.CreateTable(
                name: "LegConnections",
                columns: table => new
                {
                    LegId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextLegId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegConnections", x => new { x.LegId, x.NextLegId });
                    table.ForeignKey(
                        name: "FK_LegConnections_Legs_LegId",
                        column: x => x.LegId,
                        principalTable: "Legs",
                        principalColumn: "LegId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LegConnections_Legs_NextLegId",
                        column: x => x.NextLegId,
                        principalTable: "Legs",
                        principalColumn: "LegId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Legs",
                columns: columns,
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 0, 7, 0), null, 1 },
                    { 2, new TimeSpan(0, 0, 0, 3, 0), null, 1 },
                    { 3, new TimeSpan(0, 0, 0, 10, 0), null, 1 },
                    { 4, new TimeSpan(0, 0, 0, 12, 0), null, 3 },
                    { 5, new TimeSpan(0, 0, 0, 9, 0), null, 1 },
                    { 6, new TimeSpan(0, 0, 0, 5, 0), null, 3 },
                    { 7, new TimeSpan(0, 0, 0, 11, 0), null, 3 },
                    { 8, new TimeSpan(0, 0, 0, 8, 0), null, 2 },
                    { 9, new TimeSpan(0, 0, 0, 6, 0), null, 2 }
                });

            migrationBuilder.InsertData(
                table: "LegConnections",
                columns: columnsArray,
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 3 },
                    { 3, 4 },
                    { 4, 5 },
                    { 4, 9 },
                    { 5, 6 },
                    { 5, 7 },
                    { 6, 8 },
                    { 7, 8 },
                    { 8, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegConnections_NextLegId",
                table: "LegConnections",
                column: "NextLegId");

            migrationBuilder.CreateIndex(
                name: "IX_Legs_FlightId",
                table: "Legs",
                column: "FlightId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegConnections");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "WaitingFlights");

            migrationBuilder.DropTable(
                name: "Legs");

            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
