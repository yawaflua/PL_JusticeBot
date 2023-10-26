using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiscordApp.Migrations
{
    /// <inheritdoc />
    public partial class InternalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Autobranches",
                schema: "public",
                columns: table => new
                {
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    BranchName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autobranches", x => x.ChannelId);
                });

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

            migrationBuilder.CreateTable(
                name: "Passport",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Employee = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Applicant = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<long>(type: "bigint", nullable: false),
                    Support = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    RpName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passport", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Autobranches",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Autoreactions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Passport",
                schema: "public");
        }
    }
}
