using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoTecWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistSongs_Songs_SongId1",
                table: "PlaylistSongs");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistSongs_SongId1",
                table: "PlaylistSongs");

            migrationBuilder.DropColumn(
                name: "SongId1",
                table: "PlaylistSongs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SongId1",
                table: "PlaylistSongs",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_SongId1",
                table: "PlaylistSongs",
                column: "SongId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistSongs_Songs_SongId1",
                table: "PlaylistSongs",
                column: "SongId1",
                principalTable: "Songs",
                principalColumn: "Id");
        }
    }
}
