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
    public class DbContextRolePersonRepository :
        BaseDbRepository<RolePerson>,
        IRolePersonRepository
    {
        public DbContextRolePersonRepository(
            ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override IQueryable<RolePerson> Includes(IQueryable<RolePerson> includes)
        {
            var inc = base.Includes(includes);
            inc = inc.Include(r => r.Person)
                .Include(r => r.Role);
            return inc;
        }
    }
}