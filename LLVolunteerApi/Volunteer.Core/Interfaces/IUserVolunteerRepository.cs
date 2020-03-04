using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;

namespace Volunteer.Core.Interfaces
{
    public interface IUserVolunteerRepository
    {
        Task<PaginatedList<TodoInfoData>> GetTodoInfo(TodoInfoRequest request);
    }
}
