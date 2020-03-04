using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Activities.Offices;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Activities.Dormitory
{
    /// <summary>
    /// 时间段（列如: 上午 ， 下午）
    /// </summary>
    public class DormitoryTimeDTO:BaseEntity
    {
        /// <summary>
        /// 时间段名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOpen { get; set; }
        /// <summary>
        /// 不允许  开启  寝室楼类型
        /// </summary>
        public string IsDontAllow { get; set; }
    }
}
