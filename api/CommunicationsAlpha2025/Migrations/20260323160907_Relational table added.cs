using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicationsAlpha2025.Migrations
{
    /// <inheritdoc />
    public partial class Relationaltableadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "calc");

            migrationBuilder.CreateTable(
                name: "Specifications",
                schema: "calc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecificationId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fundingStreams",
                schema: "calc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecificationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fundingStreams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fundingStreams_Specifications_SpecificationId",
                        column: x => x.SpecificationId,
                        principalSchema: "calc",
                        principalTable: "Specifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fundingStreams_SpecificationId",
                schema: "calc",
                table: "fundingStreams",
                column: "SpecificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fundingStreams",
                schema: "calc");

            migrationBuilder.DropTable(
                name: "Specifications",
                schema: "calc");
        }
    }
}
