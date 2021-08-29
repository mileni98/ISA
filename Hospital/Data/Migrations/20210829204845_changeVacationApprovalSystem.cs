using Microsoft.EntityFrameworkCore.Migrations;

namespace Hospital.Data.Migrations
{
    public partial class changeVacationApprovalSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "Vacation",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasBeenReviewed",
                table: "Vacation",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenReviewed",
                table: "Vacation");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "Vacation",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
