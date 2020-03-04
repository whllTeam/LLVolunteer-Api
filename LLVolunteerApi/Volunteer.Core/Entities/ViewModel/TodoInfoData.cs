using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;

namespace Volunteer.Core.Entities.ViewModel
{
    public class TodoInfoData
    {
        public VolunteerType VolunteerType { get; set; }
        public string VolunteerTypeStr => VolunteerType.ToString();
        /// <summary>
        /// 寝室楼、 宿舍楼  对应 相应枚举值
        /// 志愿活动  对应 相应表 id  值
        /// </summary>
        public int VolunteerActivityId { get; set; }
        public string VolunteerActivityStr { get; set; }
        #region 志愿组织下 志愿活动

        /// <summary>
        /// 志愿组织 id
        /// </summary>
        public int OrgId { get; set; }
        /// <summary>
        /// 志愿组织名称
        /// </summary>
        public string OrgName { get; set; }

        public string CreateTime { get; set; }
        public int WeekValue { get; set; }
        public string WeekName { get; set; }
        public string TimeName { get; set; }
        #endregion
        public string TodoTime { get; set; }
        public bool IsValid => string.IsNullOrEmpty(TodoTime) ? false : DateTime.Now < DateTime.Parse(TodoTime);
        public string TodoState => IsValid ? "有效" : "已过期";
    }
}
