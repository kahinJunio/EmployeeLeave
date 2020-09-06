using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeLeave.Data.Migrations
{
    public partial class AddedComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cancel",
                table: "LeaveHistories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "LeaveHistories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancel",
                table: "LeaveHistories");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "LeaveHistories");
        }
    }
}
