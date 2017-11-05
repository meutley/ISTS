using System;
using Microsoft.EntityFrameworkCore;

using ISTS.Domain.PostalCodes;
using ISTS.Domain.Rooms;
using ISTS.Domain.Sessions;
using ISTS.Domain.Studios;
using ISTS.Domain.Users;

namespace ISTS.Infrastructure.Model
{
    public class IstsContext : DbContext
    {
        public DbSet<PostalCode> PostalCodes { get; set; }

        public DbSet<PostalCodeDistance> PostalCodeDistances { get; set; }

        public DbSet<StudioSearchResult> StudioSearchResults { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<SessionRequest> SessionRequests { get; set; }

        public DbSet<Studio> Studios { get; set; }

        public DbSet<User> Users { get; set; }
        
        public IstsContext(DbContextOptions<IstsContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Stored procedure results
            modelBuilder.Entity<PostalCodeDistance>().HasKey(p => p.Code);
            modelBuilder.Entity<StudioSearchResult>();
            
            // Tables
            modelBuilder
                .Entity<PostalCode>(postalCode =>
                {
                    postalCode.ToTable("PostalCode");

                    postalCode.HasKey(x => new
                    {
                        x.Code,
                        x.City,
                        x.State
                    });
                });
            
            modelBuilder
                .Entity<Room>(room =>
                {                    
                    room.ToTable("Room");

                    room.HasKey(x => x.Id);
                });

            modelBuilder
                .Entity<Session>(session =>
                {
                    session.ToTable("Session");
                    
                    session.HasKey(x => x.Id);
                    session.Ignore(x => x.Schedule);
                });

            modelBuilder
                .Entity<SessionRequest>(request =>
                {
                    request.ToTable("SessionRequest");

                    request.HasKey(x => x.Id);
                    request.Ignore(x => x.RequestedTime);
                });

            modelBuilder
                .Entity<Studio>(studio =>
                {                    
                    studio.HasKey(x => x.Id);
                    
                    studio
                        .ToTable("Studio")
                        .HasMany(x => x.Rooms)
                        .WithOne(x => x.Studio)
                        .HasForeignKey(x => x.StudioId);
                });

            modelBuilder
                .Entity<User>(user =>
                {                    
                    user
                        .ToTable("User")
                        .HasMany(x => x.Studios)
                        .WithOne(x => x.OwnerUser)
                        .HasForeignKey(x => x.OwnerUserId);
                    
                    user.HasKey(x => x.Id);
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}