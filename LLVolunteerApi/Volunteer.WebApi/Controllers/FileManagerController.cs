using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Extensions.Convert.ConvertHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using UploadFileEx;
using Volunteer.Core.Entities.FileManager;
using Volunteer.Core.Interfaces;

namespace Volunteer.WebApi.Controllers
{
    [Route("api/FileManager")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {
        public FileManagerController(
            IFileManagerRepository repository
        )
        {
             Repository = repository;
        }
        public IFileManagerRepository Repository { get; set; }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> UploadFile()
        {
            string fileTypeStr = Request.Query["fileType"];
            if (string.IsNullOrEmpty(fileTypeStr))
            {
                return BadRequest("fileType cannot  null");
            }

            FileTypeEnum fileType = (FileTypeEnum)Enum.Parse(typeof(FileTypeEnum), fileTypeStr);
            string name = string.Empty;
            switch (fileType)
            {
                case FileTypeEnum.志愿动态附件:
                    name = "volunteerFile";
                    break;
                case FileTypeEnum.志愿动态图片:
                    name = "volunteerImage";
                    break;
                case FileTypeEnum.志愿组织图片:
                    name = "orgImage";
                    break;
                case FileTypeEnum.志愿组织活动图:
                    name = "orgActivityImage";
                    break;
            }
            string[] fileInfo = await Request.StreamFiles(Path.Combine("Upload",name));
            var result = await Repository.UploadFile(fileType, fileInfo[0] ,fileInfo[1]);
            return Ok(result);
        }
    }
}