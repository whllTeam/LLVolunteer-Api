using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Activities.Notes;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;
using Volunteer.Core.Interfaces;
using Volunteer.Infrastructure.Database;

namespace Volunteer.Infrastructure.Repositories
{
    /// <summary>
    /// 志愿报名记录
    /// </summary>
    public class SignActivityNotesRepository : ISignActivityNotesRepository
    {
        private readonly VolunteerContext _context;
        private readonly IOfficeRepository _officeRepository;
        private readonly IDormitoryRepository _dormitoryRepository;
        public SignActivityNotesRepository(
            VolunteerContext context,
            IOfficeRepository officeRepository,
            IDormitoryRepository dormitoryRepository
            )
        {
            _context = context;
            _officeRepository = officeRepository;
            _dormitoryRepository = dormitoryRepository;
        }
        public async Task<bool> AddActivityNotes(SignActivityNotesDTO model)
        {
            _context.SignActivityNotesDtos.Add(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<SignActivityNotesDTO>> GetSignActivityNotes(SignQuery query)
        {
            if (query.VolunteerType != null)
            {
                var nowTimeWeek = DateTime.Now.DayOfWeek;
                if (string.IsNullOrEmpty(query.StartTime))
                {
                    query.StartTime = DateTime.Now.AddDays(
                            -1 * (nowTimeWeek == DayOfWeek.Sunday ? 7 : nowTimeWeek.GetHashCode()))
                        .ToString();
                }

                if (string.IsNullOrEmpty(query.EndTime))
                {
                    query.EndTime = DateTime.Now.AddDays(
                            (nowTimeWeek == DayOfWeek.Sunday) ? nowTimeWeek.GetHashCode() : (7 - nowTimeWeek.GetHashCode()))
                        .ToString();
                }
                IEnumerable<SignActivityNotesDTO> signActivityModel = await _context.SignActivityNotesDtos
                    .Where(t => t.IsDel == false &&
                        DateTime.Parse(query.StartTime) < DateTime.Parse(t.CreateTime) &&
                        DateTime.Parse(t.CreateTime) < DateTime.Parse(query.EndTime))
                    .ToListAsync();
                if (!string.IsNullOrEmpty(query.UserName))
                {
                    signActivityModel = signActivityModel.Where(t => t.UserName == query.UserName);
                }
                // 根据志愿类型  进行分类
                var result = signActivityModel.GroupBy(t => t.Type);

                var volunteerNotes = result.FirstOrDefault(t => t.Key == query.VolunteerType);
                if (volunteerNotes != null)
                {
                    if (query.VolunteerType == VolunteerType.所有)
                    {
                        return signActivityModel;
                    }
                    else
                    {
                        return volunteerNotes.AsEnumerable();
                    }
                }
                else
                {
                    return new List<SignActivityNotesDTO>();
                }
            }
            else
            {
                return new List<SignActivityNotesDTO>();
            }
        }

        public async Task<IEnumerable<ActivityNotes>> GetSignActivityNotes(string UserName, bool isAll = false)
        {
            var notes = await _context.SignActivityNotesDtos
                .Where(t => t.UserName == UserName || isAll)
                .OrderBy(t => t.CreateTime)
                .AsNoTracking()
                .ToListAsync();
            var notesGroup = notes.GroupBy(t => t.Type);
            var result = new List<ActivityNotes>();
            foreach (var note in notesGroup)
            {
                if (note.Key == VolunteerType.办公室)
                {
                    var ids = note.Select(t => t.VolunteerTableId);
                    var tables = _context.OfficeTableDto
                        .Where(o => ids.Contains(o.Id))
                        .AsNoTracking()
                        .ToList();
                    var officeTypes = await _context.OfficeTypeDto.AsNoTracking().ToListAsync();
                    var officeTimes = await _context.OfficeTimeDto.AsNoTracking().ToListAsync();
                    var officeWeeks = await _context.OfficeWeekDto.AsNoTracking().ToListAsync();
                    note.ToList().ForEach(n =>
                    {
                        var tab = tables.FirstOrDefault(t => t.Id == n.VolunteerTableId);
                        if (tab != null)
                        {
                            var offType = officeTypes.FirstOrDefault(t => t.Id == tab.OfficeTypeId);
                            if (offType != null)
                            {
                                // tab.OfficeTypeDtos.Clear();
                                tab.OfficeTypeDtos.Add(offType);
                            }

                            var offTime = officeTimes.FirstOrDefault(t => t.Id == tab.TimeIntervalId);
                            if (offTime != null)
                            {
                                // tab.OfficeTimeDtos.Clear();
                                tab.OfficeTimeDtos.Add(offTime);
                            }

                            var offWeek = officeWeeks.FirstOrDefault(t => t.Id == tab.OfficeWeekId);
                            if (offWeek != null)
                            {
                                // tab.OfficeWeekDto.Clear();
                                tab.OfficeWeekDto.Add(offWeek);
                            }
                            n.OfficeTableDtos.Add(tab);
                            result.Add(new ActivityNotes()
                            {
                                Action = n.Action,
                                UserName = isAll ? n.UserName.Substring(0, n.UserName.Length / 2) + "****" : n.UserName,
                                Type = n.Type,
                                CreateTime = n.CreateTime,
                                DetailTimes = offTime.Name,
                                VolunteerName = offType.Name,
                                WeekInfo = offWeek.Name
                            });
                        }
                    });
                }
                else if (note.Key == VolunteerType.寝室楼)
                {
                    var ids = note.Select(t => t.VolunteerTableId);
                    var tables = _context.DormitoryTableDto
                            .Where(d => ids.Contains(d.Id))
                        .AsNoTracking()
                        .ToList();
                    var dorTypes = await _context.DormitoryTypeDto.AsNoTracking().ToListAsync();
                    var dorTimes = await _context.DormitoryTimeDto.AsNoTracking().ToListAsync();
                    var dorWeeks = await _context.DormitoryWeekDto.AsNoTracking().ToListAsync();
                    note.ToList().ForEach(n =>
                    {
                        var tab = tables.FirstOrDefault(t => t.Id == n.VolunteerTableId);
                        if (tab != null)
                        {
                            var dorType = dorTypes.FirstOrDefault(t => t.Id == tab.DormitoryTypeId);
                            if (dorType != null)
                            {
                                // tab.DormitoryTypeDtos.Clear();
                                tab.DormitoryTypeDtos.Add(dorType);
                            }

                            var dorTime = dorTimes.FirstOrDefault(t => t.Id == tab.TimeDayId);
                            if (dorTime != null)
                            {
                                // tab.DormitoryTimeDtos.Clear();
                                tab.DormitoryTimeDtos.Add(dorTime);
                            }

                            var dorWeek = dorWeeks.FirstOrDefault(t => t.Id == tab.DormitoryWeekId);
                            if (dorWeek != null)
                            {
                                // tab.DormitoryWeekDtos.Clear();
                                tab.DormitoryWeekDtos.Add(dorWeek);
                            }
                            n.DormitoryTableDtos.Add(tab);
                            result.Add(new ActivityNotes()
                            {
                                Action = n.Action,
                                UserName = isAll ? n.UserName.Substring(0, n.UserName.Length / 2) + "****" : n.UserName,
                                Type = n.Type,
                                CreateTime = n.CreateTime,
                                DetailTimes = dorTime.Name,
                                VolunteerName = dorType.Name,
                                WeekInfo = dorWeek.Name
                            });
                        }
                    });
                }
                else if (note.Key == VolunteerType.志愿组织活动)
                {
                    var ids = note.Select(t => t.VolunteerTableId);
                    var activitys = _context.ActivityForOrganizationDtos
                        .Where(o => ids.Contains(o.Id)).AsNoTracking().ToList();
                    var orgIds = activitys.Select(a => a.OrganizationInfoId);
                    var orgInfos = _context.OrganizationInfoDtos.Where(t =>
                        orgIds.Contains(t.Id)).AsNoTracking().ToList();
                    note.ToList().ForEach(n =>
                    {
                        var activity = activitys.FirstOrDefault(t => t.Id == n.VolunteerTableId);
                        if (activity != null)
                        {
                            var orgInfo = orgInfos.FirstOrDefault(t => t.Id == activity.OrganizationInfoId);
                            result.Add(new ActivityNotes()
                            {
                                Action = n.Action,
                                UserName = isAll ? n.UserName.Substring(0, n.UserName.Length / 2) + "****" : n.UserName,
                                Type = n.Type,
                                CreateTime = n.CreateTime,
                                DetailTimes = "",
                                VolunteerName = activity.ActivityName,
                                WeekInfo = "",
                                OrganizationName = orgInfo.OrganizerName
                            });
                        }
                    });
                }
            }

            return result.OrderBy(t => t.CreateTime);
        }

        public async Task<IEnumerable<ActivityNotes>> GetSignActivityNotesAll()
        {
            return await GetSignActivityNotes("", true);
        }
    }
}
