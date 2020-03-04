using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volunteer.Core.Entities.FileManager;
using Volunteer.Core.Entities.ViewModel;

namespace Volunteer.Core.Interfaces
{
    public interface IFileManagerRepository
    {
        Task<FileManagerInfo> UploadFile(FileTypeEnum fileType,string realName, string filePath);
        Task<bool> UploadFileBind(int uploadId, List<FileManagerInfo> fileManagerInfos);
    }
}
