using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AhoraSi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Character",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    History = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Character", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieOrSerie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<byte>(type: "tinyint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Valoration = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieOrSerie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieOrSerie_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "CharacterOfShow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    MovieOrSerieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterOfShow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterOfShow_Character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterOfShow_MovieOrSerie_MovieOrSerieId",
                        column: x => x.MovieOrSerieId,
                        principalTable: "MovieOrSerie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenreOfShow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieOrSerieId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreOfShow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenreOfShow_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreOfShow_MovieOrSerie_MovieOrSerieId",
                        column: x => x.MovieOrSerieId,
                        principalTable: "MovieOrSerie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterMovieOrSerie_MovieOrSeriesId",
                table: "CharacterMovieOrSerie",
                column: "MovieOrSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterOfShow_CharacterId",
                table: "CharacterOfShow",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterOfShow_MovieOrSerieId",
                table: "CharacterOfShow",
                column: "MovieOrSerieId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreOfShow_GenreId",
                table: "GenreOfShow",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreOfShow_MovieOrSerieId",
                table: "GenreOfShow",
                column: "MovieOrSerieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieOrSerie_GenreId",
                table: "MovieOrSerie",
                column: "GenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterMovieOrSerie");

            migrationBuilder.DropTable(
                name: "CharacterOfShow");

            migrationBuilder.DropTable(
                name: "GenreOfShow");

            migrationBuilder.DropTable(
                name: "Character");

            migrationBuilder.DropTable(
                name: "MovieOrSerie");

            migrationBuilder.DropTable(
                name: "Genre");
        }
    }
}
