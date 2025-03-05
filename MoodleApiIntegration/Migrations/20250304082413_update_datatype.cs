using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoodleApiIntegration.Migrations
{
    /// <inheritdoc />
    public partial class update_datatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IS_SUCCESS",
                table: "onHoldStudentDtos",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IS_SUCCESS",
                table: "onHoldStudentDtos",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
