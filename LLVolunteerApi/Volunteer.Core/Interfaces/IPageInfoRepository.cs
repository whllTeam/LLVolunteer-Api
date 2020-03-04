using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Essay;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;

namespace Volunteer.Core.Interfaces
{
    public interface IPageInfoRepository
    {
        /// <summary>
        /// 获取文章信息
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<PaginatedList<PageInfoDTO>> GetPageInfo(QueryParameters parameters);
        /// <summary>
        /// 添加一篇文章信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddPageInfo(PageInfoRequest request);
        /// <summary>
        /// 删除一篇文章（假删除）
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        Task<bool> DelPageInfo(int pageId, ShowType type);
        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> ModifyPageInfo(PageInfoRequest request);

        PageInfoDTO GetPageInfo(int pageId);
        Task<List<FileManagerInfo>> GetFileList(int id);
    }
}
