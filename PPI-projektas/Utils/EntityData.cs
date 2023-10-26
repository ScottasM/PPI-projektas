using Microsoft.EntityFrameworkCore;
using PPI_projektas.objects;
using System;

namespace PPI_projektas.Utils
{
    public class EntityData : DbContext
    {
        public  EntityData(DbContextOptions<EntityData> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
