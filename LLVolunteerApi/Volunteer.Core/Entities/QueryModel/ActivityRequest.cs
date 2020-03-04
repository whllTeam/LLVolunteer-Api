using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volunteer.Core.Entities.Organization;
using Volunteer.Core.Entities.ViewModel;

namespace Volunteer.Core.Entities.QueryModel
{
    public class ActivityRequest
    {
        public int Id { get; set; }
        [Required]
        public string ActivityName { get; set; }
        /// <summary>
        /// 志愿活动描述
        /// </summary>
        [Required]
        public string ActivityDes { get; set; }
        /// <summary>
        /// 启动时间
        /// </summary>
        [Required]
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public string EndTime { get; set; }
        /// <summary>
        /// 活动最大人数
        /// </summary>
        [Required]
        public int SignMaxNum { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        [Required]
        public double VolunteerTime { get; set; }

        public ImageForActivityDTO DesImg { get; set; }
        public int OrganizationInfoId { get; set; }
        public List<FileManagerInfo> FileInfo { get; set; }
    }
}
