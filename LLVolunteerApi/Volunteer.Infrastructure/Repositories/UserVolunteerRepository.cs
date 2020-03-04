using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertHelper;
using Microsoft.EntityFrameworkCore;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;
using Volunteer.Core.Interfaces;
using Volunteer.Infrastructure.Database;

namespace Volunteer.Infrastructure.Repositories
{
    public class UserVolunteerRepository : IUserVolunteerRepository
    {
        private readonly VolunteerContext _context;

        public UserVolunteerRepository(
            VolunteerContext context
        )
        {
            _context = context;
        }
        public async Task<PaginatedList<TodoInfoData>> GetTodoInfo(TodoInfoRequest request)
        {
            int totalCount = 0;
            var data = new List<TodoInfoData>();
            if (request.VolunteerType == VolunteerType.所有)
            {
                #region 寝室楼

                var dormitory = _context.DormitoryCheckDtos.Where(t=>t.UserName == request.UserName);
                int dormitoryCount = await dormitory.CountAsync();
                totalCount = totalCount + dormitoryCount;
                var dormitoryWeeks = _context.DormitoryWeekDto.ToList();
                var dormitoryTypes = _context.DormitoryTypeDto.ToList();
                var dormitoryTimes = _context.DormitoryTimeDto.ToList();
                var dormitoryData = dormitory.ToList().Select(t =>
                {
                    var week = dormitoryWeeks.FirstOrDefault(d => d.Id == t.DormitoryWeekId);
                    var weekValue = week == null ? 0 : week.WeekValue;
                    var type = dormitoryTypes.FirstOrDefault(d => d.Id == t.DormitoryTypeId);
                    var time = dormitoryTimes.FirstOrDefault(d => d.Id == t.TimeDayId);
                    return new TodoInfoData()
                    {
                        VolunteerType = VolunteerType.寝室楼,
                        VolunteerActivityId = t.DormitoryTypeId,
                        VolunteerActivityStr = type == null ? "未知" : type.Name,
                        TodoTime = GetTodoTime(weekValue, t.CreateTime),
                        CreateTime = t.CreateTime,
                        WeekValue = weekValue,
                        WeekName = week.Name,
                        TimeName = time.Name
                    };
                }) ;
                data.AddRange(dormitoryData);

                #endregion

                #region 办公室
                var office = _context.OfficeCheckDtos.Where(t=>t.UserName == request.UserName);
                int officeCount = await office.CountAsync();
                totalCount = totalCount + officeCount;
                var officeWeeks = _context.OfficeWeekDto.ToList();
                var officeTimes = _context.OfficeTimeDto.ToList();
                var officeTypes = _context.OfficeTypeDto.ToList();
                var officeData = office.ToList().Select(t =>
                {
                    var week = officeWeeks.FirstOrDefault(d => d.Id == t.OfficeTypeId);
                    var weekValue = week == null ? 0 : week.WeekValue;
                    var type = officeTypes.FirstOrDefault(d => d.Id == t.OfficeTypeId);
                    var time = officeTimes.FirstOrDefault(d => d.Id == t.TimeDayId);
                    return new TodoInfoData()
                    {
                        VolunteerType = VolunteerType.办公室,
                        VolunteerActivityId = t.OfficeTypeId,
                        VolunteerActivityStr = type == null ? "未知" : type.Name,
                        TodoTime = GetTodoTime(weekValue, t.CreateTime),
                        CreateTime = t.CreateTime,
                        WeekValue = weekValue,
                        WeekName = week.Name,
                        TimeName = time.Name
                    };
                });
                data.AddRange(officeData);


                #endregion

                #region 志愿组织下的志愿活动

                var activity = _context.ActivitySignTableDtos.Where(t=>t.UserName==request.UserName);
                int activityCount = await activity.CountAsync();
                totalCount = totalCount + activityCount;
                var activityIds = activity.Select(t => t.ActivityForOrganizationId);
                var activitys = _context.ActivityForOrganizationDtos.Where(t=> activityIds.Contains(t.Id)).Include(t=>t.OrganizationInfoDto).ToList();
                var activityData = activity.ToList().Select(t =>
                {
                    var org = activitys.FirstOrDefault(a => a.Id == t.ActivityForOrganizationId);
                    return new TodoInfoData()
                    {
                        VolunteerType = VolunteerType.志愿组织活动,
                        VolunteerActivityId = t.ActivityForOrganizationId,
                        VolunteerActivityStr = org == null ? "未知" : org.ActivityName,
                        TodoTime = org.EndTime,
                        OrgId = org.OrganizationInfoId,
                        OrgName = org.OrganizationInfoDto.OrganizerName,
                        CreateTime = t.CreateTime
                    };
                });
                data.AddRange(activityData);

                #endregion
            }

            return new PaginatedList<TodoInfoData>(request.PageIndex, request.PageSize, totalCount, data);
        }

        private string GetTodoTime(int weekValue, string createTime)
        {
            var time = DateTime.Parse(createTime);
            int day = time.DayOfWeek == DayOfWeek.Sunday ? 7 : time.DayOfWeek.GetHashCode();
            var newTime = time.AddDays(-1 * day+ weekValue + 7).AddHours(-1*time.Hour).AddMinutes(-1*time.Minute);
            return newTime.ToString("yyyy-MM-dd");
        }
    }
}
