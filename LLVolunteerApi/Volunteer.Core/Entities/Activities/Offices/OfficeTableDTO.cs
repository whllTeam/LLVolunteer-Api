using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Activities.Offices
{
    /// <summary>
    /// 办公室  值班
    /// </summary>
    public class OfficeTableDTO:BaseEntity
    {
        public OfficeTableDTO()
        {
            OfficeTimeDtos = new List<OfficeTimeDTO>();
            OfficeTypeDtos = new List<OfficeTypeDTO>();
            OfficeWeekDto = new List<OfficeWeekDTO>();
        }
        /// <summary>
        /// 办公室类型  id
        /// </summary>
        public int OfficeTypeId { get; set; }

        public List<OfficeTypeDTO> OfficeTypeDtos { get; set; }
        /// <summary>
        /// 时间段 id
        /// </summary>
        public int TimeIntervalId { get; set; }

        public List<OfficeTimeDTO> OfficeTimeDtos { get; set; }
        /// <summary>
        /// 周 id
        /// </summary>
        public int OfficeWeekId { get; set; }

        public List<OfficeWeekDTO> OfficeWeekDto { get; set; }
        /// <summary>
        /// 当前时间段  是否允许
        /// </summary>
        public bool IsOpen { get; set; }
    }
}
