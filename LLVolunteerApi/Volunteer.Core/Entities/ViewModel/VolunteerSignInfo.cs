using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;

namespace Volunteer.Core.Entities.ViewModel
{
    public class VolunteerSignInfo
    {
        /// <summary>
        /// 报名人姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 志愿服务类型
        /// </summary>
        public VolunteerType Type { get; set; }
        /// <summary>
        /// 志愿项目具体名称
        /// </summary>
        public string VolunteerName { get; set; }

        public int VolunteerNameId { get; set; }
        public CheckStateType SignState { get; set; }
        public string SignStateStr => SignState.ToString();
        /// <summary>
        /// 星期
        /// </summary>
        public string WeekInfoName { get; set; }
        public int WeekId { get; set; }
        /// <summary>
        /// 具体时间段
        /// </summary>
        public string DetailTimes { get; set; }

        public int DetailTimeId { get; set; }

        public string CreateTime { get; set; }
        public string TypeStr => Type.ToString();
        public bool IsSign { get; set; }
        public string IsSignStr => IsSign ? "已报名" : "已取消";
        public int SignCount { get; set; }
        public int CancelCount { get; set; }
        public int WeekSequence { get; set; }
        public int TimeSequence { get; set; }
        public int SignTableId { get; set; }
        public string CancheckStr => CanCheck ? "有效" : "无效";
        private bool _canCheck;
        public bool CanCheck
        {
            get => CanCheckHandle() && SignState != CheckStateType.审核通过;
            set => _canCheck = value;
        }

        private bool CanCheckHandle()
        {
            var time = DateTime.Parse(CreateTime);
            var nowTime = DateTime.Now;
            // 本周  有效
            // sunday 0; monday 1
            if (
                (nowTime.DayOfWeek.GetHashCode() - time.DayOfWeek.GetHashCode()) >= 0
                && (nowTime - time).TotalDays < 7
            )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
