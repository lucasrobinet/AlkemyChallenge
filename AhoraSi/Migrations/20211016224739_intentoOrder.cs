using Microsoft.EntityFrameworkCore.Migrations;

namespace AhoraSi.Migrations
{
    public partial class intentoOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "MovieOrSerie",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "MovieOrSerie");
        }
    }
}
