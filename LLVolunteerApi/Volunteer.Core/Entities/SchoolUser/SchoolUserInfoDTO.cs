using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.SchoolUser
{
    public class SchoolUserInfoDTO:BaseEntity
    {
        /// <summary>
        /// 登陆  用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 学生认证后  姓名
        /// </summary>
        public string SchoolUserName { get; set; }
        /// <summary>
        /// 学生认证后  学号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 是否已认证  （存在  认证 取消情况）
        /// </summary>
        public bool IsAuthorize { get; set; }
    }
}
