using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer.Core.Entities.Activities.Dormitory;

namespace Volunteer.Infrastructure.Database.EntityConfigurations
{
    public class DormitoryConfiguration : IEntityTypeConfiguration<DormitoryTableDTO>
    {
        public void Configure(EntityTypeBuilder<DormitoryTableDTO> builder)
        {
            builder
                .HasMany(t => t.DormitoryTypeDtos);
            builder
                .HasMany(t => t.DormitoryWeekDtos);
            builder
                .HasMany(t => t.DormitoryTimeDtos);
            builder.ToTable("dormitoryTable");
        }
    }
}
