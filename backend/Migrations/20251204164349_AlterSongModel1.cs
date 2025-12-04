using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class AlterSongModel1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SongUrl",
                table: "Songs",
                newName: "FileUrl");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Songs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Songs");

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "Songs",
                newName: "SongUrl");
        }
    }
}
