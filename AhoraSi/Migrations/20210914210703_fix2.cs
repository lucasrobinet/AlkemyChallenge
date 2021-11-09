using Microsoft.EntityFrameworkCore.Migrations;

namespace AhoraSi.Migrations
{
    public partial class fix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieOrSerie_Genre_GenreId",
                table: "MovieOrSerie");

            migrationBuilder.DropTable(
                name: "CharacterMovieOrSerie");

            migrationBuilder.DropIndex(
                name: "IX_MovieOrSerie_GenreId",
                table: "MovieOrSerie");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "MovieOrSerie");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "MovieOrSerie",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CharacterMovieOrSerie",
                columns: table => new
                {
                    CharactersId = table.Column<int>(type: "int", nullable: false),
                    MovieOrSeriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterMovieOrSerie", x => new { x.CharactersId, x.MovieOrSeriesId });
                    table.ForeignKey(
                        name: "FK_CharacterMovieOrSerie_Character_CharactersId",
                        column: x => x.CharactersId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterMovieOrSerie_MovieOrSerie_MovieOrSeriesId",
                        column: x => x.MovieOrSeriesId,
                        principalTable: "MovieOrSerie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieOrSerie_GenreId",
                table: "MovieOrSerie",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterMovieOrSerie_MovieOrSeriesId",
                table: "CharacterMovieOrSerie",
                column: "MovieOrSeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieOrSerie_Genre_GenreId",
                table: "MovieOrSerie",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
