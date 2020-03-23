using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Isen.Dotnet.Library.Model;
using Isen.Dotnet.Library.Repository.Interface;
using Microsoft.EntityFrameworkCore.Internal;
using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Isen.Dotnet.Library.Context
{
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IServiceRepository _serviceRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePersonRepository _rolePersonRepository;


        public SeedData(
            ApplicationDbContext dbContext,
            IServiceRepository serviceRepository,
            IPersonRepository personRepository,
            IRoleRepository roleRepository,
            IRolePersonRepository rolePersonRepository)
        {
            _dbContext = dbContext;
            _serviceRepository = serviceRepository;
            _personRepository = personRepository;
            _roleRepository = roleRepository;
            _rolePersonRepository = rolePersonRepository;
        }

        public void DropCreateDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public void AddPersons()
        {
            // Ne rien faire si non vide
            if(_dbContext.PersonCollection.Any()) return;
            var persons = GetPersons();
            persons.ForEach(person => _personRepository.Update(person));
            _personRepository.SaveChanges();
        }
        public void AddService()
        {
            // Ne rien faire si non vide
            if(_dbContext.ServiceCollection.Any()) return;

            var services =  new List<Service>();
            {
                services.Add(new Service { Name = "Ressouces Humaine"});
                services.Add(new Service { Name = "Comptabilité"});
                services.Add(new Service { Name = "Direction"});
                services.Add(new Service { Name = "Informatique"});
                services.Add(new Service { Name = "Logistique"});
                services.Add(new Service { Name = "Electronique"});
                services.Add(new Service { Name = "Développement"});
                services.Add(new Service { Name = "Innovation"});
                services.Add(new Service { Name = "R&D"});
                services.Add(new Service { Name = "Communication"});
                services.Add(new Service { Name = "Commercial"});
                services.Add(new Service { Name = "Assistance"});
                services.Add(new Service { Name = "Dépannage"});
            };
            services.ForEach(service => _serviceRepository.Update(service));
            _serviceRepository.SaveChanges();
        }
        public void AddRole()
        {
            // Ne rien faire si non vide
            if(_dbContext.RoleCollection.Any()) return;

            var roles = new List<Role>();
            {
                roles.Add(new Service { Name = "Utilisateur"});
                roles.Add(new Service { Name = "Manager"});
                roles.Add(new Service { Name = "Administrateur"});
                roles.Add(new Service { Name = "Super Administrateur"});
            };      
            roles.ForEach(role => _roleRepository.Update(role));
            _roleRepository.SaveChanges();
        }
        public void AddRolePerson()
        {
            // Ne rien faire si non vide
            if(_dbContext.RolePersonCollection.Any()) return;
            var rp = GetRolePersons();
            rps.ForEach(rp => _rolePersonRepository.Update(rp));
            _rolePersonRepository.SaveChanges();
        }

        public List<Person> GetPersons()
        {
            var persons = new List<Person>();
            for(var i = 0 ; i < 25 ; i++)
            {
                persons.Add(RandomPerson);
            }
            return persons;
        }

        private List<string> _firstNames => new List<string>
        {
            "Sang", 
            "Anne",
            "Boris",
            "Pierre",
            "Laura",
            "Hadrien",
            "Camille",
            "Louis",
            "Alicia"
        };
        private List<string> _lastNames => new List<string>
        {
            "Schuck",
            "Arbousset",
            "Lopasso",
            "Jubert",
            "Lebrun",
            "Dutaud",
            "Sarrazin",
            "Vu Dinh"
        };

        private readonly Random _random;

        // Générateur de prénom
        private string RandomFirstName => 
            _firstNames[_random.Next(_firstNames.Count)];
        // Générateur de nom
        private string RandomLastName => 
            _lastNames[_random.Next(_lastNames.Count)];

        // Générateur de date
        private DateTime RandomDate =>
            new DateTime(_random.Next(1980, 2010), 1, 1)
                .AddDays(_random.Next(0, 365));
        
        //Générateur de numéro de téléphone
        private string RandomPhone()
        {
            string telNo = "06.";
            for (int i = 1; i < 9; i++)
            {
                int number = _random.Next(0, 10);
                telNo = telNo + number.ToString();
                if((i%2 == 0) && (i != 8))
                {
                    telNo = telNo + ".";
                }
            }
            return telNo;
        }

        // Générateur de personne
        private Person RandomPerson
        {
            get
            {
                var randFirstName = RandomFirstName;
                var randLastName = RandomLastName;
                var mail = $"{randFirstName}.{randLastName}@gmail.com";
                mail = mail
                    .ToLower()
                    .Replace(' ', '_');
                return new Person()
                {
                    FirstName = randFirstName,
                    LastName = randLastName,
                    DateOfBirth = RandomDate,
                    PhoneNumber = RandomPhone(),
                    Email = mail,
                    ServiceMembership = serviceRepository[_random.Next(0, serviceRepository.Count)]
                };
            }
        }


        public List<RolePerson> GetRolePersons()
        {
            var rolePerson = new List<RolePerson>();
            foreach(Role role in _roleRepository.ToList())
            {
                
                var personsWithThisRole =  new List<Person>();
                var numberOfPerson = _random.Next(0, 50);
                for(int i = 0 ; i < numberOfPerson; i++)
                {
                    var check = 1;
                    while(check == 1)
                    {
                        var indice = _random.Next(0, 50);
                        var randPerson = _personRepository[indice];
                        if((personsWithThisRole.Exists(x => x.Id == randPerson.Id))==false)
                        {
                            personsWithThisRole.Add(randPerson);
                            check = 0;
                        }
                    }
                    
                }
                foreach(Person person in personsWithThisRole)
                {
                    var relation = new RolePerson
                    {
                        PersonId = person.Id,
                        Person = person,
                        RoleId = role.Id,
                        Role = role
                    };
                    rolePerson.Add(relation);
                }
            }
            return rolePerson;
        }
    }
}