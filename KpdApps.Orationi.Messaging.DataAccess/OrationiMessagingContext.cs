using KpdApps.Orationi.Messaging.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace KpdApps.Orationi.Messaging.DataAccess
{
    public class OrationiMessagingContext : DbContext
    {
        public OrationiMessagingContext(DbContextOptions<OrationiMessagingContext> options)
            : base(options)
        {

        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<RequestCode> RequestCodes { get; set; }

        public DbSet<RequestCodeAlias> RequestCodeAliases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<RequestCode>().ToTable("RequestCodes");
            modelBuilder.Entity<RequestCodeAlias>().ToTable("RequestCodeAliases");
        }
    }
}
