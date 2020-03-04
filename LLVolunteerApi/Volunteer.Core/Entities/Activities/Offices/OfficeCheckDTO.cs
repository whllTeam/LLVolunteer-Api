using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Activities.Offices
{
    public class OfficeCheckDTO:BaseEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        public int OfficeSignId { get; set; }
        public List<OfficeSignDTO> OfficeSignDtos { get; set; }
        public int OfficeWeekId { get; set; }
        /// <summary>
        /// 时间段Id
        /// </summary>
        public int TimeDayId { get; set; }
        /// <summary>
        /// 寝室楼Id
        /// </summary>
        public int OfficeTypeId { get; set; }
    }
}
