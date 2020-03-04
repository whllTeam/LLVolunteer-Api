using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Organization;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;

namespace Volunteer.Core.Interfaces
{
    public interface IActivityRepository
    {
        Task<bool> AddActivity(ActivityRequest model);
        Task<PaginatedList<ActivityForOrganizationDTO>> GetActivities(QueryParameters parms);
        Task<ActivityForOrganizationDTO> GetActivity(int id);
        Task<bool> ModifyActivity(ActivityRequest model);
        Task<bool> DelActivity(int id, ShowType type);
        bool ActivitySign(SignQuery query);
        Task<IEnumerable<ActivitySignData>> GetSignInfo(SignQuery query);
        Task<IEnumerable<ActivitySelfSignData>> GetSignSelfInfo(SignQuery query);
        Task<PaginatedList<ActivitySignInfoQuery>> GetActivitySignInfoQuery(QueryParameters query);
        Task<PaginatedList<ActivitySignInfoTable>> GetActivitySignInfoTable(QueryParameters query);
        Task<List<FileManagerInfo>> GetFileList(int id);
    }
}
