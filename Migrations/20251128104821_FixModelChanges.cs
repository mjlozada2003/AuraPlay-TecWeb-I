using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoTecWeb.Migrations
{
    /// <inheritdoc />
    public partial class FixModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSongs_Songs_SongId",
                table: "PlaylistSongs");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSongs_Songs_SongId",
                table: "PlaylistSongs",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSongs_Songs_SongId",
                table: "PlaylistSongs");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSongs_Songs_SongId",
                table: "PlaylistSongs",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
