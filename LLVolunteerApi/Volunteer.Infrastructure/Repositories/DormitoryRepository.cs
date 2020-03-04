using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ConvertHelper;
using Microsoft.EntityFrameworkCore;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Dormitory;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Activities.Notes;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;
using Volunteer.Core.Interfaces;
using Volunteer.Infrastructure.Database;

namespace Volunteer.Infrastructure.Repositories
{
    public class DormitoryRepository : IDormitoryRepository
    {
        private readonly VolunteerContext _context;

        public DormitoryRepository(VolunteerContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<DormitoryTableDTO>> GetDormitoryTable(QueryParameters parameters)
        {
            var query = _context.DormitoryTableDto.AsQueryable();
            var count = await query.CountAsync();
            var data = await query
                ?.Where(t => t.IsDel == false)
                ?.OrderBy(t => t.Sequence)
                ?.Skip(parameters.PageIndex * parameters.PageSize)
                ?.Take(parameters.PageSize)
                ?.ToListAsync();
            return new PaginatedList<DormitoryTableDTO>(parameters.PageIndex, parameters.PageSize, count, data);
        }

        public async Task<IEnumerable<DormitoryTypeDTO>> GetDormitoryType()
        {
            return await _context.DormitoryTypeDto
                .Where(t => t.IsDel == false)
                .OrderBy(t => t.Sequence)
                .ToListAsync();
        }

        public async Task<IEnumerable<DormitoryTimeDTO>> GeTimeDay()
        {
            return await _context.DormitoryTimeDto
                .Where(t => t.IsDel == false)
                .OrderBy(t => t.Sequence)
                .ToListAsync();
        }

        public async Task<IEnumerable<DormitoryWeekDTO>> GetDormitoryWeek()
        {
            return await _context.DormitoryWeekDto
                .Where(t => t.IsDel == false)
                .OrderBy(t => t.Sequence)
                .ToListAsync();
        }

        private async Task<IEnumerable<DormitorySignDTO>> GetDormitorySignInfo(SignQuery query)
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

            return await _context.DormitorySignDtos
                .Where(
                    t => t.IsDel == false &&
                         DateTime.Parse(t.CreateTime) > DateTime.Parse(query.StartTime) &&
                         DateTime.Parse(t.CreateTime) < DateTime.Parse(query.EndTime) &&
                         t.IsSign == true)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }

        private async Task<IEnumerable<DormitoryTableDTO>> GetDormitoryTableAsync(
            Expression<Func<DormitoryTableDTO, bool>> where)
        {
            return await _context.DormitoryTableDto
                .Where(where)
                .ToListAsync();
        }

        /// <summary>
        /// 寝室楼  报名信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SignData>> GetSignInfo(SignQuery query)
        {
            var list = new List<SignData>();
            //得到  所有  报名数据
            IEnumerable<DormitorySignDTO> data = await GetDormitorySignInfo(query);
            // 根据  报名表  id  分组
            var signInfo = data
                .GroupBy(t => t.DormitoryId);
            if (!string.IsNullOrEmpty(query.UserName))
            {
                data = data.Where(t => t.UserName == query.UserName);
            }

            // 得到  当前   报名表信息
            var tables = await GetDormitoryTableAsync(t =>
                t.IsDel == false &&
                signInfo.Select(k => k.Key).Contains(t.Id)
            );
            foreach (var table in tables.GroupBy(t => t.DormitoryTypeId))
            {
                // table  表示  某类型  下的  id
                var sign = new SignData
                {
                    Key = table.Key.ToString()
                };

                foreach (var dormitoryType in table)
                {
                    var info = signInfo.FirstOrDefault(t => t.Key == dormitoryType.Id);
                    sign.Data.Add($"{dormitoryType.DormitoryWeekId},{dormitoryType.TimeDayId}",
                        info == null ? 0 : info.Count());
                }

                list.Add(sign);
            }

            return list;
        }

