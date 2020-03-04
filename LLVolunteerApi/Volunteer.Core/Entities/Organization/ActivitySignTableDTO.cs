using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Organization
{
    public class ActivitySignTableDTO : BaseEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public int ActivityForOrganizationId { get; set; }
        public List<ActivityForOrganizationDTO> ActivityForOrganizationDtos { get; set; }

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
    }
}
