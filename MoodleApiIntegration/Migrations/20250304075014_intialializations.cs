using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodleApiIntegration.Migrations
{
    /// <inheritdoc />
    public partial class intialializations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "job_Logs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Master_ID = table.Column<int>(type: "int", nullable: false),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    exception_message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IS_SUCCESS = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_Logs", x => x.LogID);
                });

            migrationBuilder.CreateTable(
                name: "master_Jobs",
                columns: table => new
                {
                    Master_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date_run = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Current_status = table.Column<int>(type: "int", nullable: false),
                    ServerIP = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_Jobs", x => x.Master_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job_Logs");

            migrationBuilder.DropTable(
                name: "master_Jobs");
        }
    }
}
