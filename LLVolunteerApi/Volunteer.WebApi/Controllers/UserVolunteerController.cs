using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Interfaces;

namespace Volunteer.WebApi.Controllers
{
    /// <summary>
    /// 用户志愿管理API
    /// </summary>
    [Route("api/UserVolunteer")]
    [ApiController]
    public class UserVolunteerController : ControllerBase
    {
        private readonly IUserVolunteerRepository _repository;

        public UserVolunteerController(
            IUserVolunteerRepository resRepository
        )
        {
            _repository = resRepository;
        }
        /// <summary>
        /// 获取当前用户的待办信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("todoInfoQuery")]
        public async Task<IActionResult> GetTodoInfo([FromQuery]TodoInfoRequest request)
        {
            if (request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            var data = await _repository.GetTodoInfo(request);
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