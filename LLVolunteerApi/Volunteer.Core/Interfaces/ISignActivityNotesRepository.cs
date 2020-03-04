using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volunteer.Core.Entities.Activities.Notes;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;

namespace Volunteer.Core.Interfaces
{
    public interface ISignActivityNotesRepository
    {
        /// <summary>
        /// 添加 志愿报名记录
        /// </summary>
        /// <returns></returns>
        Task<bool> AddActivityNotes(SignActivityNotesDTO model);
        /// <summary>
        /// 查询 志愿服务报名记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<SignActivityNotesDTO>> GetSignActivityNotes(SignQuery query);

        Task<IEnumerable<ActivityNotes>> GetSignActivityNotes(string userName, bool isAll = false);
        Task<IEnumerable<ActivityNotes>> GetSignActivityNotesAll();
    }
}
