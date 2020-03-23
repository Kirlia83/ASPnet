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
    public class DbContextRoleRepository :
        BaseDbRepository<Role>,
        IRoleRepository
    {
        public DbContextRoleRepository(
            ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override IQueryable<Role> Includes(IQueryable<Role> includes)
        {
            var inc = base.Includes(includes);
            inc = inc.Include(r => r.PersonsRole)
                .ThenInclude(pr => pr.Person);
            return inc;
        }
    }
}