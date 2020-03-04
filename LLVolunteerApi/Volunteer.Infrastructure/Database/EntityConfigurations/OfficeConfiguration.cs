using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer.Core.Entities.Activities.Dormitory;
using Volunteer.Core.Entities.Activities.Offices;

namespace Volunteer.Infrastructure.Database.EntityConfigurations
{
    public class OfficeConfiguration : IEntityTypeConfiguration<OfficeTableDTO>
    {
        public void Configure(EntityTypeBuilder<OfficeTableDTO> builder)
        {
            builder
                .HasMany(t => t.OfficeTimeDtos);
            builder
                .HasMany(t => t.OfficeTypeDtos);
            builder
                .HasMany(t => t.OfficeWeekDto);
            builder.ToTable("officeTable");
        }
    }
}
