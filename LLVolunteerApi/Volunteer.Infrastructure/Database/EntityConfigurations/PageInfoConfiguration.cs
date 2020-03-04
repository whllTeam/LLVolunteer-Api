using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer.Core.Entities.Essay;

namespace Volunteer.Infrastructure.Database.EntityConfigurations
{
    public class PageInfoConfiguration : IEntityTypeConfiguration<PageInfoDTO>
    {
        public void Configure(EntityTypeBuilder<PageInfoDTO> builder)
        {
            // 文章  对于  插图  1对 多
            builder
                .HasMany(t => t.PageImgs)
                .WithOne(t => t.PageInfoDto)
                .HasForeignKey(t => t.PageInfoId);
            // 文章  对于 组织   多  对 1 
            builder
                .HasOne(t => t.OrganizationInfoDto)
                .WithMany(t => t.PageInfoDtos)
                .HasForeignKey(t => t.OrganizationInfoId);

            builder
                .ToTable("PageInfo");
        }
    }
}
