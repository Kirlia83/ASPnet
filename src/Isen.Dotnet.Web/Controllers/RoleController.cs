using System;
using System.Linq;
using Isen.Dotnet.Library.Context;
using Isen.Dotnet.Library.Model;
using Isen.Dotnet.Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Isen.Dotnet.Web.Controllers
{
    public class RoleController : BaseController<Role>
    {
        public RoleController(
            ILogger<RoleController> logger,
            ApplicationDbContext context) : base(logger, context)
        {
        }               

        // Exemple d'override de la query : liste les personnes
        protected override IQueryable<Role> BaseQuery() =>
            base.BaseQuery()
                .Include(r => r.PersonsRole)
                .ThenInclude(pr => pr.Person);
    }
}