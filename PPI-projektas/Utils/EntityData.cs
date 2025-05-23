﻿﻿using Microsoft.EntityFrameworkCore;
using PPI_projektas.objects;

namespace PPI_projektas.Utils
{
    public class EntityData : DbContext
    {
        public EntityData() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=185.34.52.6;user=NotesApp;password=AlioValioIrInternetas;database=NotesApp";
            var serverVersion = MariaDbServerVersion.AutoDetect(connectionString);
            optionsBuilder.UseMySql(connectionString, serverVersion);
            optionsBuilder.AddInterceptors(new LastEditTimeInterceptor());
        }
        public  EntityData(DbContextOptions<EntityData> options) : base(options) { Database.EnsureCreated(); }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Note>().HasKey(u => u.Id);
            modelBuilder.Entity<Group>().HasKey(u => u.Id);
            modelBuilder.Entity<Tag>().HasKey(t => t.Id);

            modelBuilder.Entity<Note>()
                .HasMany(n => n.FavoriteByUsers)
                .WithMany(u => u.FavoriteNotes);

            modelBuilder.Entity<Note>()
                .HasOne(n => n.Author)
                .WithMany(u => u.CreatedNotes)
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<Group>()
                .HasMany(n => n.Members)
                .WithMany(u => u.Groups);

            modelBuilder.Entity<User>()
                .HasMany(n => n.Groups)
                .WithMany(u => u.Members);

            modelBuilder.Entity<Note>()
                .HasOne(n => n.Group)
                .WithMany(u => u.Notes);

            modelBuilder.Entity<Note>()
                .HasMany(n => n.Tags);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Owner)
                .WithMany(u => u.OwnedGroups);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Administrators)
                .WithMany(u => u.AdministratorOfGroups);

            modelBuilder.Entity<Note>()
                .HasMany(n => n.Editors)
                .WithMany(u => u.EditorOfNotes);
        }
    }
}