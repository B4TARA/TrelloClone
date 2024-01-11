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
        public DbSet<Comment> Comments { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
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

            builder.Entity<Comment>(builder =>
            {
                builder.ToTable("COMMENTS", schema: "public").HasKey(x => x.Id);
            });

            builder.Entity<File>(builder =>
            {
                builder.ToTable("FILES", schema: "public").HasKey(x => x.Id);
            });

            builder.Entity<Update>(builder =>
            {
                builder.ToTable("UPDATES", schema: "public").HasKey(x => x.Id);
            });
        }
    }
}
