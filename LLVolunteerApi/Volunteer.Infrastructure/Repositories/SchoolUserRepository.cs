using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.SchoolUser;
using Volunteer.Core.Interfaces;
using Volunteer.Infrastructure.Database;

namespace Volunteer.Infrastructure.Repositories
{
    public class SchoolUserRepository: ISchoolUserRepository
    {
        public SchoolUserRepository(
            VolunteerContext context
        )
        {
            Context = context;
        }
        public VolunteerContext Context { get;  }
        /// <summary>
        /// 用户 是否认证
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> UserHasAuthorized(string userName)
        {
            return await Context.SchoolUserInfoDtos.AnyAsync(t => t.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase) && t.IsAuthorize == true);
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        public async Task<bool> AddSchoolUser(SchoolUserInfoRequest school)
        {
            var isExit = await Context.SchoolUserInfoDtos.AnyAsync(t =>
                t.IsAuthorize == true && t.IsDel == false
                                      && t.UserId == school.UserId
            );
            // 暂时取消限制
            //if (isExit)
            //{
            //    return false;
            //}
            var model = new SchoolUserInfoDTO()
            {
                UserName = school.UserName,
                IsAuthorize = true,
                SchoolUserName = school.SchoolUserName,
                UserId = school.UserId
            };
            Context.SchoolUserInfoDtos.Add(model);
            return await Context.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 通过登陆名  获取  学生认证信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<SchoolUserInfoDTO> GetSchoolUserByLoginName(string userName)
        {
            var model = await Context.SchoolUserInfoDtos
                .FirstOrDefaultAsync(t => t.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase));
            return model;
        }
        /// <summary>
        /// 通过学号  获取  学生认证信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<SchoolUserInfoDTO> GetSchoolUserByUserId(string userId)
        {
            var model = await Context.SchoolUserInfoDtos
                .FirstOrDefaultAsync(t => t.UserId == userId);
            return model;
        }
    }
}
