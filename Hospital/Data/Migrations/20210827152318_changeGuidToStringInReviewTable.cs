using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hospital.Data.Migrations
{
    public partial class changeGuidToStringInReviewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Review");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Review",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewedId",
                table: "Review",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Review",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Review",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Review");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Review",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewedId",
                table: "Review",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
