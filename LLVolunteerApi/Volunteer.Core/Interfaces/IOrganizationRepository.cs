using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Organization;
using Volunteer.Core.Entities.QueryModel;

namespace Volunteer.Core.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<OrganizationInfoDTO>> GetOrganization();
        Task<bool> AddOrganization(OrganizationRequest model);
        Task<bool> DelOrganization(int orgId, ShowType type);
        Task<PaginatedList<OrganizationInfoDTO>> GetOrganizationForAdmin(QueryParameters parms);
        Task<bool> ModifyOrganization(OrganizationRequest vo);
        Task<OrganizationInfoDTO> GetOrganization(int id);
    }
}
