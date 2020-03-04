using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer.Core.Entities.Activities.Dormitory;

namespace Volunteer.Infrastructure.Database.EntityConfigurations
{
    public class DormitoryCheckConfiguration : IEntityTypeConfiguration<DormitoryCheckDTO>
    {
        public void Configure(EntityTypeBuilder<DormitoryCheckDTO> builder)
        {
            builder.HasMany(t => t.DormitorySignDtos);
            builder.ToTable("DormitoryCheck");
        }
    }
}
