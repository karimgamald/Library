using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class Version_24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Memberships_MemberShipId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships");

            migrationBuilder.RenameTable(
                name: "Memberships",
                newName: "MemberShips");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberShips",
                table: "MemberShips",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_MemberShips_MemberShipId",
                table: "Users",
                column: "MemberShipId",
                principalTable: "MemberShips",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_MemberShips_MemberShipId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberShips",
                table: "MemberShips");

            migrationBuilder.RenameTable(
                name: "MemberShips",
                newName: "Memberships");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Memberships_MemberShipId",
                table: "Users",
                column: "MemberShipId",
                principalTable: "Memberships",
                principalColumn: "Id");
        }
    }
}
