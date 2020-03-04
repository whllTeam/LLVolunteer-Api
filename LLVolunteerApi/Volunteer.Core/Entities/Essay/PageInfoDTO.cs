using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volunteer.Core.Entities.Base;
using Volunteer.Core.Entities.Organization;
using Volunteer.Core.Entities.ViewModel;

namespace Volunteer.Core.Entities.Essay
{
    /// <summary>
    /// 发布动态信息
    /// </summary>
    public class PageInfoDTO:BaseEntity
    {
        /// <summary>
        /// 发布者姓名
        /// </summary>
        public string PubliserhName { get; set; } = "admin";
        /// <summary>
        /// 标题
        /// </summary>
        public string  Title{ get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 所属志愿组织
        /// </summary>
        public OrganizationInfoDTO OrganizationInfoDto { get; set; }
        /// <summary>
        /// 志愿组织id
        /// </summary>
        public int OrganizationInfoId { get; set; }
        /// <summary>
        /// 所包含的图片
        /// </summary>
        public List<PageImgDTO> PageImgs { get; set; }
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
