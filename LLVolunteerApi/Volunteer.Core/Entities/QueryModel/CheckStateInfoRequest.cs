using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;

namespace Volunteer.Core.Entities.QueryModel
{
    public class CheckStateInfoRequest
    {
        /// <summary>
        /// 当前周  数据
        /// </summary>
        public bool IsCurrentWeek { get; set; } =true;
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public VolunteerType VolunteerType { get; set; }
        /// <summary>
        /// 志愿类型  下 活动id
        /// </summary>
        public int VolunteerActivityId { get; set; }
    }
}
