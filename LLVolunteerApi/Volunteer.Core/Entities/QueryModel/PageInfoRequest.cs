using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using Volunteer.Core.Entities.ViewModel;

namespace Volunteer.Core.Entities.QueryModel
{
    public class PageInfoRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int OrganizationInfoId { get; set; }
        [Required]
        public string Title { get; set; }
        public List<FileManagerInfo> FileInfo { get; set; }
    }
}
