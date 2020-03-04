using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volunteer.Core.Entities;
using Volunteer.Core.Entities.Activities.Enum;
using Volunteer.Core.Entities.Essay;
using Volunteer.Core.Entities.Organization;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Interfaces;
using Volunteer.Infrastructure.Database;

namespace Volunteer.Infrastructure.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly VolunteerContext _context;
        private readonly IActivityRepository _repository;
        public OrganizationRepository(
            VolunteerContext context,
            IActivityRepository repository
            )
        {
            _repository = repository;
            _context = context;
        }
        public async Task<IEnumerable<OrganizationInfoDTO>> GetOrganization()
        {
            var data = await _context.OrganizationInfoDtos
                ?.Where(t => t.IsDel == false)
                ?.OrderBy(t => t.Sequence)
                //?.Include(t => t.ImageForOrganizionDtos)
                ?.Include(t => t.ActivityForOrganizationDtos) 
                //?.ThenInclude(t=>t.DesImg) //使用新版本文件
                ?.ToListAsync();
            data.ForEach(t =>
            {
                t.ActivityForOrganizationDtos?.ForEach( a =>
                {
                    var imgList =  _repository.GetFileList(a.Id).Result;
                    a.DesImg = imgList?.Select(i => new ImageForActivityDTO()
                        {
                            LocalPath = i.FilePath
                        }).FirstOrDefault();
                });
            });
            return data;
        }

        public async Task<bool> AddOrganization(OrganizationRequest vo)
        {
            var model = new OrganizationInfoDTO()
            {
                Contact = vo.Contact,
                Description = vo.Description,
                From = vo.From,
                OrganizerName = vo.OrganizerName
            };
            _context.OrganizationInfoDtos.Add(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ModifyOrganization(OrganizationRequest vo)
        {
            var model = await _context.OrganizationInfoDtos.FirstOrDefaultAsync(t => t.Id == vo.Id);
            if (model == null)
            {
                return false;
            }
            else
            {
                model.Contact = vo.Contact;
                model.Description = vo.Description;
                model.From = vo.From;
                model.OrganizerName = vo.OrganizerName;
                return await _context.SaveChangesAsync() > 0;
            }
        }
        public async Task<bool> DelOrganization(int orgId, ShowType type)
        {
            var model = await _context.OrganizationInfoDtos
                .FirstOrDefaultAsync(t => t.Id == orgId
                    && ((t.IsDel == false) || (type == ShowType.启用)));
            if (model != null)
            {
                model.IsDel = type == ShowType.关闭;
            }
            else
            {
                return false;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PaginatedList<OrganizationInfoDTO>> GetOrganizationForAdmin(QueryParameters parms)
        {
            var totalCount = await _context.OrganizationInfoDtos.CountAsync();
            var data = await _context.OrganizationInfoDtos
                .Where(t => t.IsDel == false || parms.IsAdmin)
                .OrderBy(t => t.Sequence)
                .Skip((parms.PageIndex - 1) * parms.PageSize)
                .Take(parms.PageSize)
                .ToListAsync();
            return new PaginatedList<OrganizationInfoDTO>(parms.PageIndex, parms.PageSize, totalCount, data);
        }

        public async Task<OrganizationInfoDTO> GetOrganization(int id)
        {
            return await _context.OrganizationInfoDtos.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
