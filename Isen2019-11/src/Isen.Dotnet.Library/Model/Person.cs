using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Isen.Dotnet.Library.Model
{
    public class Person : BaseEntity
    {        
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public DateTime? DateOfBirth {get;set;}

        public string Email {get; set;}
        public string PhoneNumber {get; set;}
        public Service ServiceMembership {get; set;}
        public ICollection<RolePerson> RolesPerson {get; set;}


        //public City BirthCity {get;set;}
        //public int? BirthCityId {get;set;}

        //public City ResidenceCity {get;set;}
        //public int? ResidenceCityId {get;set;}
        
        [NotMapped] // ne pas générer ce champ dans la bdd
        public int? Age => DateOfBirth.HasValue ?
            // Nb de jours entre naissance et today / 365
            (int)((DateTime.Now - DateOfBirth.Value).TotalDays / 365) :
            // Pas de date de naissance alors, un entier null
            new int?();
        
        public override string ToString() =>
            $"(ID : {Id}) {LastName} {FirstName} ";
        
    }
}