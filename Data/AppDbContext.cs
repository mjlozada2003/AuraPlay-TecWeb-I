using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Song> Songs => Set<Song>();
        public DbSet<Playlist> Playlists => Set<Playlist>();
        public DbSet<User> Users => Set<User>();
        public DbSet<PlaylistSong> PlaylistSongs => Set<PlaylistSong>();
        
        public DbSet<Statistics> Statistics => Set<Statistics>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configuración M:N (Playlist - Song)
            modelBuilder.Entity<PlaylistSong>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Song)
                .WithMany(s => s.PlaylistSongs) 
                .HasForeignKey(ps => ps.SongId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. Configuración 1:N (User - Playlist)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Playlists)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Si borras usuario, se borran sus playlists

            // 3. Configuración 1:1 (Song - Statistics)
            modelBuilder.Entity<Song>()
                .HasOne(s => s.Statistics)
                .WithOne(st => st.Song)
                .HasForeignKey<Statistics>(st => st.SongId)
                .OnDelete(DeleteBehavior.Cascade); // Si borras canción, se borran sus estadísticas
        }
    }
}