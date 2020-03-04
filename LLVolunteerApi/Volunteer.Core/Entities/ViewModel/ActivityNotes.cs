using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;

namespace Volunteer.Core.Entities.ViewModel
{
    public class ActivityNotes
    {
        /// <summary>
        /// 报名人姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 报名动作 （取消：1，报名：0）
        /// </summary>
        public SignType Action { get; set; }
        /// <summary>
        /// 志愿服务类型
        /// </summary>
        public VolunteerType Type { get; set; }
        /// <summary>
        /// 志愿项目具体名称
        /// </summary>
        public string VolunteerName { get; set; }
        /// <summary>
        /// 星期
        /// </summary>
        public string WeekInfo { get; set; }
        /// <summary>
        /// 具体时间段
        /// </summary>
        public string DetailTimes { get; set; }
        /// <summary>
        /// 志愿组织名称
        /// </summary>
        public string OrganizationName { get; set; }

        public string CreateTime { get; set; }
        public string TypeStr => Type.ToString();
    }
}
