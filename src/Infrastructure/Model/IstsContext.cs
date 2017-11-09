using System;
using Microsoft.EntityFrameworkCore;

using ISTS.Domain.PostalCodes;
using ISTS.Domain.Rooms;
using ISTS.Domain.Sessions;
using ISTS.Domain.SessionRequests;
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

                    room.HasMany(x => x.RoomFunctions)
                        .WithOne(x => x.Room)
                        .HasForeignKey(x => x.RoomId);
                });

            modelBuilder
                .Entity<RoomFunction>(func =>
                {
                    func.ToTable("RoomFunction");

                    func.HasKey(x => x.Id);
                    func.Ignore(x => x.BaseBillingRate);
                    
                    func.Property(x => x.Name)
                        .HasMaxLength(50)
                        .IsRequired();

                    func.Property(x => x.Description)
                        .HasMaxLength(250);

                    func.Property(x => x.BillingRateName)
                        .HasMaxLength(30);
                });

            modelBuilder
                .Entity<Session>(session =>
                {
                    session.ToTable("Session");
                    
                    session.HasKey(x => x.Id);
                    session.Ignore(x => x.Schedule);

                    session.HasOne(x => x.SessionRequest)
                        .WithOne(x => x.Session)
                        .HasForeignKey<SessionRequest>(x => x.SessionId);
                });

            modelBuilder
                .Entity<SessionRequest>(request =>
                {
                    request.ToTable("SessionRequest");

                    request.HasKey(x => x.Id);
                    request.Property(x => x.RejectedReason)
                        .HasMaxLength(100);
                        
                    request.Ignore(x => x.RequestedTime);
                });

            modelBuilder
                .Entity<SessionRequestStatus>(status =>
                {
                    status.ToTable("SessionRequestStatus");

                    status.HasKey(x => x.Id);
                    status.Property(x => x.Description)
                        .HasMaxLength(50)
                        .IsRequired();
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

                    user.HasOne(x => x.TimeZone)
                        .WithMany(x => x.Users)
                        .HasForeignKey(x => x.TimeZoneId);
                    
                    user.HasKey(x => x.Id);
                });

            modelBuilder
                .Entity<UserTimeZone>(tz =>
                {
                    tz.HasKey(x => x.Id);
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}