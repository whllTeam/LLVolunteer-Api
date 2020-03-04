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
using Volunteer.Core.Entities.Organization;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Interfaces;

namespace Volunteer.WebApi.Controllers
{
    /// <summary>
    /// 志愿组织 API
    /// </summary>
    [Route("api/organization")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository _iOrganizationRepository;
        public IDistributedCache Cache { get; }
        public ILogger Logger { get; }

        public OrganizationController(
            IOrganizationRepository iOrganizationRepository,
            IDistributedCache cache,
            ILogger<OrganizationController> logger
            )
        {
            _iOrganizationRepository = iOrganizationRepository;
            Cache = cache;
            Logger = logger;
        }
        /// <summary>
        /// 获取  志愿组织信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOrganization()
        {
            List<OrganizationInfoDTO> data = null;
            data = await Cache.GetObjectAsync<List<OrganizationInfoDTO>>("organization");
            if (data!=null)
            {
                Logger.LogInformation("从缓存中获取数据");
            }
            else
            {
                data = (await _iOrganizationRepository.GetOrganization()).ToList();
                await Cache.SetObjectAsync("organization", data);
                Logger.LogInformation("从DB中获取数据");
            }
            return Ok(data);
        }
        /// <summary>
        /// 添加志愿组织信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddOrganization([FromBody] OrganizationRequest model)
        {
            var result = await _iOrganizationRepository.AddOrganization(model);
            return Ok(result);
        }
        /// <summary>
        /// 启用/关闭 志愿组织
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpDelete("{orgId}/{type}")]
        public async Task<IActionResult> DelOrganization(int orgId, ShowType type)
        {
            var result = await _iOrganizationRepository.DelOrganization(orgId,type);
            return Ok(result);
        }
        /// <summary>
        /// 更新志愿组织信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vo"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyOrganization(int id, OrganizationRequest vo)
        {
            var model = await _iOrganizationRepository.GetOrganization(id);
            if (model == null)
            {
                return BadRequest(false);
            }
            else
            {
                vo.Id = id;
                var result = await _iOrganizationRepository.ModifyOrganization(vo);
                return Ok(result);
            }
        }
        /// <summary>
        /// admin 端  查询  志愿组织信息
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<IActionResult> GetOrganizationForAdmin([FromQuery]QueryParameters parms)
        {
            if (parms.PageIndex <= 0)
            {
                parms.PageIndex = 1;
            }

            var data = await _iOrganizationRepository.GetOrganizationForAdmin(parms);
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
    }
}