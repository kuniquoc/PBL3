using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaNangTourism.Data.Migrations
{
    /// <inheritdoc />
    public partial class Schedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    scheduleID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    scheduleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    creationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    describe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false),
                    isPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.scheduleID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedule");
        }
    }
}
