using System;
using System.Collections.Generic;
using System.Text;

namespace Volunteer.Core.Entities.ViewModel
{
    public class CheckStateInfo
    {
        /// <summary>
        ///  登陆名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 认证后姓名
        /// </summary>
        public string RealUserName { get; set; }
        /// <summary>
        /// 是否认证
        /// </summary>
        public bool IsAuthorize { get; set; }
        public int VolunteerActivityId { get; set; }
        public int VolunteerWeekId { get; set; }
        public int VolunteerTimeId { get; set; }

        #region 自定义字段

        public string UserNameStr => IsAuthorize ? $"{RealUserName}-(已认证)" : $"{UserName}-(未认证)";

        #endregion
    }
}
