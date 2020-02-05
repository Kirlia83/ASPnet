using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Isen.Dotnet.Library.Model
{
    public class RolePerson
    {
        public int PersonId { get; set; }  
        public Person Person { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}