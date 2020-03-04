using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.FileManager;

namespace Volunteer.Core.Entities.ViewModel
{
    public class FileManagerInfo
    {
        public FileTypeEnum  FileType{ get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
