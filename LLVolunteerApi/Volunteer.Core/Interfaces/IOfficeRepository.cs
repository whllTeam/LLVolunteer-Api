using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Notes;
using Volunteer.Core.Entities.Activities.Offices;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;

namespace Volunteer.Core.Interfaces
{
    public interface IOfficeRepository
    {
        /// <summary>
        /// 分页  获取  办公室值班表
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<PaginatedList<OfficeTableDTO>> GetOfficeTable(QueryParameters parameters);
        /// <summary>
        /// 办公室类型
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<OfficeTypeDTO>> GetOfficeType();
        /// <summary>
        /// 办公室  周类型
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<OfficeWeekDTO>> GetOfficeWeek();
        /// <summary>
        /// 办公室  时间段
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<OfficeTimeDTO>> GetTimes();

        Task<IEnumerable<OfficeSignDTO>> GetOfficeSignInfo(SignQuery query);
        /// <summary>
        /// 获得范围内的报名信息
        /// </summary>
        /// <param name="query"></param>
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
        /// <param name="query"></param>
        /// <returns></returns>
        Task<bool> OfficeSign(SignQuery query);

        Task<IEnumerable<VolunteerCount>> GetOfficeSignCount(SignQuery query);
        Task<IEnumerable<SignActivityNotesDTO>> GetOfficeSignDetailInfo(SignQuery query);
        Task<PaginatedList<VolunteerSignInfo>> GetOfficeSignInfoQuery(SignQuery query);
        bool CheckStateForOffice(CheckStateRequest request);
        Task<List<CheckStateInfo>> GetOfficeCheckTableInfo(CheckStateInfoRequest request);
    }
}
