using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer.Core.Entities.Activities.Dormitory;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Activities.Offices;

namespace Volunteer.Infrastructure.Database.EntityConfigurations
{
    public class OfficeSignConfiguration: IEntityTypeConfiguration<OfficeSignDTO>
    {
        public void Configure(EntityTypeBuilder<OfficeSignDTO> builder)
        {
            builder
                .HasMany(t => t.OfficeDtos);
            builder.Property(t => t.CheckState).HasDefaultValue(CheckStateType.未审核);
            builder.ToTable("officeSign");
        }
    }
}
