using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddedRABill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RABills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    BillDate = table.Column<long>(type: "INTEGER", nullable: false),
                    ApprovalDate = table.Column<long>(type: "INTEGER", nullable: false),
                    MeasurementBookId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 6, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RABills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RABills_MeasurementBooks_MeasurementBookId",
                        column: x => x.MeasurementBookId,
                        principalTable: "MeasurementBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RABillItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemDescription = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    UnitRate = table.Column<double>(type: "REAL", nullable: false),
                    AcceptedMeasuredQty = table.Column<float>(type: "REAL", nullable: false),
                    TillLastRAQty = table.Column<float>(type: "REAL", nullable: false),
                    CurrentRAQty = table.Column<float>(type: "REAL", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MBookItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    RABillId = table.Column<int>(type: "INTEGER", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 6, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RABillItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RABillItem_RABills_RABillId",
                        column: x => x.RABillId,
                        principalTable: "RABills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RABillItem_RABillId",
                table: "RABillItem",
                column: "RABillId");

            migrationBuilder.CreateIndex(
                name: "IX_RABills_MeasurementBookId",
                table: "RABills",
                column: "MeasurementBookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RABillItem");

            migrationBuilder.DropTable(
                name: "RABills");
        }
    }
}
