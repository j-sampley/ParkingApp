using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using ParkingApp.Common.Models.User;

namespace ParkingApp.Api.Database;

public class ParkingDbContext : IdentityDbContext<UserDataModel>
{
    public ParkingDbContext(DbContextOptions<ParkingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserDataModel>()
            .HasOne(u => u.Address)
            .WithOne()
            .HasForeignKey<UserDataModel>(u => u.Key);

        modelBuilder.Entity<UserDataModel>()
            .HasMany(u => u.Contacts)
            .WithOne()
            .HasForeignKey(c => c.UserKey);

        modelBuilder.Entity<UserDataModel>()
            .HasMany(u => u.Vehicles)
            .WithOne()
            .HasForeignKey(c => c.UserKey);
    }
}