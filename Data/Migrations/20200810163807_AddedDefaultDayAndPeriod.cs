using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeLeave.Data.Migrations
{
    public partial class AddedDefaultDayAndPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfDays",
                table: "LeaveTypes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfDays",
                table: "LeaveTypes");
        }
    }
}
