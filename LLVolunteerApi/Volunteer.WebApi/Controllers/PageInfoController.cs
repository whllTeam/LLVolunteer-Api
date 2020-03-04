using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheHelper.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Essay;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;
using Volunteer.Core.Interfaces;

namespace Volunteer.WebApi.Controllers
{
    /// <summary>
    /// 志愿动态API
    /// </summary>
    [Route("api/pageInfo")]
    [ApiController]
    public class PageInfoController : ControllerBase
    {
        private readonly IPageInfoRepository _pageInfoRepository;
        public IDistributedCache Cache { get; }
        public ILogger Logger { get; }
        public PageInfoController(
            IPageInfoRepository pageInfoRepository,
            IDistributedCache cache,
            ILogger<PageInfoController> logger)
        {
            _pageInfoRepository = pageInfoRepository;
            Cache = cache;
            Logger = logger;
        }
        /// <summary>
        /// 获取志愿动态信息
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPaperInfo([FromQuery] QueryParameters parms)
        {
            if (parms.PageIndex <= 0)
            {
                parms.PageIndex = 1;
            }

            ResponseList<List<PageInfoDTO>> data = null;
            data = await Cache.GetObjectAsync<ResponseList<List<PageInfoDTO>>>($"pageInfo_{parms.PageIndex}_{parms.PageSize}");
            if (data!=null)
            {
                Logger.LogInformation("从缓存中获取数据");
            }
            else
            {
                var localData = await _pageInfoRepository.GetPageInfo(parms);
                data = new ResponseList<List<PageInfoDTO>>
                {
                    HasNext = localData.HasNext,
                    HasPrevious = localData.HasPrevious,
                    PageCount = localData.PageCount,
                    PageIndex = localData.PageIndex,
                    PageSize = localData.PageSize,
                    TotalItemsCount = localData.TotalItemsCount,
                    Data = localData
                };
                Logger.LogInformation("从DB中获取数据");
                await Cache.SetObjectAsync($"pageInfo_{parms.PageIndex}_{parms.PageSize}", data);
            }

            return Ok(data);
        }
        /// <summary>
        /// 添加志愿动态信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddPageInfo([FromBody] PageInfoRequest model)
        {
            var result = await _pageInfoRepository.AddPageInfo(model);
            return Ok(result);
        }
        /// <summary>
        /// 删除志愿动态
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpDelete("{pageId}/{type}")]
        public async Task<IActionResult> DelPageInfo(int pageId, ShowType type)
        {
            var result = await _pageInfoRepository.DelPageInfo(pageId, type);
            return Ok(result);
        }
        /// <summary>
        /// 更新志愿动态
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{pageId}")]
        public async Task<IActionResult> ModifyPageInfo(int pageId, PageInfoRequest request)
        {
            if (_pageInfoRepository.GetPageInfo(pageId) == null)
            {
                return BadRequest();
            }
            request.Id = pageId;
            var result = await _pageInfoRepository.ModifyPageInfo(request);
            return Ok(result);
        }
        /// <summary>
        /// 文章 文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("file/{id}")]
        public async Task<IActionResult> GetFileList([FromRoute] int id)
        {
            var result = await _pageInfoRepository.GetFileList(id);
            return Ok(result);
        }
    }
}