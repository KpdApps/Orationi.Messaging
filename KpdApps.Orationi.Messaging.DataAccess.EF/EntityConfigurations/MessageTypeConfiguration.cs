using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KpdApps.Orationi.Messaging.DataAccess.Common.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EF.EntityConfigurations
{
    public class MessageTypeConfiguration : EntityTypeConfiguration<Message>
    {
        public MessageTypeConfiguration()
        {
            ToTable("Messages");

            HasRequired(p => p.MessageStatusCode)
                .WithMany(p => p.Messages)
                .HasForeignKey(p => p.StatusCode);

            HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.Created)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(p => p.Modified)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}
