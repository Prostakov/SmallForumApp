using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmallForumApp.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class PostConfiguration
    {
        public PostConfiguration(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Text)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .IsRequired();
        }
    }
}
