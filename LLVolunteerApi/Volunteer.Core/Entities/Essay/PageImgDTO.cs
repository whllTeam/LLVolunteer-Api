using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Essay
{
    /// <summary>
    /// 文章所包含的图片
    /// </summary>
    public class PageImgDTO:BaseEntity
    {
        /// <summary>
        /// 本地路径
        /// </summary>
        public string LocalPath { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        public int  Width{ get; set; }
        /// <summary>
        /// 高
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 图片类型 （.jpg .png）
        /// </summary>
        public string ImageType { get; set; }
        /// <summary>
        /// 所对于的文章
        /// </summary>
        public PageInfoDTO PageInfoDto { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public int PageInfoId { get; set; }
    }
}
