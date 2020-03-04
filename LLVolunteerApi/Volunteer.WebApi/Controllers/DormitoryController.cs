using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Interfaces;

namespace Volunteer.WebApi.Controllers
{
    /// <summary>
    /// 寝室楼Api
    /// </summary>
    [Route("api/dormitory")]
    [ApiController]
    public class DormitoryController : ControllerBase
    {
        private readonly IDormitoryRepository _dormitoryRepository;
        public DormitoryController(IDormitoryRepository dormitoryRepository)
        {
            _dormitoryRepository = dormitoryRepository;
        }
        /// <summary>
        /// 获取寝室楼值班表
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDormitoryTable([FromQuery] QueryParameters parameters)
        {
            var data =  await _dormitoryRepository.GetDormitoryTable(parameters);

            return Ok(data);
        }
        /// <summary>
        /// 获取寝室楼类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("dormitoryType")]
        public async Task<IActionResult> GetDormitoryType()
        {
            var data = await _dormitoryRepository.GetDormitoryType();
            return Ok(data);
        }
        /// <summary>
        /// 获取寝室楼时间段信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("timeDay")]
        public async Task<IActionResult> GetTimeDay()
        {
            var data = await _dormitoryRepository.GeTimeDay();
            return Ok(data);
        }
        /// <summary>
        /// 获取寝室楼周信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("week")]
        public async Task<IActionResult> GetWeeks()
        {
            var data = await _dormitoryRepository.GetDormitoryWeek();
            return Ok(data);
        }
        /// <summary>
        /// 当前寝室楼报名信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("sign")]
        public async Task<IActionResult> GetSignInfo([FromQuery] SignQuery query)
        {
            var data = await _dormitoryRepository.GetSignInfo(query);
            return Ok(data);
        }
        /// <summary>
        /// 获取当前用户的寝室楼值班信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("signSelf")]
        public async Task<IActionResult> GetSignInfoSelf([FromQuery] SignQuery query)
        {
            var data = await _dormitoryRepository.GetSignSelfInfo(query);
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
            var data = await _dormitoryRepository.DormitorySign(query);
            return Ok(data);
        }
        /// <summary>
        /// 获取当前用户 寝室楼值班次数信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("signCount")]
        public async Task<IActionResult> GetDormitorySignCount([FromQuery] SignQuery query)
        {
            if (string.IsNullOrEmpty(query.UserName))
            {
                return BadRequest("UserName 不能为空");
            }
            var result = await _dormitoryRepository.GetDormitorySignCount(query);
            return Ok(result);
        }
        /// <summary>
        /// 当前用户 寝室楼报名记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("signDetailInfo")]
        public async Task<IActionResult> GetDormitorySignDetailInfo([FromQuery] SignQuery query)
        {
            var result = await _dormitoryRepository.GetDormitorySignDetailInfo(query);
            return Ok(result);
        }
        /// <summary>
        /// admin 端 寝室楼报名信息
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
            var data = await _dormitoryRepository.GetDormitorySignInfoQuery(query);
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
        /// 寝室楼值班  审核
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("checkState")]
        public IActionResult CheckStateForDormitory([FromBody] CheckStateRequest request)
        {
            var result =  _dormitoryRepository.CheckStateForDormitory(request);
            return Ok(result);
        }
        /// <summary>
        /// 获取 寝室楼审核 值班信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("checkStateInfo")]
        public async Task<IActionResult> GetCheckTableInfo([FromQuery]CheckStateInfoRequest request)
        {
            var result = await _dormitoryRepository.GetDormitoryCheckTableInfo(request);
            return Ok(result);
        }

    }
}