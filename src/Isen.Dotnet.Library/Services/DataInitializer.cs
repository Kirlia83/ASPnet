using System;
using System.Collections.Generic;
using System.Linq;
using Isen.Dotnet.Library.Context;
using Isen.Dotnet.Library.Model;
using Microsoft.Extensions.Logging;

namespace Isen.Dotnet.Library.Services
{
    public class DataInitializer : IDataInitializer
    {
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

        List<string> _serviceName = new List<string>()
            {
                "Ressouces Humaine",
                "Comptabilité",
                "Direction",
                "Informatique",
                "Logistique", 
                "Electronique", 
                "Développement",
                "Innovation", 
                "R&D", 
                "Communication", 
                "Commercial",
                "Assistance",
                "Dépannage"
            };

        private List<Service> GetServices()
        {
            var services = new List<Service>();
            for(int i = 0; i<_serviceName.Count; i++)
            {
                var service = new Service();
                service.Name = _serviceName[i];
                services.Add(service);
            }
            return services;
        }

        private List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role { Name = "Utilisateur", Id = 254},
                new Role { Name = "Manager", Id = 146},
                new Role { Name = "Administrateur", Id = 186},
                new Role { Name = "SuperAdministrateur", Id = 286}
            };
        }

        // Générateur aléatoire
        private readonly Random _random;

        // DI de ApplicationDbContext
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataInitializer> _logger;
        public DataInitializer(
            ILogger<DataInitializer> logger,
            ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
            _random = new Random();
        }

        //Générateur de services
        /*private Service RandomService => 
            _services[_random.Next(_services.Count)];*/
            

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
                };
            }
        }

        // Générateur de personnes
        public List<Person> GetPersons(int size)
        {
            var persons = new List<Person>();
            for(var i = 0 ; i < size ; i++)
            {
                persons.Add(RandomPerson);
            }
            return persons;
        }

        //Générateur de roles
        public List<RolePerson> GetRolePersons(List<Person> allPersons, List<Role> roles)
        {
            var rolePerson = new List<RolePerson>();
            foreach(Role role in roles)
            {
                
                var personsWithThisRole =  new List<Person>();
                var numberOfPerson = _random.Next(0, 50);
                for(int i = 0 ; i < numberOfPerson; i++)
                {
                    var check = 1;
                    while(check == 1)
                    {
                        var indice = _random.Next(0, 50);
                        var randPerson = allPersons[indice];
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

        // Générateur de services
        public void AddServices()
        {
            _logger.LogWarning("Adding services...");
            if (_context.ServiceCollection.Any()) return;
            var services = GetServices();
            _context.AddRange(services);
            _context.SaveChanges();
        }

        
        public void DropDatabase()
        {
            _logger.LogWarning("Dropping database");
            _context.Database.EnsureDeleted();
        }
            

        public void CreateDatabase()
        {
            _logger.LogWarning("Creating database");
            _context.Database.EnsureCreated();
        }
        public void AddData()
        {
            var persons = GetPersons(50);
            _logger.LogWarning("Persons created");
            var roles = GetRoles();
            _logger.LogWarning("roles created");
            

            _logger.LogWarning("Adding services...");
            if (_context.ServiceCollection.Any()) return;
            var services = GetServices();
            _context.AddRange(services);
            _context.SaveChanges();

            foreach(Person person in persons)
            {
                person.ServiceMembership = services[_random.Next(0, services.Count)];
            }

            var tabRolePerson = GetRolePersons(persons, roles);

            _logger.LogWarning("Adding persons...");

            // S'il y a déjà des personnes dans la base -> ne rien faire
            if (_context.PersonCollection.Any()) return;
            foreach(Person person in persons)
            {
                var roleCollection = new List<RolePerson>();
                foreach(RolePerson rp in tabRolePerson)
                {
                    if(rp.PersonId == person.Id)
                    {
                        roleCollection.Add(rp);
                    }
                }
                person.RolesPerson = roleCollection;
            }

            // Les ajouter au contexte
            _context.AddRange(persons);
            // Sauvegarder le contexte
            _context.SaveChanges();
            _logger.LogWarning("Adding roles...");

            if (_context.RoleCollection.Any()) return;
            foreach(Role role in roles)
            {
                var personCollection = new List<RolePerson>();
                foreach(RolePerson rp in tabRolePerson)
                {
                    if(rp.RoleId == role.Id)
                    {
                        personCollection.Add(rp);
                    }
                }
                role.PersonsRole = personCollection;
            }

            // Les ajouter au contexte
            _context.AddRange(roles);
            // Sauvegarder le contexte
            _context.SaveChanges();

        }
        
    }
}