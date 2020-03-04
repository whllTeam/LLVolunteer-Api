using System;
using System.Collections.Generic;
using System.Text;

namespace Volunteer.Core.Entities.ViewModel
{
    public class ActivitySignInfoQuery
    {
        public string UserName { get; set; }
        public string ActivityName { get; set; }
        public string OrganizationName { get; set; }
        public string CreateTime { get; set; }
        public bool IsSign { get; set; }
        public string IsSignStr => IsSign ? "已报名" : "已取消";
        public int SignCount { get; set; }
        public int CancelCount { get; set; }
    }
}
