using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;
using Volunteer.Core.Entities.Essay;

namespace Volunteer.Core.Entities.Organization
{
    /// <summary>
    /// 志愿组织信息
    /// </summary>
    public class OrganizationInfoDTO:BaseEntity
    {
        /// <summary>
        /// 组织姓名
        /// </summary>
        public string OrganizerName { get; set; }
        /// <summary>
        /// 来自于 (学校、社会)
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Logo地址
        /// </summary>
        public string LogoUrl { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 发表文章
        /// </summary>
        public List<PageInfoDTO> PageInfoDtos{ get; set; }
        /// <summary>
        /// 志愿组织 所对于的图
        /// </summary>
        public List<ImageForOrganizionDTO> ImageForOrganizionDtos { get; set; }
        /// <summary>
        /// 所包含的志愿活动
        /// </summary>
        public List<ActivityForOrganizationDTO> ActivityForOrganizationDtos { get; set; }

        /// <summary>
        /// 图片  保存 FileUploadDTO 多个主键， 逗号隔开
        /// </summary>
        public string FileImageIds { get; set; }
        /// <summary>
        /// 附件 保存 FileUploadDTO 多个主键， 逗号隔开
        /// </summary>
        public string FileExIds { get; set; }
    }
}
