using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaNangTourism.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProcessingReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessingReport",
                columns: table => new
                {
                    prcReportID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    prcContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    processTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingReport", x => x.prcReportID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessingReport");
        }
    }
}
