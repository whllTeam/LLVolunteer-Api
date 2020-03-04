using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Volunteer.Core.Entities.Activities;
using Volunteer.Infrastructure.Database.SeedData;

namespace Volunteer.Infrastructure.Database
{
    public class VolunteerContextSeed
    {
        public static  void InitData(VolunteerContext context)
        {
            Seed._volunteerContext = context;
            if (context != null)
            {
                #region 周类型

                if (!context.DormitoryWeekDto.Any())
                {
                    context.DormitoryWeekDto.AddRange(Seed.GetDormitoryWeekDtos());
                }

                if (!context.OfficeWeekDto.Any())
                {
                    context.OfficeWeekDto.AddRange(Seed.GetOfficeWeekDtos());
                }
                #endregion

                #region 寝室楼 时间段 类型

                if (!context.DormitoryTimeDto.Any())
                {
                    context.DormitoryTimeDto.AddRange(Seed.GetDormitoryTimeDtos());
                }

                if (!context.OfficeTimeDto.Any())
                {
                    context.OfficeTimeDto.AddRange(Seed.GetOfficeTimeDtos());
                }

                if (!context.DormitoryTypeDto.Any())
                {
                    context.DormitoryTypeDto.AddRange(Seed.GetDormitoryTypeDtos());
                }

                if (!context.OfficeTypeDto.Any())
                {
                    context.OfficeTypeDto.AddRange(Seed.GetOfficeTypeDtos());
                }
                context.SaveChanges();
                #endregion

                //using (var transaction = context.Database.BeginTransaction())
                //{
                //    try
                //    {
                //        if (!context.DormitoryDto.Any())
                //        {
                //            context.DormitoryDto.AddRange(Seed.GetDormitoryDtos());
                //        }
                //        if (!context.OfficeDto.Any())
                //        {
                //            context.OfficeDto.AddRange(Seed.GetOfficeDtos());
                //        }
                //        context.SaveChanges();
                //        transaction.Commit();
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e);
                //        throw;
                //    }
                //}
               JsonSerializerSettings js = new JsonSerializerSettings();
                js.NullValueHandling = NullValueHandling.Ignore;
               File.WriteAllText("data/DormitoryTableDTO.json",JsonConvert.SerializeObject(Seed.GetDormitoryTableDtos(),js));
               File.WriteAllText("data/OfficeTableDTO.json", JsonConvert.SerializeObject(Seed.GetOfficeTableDtos(),js));
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
