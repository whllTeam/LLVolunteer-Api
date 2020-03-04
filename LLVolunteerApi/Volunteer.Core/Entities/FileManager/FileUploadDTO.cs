using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Base;

namespace Volunteer.Core.Entities.FileManager
{
    /// <summary>
    /// 文件 model (目前不和其它表建立关系)  之前 单独图片model 建立的 关系 废弃 
    /// 用于 和 动态进行关联
    /// 统一采用当前 数据库  进行  文件管理
    /// </summary>
    public class FileUploadDTO:BaseEntity
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 所在路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 格式（拓展名）
        /// </summary>
        public string FileExt { get; set; }
        /// <summary>
        /// 上传文件 归属功能
        /// </summary>
        public FileTypeEnum FileType{ get; set; }  
    }
}
