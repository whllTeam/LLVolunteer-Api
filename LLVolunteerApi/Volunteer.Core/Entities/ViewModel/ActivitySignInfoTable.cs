using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Organization;

namespace Volunteer.Core.Entities.ViewModel
{
    public class ActivitySignInfoTable:ActivityForOrganizationDTO
    {
        /// <summary>
        ///  活动报名 名单
        /// </summary>
        public List<string> SignUserInfo { get; set; }
        /// <summary>
        /// 字符串形式
        /// </summary>
        public string SignUserInfoStr => SignUserInfo != null ? string.Join(" ", SignUserInfo) : "";
        /// <summary>
        /// 当前已报名的人数
        /// </summary>
        public int HasSignCount { get; set; }
        /// <summary>
        /// 志愿组织名称
        /// </summary>
        public string OrganizationName { get; set; }
    }
}
