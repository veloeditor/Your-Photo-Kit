using Microsoft.EntityFrameworkCore.Migrations;

namespace YourPhotoKit.Migrations
{
    public partial class TripGalleryURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GalleryUrl",
                table: "Trips",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1a60bf12-3ea3-4af8-95d4-eab6eb4b38bc", "AQAAAAEAACcQAAAAEIYt1f2v+8xFfe7/oUACkPH6/lXtBO+dRsi3658BfCdbdRtXZEbYSRwHxats+ANp0g==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GalleryUrl",
                table: "Trips");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "732cef63-560a-4508-b461-21dfd5db0878", "AQAAAAEAACcQAAAAENJSirpd00xvHYrT2gGCq2FjwSTXSRD0TwxyJSl+kVa4Pbk12hj+0+99BswKtPtq7w==" });
        }
    }
}
