using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrudByNgAndDotNet.API.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class UpdateToUseApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "IdentityUser",
                keyColumn: "Id",
                keyValue: "edc267ec-d43c-4e3b-8108-a1a1f819906d",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "db913dfc-eaf2-4633-99cf-2da11c8829c7", "ADMIN@CRUDBYNGANDDOTNET.COM", "ADMIN@CRUDBYNGANDDOTNET.COM", "AQAAAAIAAYagAAAAEBDy+9LLC8LPCfryjNIKRfWjp/IHL0dKzhLAnzVy9mmpG0YVWvf+ZJ/1EE2I2ctsQg==", "9a7c2b4d-9937-434b-85be-c574d7cdf63b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "IdentityUser",
                keyColumn: "Id",
                keyValue: "edc267ec-d43c-4e3b-8108-a1a1f819906d",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "574a707e-61f2-4014-8dc4-019bb5495c60", "ADMIN@CrudByNgAndDotNet.COM", "ADMIN@CrudByNgAndDotNet.COM", "AQAAAAIAAYagAAAAELsu4th4WIJV47vTbarcgtHv4XWc7n1Y840hfLP3ImCSI9mEWBNn0fBZk+UpWM3iCQ==", "88b931e3-7281-447d-b3b2-4d2fb570e372" });
        }
    }
}
