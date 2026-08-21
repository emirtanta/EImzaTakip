using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EImzaTakip.Migrations
{
    /// <inheritdoc />
    public partial class Açıklamakolonunuzorunluolmaktançıkar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$BSYFrTKRghdr1/ObNG/Ps.iehfQLpbUX2OVsVtuX8/x9Tc8tFHMmG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$q9CfZTE0eQgpvK0VvfvPcObZMdvd1uMYcUnE0pBG/BzR2OH0pZyi.");
        }
    }
}
