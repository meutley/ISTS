using System;
using Microsoft.EntityFrameworkCore;

namespace ISTS.Infrastructure.Model
{
    public class IstsContext : DbContext
    {
        public DbSet<Studio> Studios { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Session> Sessions { get; set; }
        
        public IstsContext(DbContextOptions<IstsContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Studio>(studio =>
                {
                    studio
                        .HasMany(x => x.Rooms)
                        .WithOne(x => x.Studio)
                        .HasForeignKey(x => x.StudioId);
                });

            modelBuilder
                .Entity<Room>(room =>
                {
                    room
                        .HasMany(x => x.Sessions)
                        .WithOne(x => x.Room)
                        .HasForeignKey(x => x.RoomId);
                });

            modelBuilder
                .Entity<Session>();

            base.OnModelCreating(modelBuilder);
        }
    }
}