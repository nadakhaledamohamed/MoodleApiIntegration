using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodleApiIntegration.Migrations
{
    /// <inheritdoc />
    public partial class intialialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_master_Jobs",
                table: "master_Jobs");

            migrationBuilder.RenameTable(
                name: "master_Jobs",
                newName: "Master_Jobs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Master_Jobs",
                table: "Master_Jobs",
                column: "Master_ID");

            migrationBuilder.CreateTable(
                name: "onHoldStudentDtos",
                columns: table => new
                {
                    StopListID = table.Column<int>(type: "int", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    ReasonID = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClearedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    IS_SUCCESS = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "onHoldStudentDtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Master_Jobs",
                table: "Master_Jobs");

            migrationBuilder.RenameTable(
                name: "Master_Jobs",
                newName: "master_Jobs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_master_Jobs",
                table: "master_Jobs",
                column: "Master_ID");
        }
    }
}
