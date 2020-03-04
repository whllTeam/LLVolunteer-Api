using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;

namespace Volunteer.Core.Entities.ViewModel
{
    public class VolunteerCount
    {
        /// <summary>
        /// 具体志愿服务名称
        /// </summary>
        public string VolunteerName { get; set; }
        /// <summary>
        /// 服务id
        /// </summary>
        public int VolunteerId { get; set; }
        /// <summary>
        /// 志愿服务类型
        /// </summary>
        public VolunteerType VolunteerType { get; set; }
        /// <summary>
        /// 值班次数
        /// </summary>
        public int Count { get; set; }
    }
}
