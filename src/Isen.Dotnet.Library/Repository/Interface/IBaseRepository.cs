using System;
using System.Collections.Generic;
using System.Linq;
using Isen.Dotnet.Library.Model;

namespace Isen.Dotnet.Library.Repository.Interface
{
    public interface IBaseRepository<T>
        where T : BaseEntity<T>
    {
        IQueryable<T> Context { get; }
        void SaveChanges();

        T Single(int id);

        void Update(T entity);
        void Delete(int id);
        void Delete(T entity);

        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);
    }
}