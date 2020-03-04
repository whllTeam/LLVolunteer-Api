using System;
using System.Collections.Generic;
using System.Text;

namespace Volunteer.Core.Entities.QueryModel
{
    public class OrganizationRequest
    {
        public int Id { get; set; }
        /// <summary>
        /// 组织姓名
        /// </summary>
        public string OrganizerName { get; set; }
        /// <summary>
        /// 来自于 (学校、社会)
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }
    }
}
