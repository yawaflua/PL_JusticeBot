using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiscordApp.Migrations
{
    /// <inheritdoc />
    public partial class InitMigrate29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtsPatent",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Employee = table.Column<string>(type: "text", nullable: false),
                    passportId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<long>(type: "bigint", nullable: false),
                    Number = table.Column<int[]>(type: "integer[]", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Size = table.Column<string>(type: "text", nullable: false),
                    isAllowedToResell = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtsPatent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtsPatent_Passport_passportId",
                        column: x => x.passportId,
                        principalSchema: "public",
                        principalTable: "Passport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BooksPatent",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Employee = table.Column<string>(type: "text", nullable: false),
                    passportId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Annotation = table.Column<string>(type: "text", nullable: false),
                    Janre = table.Column<string>(type: "text", nullable: false),
                    isAllowedToResell = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksPatent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BooksPatent_Passport_passportId",
                        column: x => x.passportId,
                        principalSchema: "public",
                        principalTable: "Passport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtsPatent_passportId",
                schema: "public",
                table: "ArtsPatent",
                column: "passportId");

            migrationBuilder.CreateIndex(
                name: "IX_BooksPatent_passportId",
                schema: "public",
                table: "BooksPatent",
                column: "passportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtsPatent",
                schema: "public");

            migrationBuilder.DropTable(
                name: "BooksPatent",
                schema: "public");
        }
    }
}
