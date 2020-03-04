using System;
using System.Collections.Generic;
using System.Text;
using Volunteer.Core.Entities.Activities;
using Volunteer.Core.Entities.Activities.Dormitory;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Activities.Offices;

namespace Volunteer.Infrastructure.Database.SeedData
{
    public class Seed
    {
        public static VolunteerContext _volunteerContext;
        /// <summary>
        ///  办公室 周 类型  
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<OfficeWeekDTO> GetOfficeWeekDtos()
        {
            return new List<OfficeWeekDTO>()
            {
                new OfficeWeekDTO()
                {
                    Name = "周一",
                    IsOpen = true,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsDontAllow = "",
                    Sequence = 1
                },
                new OfficeWeekDTO()
                {
                    Name = "周二",
                    IsOpen = true,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsDontAllow = "",
                    Sequence = 2
                },
                new OfficeWeekDTO()
                {
                    Name = "周三",
                    IsOpen = true,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsDontAllow = "",
                    Sequence = 3
                },
                new OfficeWeekDTO()
                {
                    Name = "周四",
                    IsOpen = true,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsDontAllow = "",
                    Sequence = 4
                },
                new OfficeWeekDTO()
                {
                    Name = "周五",
                    IsOpen = true,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsDontAllow = "",
                    Sequence = 5
                }
            };
        }
        /// <summary>
        ///  寝室楼  周类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DormitoryWeekDTO> GetDormitoryWeekDtos()
        {
            return new List<DormitoryWeekDTO>()
            {
                new DormitoryWeekDTO()
                {
                    Name = "周一",
                    IsOpen = true,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsDontAllow = "",
                    Sequence = 1
                },
                new DormitoryWeekDTO()
                {
                    Name = "周二",
                    IsOpen = true,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsDontAllow = "",
                    Sequence = 2
                },
                new DormitoryWeekDTO()
                {
                    Name = "周三",
                    IsOpen = true,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsDontAllow = "",
                    Sequence = 3
                },
                new DormitoryWeekDTO()
                {
                    Name = "周四",
                    IsOpen = true,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsDontAllow = "",
                    Sequence = 4
                },
                new DormitoryWeekDTO()
                {
                    Name = "周五",
                    IsOpen = true,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsDontAllow = "2,",  /*  不允许 晚上*/
                    Sequence = 5
                }
            };
        }
        /// <summary>
        /// 寝室楼  类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DormitoryTypeDTO> GetDormitoryTypeDtos()
        {
            return new List<DormitoryTypeDTO>()
            {
                new DormitoryTypeDTO()
                {
                    IsDel = false,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsOpen = true,
                    Name = "4B",
                    Gender = GenderType.男性,
                    IsAllowGender = true,
                    Sequence = 1
                },new DormitoryTypeDTO()
                {
                    IsDel = false,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsOpen = true,
                    Name = "6A",
                    Gender = GenderType.女性,
                    IsAllowGender = false,
                    Sequence = 2
                },new DormitoryTypeDTO()
                {
                    IsDel = false,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsOpen = true,
                    Name = "7B",
                    Gender = GenderType.男性,
                    IsAllowGender = false,
                    Sequence = 3
                }
            };
        }
        /// <summary>
        /// 寝室楼 值班段
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DormitoryTimeDTO> GetDormitoryTimeDtos()
        {
            return new List<DormitoryTimeDTO>()
            {
                new DormitoryTimeDTO()
                {
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsOpen = true,
                    Name = "中午",
                    IsDontAllow = "",
                    Sequence = 1
                },
                new DormitoryTimeDTO()
                {
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsOpen = true,
                    Name = "晚上",
                    IsDontAllow = "",
                    Sequence = 2
                }
            };
        }
        /// <summary>
        /// 寝室楼值班安排
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DormitoryTableDTO> GetDormitoryTableDtos()
        {
            StringBuilder sb = new StringBuilder();
            var weeks = _volunteerContext.DormitoryWeekDto;
            var times = _volunteerContext.DormitoryTimeDto;
            var types = _volunteerContext.DormitoryTypeDto;
            var list = new List<DormitoryTableDTO>();
            int i = 0;
            foreach (var week in weeks)
            {
                i++;
                foreach (var time in times)
                {
                    foreach (var typeDto in types)
                    {
                        list.Add(new DormitoryTableDTO()
                        {
                            CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                            IsDel = false,
                            IsOpen = true,
                            DormitoryTypeId = typeDto.Id,
                            TimeDayId = time.Id,
                            DormitoryWeekId = week.Id,
                            Sequence = i
                        });
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 办公室 类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<OfficeTypeDTO> GetOfficeTypeDtos()
        {
            return new List<OfficeTypeDTO>()
            {
                new OfficeTypeDTO()
                {
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsOpen = true,
                    Name = "B-119",
                    Sequence = 1
                },
                new OfficeTypeDTO()
                {
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsOpen = true,
                    Name = "B-224",
                    Sequence = 2
                }
            };
        }
        /// <summary>
        /// 办公室  值班  时间段类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<OfficeTimeDTO> GetOfficeTimeDtos()
        {
            return new List<OfficeTimeDTO>()
            {
                new OfficeTimeDTO()
                {
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsOpen = true,
                    Name = "1-2节课",
                    IsDontAllow = "",
                    Sequence = 1
                },new OfficeTimeDTO()
                {
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsOpen = true,
                    Name = "3-4节课",
                    IsDontAllow = "",
                    Sequence = 2
                },new OfficeTimeDTO()
                {
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsOpen = true,
                    Name = "5-6节课",
                    IsDontAllow = "",
                    Sequence = 3
                },new OfficeTimeDTO()
                {
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                    IsDel = false,
                    IsOpen = true,
                    Name = "7-8节课",
                    IsDontAllow = "",
                    Sequence = 4
                },
            };
        }

        public static IEnumerable<OfficeTableDTO> GetOfficeTableDtos()
        {
            var weeks = _volunteerContext.OfficeWeekDto;
            var times = _volunteerContext.OfficeTimeDto;
            var types = _volunteerContext.OfficeTypeDto;
            var list = new List<OfficeTableDTO>();
            int i = 0;
            foreach (var week in weeks)
            {
                i++;
                foreach (var time in times)
                {
                    foreach (var typeDto in types)
                    {
                        list.Add(new OfficeTableDTO()
                        {
                            CreateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                            IsDel = false,
                            IsOpen = true,
                            OfficeTypeId = typeDto.Id,
                            TimeIntervalId = time.Id,
                            OfficeWeekId = week.Id,
                            Sequence = i
                        });
                    }
                }
            }
            return list;
        }

    }
}
