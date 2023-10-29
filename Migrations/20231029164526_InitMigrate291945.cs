using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordApp.Migrations
{
    /// <inheritdoc />
    public partial class InitMigrate291945 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passport_Bizness_BiznessId",
                schema: "public",
                table: "Passport");

            migrationBuilder.DropIndex(
                name: "IX_Passport_BiznessId",
                schema: "public",
                table: "Passport");

            migrationBuilder.DropColumn(
                name: "BiznessId",
                schema: "public",
                table: "Passport");

            migrationBuilder.AddColumn<int[]>(
                name: "BiznessEmployes",
                table: "Bizness",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiznessEmployes",
                table: "Bizness");

            migrationBuilder.AddColumn<int>(
                name: "BiznessId",
                schema: "public",
                table: "Passport",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passport_BiznessId",
                schema: "public",
                table: "Passport",
                column: "BiznessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passport_Bizness_BiznessId",
                schema: "public",
                table: "Passport",
                column: "BiznessId",
                principalTable: "Bizness",
                principalColumn: "Id");
        }
    }
}
