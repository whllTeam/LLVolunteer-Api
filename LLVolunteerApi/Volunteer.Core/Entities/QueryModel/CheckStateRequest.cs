using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Volunteer.Core.Entities.QueryModel
{
    public class CheckStateRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 对应报名表id
        /// </summary>
        [Required]
        public int VolunteerSignId { get; set; }
        /// <summary>
        /// 星期id
        /// </summary>
        [Required]
        public int VolunteerWeekId { get; set; }
        /// <summary>
        /// 时间段Id
        /// </summary>
        [Required]
        public int TimeDayId { get; set; }
        /// <summary>
        /// 具体活动Id
        /// </summary>
        [Required]
        public int VolunteerTypeId { get; set; }
    }
}
