using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheHelper.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Organization;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;
using Volunteer.Core.Interfaces;

namespace Volunteer.WebApi.Controllers
{
    /// <summary>
    /// 志愿组织下志愿活动Api
    /// </summary>
    [Route("api/activity")]
    [ApiController]
    public class ActivityForOrgController : ControllerBase
    {
        private readonly IActivityRepository _repository;
        public IDistributedCache Cache { get; }
        public ILogger Logger { get; }

        public ActivityForOrgController(
            IActivityRepository repository,
            IDistributedCache cache,
            ILogger<ActivityForOrgController> logger
        )
        {
            _repository = repository;
            Cache = cache;
            Logger = logger;
        }
        /// <summary>
        /// 获取 志愿活动
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActivities([FromQuery]QueryParameters parameters)
        {
            ResponseList<List<ActivityForOrganizationDTO>> data = null;
            Logger.LogInformation($"{nameof(GetActivities)}---controller");
            if (parameters.PageIndex <= 0)
            {
                parameters.PageIndex = 1;
            }
            data = await Cache.GetObjectAsync<ResponseList<List<ActivityForOrganizationDTO>>>($"getactivity_{parameters.PageIndex}_{parameters.PageSize}");
            if (data != null)
            {
                Logger.LogInformation($"从缓存中获取数据   key  getactivity_{parameters.PageIndex}_{parameters.PageSize}");
            }
            else
            {
                // 特殊复杂 类型  反序列化会报错
                var localResult = await _repository.GetActivities(parameters);
                data = new ResponseList<List<ActivityForOrganizationDTO>>
                {
                    HasNext = localResult.HasNext,
                    HasPrevious = localResult.HasPrevious,
                    PageCount = localResult.PageCount,
                    PageIndex = localResult.PageIndex,
                    PageSize = localResult.PageSize,
                    TotalItemsCount = localResult.TotalItemsCount,
                    Data = localResult
                };
                await Cache.SetObjectAsync($"getactivity_{parameters.PageIndex}_{parameters.PageSize}", data);
                Logger.LogInformation("从DB中获取数据");

            }
            return Ok(data);
        }
        /// <summary>
        /// 添加志愿活动
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddActivity([FromBody]ActivityRequest model)
        {
            var result = await _repository.AddActivity(model);
            return Ok(result);
        }
        /// <summary>
        /// 修改志愿活动
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vo"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyActivity(int id, ActivityRequest vo)
        {
            var model = await _repository.GetActivity(id);
            if (model == null)
            {
                return BadRequest(false);
            }
            else
            {
                vo.Id = id;
            }
            var result = await _repository.ModifyActivity(vo);
            return Ok(result);
        }
        /// <summary>
        /// 修改 当前活动是否开启
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{type}")]
        public async Task<IActionResult> DelActivity(int id, ShowType type)
        {
            var model = await _repository.GetActivity(id);
            if (model == null)
            {
                return BadRequest(false);
            }
            else
            {
                var result = await _repository.DelActivity(id, type);
                return Ok(result);
            }
        }
        /// <summary>
        /// 报名、取消报名
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("sign")]
        public IActionResult ActivitySign([FromBody]SignQuery query)
        {
            var data = _repository.ActivitySign(query);
            return Ok(data);
        }
        /// <summary>
        /// 得到当前活动报名信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("sign")]
        public async Task<IActionResult> GetSignInfo([FromQuery] SignQuery query)
        {
            var data = await _repository.GetSignInfo(query);
            return Ok(data);
        }
        /// <summary>
        /// 得到当前用户报名信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("signSelf")]
        public async Task<IActionResult> GetSignInfoSelf([FromQuery] SignQuery query)
        {
            if (string.IsNullOrEmpty(query.UserName))
            {
                return BadRequest();
            }
            else
            {
                var data = await _repository.GetSignSelfInfo(query);
                return Ok(data);
            }
        }
        /// <summary>
        /// 得到活动报名记录信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("signInfoQuery")]
        public async Task<IActionResult> GetActivitySignInfoQuery([FromQuery]QueryParameters query)
        {
            if (query.PageIndex <= 0)
            {
                query.PageIndex = 1;
            }
            var data = await _repository.GetActivitySignInfoQuery(query);
            var result = new
            {
                data.HasNext,
                data.HasPrevious,
                data.PageCount,
                data.PageIndex,
                data.PageSize,
                data.TotalItemsCount,
                data
            };
            return Ok(result);
        }
        /// <summary>
        /// admin 端： 获取志愿组织下  志愿活动 报名信息
        /// </summary>
        /// 
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("activitySignInfo")]
        public async Task<IActionResult> GetActivitySignInfoTable([FromQuery]QueryParameters query)
        {
            if (query.PageIndex <= 0)
            {
                query.PageIndex = 1;
            }
            var data = await _repository.GetActivitySignInfoTable(query);
            var result = new
            {
                data.HasNext,
                data.HasPrevious,
                data.PageCount,
                data.PageIndex,
                data.PageSize,
                data.TotalItemsCount,
                data
            };
            return Ok(result);
        }
        /// <summary>
        /// 获取志愿组织下志愿活动 文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("file/{id}")]
        public async Task<IActionResult> GetFileList([FromRoute] int id)
        {
            var result = await _repository.GetFileList(id);
            return Ok(result);
        }
    }
}