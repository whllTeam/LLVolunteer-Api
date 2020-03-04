using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Dormitory;
using Volunteer.Core.Entities.Activities.Notes;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;

namespace Volunteer.Core.Interfaces
{
    public interface IDormitoryRepository
    {
        /// <summary>
        /// 寝室楼  值班表
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<PaginatedList<DormitoryTableDTO>> GetDormitoryTable(QueryParameters parameters);
        /// <summary>
        /// 寝室楼 类型
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DormitoryTypeDTO>> GetDormitoryType();
        /// <summary>
        /// 寝室楼 时间段
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DormitoryTimeDTO>>  GeTimeDay();
        /// <summary>
        /// 寝室楼 周类型
        /// </summary>
        Task<IEnumerable<DormitoryWeekDTO>> GetDormitoryWeek();
        /// <summary>
        /// 获得范围内的报名信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Task<IEnumerable<SignData>> GetSignInfo(SignQuery query);
        /// <summary>
        /// 得到某用户的报名信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<SelfSignData>> GetSignSelfInfo(SignQuery query);
        /// <summary>
        /// 报名、取消报名
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="weekId"></param>
        /// <param name="timeId"></param>
        /// <param name="isCancel"></param>
        /// <returns></returns>
        Task<bool> DormitorySign(SignQuery query);

        /// <summary>
        /// 得到办公室值班报名的次数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IList<VolunteerCount>> GetDormitorySignCount(SignQuery query);
        /// <summary>
        /// 得到办公室报名明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<SignActivityNotesDTO>> GetDormitorySignDetailInfo(SignQuery query);

        Task<PaginatedList<VolunteerSignInfo>> GetDormitorySignInfoQuery(SignQuery query);
        bool CheckStateForDormitory(CheckStateRequest request);
        Task<List<CheckStateInfo>> GetDormitoryCheckTableInfo(CheckStateInfoRequest request);
    }
}
