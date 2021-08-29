using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hospital.Data.Migrations
{
    public partial class changeVacationWorkerIdType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vacation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    WorkerId = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: true),
                    ApprovalTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacation", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vacation");
        }
    }
}
