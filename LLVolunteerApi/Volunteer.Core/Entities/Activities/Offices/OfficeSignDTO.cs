using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Activities.Offices
{
    public class OfficeSignDTO:BaseEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 办公室值班Id
        /// </summary>
        public int OfficeId { get; set; }

        public List<OfficeTableDTO> OfficeDtos { get; set; }
        /// <summary>
        /// 报名次数
        /// </summary>
        public int SignCount { get; set; }
        /// <summary>
        /// 取消报名次数
        /// </summary>
        public int CancelSignCount { get; set; }
        /// <summary>
        /// 是否  报名
        /// </summary>
        public bool IsSign { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public CheckStateType CheckState { get; set; }
        
    }
}
