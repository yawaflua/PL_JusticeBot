using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordApp.Migrations
{
    /// <inheritdoc />
    public partial class AddBitrhDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Autoreactions",
                schema: "public");

            migrationBuilder.AddColumn<long>(
                name: "birthDate",
                schema: "public",
                table: "Passport",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "birthDate",
                schema: "public",
                table: "Passport");

            migrationBuilder.CreateTable(
                name: "Autoreactions",
                schema: "public",
                columns: table => new
                {
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    EmoteId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autoreactions", x => x.ChannelId);
                });
        }
    }
}
