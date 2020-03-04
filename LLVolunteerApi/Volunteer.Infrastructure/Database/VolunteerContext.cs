using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Volunteer.Core.Entities.Activities;
using Volunteer.Core.Entities.Activities.Dormitory;
using Volunteer.Core.Entities.Activities.Notes;
using Volunteer.Core.Entities.Activities.Offices;
using Volunteer.Core.Entities.Essay;
using Volunteer.Core.Entities.FileManager;
using Volunteer.Core.Entities.Organization;
using Volunteer.Core.Entities.SchoolUser;
using Volunteer.Infrastructure.Database.EntityConfigurations;

namespace Volunteer.Infrastructure.Database
{
    public class VolunteerContext : DbContext
    {
        public VolunteerContext(DbContextOptions<VolunteerContext> option) : base(option)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DormitoryConfiguration());
            modelBuilder.ApplyConfiguration(new DormitorySignConfiguration());
            modelBuilder.ApplyConfiguration(new OfficeConfiguration());
            modelBuilder.ApplyConfiguration(new OfficeSignConfiguration());
            modelBuilder.ApplyConfiguration(new SignActivityNotesConfiguration());
            modelBuilder.ApplyConfiguration(new PageInfoConfiguration());
            modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
            modelBuilder.ApplyConfiguration(new ActivitySignTableConfiguration());
            modelBuilder.ApplyConfiguration(new DormitoryCheckConfiguration());
            modelBuilder.ApplyConfiguration(new OfficeCheckConfiguration());

            #region 寝室楼

            modelBuilder.Entity<DormitoryTypeDTO>()
                .ToTable("dormitoryType");
            // 设置 志愿服务时长默认为2
            modelBuilder.Entity<DormitoryTypeDTO>()
                .Property(t => t.VolunteerTime)
                .HasDefaultValue(2);
            modelBuilder.Entity<DormitoryTimeDTO>()
                .ToTable("dormitoryTime");
            modelBuilder.Entity<DormitoryWeekDTO>()
                .ToTable("dormitoryWeek");

            #endregion

            #region 办公室

            modelBuilder.Entity<OfficeTypeDTO>()
                .ToTable("officeType");
            // 办公室值班 志愿服务时长默认 2小时
            modelBuilder.Entity<OfficeTypeDTO>()
                .Property(t => t.VolunteerTime)
                .HasDefaultValue(2);
            modelBuilder.Entity<OfficeTimeDTO>()
                .ToTable("officeTime");
            modelBuilder.Entity<OfficeWeekDTO>()
                .ToTable("officeWeek");


            #endregion

            #region 信息动态

            modelBuilder.Entity<PageImgDTO>()
                .ToTable("PageImg");
            
            // 志愿组织对应图

            modelBuilder.Entity<ImageForOrganizionDTO>()
                .ToTable("ImageForOrganizion");

            // 志愿组织  对应的  志愿活动
            modelBuilder.Entity<ActivityForOrganizationDTO>()
                .HasOne(t=>t.DesImg)
                .WithOne(t=>t.ActivityForOrganizationDto)
                .HasForeignKey<ActivityForOrganizationDTO>(t=>t.ImageForActivity);

            modelBuilder.Entity<ActivityForOrganizationDTO>()
               .ToTable("ActivityForOrganization");
            // 志愿组织下的活动  默认4小时
            modelBuilder.Entity<ActivityForOrganizationDTO>()
                .Property(t => t.VolunteerTime)
                .HasDefaultValue(4);

            modelBuilder.Entity<ImageForActivityDTO>()
                .HasOne(t => t.ActivityForOrganizationDto)
                .WithOne(t => t.DesImg)
                .HasForeignKey<ImageForActivityDTO>(t => t.ActivityId);
 
            modelBuilder.Entity<ImageForActivityDTO>()
                .ToTable("ImageForActivity");

            #endregion

            #region 学生信息认证

            modelBuilder.Entity<SchoolUserInfoDTO>()
                .ToTable("SchoolUserInfo");

            #endregion

            #region 文件上传

            modelBuilder.Entity<FileUploadDTO>()
                .ToTable("FileUpload");

