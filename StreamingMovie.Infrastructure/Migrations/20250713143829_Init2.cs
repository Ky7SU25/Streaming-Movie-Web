using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamingMovie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "MovieVideo",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "ProcessStatus",
                table: "MovieVideo",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "EpisodeVideo",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "ProcessStatus",
                table: "EpisodeVideo",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "MovieVideo");

            migrationBuilder.DropColumn(
                name: "ProcessStatus",
                table: "MovieVideo");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "EpisodeVideo");

            migrationBuilder.DropColumn(
                name: "ProcessStatus",
                table: "EpisodeVideo");
        }
    }
}
