using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gallery.Migrations
{
    public partial class AddedImagesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(MAX)", unicode: false, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
