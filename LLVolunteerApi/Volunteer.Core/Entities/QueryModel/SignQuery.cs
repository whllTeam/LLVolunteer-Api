using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;

namespace Volunteer.Core.Entities.QueryModel
{
    public class SignQuery
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 是否报名
        /// </summary>
        public SignType IsSign { get; set; }
        /// <summary>
        /// 周id
        /// </summary>
        public int WeekId { get; set; }
        /// <summary>
        /// 时间段id
        /// </summary>
        public int TimeId { get; set; }
        /// <summary>
        /// 志愿服务 所包含内容id
        /// </summary>
        public int VolunteerTypeId { get; set; }
        /// <summary>
        /// 志愿服务类型
        /// </summary>
        public VolunteerType? VolunteerType { get; set; }

        public bool IsAll { get; set; } = false;

        #region admin  使用

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool IgnoreTime { get; set; } = false;

        #endregion
    }
}
