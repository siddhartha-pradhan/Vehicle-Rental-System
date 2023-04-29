using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalSystem.Infrastructure.Migrations
{
    public partial class RemovalOfName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Vehicles");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "Images",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Vehicles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
