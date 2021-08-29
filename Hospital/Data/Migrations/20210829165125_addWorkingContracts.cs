using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hospital.Data.Migrations
{
    public partial class addWorkingContracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkingContract",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    WorkerId = table.Column<string>(nullable: true),
                    PharmacyId = table.Column<Guid>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    WorkTimeStart = table.Column<TimeSpan>(nullable: false),
                    WorkTimeEnd = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingContract", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingContract");
        }
    }
}
