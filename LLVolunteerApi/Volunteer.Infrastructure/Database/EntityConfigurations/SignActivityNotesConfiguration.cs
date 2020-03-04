using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer.Core.Entities.Activities.Notes;

namespace Volunteer.Infrastructure.Database.EntityConfigurations
{
    public class SignActivityNotesConfiguration:IEntityTypeConfiguration<SignActivityNotesDTO>
    {
        public void Configure(EntityTypeBuilder<SignActivityNotesDTO> builder)
        {
            builder.HasMany(t => t.DormitoryTableDtos);
            builder.HasMany(t => t.OfficeTableDtos);
            builder.HasMany(t => t.ActivitySignTableDtos);
            builder.ToTable("SignActivityNotes");
        }
    }
}
