using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Activities.Dormitory
{
    /// <summary>
    /// 
    /// </summary>
    public class DormitoryTableDTO:BaseEntity
    {
        public DormitoryTableDTO()
        {
            DormitoryWeekDtos = new List<DormitoryWeekDTO>();
            DormitoryTimeDtos = new List<DormitoryTimeDTO>();
            DormitoryTypeDtos = new List<DormitoryTypeDTO>();
        }
        /// <summary>
        /// 周  id
        /// </summary>
        public int DormitoryWeekId { get; set; }
        public List<DormitoryWeekDTO> DormitoryWeekDtos { get; set; }
        /// <summary>
        /// 时间段Id
        /// </summary>
        public int TimeDayId { get; set; }

        public List<DormitoryTimeDTO>  DormitoryTimeDtos { get; set; }
        /// <summary>
        /// 寝室楼Id
        /// </summary>
        public int DormitoryTypeId { get; set; }

        public List<DormitoryTypeDTO> DormitoryTypeDtos { get; set; }
        /// <summary>
        /// 是否开启 （某天、某时段）
        /// </summary>
        public bool IsOpen { get; set; }
    }
}
