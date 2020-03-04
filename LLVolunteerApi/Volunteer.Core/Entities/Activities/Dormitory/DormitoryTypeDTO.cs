using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Activities.Offices;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Activities.Dormitory
{
    /// <summary>
    /// 办公室类型
    /// </summary>
    public class DormitoryTypeDTO:BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOpen { get; set; }
        /// <summary>
        /// 寝室楼 性别
        /// </summary>
        public GenderType Gender { get; set; }
        /// <summary>
        /// 是否允许 跨性别 访问
        /// </summary>
        public bool IsAllowGender { get; set; }
        /// <summary>
        /// 志愿服务时长
        /// </summary>
        public double VolunteerTime { get; set; }
    }
}