            #endregion
        }

        #region 寝室楼值班
        /// <summary>
        /// 寝室楼具体值班信息
        /// </summary>
        public virtual DbSet<DormitoryTableDTO> DormitoryTableDto { get; set; }
        /// <summary>
        /// 寝室楼 类型
        /// </summary>
        public virtual DbSet<DormitoryTypeDTO> DormitoryTypeDto { get; set; }
        /// <summary>
        /// 寝室楼  时间段类型
        /// </summary>
        public virtual DbSet<DormitoryTimeDTO> DormitoryTimeDto { get; set; }
        /// <summary>
        /// 寝室楼  周类型
        /// </summary>
        public virtual DbSet<DormitoryWeekDTO> DormitoryWeekDto { get; set; }
        #endregion

        #region 办公室值班
        /// <summary>
        /// 办公室  具体值班时间
        /// </summary>
        public virtual DbSet<OfficeTableDTO> OfficeTableDto { get; set; }
        /// <summary>
        /// 办公室类型
        /// </summary>
        public virtual DbSet<OfficeTypeDTO> OfficeTypeDto { get; set; }
        /// <summary>
        /// 办公室 值班 时间类型
        /// </summary>
        public virtual DbSet<OfficeTimeDTO> OfficeTimeDto { get; set; }

        /// <summary>
        /// 办公室  周类型
        /// </summary>
        public virtual DbSet<OfficeWeekDTO> OfficeWeekDto { get; set; }

        #endregion

        #region 报名
        /// <summary>
        /// 寝室楼报名表
        /// </summary>
        public virtual DbSet<DormitorySignDTO> DormitorySignDtos { get; set; }

        /// <summary>
        /// 办公室报名表
        /// </summary>
        public virtual DbSet<OfficeSignDTO> OfficeSignDtos { get; set; }
        /// <summary>
        /// 报名记录
        /// </summary>
        public DbSet<SignActivityNotesDTO> SignActivityNotesDtos { get; set; }

        #endregion

        #region 信息动态
        /// <summary>
        /// 文章动态
        /// </summary>
        public DbSet<PageInfoDTO> PageInfoDtos { get; set; }
        /// <summary>
        /// 文章动态所对于的图
        /// </summary>
        public DbSet<PageImgDTO> PageImgDtos { get; set; }

        #endregion

        #region 志愿组织
        /// <summary>
        /// 志愿组织信息
        /// </summary>
        public DbSet<OrganizationInfoDTO> OrganizationInfoDtos { get; set; }
        /// <summary>
        /// 志愿组织对应的图
        /// </summary>
        public DbSet<ImageForOrganizionDTO> ImageForOrganizionDtos { get; set; }
        /// <summary>
        /// 志愿组织对应的活动
        /// </summary>
        public DbSet<ActivityForOrganizationDTO> ActivityForOrganizationDtos { get; set; }
        /// <summary>
        /// 志愿活动所对应的图
        /// </summary>
        public DbSet<ImageForActivityDTO> ImgForActivityDtos { get; set; }
        /// <summary>
        /// 志愿组织对应活动的报名
        /// </summary>
        public DbSet<ActivitySignTableDTO> ActivitySignTableDtos { get; set; }
        #endregion

        #region 审核表

        public DbSet<DormitoryCheckDTO> DormitoryCheckDtos { get; set; }
        public DbSet<OfficeCheckDTO> OfficeCheckDtos { get; set; }

        #endregion

        #region 学生认证信息

        /// <summary>
        /// 存放学生认证信息
        /// </summary>
        public DbSet<SchoolUserInfoDTO> SchoolUserInfoDtos { get; set; }

        #endregion
        #region 文件处理
        /// <summary>
        /// 存放上传的所有文件
        /// </summary>
        public DbSet<FileUploadDTO> FileUploadDtos { get; set; }

        #endregion
    }
    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<VolunteerContext>
    {
        public VolunteerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VolunteerContext>();
            optionsBuilder.UseMySql("server=localhost;database=VolunteerServices.Base;port=3306;user=root;password=12345678;sslmode=none");

            return new VolunteerContext(optionsBuilder.Options);
        }
    }
}