        /// <summary>
        /// 得到当前 用户报名信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SelfSignData>> GetSignSelfInfo(SignQuery query)
        {
            // 得到 已报名的数据
            var data = (await GetDormitorySignInfo(query))
                .Where(t => t.UserName == query.UserName);
            var tables = _context.DormitoryTableDto
                .Where(t => data.Select(d => d.DormitoryId).Contains(t.Id))
                .AsEnumerable();
            return tables.Select(t => new SelfSignData()
            {
                WeekId = t.DormitoryWeekId,
                TimeId = t.TimeDayId,
                VolunteerId = t.DormitoryTypeId
            });
        }

        /// <summary>
        /// 报名 、 取消报名
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> DormitorySign(SignQuery query)
        {
            // 获取  当前 用户  报名信息
            var data = (await GetDormitorySignInfo(query))
                .Where(t => t.UserName == query.UserName);
            var tables = _context.DormitoryTableDto
                .FirstOrDefault(t => t.IsDel == false
                                     && t.DormitoryWeekId == query.WeekId
                                     && t.DormitoryTypeId == query.VolunteerTypeId
                                     && t.TimeDayId == query.TimeId);

            if (tables == null)
            {
                return false;
            }

            var model = data.FirstOrDefault(t => t.DormitoryId == tables?.Id);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region 报名记录

                    var signNotes = new SignActivityNotesDTO()
                    {
                        Action = query.IsSign,
                        CreateTime = DateTime.Now.DataToStr(),
                        IsDel = false,
                        Sequence = 1,
                        VolunteerTableId = tables.Id,
                        UserName = query.UserName,
                        Type = VolunteerType.寝室楼,
                    };
                    _context.SignActivityNotesDtos.Add(signNotes);

                    #endregion

                    // 0 报名, 1 取消
                    if (query.IsSign == SignType.报名)
                    {
                        // 第一次报名
                        if (model == null)
                        {

                            var signDtos = (new DormitorySignDTO()
                            {
                                CancelSignCount = 0,
                                CreateTime = DateTime.Now.DataToStr(),
                                DormitoryId = tables.Id,
                                IsDel = false,
                                Sequence = 1,
                                SignCount = 1,
                                UserName = query.UserName,
                                IsSign = true
                            });
                            _context.DormitorySignDtos
                                .Add(signDtos);
                        }
                        else
                        {
                            // 不是第一次报名
                            model.IsSign = true;
                            model.SignCount = model.SignCount + 1;
                        }
                    }
                    else
                    {
                        //  参数有误
                        if (model == null)
                        {
                            return false;
                        }
                        else
                        {
                            // 审核通过  不允许取消报名
                            if (model.CheckState == CheckStateType.审核通过)
                            {
                                return false;
                            }

                            model.IsSign = false;
                            model.CancelSignCount = model.CancelSignCount + 1;
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public async Task<IList<VolunteerCount>> GetDormitorySignCount(SignQuery query)
        {
            // 默认取 所有时间数据
            var volunteerCounts = await _context.DormitoryTypeDto
                .Select(t => new VolunteerCount()
                {
                    VolunteerId = t.Id,
                    VolunteerName = t.Name,
                    VolunteerType = VolunteerType.寝室楼
                }).ToListAsync();
            var signData = _context.DormitorySignDtos
                .Where(t => t.IsSign == true &&
                            t.IsDel == false &&
                            t.UserName == query.UserName)
                .GroupBy(t => t.DormitoryId);
            // 获取 报名表 中数据，  并按照  寝室楼类型  分组
            var tableData = _context.DormitoryTableDto
                    .Where(t => signData.Select(s => s.Key).Contains(t.Id) && t.IsDel == false)
                    .GroupBy(t => t.DormitoryTypeId)
                ;
            foreach (var table in tableData)
            {
                var volunteerCount = volunteerCounts.FirstOrDefault(t => t.VolunteerId == table.Key);
                if (volunteerCount == null)
                {
                    throw new NullReferenceException(nameof(volunteerCount));
                }
                else
                {
                    volunteerCount.Count = table.Count();
                }
            }

            return volunteerCounts.ToList();
        }

        public async Task<IEnumerable<SignActivityNotesDTO>> GetDormitorySignDetailInfo(SignQuery query)
        {
            var result = await _context.SignActivityNotesDtos
                .Where(t => t.IsDel == false &&
                            t.UserName == query.UserName &&
                            t.Type == VolunteerType.寝室楼)
                .OrderBy(t => t.CreateTime)
                .ToListAsync();
            return result;
        }

        public async Task<PaginatedList<VolunteerSignInfo>> GetDormitorySignInfoQuery(SignQuery query)
        {
            var nowTimeWeek = DateTime.Now.DayOfWeek;
            if (!string.IsNullOrEmpty(query.StartTime) || query.IgnoreTime)
            {
                query.StartTime = DateTime.Now.AddDays(
                        -1 * (nowTimeWeek == DayOfWeek.Sunday ? 7 : nowTimeWeek.GetHashCode()))
                    .ToString();
            }

            if (!string.IsNullOrEmpty(query.EndTime) || query.IgnoreTime)
            {
                query.EndTime = DateTime.Now.AddDays(
                        (nowTimeWeek == DayOfWeek.Sunday) ? nowTimeWeek.GetHashCode() : (7 - nowTimeWeek.GetHashCode()))
                    .ToString();
            }

            // 获取  某类型的 tableId
            var volunteerTableIds = _context.DormitoryTableDto
                .Where(t => t.DormitoryTypeId == query.VolunteerTypeId).Select(t => t.Id);
            var dto = _context.DormitorySignDtos
                .Where(
                    t => t.IsDel == false
                         && (
                             (
                                 DateTime.Parse(t.CreateTime) > DateTime.Parse(query.StartTime) &&
                                 DateTime.Parse(t.CreateTime) < DateTime.Parse(query.EndTime)
                             ) || query.IgnoreTime)
                         &&
                         t.IsSign == true
                         && (volunteerTableIds.Contains(t.DormitoryId) || query.IsAll));
            int totalCount = await dto.CountAsync();
            var signdtos = await dto
                .OrderByDescending(t => t.CreateTime)
                .Skip((query.PageIndex - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
            var tableIds = signdtos.Select(t => t.DormitoryId);
            var dormitoryTable = await _context.DormitoryTableDto
                .Where(t => tableIds.Contains(t.Id)).ToListAsync();

            var weeks = await _context.DormitoryWeekDto.ToListAsync();
            var times = await _context.DormitoryTimeDto.ToListAsync();
            var volunteerTypes = await _context.DormitoryTypeDto.ToListAsync();
            var result = new List<VolunteerSignInfo>();
            foreach (var tableDto in signdtos)
            {
                var table = dormitoryTable.FirstOrDefault(t => t.Id == tableDto.DormitoryId);
                var week = weeks.FirstOrDefault(t => t.Id == table.DormitoryWeekId);
                var time = times.FirstOrDefault(t => t.Id == table.TimeDayId);
                var volunteerType = volunteerTypes.FirstOrDefault(t => t.Id == table.DormitoryTypeId);
                var volunteerSignInfo = new VolunteerSignInfo()
                {
                    CreateTime = tableDto.CreateTime,
                    DetailTimeId = time.Id,
                    DetailTimes = time.Name,
                    SignState = tableDto.CheckState,
                    Type = VolunteerType.寝室楼,
                    UserName = tableDto.UserName,
                    VolunteerName = volunteerType.Name,
                    VolunteerNameId = volunteerType.Id,
                    WeekId = week.Id,
                    WeekInfoName = week.Name,
                    IsSign = tableDto.IsSign,
                    CancelCount = tableDto.CancelSignCount,
                    SignCount = tableDto.SignCount,
                    WeekSequence = week.Sequence,
                    TimeSequence = time.Sequence,
                    SignTableId = tableDto.Id
                };
                result.Add(volunteerSignInfo);
            }

            var orderResult = result.OrderBy(t => t.WeekSequence).ThenBy(t => t.TimeSequence)
                .ThenByDescending(t => t.CreateTime);
            return new PaginatedList<VolunteerSignInfo>(query.PageIndex, query.PageSize, totalCount, orderResult);
        }

        public bool CheckStateForDormitory(CheckStateRequest request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                // 开启事物
                try
                {
                    // 先  对  报名表  进行  状态更改
                    var signTable = _context.DormitorySignDtos.FirstOrDefault(
                        t => t.Id == request.VolunteerSignId
                             &&
                             t.UserName == request.UserName
                             &&
                             t.IsSign == true);
                    // 验证数据
                    if (signTable == null)
                    {
                        return false;
                    }

                    signTable.CheckState = CheckStateType.审核通过;
                    // 确保  当前 一周  时间段  不包含  相同  weekId  、 timeDayId  数据
                    var time = DateTime.Now;
                    int day = time.DayOfWeek == DayOfWeek.Sunday ? 6 : 7 - time.DayOfWeek.GetHashCode();
                    var belowTime = time.AddDays(-1 * day).AddHours(-1 * time.Hour).AddMinutes(-1 * time.Minute);
                    //  同一个时间段（某天、某节课）  只能选择  一个  办公室
                    var haveData = _context.DormitoryCheckDtos.Any(
                        t => t.DormitoryWeekId == request.VolunteerWeekId
                              //  && t.DormitoryTypeId == request.VolunteerTypeId
                             && t.DormitorySignId == request.VolunteerSignId
                             && t.TimeDayId == request.TimeDayId
                             && DateTime.Parse(t.CreateTime) > belowTime);
                    if (haveData)
                    {
                        return false;
                    }

                    // 审核表  添加  数据
                    var checkModel = new DormitoryCheckDTO()
                    {
                        DormitorySignId = request.VolunteerSignId,
                        DormitoryTypeId = request.VolunteerTypeId,
                        DormitoryWeekId = request.VolunteerWeekId,
                        TimeDayId = request.TimeDayId,
                        UserName = request.UserName
                    };
                    _context.DormitoryCheckDtos.Add(checkModel);
                    _context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public async Task<List<CheckStateInfo>> GetDormitoryCheckTableInfo(CheckStateInfoRequest request)
        {
            var endTimeNormal = DateTime.Now;
            int day = endTimeNormal.DayOfWeek == DayOfWeek.Sunday ? 6 :  endTimeNormal.DayOfWeek.GetHashCode() -1;
            var startTimeNormal = endTimeNormal
                .AddDays(-1 * day)
                .AddHours(-1 * endTimeNormal.Hour)
                .AddMinutes(-1 * endTimeNormal.Minute);
            if (!request.IsCurrentWeek)
            {
                DateTime.TryParse(request.StartTime, out startTimeNormal);
                DateTime.TryParse(request.EndTime, out endTimeNormal);
            }

            var checkTable = await _context.DormitoryCheckDtos
                .Where(t => t.IsDel == false
                            &&
                            DateTime.Parse(t.CreateTime) >= startTimeNormal &&
                            DateTime.Parse(t.CreateTime) <= endTimeNormal
                            && t.DormitoryTypeId == request.VolunteerActivityId)
                .ToListAsync();
            var authorizeUserInfo = await _context.SchoolUserInfoDtos
                .Where(t => t.IsAuthorize == true && t.IsDel == false).ToListAsync();
            return checkTable.Select(t =>
            {
                var userInfo = authorizeUserInfo.FirstOrDefault(a =>
                    a.UserName.Equals(t.UserName, StringComparison.CurrentCultureIgnoreCase));
                return new CheckStateInfo()
                {
                    UserName = t.UserName,
                    RealUserName = userInfo?.SchoolUserName ?? string.Empty,
                    IsAuthorize = userInfo?.IsAuthorize ?? false,
                    VolunteerActivityId = t.DormitoryTypeId,
                    VolunteerTimeId = t.TimeDayId,
                    VolunteerWeekId = t.DormitoryWeekId
                };
            }).ToList();
        }
    }
}
