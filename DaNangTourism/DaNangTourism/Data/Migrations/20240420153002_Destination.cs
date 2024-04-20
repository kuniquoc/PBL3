using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaNangTourism.Data.Migrations
{
    /// <inheritdoc />
    public partial class Destination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Destination",
                columns: table => new
                {
                    destinationID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    describe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _openTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _closeTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _openDayTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _imgURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _rating = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destination", x => x.destinationID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Destination");
        }
    }
}
