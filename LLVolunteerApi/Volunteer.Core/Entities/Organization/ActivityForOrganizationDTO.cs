using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.Organization
{
    /// <summary>
    /// 志愿组织 所开展的活动
    /// </summary>
    public class ActivityForOrganizationDTO:BaseEntity
    {
        /// <summary>
        /// 志愿活动名称
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// 志愿活动描述
        /// </summary>
        public string ActivityDes { get; set; }
        /// <summary>
        /// 启动时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 报名最大所容纳的人数
        /// </summary>
        public int SignMaxNum { get; set; }
        /// <summary>
        /// 志愿服务时长
        /// </summary>
        public double VolunteerTime { get; set; }
        /// <summary>
        /// 所对应的志愿组织
        /// </summary>
        public OrganizationInfoDTO OrganizationInfoDto { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public int OrganizationInfoId { get; set; }

        /// <summary>
        /// 描述 图
        /// </summary>
        public ImageForActivityDTO DesImg{ get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public int ImageForActivity { get; set; }

        public bool IsOpen { get; set; }
        private ActivityStateType _activityState;

        /// <summary>
        /// 图片  保存 FileUploadDTO 多个主键， 逗号隔开
        /// </summary>
        public string FileImageIds { get; set; }
        /// <summary>
        /// 附件 保存 FileUploadDTO 多个主键， 逗号隔开
        /// </summary>
        public string FileExIds { get; set; }

        #region 自定义字段
        [NotMapped]
        public ActivityStateType ActivityState
        {
            get => GetStateType();
            set => _activityState = value;
        }
        /// <summary>
        /// 是否 可以报名
        /// </summary>
        [NotMapped]
        public bool CanSignActivity => ActivityState == ActivityStateType.进行中;
        [NotMapped]
        public string ActivityStateTypeStr => ActivityState.ToString();
        private ActivityStateType GetStateType()
        {
            try
            {
                var startTime = DateTime.Parse(StartTime);
                var endTime = DateTime.Parse(EndTime);
                if (DateTime.Now >= startTime && DateTime.Now <= endTime)
                {
                    return ActivityStateType.进行中;
                }
                else if (DateTime.Now <= startTime)
                {
                    return ActivityStateType.未开始;
                }
                else
                {
                    return ActivityStateType.已结束;
                }
            }
            catch
            {
                return ActivityStateType.已结束;
            }
        }
#endregion
    }
}
