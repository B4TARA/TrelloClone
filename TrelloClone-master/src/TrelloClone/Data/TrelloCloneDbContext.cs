using Microsoft.EntityFrameworkCore;
using System;
using TrelloClone.Models;

namespace TrelloClone.Data
{
    public class TrelloCloneDbContext : DbContext
    {

        public TrelloCloneDbContext(DbContextOptions<TrelloCloneDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Database.EnsureCreated();
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //base.OnModelCreating(builder);

            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<User>(builder =>
            {
                builder.ToTable("USERS", schema: "public").HasKey(x => x.Id);
            });

            builder.Entity<Card>(builder =>
            {
                builder.ToTable("CARDS", schema: "public").HasKey(x => x.Id);
            });

            builder.Entity<Column>(builder =>
            {
                builder.ToTable("COLUMNS", schema: "public").HasKey(x => x.Id);
            });
        }
    }
}
