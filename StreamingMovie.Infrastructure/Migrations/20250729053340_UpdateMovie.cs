using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamingMovie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmbeddingJson",
                table: "Series",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "EmbeddingJson",
                table: "Movie",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbeddingJson",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "EmbeddingJson",
                table: "Movie");
        }
    }
}
