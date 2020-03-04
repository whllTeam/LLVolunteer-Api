using System;
using System.Collections.Generic;
using System.Text;
using ConvertHelper;

namespace Volunteer.Core.Entities.Base
{
    public class BaseEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; } = false;

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; } = DateTime.Now.DataToStr();

        /// <summary>
        /// 排序号
        /// </summary>
        public int Sequence { get; set; } = 1;
    }
}
