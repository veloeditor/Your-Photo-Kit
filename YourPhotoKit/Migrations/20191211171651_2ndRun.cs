using Microsoft.EntityFrameworkCore.Migrations;

namespace YourPhotoKit.Migrations
{
    public partial class _2ndRun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "TripGear");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Trips",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsPacked",
                table: "TripGear",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f803c50d-27ea-4653-af34-66effca4f52e", "AQAAAAEAACcQAAAAEN4MeZ10rBJ6ce3Hi9dodj22eMUOvg3iUagQYHT8ui/fg8dOd3tbdVaflTqXZhhTaQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPacked",
                table: "TripGear");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 5);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "TripGear",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9975ce08-4d3b-4af5-927a-07b9ccf767be", "AQAAAAEAACcQAAAAEFabpXzDEi4X4/WXPKUqi0QR075SUTPSrj8HaL8JauYyMfaWJ/LIsZT/UHXPQGmKZg==" });
        }
    }
}
