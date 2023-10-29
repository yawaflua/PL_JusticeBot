using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiscordApp.Migrations
{
    /// <inheritdoc />
    public partial class InitMigrate291903 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BiznessId",
                schema: "public",
                table: "Passport",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Date",
                schema: "public",
                table: "BooksPatent",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "Bizness",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicantId = table.Column<int>(type: "integer", nullable: false),
                    Employee = table.Column<string>(type: "text", nullable: false),
                    BiznessName = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<long>(type: "bigint", nullable: false),
                    BiznessType = table.Column<string>(type: "text", nullable: false),
                    CardNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bizness", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bizness_Passport_ApplicantId",
                        column: x => x.ApplicantId,
                        principalSchema: "public",
                        principalTable: "Passport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passport_BiznessId",
                schema: "public",
                table: "Passport",
                column: "BiznessId");

            migrationBuilder.CreateIndex(
                name: "IX_Bizness_ApplicantId",
                table: "Bizness",
                column: "ApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passport_Bizness_BiznessId",
                schema: "public",
                table: "Passport",
                column: "BiznessId",
                principalTable: "Bizness",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passport_Bizness_BiznessId",
                schema: "public",
                table: "Passport");

            migrationBuilder.DropTable(
                name: "Bizness");

            migrationBuilder.DropIndex(
                name: "IX_Passport_BiznessId",
                schema: "public",
                table: "Passport");

            migrationBuilder.DropColumn(
                name: "BiznessId",
                schema: "public",
                table: "Passport");

            migrationBuilder.AlterColumn<int>(
                name: "Date",
                schema: "public",
                table: "BooksPatent",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
