using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Interfaces;
using Volunteer.Infrastructure.Database;

namespace Volunteer.WebApi.Controllers
{
    /// <summary>
    /// 办公室 API
    /// </summary>
    [Route("api/office")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly IOfficeRepository _officeRepository;
        public OfficesController(IOfficeRepository repository)
        {
            _officeRepository = repository;
        }
        /// <summary>
        /// 办公室值班表信息
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOfficeTable([FromQuery]QueryParameters parameters)
        {
            var postList = await _officeRepository.GetOfficeTable(parameters);
            return Ok(postList);
        }
        /// <summary>
        /// 办公室 类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("officeType")]
        public async Task<IActionResult> GetOfficeType()
        {
            var data = await _officeRepository.GetOfficeType();
            return Ok(data);
        }
        /// <summary>
        /// 办公室时间段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("timeDay")]
        public async Task<IActionResult> GetTime()
        {
            var data = await _officeRepository.GetTimes();
            return Ok(data);
        }
        /// <summary>
        /// 周信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("week")]
        public async Task<IActionResult> GetWeek()
        {
            var data = await _officeRepository.GetOfficeWeek();
            return Ok(data);
        }
        /// <summary>
        /// 当前时间段  办公室报名信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("sign")]
        public async Task<IActionResult> GetSignInfo([FromQuery] SignQuery query)
        {
            var data = await _officeRepository.GetSignInfo(query);
            return Ok(data);
        }
        /// <summary>
        /// 当前用户报名信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("signSelf")]
        public async Task<IActionResult> GetSignInfoSelf([FromQuery] SignQuery query)
        {
            if (string.IsNullOrEmpty(query.UserName))
            {
                return BadRequest("userName不能为空");
            }
            var data = await _officeRepository.GetSignSelfInfo(query);
            return Ok(data);
        }
        /// <summary>
        /// 报名/取消报名 寝室楼值班
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("sign")]
        public async Task<IActionResult> DormitorySign([FromBody] SignQuery query)
        {
            var data = await _officeRepository.OfficeSign(query);
            return Ok(data);
        }
        /// <summary>
        /// 当前用户  寝室楼值班报名次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("signCount")]
        public async Task<IActionResult> GetOfficeSignCount([FromQuery] SignQuery query)
        {
            if (string.IsNullOrEmpty(query.UserName))
            {
                return BadRequest("UserName 不能为空");
            }
            var result = await _officeRepository.GetOfficeSignCount(query);
            return Ok(result);
        }
        /// <summary>
        /// 当前用户  办公室报名记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("signDetailInfo")]
        public async Task<IActionResult> GetOfficeSignDetailInfo([FromQuery] SignQuery query)
        {
            var result = await _officeRepository.GetOfficeSignDetailInfo(query);
            return Ok(result);
        }
        /// <summary>
        /// admin端  办公室报名信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("signInfoQuery")]
        public async Task<IActionResult> GetDormitorySignInfoQuery([FromQuery] SignQuery query)
        {
            if (query.PageIndex <= 0)
            {
                query.PageIndex = 1;
            }
            var data = await _officeRepository.GetOfficeSignInfoQuery(query);
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
        /// 办公室报名 审核
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("checkState")]
        public IActionResult CheckStateForOffice([FromBody] CheckStateRequest request)
        {
            var result = _officeRepository.CheckStateForOffice(request);
            return Ok(result);
        }
        /// <summary>
        /// 获取 办公室审核 值班信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("checkStateInfo")]
        public async Task<IActionResult> GetCheckTableInfo([FromQuery]CheckStateInfoRequest request)
        {
            var result = await _officeRepository.GetOfficeCheckTableInfo(request);
            return Ok(result);
        }
    }
}