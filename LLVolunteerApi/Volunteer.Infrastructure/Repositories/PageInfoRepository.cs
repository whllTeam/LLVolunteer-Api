using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertHelper;
using Microsoft.EntityFrameworkCore;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Essay;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;
using Volunteer.Core.Interfaces;
using Volunteer.Infrastructure.Database;

namespace Volunteer.Infrastructure.Repositories
{
    public class PageInfoRepository : IPageInfoRepository
    {
        private readonly VolunteerContext _context;

        // 分层  未设计好
        private IFileManagerRepository Repository { get; }
        public PageInfoRepository(
            VolunteerContext context,
            IFileManagerRepository repository
            )
        {
            Repository = repository;
            _context = context;
        }
        public async Task<bool> AddPageInfo(PageInfoRequest request)
        {
            var model = new PageInfoDTO()
            {
                Content = request.Content,
                Description = request.Description,
                OrganizationInfoId = request.OrganizationInfoId,
                Title = request.Title,
        };
            _context.PageInfoDtos.Add(model);
            _context.SaveChanges();
            return await Repository.UploadFileBind(model.Id, request.FileInfo);
        }

        public async Task<bool> DelPageInfo(int pageId, ShowType type)
        {
            var model = _context.PageInfoDtos.FirstOrDefault(
                t => t.Id == pageId && 
                ((t.IsDel == false) || (type == ShowType.启用)));
            if (model != null)
            {
                model.IsDel = type == ShowType.关闭;
            }
            else
            {
                return false;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ModifyPageInfo(PageInfoRequest request)
        {
            var model = _context.PageInfoDtos.FirstOrDefault(t => t.Id == request.Id);
            if (model == null)
            {
                return false;
            }
            else
            {
                model.Content = request.Content;
                model.Description = request.Description;
                model.OrganizationInfoId = request.OrganizationInfoId;
                model.Title = request.Title;
                _context.SaveChanges();
                return await Repository.UploadFileBind(model.Id, request.FileInfo);
            }
        }

        public async Task<PaginatedList<PageInfoDTO>> GetPageInfo(QueryParameters parameters)
        {
            int totalCount = await _context.PageInfoDtos.CountAsync();
            var data = await _context.PageInfoDtos
                ?.Where(t => (t.IsDel == false || parameters.IsAdmin))
                ?.OrderBy(t => t.Sequence)
                ?.ThenBy(t => t.CreateTime)
                ?.Skip((parameters.PageIndex -1) * parameters.PageSize)
                ?.Take(parameters.PageSize)
                ?.Include(t => t.OrganizationInfoDto)
                //?.Include(t => t.PageImgs)    //使用新的  图片
                ?.ToListAsync()
                ;
            foreach (var page in data)
            {
                var imgList = await GetFileList(page.Id);
                page.PageImgs = imgList == null
                    ? new List<PageImgDTO>()
                    : imgList.Select(t => new PageImgDTO()
                    {
                        LocalPath = t.FilePath
                    }).ToList();
            }
            return new PaginatedList<PageInfoDTO>(parameters.PageIndex,parameters.PageSize, totalCount, data);
        }

        public PageInfoDTO GetPageInfo(int pageId)
        {
            var model = _context.PageInfoDtos.FirstOrDefault(t => t.Id == pageId);
            return model;
        }
        public async Task<List<FileManagerInfo>> GetFileList(int id)
        {
            var result = await _context.PageInfoDtos
                .FirstOrDefaultAsync(t => t.Id == id);
            if (result == null)
            {
                return null;
            }
            else
            {
                var imageIds = result.FileImageIds?.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var fileIds = result.FileExIds?.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var files = await _context.FileUploadDtos.Where(
                        t => (imageIds != null && imageIds.Any(s => s == t.Id.ToString())
                              || (fileIds != null && fileIds.Any(s => s == t.Id.ToString()))))
                    .Select(t => new FileManagerInfo()
                    {
                        FileId = t.Id,
                        FileName = t.FileName,
                        FileType = t.FileType,
                        FilePath = t.FilePath.Replace('\\', '/')
                    }).ToListAsync();
                return files;
            }
        }
    }
}
