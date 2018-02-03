using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmallForumApp.Models
{
    public class TestModel
    {
        public Guid Id { get; set; }
        public string TestProperty { get; set; }
    }

    public class TestModelConfiguration
    {
        public TestModelConfiguration(EntityTypeBuilder<TestModel> builder)
        {
            builder.ToTable("TestModels");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.TestProperty)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
