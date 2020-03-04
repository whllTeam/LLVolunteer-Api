using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Volunteer.Core.Entities.QueryModel
{
    public class SchoolUserInfoRequest
    {
        /// <summary>
        /// 登陆  用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 学生认证后  姓名
        /// </summary>
        [Required]
        public string SchoolUserName { get; set; }
        /// <summary>
        /// 学生认证后  学号
        /// </summary>
        [Required]
        public string UserId { get; set; }
    }
}
