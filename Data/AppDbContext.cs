using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProyectoTecWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Song> Songs => Set<Song>();
        public DbSet<Statistics> Stadistics => Set<Statistics>();
        public DbSet<Playlist> Playlists => Set<Playlist>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación 1:1 (Song - Statistics)
            modelBuilder.Entity<Song>()
                .HasOne(s => s.Stadistics)
                .WithOne(st => st.Song)
                .HasForeignKey<Statistics>(st => st.SongId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Relación 1:N (User - Playlist)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Playlists)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación N:M (Song - Playlist)
            modelBuilder.Entity<Playlist>()
                .HasMany(p => p.Songs)
                .WithMany(s => s.Playlists)
                .UsingEntity(j => j.ToTable("PlaylistSongs")); 
        }
    }
}