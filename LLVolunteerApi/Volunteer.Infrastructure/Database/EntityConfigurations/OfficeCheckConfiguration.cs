using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer.Core.Entities.Activities.Offices;

namespace Volunteer.Infrastructure.Database.EntityConfigurations
{
    public class OfficeCheckConfiguration : IEntityTypeConfiguration<OfficeCheckDTO>
    {
        public void Configure(EntityTypeBuilder<OfficeCheckDTO> builder)
        {
            builder.HasMany(t => t.OfficeSignDtos);
            builder.ToTable("OfficeCheck");
        }
    }
}
