using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaNangTourism.Data.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleDestination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
        name: "ScheduleDestination",
        columns: table => new
        {
            SDID = table.Column<string>(type: "nvarchar(450)", nullable: false),
            destinationID = table.Column<string>(type: "nvarchar(450)", nullable: false),
            scheduleID = table.Column<string>(type: "nvarchar(450)", nullable: false),
            arrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
            leaveTime = table.Column<DateTime>(type: "datetime2", nullable: false),
            costEstimate = table.Column<int>(type: "Int", nullable: false),
            note = table.Column<string>(type: "nvarchar(max)", nullable: false),
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_ScheduleDestination", x => x.SDID);
            table.ForeignKey(
                name: "FK_ScheduleDestination_Schedule",
                column: x => x.scheduleID,
                principalTable: "dbo.Schedule",
                principalColumn: "scheduleID",
                onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                name: "FK_ScheduleDestination_Destination",
                column: x => x.destinationID,
                principalTable: "dbo.Destination",
                principalColumn: "destinationID",
                onDelete: ReferentialAction.Cascade);
        });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleDestination");
        }
    }
}
