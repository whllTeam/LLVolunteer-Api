using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertHelper;
using Microsoft.EntityFrameworkCore;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Activities.Notes;
using Volunteer.Core.Entities.Organization;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Entities.ViewModel;
using Volunteer.Core.Interfaces;
using Volunteer.Infrastructure.Database;

namespace Volunteer.Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private VolunteerContext _context;
        // 分层  未设计好
        private IFileManagerRepository Repository { get; }

        public ActivityRepository(
            VolunteerContext context,
            IFileManagerRepository repository
        )
        {
            _context = context;
            Repository = repository;
        }
        public async Task<bool> AddActivity(ActivityRequest vo)
        {
            var img = new ImageForActivityDTO()
            {
                Width = 0,
                Height = 0,
                ImageType = "jpg",
                LocalPath = @"/Upload/Org_Img/2_181204105501_1.jpg",
            };
            var model = new ActivityForOrganizationDTO()
            {
                ActivityDes = vo.ActivityDes,
                ActivityName = vo.ActivityName,
                DesImg = vo.DesImg,
                StartTime = vo.StartTime,
                EndTime = vo.EndTime,
                SignMaxNum = vo.SignMaxNum,
                VolunteerTime = vo.VolunteerTime,
                OrganizationInfoId = vo.OrganizationInfoId
            };
            _context.ActivityForOrganizationDtos.Add(model);
            await _context.SaveChangesAsync();
            img.ActivityId = model.Id;
            _context.ImgForActivityDtos.Add(img);
            if (_context.SaveChanges() > 0)
            {
                // 绑定文件
                return await Repository.UploadFileBind(model.Id, vo.FileInfo);
            }
            else
            {
                return false;
            }
        }

        public async Task<PaginatedList<ActivityForOrganizationDTO>> GetActivities(QueryParameters parms)
        {
            int totalCount = await _context.ActivityForOrganizationDtos.CountAsync();
            var data = await _context.ActivityForOrganizationDtos
                .OrderBy(t => t.Sequence)
                .Skip((parms.PageIndex - 1) * parms.PageSize)
                .Take(parms.PageSize)
                // .Include(t => t.OrganizationInfoDto)
                .ToListAsync();
            return new PaginatedList<ActivityForOrganizationDTO>(parms.PageIndex, parms.PageSize, totalCount, data);
        }

        public async Task<ActivityForOrganizationDTO> GetActivity(int id)
        {
            return await _context.ActivityForOrganizationDtos.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> ModifyActivity(ActivityRequest vo)
        {
            var model = await _context.ActivityForOrganizationDtos.FirstOrDefaultAsync(t => t.Id == vo.Id);
            if (model == null)
            {
                return false;
            }
            else
            {
                model.ActivityDes = vo.ActivityDes;
                model.ActivityName = vo.ActivityName;
                model.StartTime = vo.StartTime;
                model.EndTime = vo.EndTime;
                model.SignMaxNum = vo.SignMaxNum;
                model.OrganizationInfoId = vo.OrganizationInfoId;
                model.VolunteerTime = vo.VolunteerTime;
                // model  数据  未进行修改时，savechange  为 0
                // 绑定文件
                return await Repository.UploadFileBind(model.Id, vo.FileInfo);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">要改变状态</param>
        /// <returns></returns>
        public async Task<bool> DelActivity(int id, ShowType type)
        {
            var model = _context.ActivityForOrganizationDtos
                .FirstOrDefault(t => t.Id == id && ((t.IsDel == false) || type != ShowType.关闭));
            if (model == null)
            {
                return false;
            }
            else
            {
                model.IsDel = type == ShowType.关闭 ? true : false;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public bool ActivitySign(SignQuery query)
        {
            // 判断活动表是否有该数据

            var activityTable = _context.ActivityForOrganizationDtos
            .FirstOrDefault(t => t.Id == query.VolunteerTypeId);
            if (activityTable == null)
            {
                return false;
            }

            // 得到当前用户的报名信息
            var userSignInfo = _context.ActivitySignTableDtos
                .FirstOrDefault(t => t.UserName == query.UserName
                                          && t.IsDel == false
                                          && t.ActivityForOrganizationId == query.VolunteerTypeId);

            var count = _context.ActivitySignTableDtos
                .Count(t => t.IsSign == true
                                 && t.IsDel == false
                            && t.ActivityForOrganizationId == query.VolunteerTypeId);
            if (count >= activityTable.SignMaxNum && query.IsSign == SignType.报名)
            {
                return false;
            }

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
                        VolunteerTableId = activityTable.Id,
                        UserName = query.UserName,
                        Type = VolunteerType.志愿组织活动,
                    };
                    _context.SignActivityNotesDtos.Add(signNotes);
                    #endregion

                    if (query.IsSign == SignType.报名)
                    {
                        // 第一次报名
                        if (userSignInfo == null)
                        {
                            var signTable = new ActivitySignTableDTO()
                            {
                                CancelSignCount = 0,
                                CreateTime = DateTime.Now.DataToStr(),
                                ActivityForOrganizationId = query.VolunteerTypeId,
                                IsDel = false,
                                Sequence = 1,
                                IsSign = true,
                                SignCount = 1,
                                UserName = query.UserName
                            };
                            _context.ActivitySignTableDtos.Add(signTable);
                        }
                        else
                        {
                            userSignInfo.IsSign = true;
                            userSignInfo.SignCount = userSignInfo.SignCount + 1;
                        }
                    }
                    else
                    {
                        // 参数错误
                        if (userSignInfo == null)
                        {
                            return false;
                        }
                        else
                        {
                            userSignInfo.IsSign = false;
                            userSignInfo.CancelSignCount = userSignInfo.CancelSignCount + 1;
                        }
                    }

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

        public async Task<IEnumerable<ActivitySignData>> GetSignInfo(SignQuery query)
        {
            var list = new List<ActivitySignData>();
            var signGroup = _context.ActivitySignTableDtos
                .Where(t => t.IsSign == true && t.IsDel == false)
                .GroupBy(t => t.ActivityForOrganizationId);
            var activitys = await _context.ActivityForOrganizationDtos
                .Where(t => signGroup.Select(s => s.Key).Contains(t.Id)).ToListAsync();
            foreach (var signTable in signGroup)
            {
                var activity = activitys.FirstOrDefault(t => t.Id == signTable.Key);
                var data = new ActivitySignData()
                {
                    ActivityId = activity.Id,
                    OrganizationId = activity.OrganizationInfoId,
                    SignCount = signTable.Count()
                };
                list.Add(data);
            }

            return list;
        }

        public async Task<IEnumerable<ActivitySelfSignData>> GetSignSelfInfo(SignQuery query)
        {
            var signTable = await _context.ActivitySignTableDtos
                .Where(t => t.UserName == query.UserName && t.IsSign == true && t.IsDel == false)
                .ToListAsync();
            var activitys = _context.ActivityForOrganizationDtos
                .Where(t => signTable.Select(s => s.ActivityForOrganizationId).Contains(t.Id))
                ;
            return activitys.Any()
                ? activitys.Select(t => new ActivitySelfSignData()
                {
                    OrganizationId = t.OrganizationInfoId,
                    ActivityId = t.Id
                }).ToList() : new List<ActivitySelfSignData>();
        }

        public async Task<PaginatedList<ActivitySignInfoQuery>> GetActivitySignInfoQuery(QueryParameters query)
        {
            var totalCount = await _context.ActivitySignTableDtos.CountAsync();
            var tables = await _context.ActivitySignTableDtos
                .OrderBy(t=>t.CreateTime)
                .Skip((query.PageIndex - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
            var orgInfos = _context.ActivityForOrganizationDtos
                .Where(t => tables.Select(a => a.ActivityForOrganizationId).Contains(t.Id))
                .Include(t=>t.OrganizationInfoDto);
            var data = tables.Select(t =>
            {
                var org = orgInfos.FirstOrDefault(o => o.Id == t.ActivityForOrganizationId);
                return new ActivitySignInfoQuery()
                {
                    ActivityName = org.ActivityName,
                    CreateTime = t.CreateTime,
                    IsSign = t.IsSign,
                    OrganizationName = org.OrganizationInfoDto.OrganizerName,
                    UserName = t.UserName,
                    CancelCount = t.CancelSignCount,
                    SignCount = t.SignCount
                };
            });
            return new PaginatedList<ActivitySignInfoQuery>(query.PageIndex,query.PageSize,totalCount,data);
        }

        public async Task<PaginatedList<ActivitySignInfoTable>> GetActivitySignInfoTable(QueryParameters query)
        {
            var result = new List<ActivitySignInfoTable>();
            var totalCount = await _context.ActivityForOrganizationDtos.CountAsync();
            // 查找  所有活动下的报名信息
            var activityTabes = await _context.ActivityForOrganizationDtos
                .OrderBy(t => t.OrganizationInfoId)
                .ThenBy(t => t.CreateTime)
                .Skip((query.PageIndex - 1) * query.PageSize)
                .Take(query.PageSize)
                .AsNoTracking()
                .ToListAsync();
            var organizations = await _context.OrganizationInfoDtos.ToListAsync();
            var authorizeUserInfo =
                await _context.SchoolUserInfoDtos.Where(t => t.IsAuthorize == true && t.IsDel == false).ToListAsync();
            // 报名信息  按照  活动  进行 分组
            var activitySignGroup =  _context.ActivitySignTableDtos
                .Where(t => t.IsSign == true && t.IsDel == false)
                .AsNoTracking()
                .ToList()
                .GroupBy(t => t.ActivityForOrganizationId);
            activityTabes.ForEach(t =>
                {
                    var model = new ActivitySignInfoTable()
                    {
                        Id = t.Id,
                        ActivityName = t.ActivityName,
                        OrganizationInfoId = t.OrganizationInfoId,
                        ActivityDes = t.ActivityDes,
                        CreateTime = t.CreateTime,
                        EndTime = t.EndTime,
                        StartTime = t.StartTime,
                        SignMaxNum = t.SignMaxNum,
                        SignUserInfo = new List<string>()
                    };
                    model.OrganizationName = organizations.FirstOrDefault(o => o.Id == model.OrganizationInfoId)
                        .OrganizerName;
                    var users = activitySignGroup.FirstOrDefault(a => a.Key == t.Id);
                    if (users != null)
                    {
                        model.SignUserInfo.AddRange(users.Select(a =>
                        {
                            var user = authorizeUserInfo.FirstOrDefault(au => au.UserName == a.UserName);
                            return user != null ? $"{user.SchoolUserName}-(已认证)" : $"{a.UserName}-(未认证)";
                        }));
                    }
                    model.HasSignCount = model.SignUserInfo.Count();
                    result.Add(model);
                }
                );
            return new PaginatedList<ActivitySignInfoTable>(query.PageIndex,query.PageSize,totalCount, result);
        }

        public async Task<List<FileManagerInfo>> GetFileList(int id)
        {
            var result = await _context.ActivityForOrganizationDtos
                .FirstOrDefaultAsync(t => t.Id == id);
            if (result == null)
            {
                return null;
            }
            else
            {
                var imageIds = result.FileImageIds?.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var fileIds = result.FileExIds?.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var files = await _context.FileUploadDtos.Where(
                        t => (imageIds != null && imageIds.Any(s=>s == t.Id.ToString())
                                                  || (fileIds!=null && fileIds.Any(s => s == t.Id.ToString()))))
                    .Select(t => new FileManagerInfo()
                    {
                        FileId = t.Id,
                        FileName = t.FileName,
                        FileType = t.FileType,
                        FilePath = t.FilePath.Replace('\\','/')
                    }).ToListAsync();
                return files;
            }
        }
    }
}
