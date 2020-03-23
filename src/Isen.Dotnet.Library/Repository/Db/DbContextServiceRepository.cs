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
    public class DbContextServiceRepository :
        BaseDbRepository<Service>,
        IServiceRepository
    {
        public DbContextServiceRepository(
            ApplicationDbContext dbContext) :
            base(dbContext)
        {
        }

        public override IQueryable<Service> Includes(IQueryable<Service> includes)
        {
            var inc = base.Includes(includes);
            return inc;
        }
    }
}