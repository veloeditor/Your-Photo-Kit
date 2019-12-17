using Microsoft.EntityFrameworkCore.Migrations;

namespace YourPhotoKit.Migrations
{
    public partial class TripTweak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Trips",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "732cef63-560a-4508-b461-21dfd5db0878", "AQAAAAEAACcQAAAAENJSirpd00xvHYrT2gGCq2FjwSTXSRD0TwxyJSl+kVa4Pbk12hj+0+99BswKtPtq7w==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Trips",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f803c50d-27ea-4653-af34-66effca4f52e", "AQAAAAEAACcQAAAAEN4MeZ10rBJ6ce3Hi9dodj22eMUOvg3iUagQYHT8ui/fg8dOd3tbdVaflTqXZhhTaQ==" });
        }
    }
}
