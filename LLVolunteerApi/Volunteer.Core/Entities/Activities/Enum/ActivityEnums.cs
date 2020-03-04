using System;
using System.Collections.Generic;
using System.Text;

namespace Volunteer.Core.Entities.Activities.Enum
{
    /// <summary>
    /// 寝室楼 枚举
    /// </summary>
    public enum DormitoryTypeEnum
    {
        寝室楼4B = 1,
        寝室楼6A,
        寝室楼6B
    }
    /// <summary>
    /// 寝室楼时间段枚举
    /// </summary>
    public enum DormitoryTimeEnum
    {
        中午 = 1,
        晚上
    }
    /// <summary>
    /// 星期枚举
    /// </summary>
    public enum WeekEnum
    {
        星期一 = 1,
        星期二,
        星期三,
        星期四,
        星期五
    }
    /// <summary>
    /// 办公室类型 枚举
    /// </summary>
    public enum OfficeTypeEnum
    {
        B119 = 1,
        B224
    }
    /// <summary>
    /// 办公室时间段
    /// </summary>
    public enum OfficeTimeEnum
    {
        节课12 = 1,
        节课34,
        节课56,
        节课78,
    }
    /// <summary>
    /// 志愿服务类型
    /// </summary>
    public enum VolunteerType
    {
        其它 = 0,
        寝室楼 = 1,
        办公室 = 2,
        志愿组织活动 = 3,
        所有 = 99
    }

    public enum ShowType
    {
        启用 = 1,
        关闭 = 2
    }

    public enum CheckStateType
    {
        未审核 =1,
        审核通过 = 2,
        审核未通过 =3,
        已过期 = 4
    }

    public enum ActivityStateType
    {
        已结束 = 1,
        进行中 = 2,
        未开始 = 3
    }
}
