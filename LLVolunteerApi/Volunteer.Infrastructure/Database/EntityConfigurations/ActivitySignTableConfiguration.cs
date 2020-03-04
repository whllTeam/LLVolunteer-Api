using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer.Core.Entities.Organization;

namespace Volunteer.Infrastructure.Database.EntityConfigurations
{
    public class ActivitySignTableConfiguration: IEntityTypeConfiguration<ActivitySignTableDTO>
    {
        public void Configure(EntityTypeBuilder<ActivitySignTableDTO> builder)
        {
            builder.HasMany(t => t.ActivityForOrganizationDtos);
            builder.ToTable("ActivitySignTable");
        }
    }
}
