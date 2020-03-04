using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Organization
{
    public class ImageForActivityDTO:BaseEntity
    {
        /// <summary>
        /// 本地路径
        /// </summary>
        public string LocalPath { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 图片类型 （.jpg .png）
        /// </summary>
        public string ImageType { get; set; }
        /// <summary>
        /// 所对应的活动
        /// </summary>
        public ActivityForOrganizationDTO ActivityForOrganizationDto { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public int ActivityId { get; set; }
    }
}
