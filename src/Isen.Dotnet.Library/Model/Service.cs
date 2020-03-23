using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Isen.Dotnet.Library.Model
{
    public class Service : BaseEntity<Service>
    {        
        public string Name {get; set;}
    }
}