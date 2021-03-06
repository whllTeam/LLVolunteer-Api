﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Volunteer.Core.Entities.ViewModel
{
    public class ActivitySignData
    {
        /// <summary>
        /// 志愿组织id
        /// </summary>
        public int OrganizationId { get; set; }
        /// <summary>
        /// 活动id
        /// </summary>
        public int ActivityId { get; set; }
        /// <summary>
        /// 报名人数
        /// </summary>
        public int SignCount { get; set; }
    }
}
