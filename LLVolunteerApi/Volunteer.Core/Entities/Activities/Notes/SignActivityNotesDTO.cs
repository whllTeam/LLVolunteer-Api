using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Activities.Dormitory;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Activities.Offices;
using Volunteer.Core.Entities.Base;
using Volunteer.Core.Entities.Organization;

namespace Volunteer.Core.Entities.Activities.Notes
{
    /// <summary>
    /// 记录 报名、取消报名记录
    /// </summary>
    public class SignActivityNotesDTO:BaseEntity
    {
        public SignActivityNotesDTO()
        {
            DormitoryTableDtos = new List<DormitoryTableDTO>();
            OfficeTableDtos = new List<OfficeTableDTO>();
            ActivitySignTableDtos = new List<ActivitySignTableDTO>();
        }
        /// <summary>
        /// 报名人姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 报名动作 （取消：1，报名：0）
        /// </summary>
        public SignType Action { get; set; }
        /// <summary>
        /// 值班表id
        /// </summary>
        public int VolunteerTableId { get; set; }
        /// <summary>
        /// 志愿服务类型
        /// </summary>
        public VolunteerType Type { get; set; }

        #region 确认关系字段

        public List<DormitoryTableDTO> DormitoryTableDtos { get; set; }
        public List<OfficeTableDTO> OfficeTableDtos { get; set; }

        public List<ActivitySignTableDTO> ActivitySignTableDtos { get; set; }
        #endregion
    }
}
