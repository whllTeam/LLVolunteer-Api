using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.SchoolUser;

namespace Volunteer.Core.Interfaces
{
    public interface ISchoolUserRepository
    {
        /// <summary>
        /// 用户是否认证
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<bool> UserHasAuthorized(string userName);
        /// <summary>
        /// 添加  学生认证信息
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        Task<bool> AddSchoolUser(SchoolUserInfoRequest school);
        /// <summary>
        /// 通过登陆名  获取  学生认证信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<SchoolUserInfoDTO> GetSchoolUserByLoginName(string userName);
        /// <summary>
        /// 通过学号  获取  学生认证信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<SchoolUserInfoDTO> GetSchoolUserByUserId(string userId);
    }
}
