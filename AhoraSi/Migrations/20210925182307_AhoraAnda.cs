using Microsoft.EntityFrameworkCore.Migrations;

namespace AhoraSi.Migrations
{
    public partial class AhoraAnda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedGenres",
                table: "MovieOrSerie");

            migrationBuilder.AddColumn<int>(
                name: "MovieOrSerieId",
                table: "Genre",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genre_MovieOrSerieId",
                table: "Genre",
                column: "MovieOrSerieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genre_MovieOrSerie_MovieOrSerieId",
                table: "Genre",
                column: "MovieOrSerieId",
                principalTable: "MovieOrSerie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genre_MovieOrSerie_MovieOrSerieId",
                table: "Genre");

            migrationBuilder.DropIndex(
                name: "IX_Genre_MovieOrSerieId",
                table: "Genre");

            migrationBuilder.DropColumn(
                name: "MovieOrSerieId",
                table: "Genre");

            migrationBuilder.AddColumn<int>(
                name: "SelectedGenres",
                table: "MovieOrSerie",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
