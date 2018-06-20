using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBase.Migrations
{
    public partial class Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostItemTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PostId = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostItemTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostImagesTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PostItemId = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostImagesTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostImagesTable_PostItemTable_PostItemId",
                        column: x => x.PostItemId,
                        principalTable: "PostItemTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostLinksTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PostItemId = table.Column<int>(nullable: false),
                    Link = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLinksTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLinksTable_PostItemTable_PostItemId",
                        column: x => x.PostItemId,
                        principalTable: "PostItemTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostImagesTable_PostItemId",
                table: "PostImagesTable",
                column: "PostItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLinksTable_PostItemId",
                table: "PostLinksTable",
                column: "PostItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostImagesTable");

            migrationBuilder.DropTable(
                name: "PostLinksTable");

            migrationBuilder.DropTable(
                name: "PostItemTable");
        }
    }
}
