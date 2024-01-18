using Cafe.Domain.Entities;
using Cafe.Domain.Enums;
using Cafe.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Dal
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<dishs> dishs { get; set; }
        public DbSet<users> users { get; set; }
        public DbSet<profile> profile { get; set; }
        public DbSet<basket> basket { get; set; }
        public DbSet<dishlist> dishlist { get; set; }
        public DbSet<order> order { get; set; }
        public DbSet<orderbasket> orderbasket { get; set; }
        public DbSet<orderitem> orderitem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<users>(builder =>
            {
                builder.ToTable("users").HasKey(x => x.IdU);

                builder.Property(x => x.IdU).ValueGeneratedOnAdd();

                builder.Property(x => x.Password).IsRequired();
                builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

                builder.HasOne(x => x.Profile)
                    .WithOne(x => x.User)
                    .HasPrincipalKey<users>(x => x.IdU)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(x => x.Basket)
                    .WithOne(x => x.User)
                    .HasPrincipalKey<users>(x => x.IdU)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(x => x.OrderBasket)
                    .WithOne(x => x.User)
                    .HasPrincipalKey<users>(x => x.IdU)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<dishs>(builder =>
            {
                builder.ToTable("dishs").HasKey(x => x.IdD);
            });

            modelBuilder.Entity<profile>(builder =>
            {
                builder.ToTable("profile").HasKey(x => x.IdP);

                builder.Property(x => x.IdP).ValueGeneratedOnAdd();
                builder.Property(x => x.Age);
                builder.Property(x => x.Address).HasMaxLength(200).IsRequired(false);

                builder.HasData(new profile()
                {
                    IdP = 1,
                    userid = 1
                });
            });
            modelBuilder.Entity<basket>(builder =>
            {
                builder.ToTable("basket").HasKey(x => x.idbus);
            });
            modelBuilder.Entity<orderbasket>(builder =>
            {
                builder.ToTable("orderbasket").HasKey(x => x.IdOrderBas);
            });
            modelBuilder.Entity<order>(builder =>
            {
                builder.ToTable("order").HasKey(x => x.idorder);
            });
            modelBuilder.Entity<orderitem>(builder =>
            {
                builder.ToTable("orderitem").HasKey(x => x.IdOrderItem);
            });
        }
    }
}
