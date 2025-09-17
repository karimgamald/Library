using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class Version_23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FineAmount",
                table: "Borrowings");

            migrationBuilder.AddColumn<decimal>(
                name: "FinePerDay",
                table: "Memberships",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExtraBooks", "ExtraDays", "ExtraPenaltys", "FinePerDay" },
                values: new object[] { 3, 14, 15.00m, 3m });

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ExtraBooks", "ExtraDays", "FinePerDay" },
                values: new object[] { 5, 21, 2m });

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ExtraBooks", "ExtraDays", "FinePerDay" },
                values: new object[] { 10, 30, 1m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinePerDay",
                table: "Memberships");

            migrationBuilder.AddColumn<int>(
                name: "FineAmount",
                table: "Borrowings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExtraBooks", "ExtraDays", "ExtraPenaltys" },
                values: new object[] { 0, 0, 0.00m });

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ExtraBooks", "ExtraDays" },
                values: new object[] { 2, 7 });

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ExtraBooks", "ExtraDays" },
                values: new object[] { 5, 10 });
        }
    }
}
