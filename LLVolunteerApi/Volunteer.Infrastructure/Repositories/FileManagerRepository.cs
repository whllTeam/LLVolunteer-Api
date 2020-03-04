using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volunteer.Core.Entities.FileManager;
using Volunteer.Core.Entities.ViewModel;
using Volunteer.Core.Interfaces;
using Volunteer.Infrastructure.Database;

namespace Volunteer.Infrastructure.Repositories
{
    public class FileManagerRepository : IFileManagerRepository
    {
        public ILogger Logger { get; }
        public FileManagerRepository(
            VolunteerContext context,
            ILogger<FileManagerRepository> logger
            )
        {
            Context = context;
            Logger = logger;
        }
        public VolunteerContext Context { get; }
        /// <summary>
        /// 上传后文件  和  内容进行绑定
        /// </summary>
        /// <param name="uploadId"></param>
        /// <param name="fileManagerInfos"></param>
        /// <returns></returns>
        public async Task<bool> UploadFileBind(int uploadId, List<FileManagerInfo> fileManagerInfos)
        {
            if (fileManagerInfos==null || !fileManagerInfos.Any())
            {
                Logger.LogInformation("文件列表为空");
                return true;
            }
            Logger.LogInformation("文件列表不为空");
            // 原图片内容直接覆盖
            foreach (var file in fileManagerInfos.GroupBy(t => t.FileType))
            {
                string ids = string.Join(",", file.Select(f => f.FileId));
                switch (file.Key)
                {
                    case FileTypeEnum.志愿动态图片:
                    case FileTypeEnum.志愿动态附件:
                        var page = Context.PageInfoDtos.FirstOrDefault(t => t.Id == uploadId);
                        if (page == null)
                        {
                            throw new ArgumentException($"{nameof(file.Key)}类型不正确---{file.ToString()}");
                        }
                        else
                        {
                            if (file.Key == FileTypeEnum.志愿动态图片)
                            {
                                page.FileImageIds = ids;
                            }
                            else
                            {
                                page.FileExIds = ids;
                            }
                        }
                        break;
                    case FileTypeEnum.志愿组织图片:
                        var org = Context.OrganizationInfoDtos.FirstOrDefault(t => t.Id == uploadId);
                        if (org == null)
                        {
                            throw new ArgumentException($"{nameof(file.Key)}类型不正确---{file.ToString()}");
                        }
                        else
                        {
                            org.FileImageIds = ids;
                        }
                        break;
                    case FileTypeEnum.志愿组织活动图:
                        var activity = Context.ActivityForOrganizationDtos.FirstOrDefault(t => t.Id == uploadId);
                        if (activity == null)
                        {
                            throw new ArgumentException($"{nameof(file.Key)}类型不正确---{file.ToString()}");
                        }
                        else
                        {
                            activity.FileImageIds = ids;
                        }
                        break;
                }
            }

            return await Context.SaveChangesAsync() > 0;

        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="filePath"></param>
        /// <param name="realName"></param>
        /// <returns></returns>
        public async Task<FileManagerInfo> UploadFile(FileTypeEnum fileType, string realName, string filePath)
        {
            var model = new FileUploadDTO()
            {
                FileExt = Path.GetExtension(filePath),
                FileName = realName,
                FilePath = filePath,
                FileType = fileType
            };
            Context.FileUploadDtos.Add(model);
            await Context.SaveChangesAsync();
            return new FileManagerInfo()
            {
                FileId = model.Id,
                FileType = fileType
            };
        }
    }
}
