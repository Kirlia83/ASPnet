using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Isen.Dotnet.Library.Context;
using Isen.Dotnet.Library.Model;
using Isen.Dotnet.Library.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Isen.Dotnet.Library.Repository.Db
{
    public class DbContextPersonRepository :
        BaseDbRepository<Person>,
        IPersonRepository
    {
        public DbContextPersonRepository(
            ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override IQueryable<Person> Includes(IQueryable<Person> includes)
        {
            var inc = base.Includes(includes);
            inc = inc.Include(p => p.ServiceMembership);
            inc = inc.Include(p => p.RolesPerson)
                .ThenInclude(rp => rp.Role);
            return inc;
        }
    }
}