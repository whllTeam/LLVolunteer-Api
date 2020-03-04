using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer.Core.Entities.Activities.Dormitory;
using Volunteer.Core.Entities.Activities.Enum;

namespace Volunteer.Infrastructure.Database.EntityConfigurations
{
    public class DormitorySignConfiguration:IEntityTypeConfiguration<DormitorySignDTO>
    {
        public void Configure(EntityTypeBuilder<DormitorySignDTO> builder)
        {
            builder
                .HasMany(t => t.DormitoryDtos);
            builder.Property(t => t.CheckState).HasDefaultValue(CheckStateType.未审核);
            builder.ToTable("dormitorySin");
        }
    }
}
