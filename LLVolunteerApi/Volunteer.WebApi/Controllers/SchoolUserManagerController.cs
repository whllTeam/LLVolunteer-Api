using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.SchoolUser;
using Volunteer.Core.Interfaces;

namespace Volunteer.WebApi.Controllers
{
    /// <summary>
    /// 学生认证 API
    /// </summary>
    [Route("api/SchoolUserManager")]
    [ApiController]
    public class SchoolUserManagerController : ControllerBase
    {
        public SchoolUserManagerController(
            ISchoolUserRepository schoolUserRepository
        )
        {
            SchoolUserRepository = schoolUserRepository;
        }
        public ISchoolUserRepository SchoolUserRepository { get; set; }
        /// <summary>
        /// 添加  学生认证信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddSchoolUserInfo([FromBody] SchoolUserInfoRequest model)
        {
            var data = await SchoolUserRepository.AddSchoolUser(model);
            return Ok(data);
        }
        /// <summary>
        /// 通过登录名获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("name")]
        public async Task<IActionResult> GetUserInfoByName([FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest($"{nameof(userName)}不能为空");
            }
            var data = await SchoolUserRepository.GetSchoolUserByLoginName(userName);
            return Ok(data);
        }
        /// <summary>
        /// 通过学号查询  
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("id")]
        public async Task<IActionResult> GetUserInfoById([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest($"{nameof(userId)}不能为空");
            }
            var data = await SchoolUserRepository.GetSchoolUserByUserId(userId);
            return Ok(data);
        }
        /// <summary>
        /// 确认当前用户是否已认证
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("user")]
        public async Task<IActionResult> UserHasAuthorized([FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest($"{nameof(userName)}不能为空");
            }
            var data = await SchoolUserRepository.UserHasAuthorized(userName);
            return Ok(data);
        }
    }
}