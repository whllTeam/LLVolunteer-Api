using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volunteer.Core.Interfaces;

namespace Volunteer.Infrastructure.Database
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly VolunteerContext _context;

        public UnitOfWork(VolunteerContext context)
        {
            _context = context;
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
