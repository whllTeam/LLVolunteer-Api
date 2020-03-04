using System;
using System.Collections.Generic;
using System.Text;

namespace Volunteer.Core.Entities.ViewModel
{
    /// <summary>
    /// 返回用户 已报名的信息
    /// </summary>
    public class SelfSignData
    {
        public int WeekId { get; set; }
        public int TimeId { get; set; }
        public int VolunteerId { get; set; }
    }
}
