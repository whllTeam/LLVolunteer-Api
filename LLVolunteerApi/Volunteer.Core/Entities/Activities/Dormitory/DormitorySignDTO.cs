using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Activities.Dormitory
{
    public class DormitorySignDTO:BaseEntity
    {
        /// <summary>
        /// login 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 寝室楼值班Id
        /// </summary>
        public int DormitoryId { get; set; }

        public List<DormitoryTableDTO> DormitoryDtos { get; set; }
        /// <summary>
        /// 报名次数
        /// </summary>
        public int SignCount { get; set; }
        /// <summary>
        /// 取消报名次数
        /// </summary>
        public int CancelSignCount { get; set; }
        /// <summary>
        /// 是否已经  报名
        /// </summary>
        public bool IsSign { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public CheckStateType CheckState { get; set; }
    }
}
