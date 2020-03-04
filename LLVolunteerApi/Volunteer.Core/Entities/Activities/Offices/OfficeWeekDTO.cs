using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Activities.Offices
{
    public class OfficeWeekDTO:BaseEntity
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
        /// 不允许  开启  时间段类型
        /// </summary>
        public string IsDontAllow { get; set; }

        public int WeekValue { get; set; } = 0;
    }
}
