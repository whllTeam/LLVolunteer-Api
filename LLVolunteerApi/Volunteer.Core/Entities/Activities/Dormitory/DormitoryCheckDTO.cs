using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Activities.Dormitory
{
    public class DormitoryCheckDTO:BaseEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        public int DormitorySignId { get; set; }
        public List<DormitorySignDTO> DormitorySignDtos { get; set; }
        public int DormitoryWeekId { get; set; }
        /// <summary>
        /// 时间段Id
        /// </summary>
        public int TimeDayId { get; set; }
        /// <summary>
        /// 寝室楼Id
        /// </summary>
        public int DormitoryTypeId { get; set; }
    }
}
