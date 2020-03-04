using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer.Core.Entities.Organization;

namespace Volunteer.Infrastructure.Database.EntityConfigurations
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<OrganizationInfoDTO>
    {
        public void Configure(EntityTypeBuilder<OrganizationInfoDTO> builder)
        {
            builder
                .HasMany(t => t.ImageForOrganizionDtos)
                .WithOne(t => t.OrganizationInfoDto)
                .HasForeignKey(t => t.OrganizationInfoId);
            // 1个志愿组织  对应  多个 志愿活动
            builder
                .HasMany(t => t.ActivityForOrganizationDtos)
                .WithOne(t => t.OrganizationInfoDto)
                .HasForeignKey(t => t.OrganizationInfoId);
            builder.ToTable("OrganizationInfo");
        }
    }
}
