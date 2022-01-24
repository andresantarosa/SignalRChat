using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalRChat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat.Persistence.Config
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.PostOwner)
                .IsRequired();

            builder.Property(b => b.PostContent)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(b => b.CreatedDate)
                .IsRequired();
        }
    }
}
