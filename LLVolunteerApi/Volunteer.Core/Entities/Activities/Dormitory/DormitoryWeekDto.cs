using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Activities.Dormitory
{
    public class DormitoryWeekDTO:BaseEntity
    {
        /// <summary>
        /// 周名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOpen { get; set; }
        /// <summary>
        /// 不允许 上午 、下午  时间 类型
        /// </summary>
        public string IsDontAllow { get; set; }

        public int WeekValue { get; set; } = 0;
    }
}
