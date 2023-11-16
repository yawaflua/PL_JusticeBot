using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordApp.Migrations
{
    /// <inheritdoc />
    public partial class Migrate16112006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RedirectType",
                schema: "public",
                table: "Redirects",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectType",
                schema: "public",
                table: "Redirects");
        }
    }
}
