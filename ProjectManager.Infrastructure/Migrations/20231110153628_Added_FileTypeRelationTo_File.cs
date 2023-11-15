using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManager.Infrastructure.Migrations
{
    public partial class Added_FileTypeRelationTo_File : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Files");

            migrationBuilder.AddColumn<int>(
                name: "FileTypeId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FileTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTypes", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.InsertData(
                table: "FileTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Image" },
                    { 2, "Document" },
                    { 3, "Archive" },
                    { 4, "Folder" },
                    { 5, "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_FileTypeId",
                table: "Files",
                column: "FileTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_FileTypes_FileTypeId",
                table: "Files",
                column: "FileTypeId",
                principalTable: "FileTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_FileTypes_FileTypeId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "FileTypes");

            migrationBuilder.DropIndex(
                name: "IX_Files_FileTypeId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FileTypeId",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
