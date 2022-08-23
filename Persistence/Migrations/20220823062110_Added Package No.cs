using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class AddedPackageNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PackageNo",
                table: "WorkOrderItem",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubItemPackageNo",
                table: "WorkOrderItem",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PackageNo",
                table: "RABillItem",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubItemPackageNo",
                table: "RABillItem",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageNo",
                table: "WorkOrderItem");

            migrationBuilder.DropColumn(
                name: "SubItemPackageNo",
                table: "WorkOrderItem");

            migrationBuilder.DropColumn(
                name: "PackageNo",
                table: "RABillItem");

            migrationBuilder.DropColumn(
                name: "SubItemPackageNo",
                table: "RABillItem");
        }
    }
}
