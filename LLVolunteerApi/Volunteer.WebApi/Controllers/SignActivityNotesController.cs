using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheHelper.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;
using Volunteer.Core.Interfaces;

namespace Volunteer.WebApi.Controllers
{
    /// <summary>
    /// 志愿活动报名动态API
    /// </summary>
    [Route("api/signActivityNotes")]
    [ApiController]
    public class SignActivityNotesController : ControllerBase
    {
        private ISignActivityNotesRepository _signActivityNotesRepository;
        public IDistributedCache Cache { get; }
        public ILogger Logger { get; }
        public SignActivityNotesController(
            ISignActivityNotesRepository signActivityNotesRepository,
            IDistributedCache cache,
            ILogger<SignActivityNotesController> logger
            )
        {
            _signActivityNotesRepository = signActivityNotesRepository;
            Cache = cache;
            Logger = logger;
        }
        /// <summary>
        /// 获取当前用户的报名记录
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSignActivityNotes([FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest();
            }
            else
            {
                var result = await _signActivityNotesRepository.GetSignActivityNotes(userName);
                return Ok(result);
            }
        }
        /// <summary>
        /// 获取所有用户的报名动态
        /// </summary>
        /// <returns></returns>
        [HttpGet("signNotes")]
        public async Task<IActionResult> GetSignActivityNotesAll()
        {
            List<ActivityNotes> data = null;
            data = await Cache.GetObjectAsync<List<ActivityNotes>>("signNotes");
            if (data!=null)
            {
                Logger.LogInformation("从缓存中获取数据");
            }
            else
            {
                data = (await _signActivityNotesRepository.GetSignActivityNotesAll()).ToList();
                await Cache.SetObjectAsync("signNotes", data);
                Logger.LogInformation("从DB中获取数据");
            }
            return Ok(data);
        }
    }
}