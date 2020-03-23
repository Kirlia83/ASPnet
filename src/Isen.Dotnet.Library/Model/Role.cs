using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Isen.Dotnet.Library.Model
{
    public class Role : BaseEntity<Role>
    {        
        public string Name {get; set;}
        public ICollection<RolePerson> PersonsRole {get; set;}
    }
}