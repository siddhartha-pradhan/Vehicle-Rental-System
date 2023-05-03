using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalSystem.Infrastructure.Migrations
{
    public partial class FinalizedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRegulat",
                table: "Customers",
                newName: "IsRegular");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRegular",
                table: "Customers",
                newName: "IsRegulat");
        }
    }
}
