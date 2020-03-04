using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;

namespace Volunteer.Core.Entities.QueryModel
{
    public class TodoInfoRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 是否当前周
        /// </summary>
        [Required]
        public bool IsCurrentWeek { get; set; } = true;

        public VolunteerType VolunteerType { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
