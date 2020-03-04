using System;
using System.Collections.Generic;
using System.Text;

namespace Volunteer.Core.Entities.ViewModel
{
    public class SignData
    {
        public SignData()
        {
            Data = new Dictionary<string, int>();
        }
        /// <summary>
        /// 报名项目id
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// key:  星期Id、时间段Id   逗号分割
        /// value:  当前  所报名项目的 总人数
        /// </summary>
        public Dictionary<string,int> Data { get; set; }
    }
}
