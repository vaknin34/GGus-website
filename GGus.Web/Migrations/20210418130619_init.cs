using Microsoft.EntityFrameworkCore.Migrations;

namespace GGus.Web.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    PhotosUrl1 = table.Column<string>(nullable: true),
                    PhotosUrl2 = table.Column<string>(nullable: true),
                    PhotosUrl3 = table.Column<string>(nullable: true),
                    PhotosUrl4 = table.Column<string>(nullable: true),
                    PhotosUrl5 = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    TrailerUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
