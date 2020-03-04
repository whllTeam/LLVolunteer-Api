using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Activities.Offices
{
    /// <summary>
    /// 时间段 （例如：上午 1-2 节）
    /// </summary>
    public class OfficeTimeDTO:BaseEntity
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
        /// 不允许  办公室 类型
        /// </summary>
        public string IsDontAllow { get; set; }
    }
}
