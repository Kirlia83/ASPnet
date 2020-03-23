using System;
using System.Collections.Generic;
using System.Linq;
using Isen.Dotnet.Library.Model;
using Isen.Dotnet.Library.Repository.Interface;

namespace Isen.Dotnet.Library.Repository.Base
{
    public abstract class BaseInMemoryRepository<T> :
        IBaseRepository<T>
        where T : BaseEntity<T>
    {
        protected List<T> _context; 
        public virtual IQueryable<T> Context
        {
            get
            {
                if (_context != null) return _context.AsQueryable();
                _context = SampleData;
                return _context.AsQueryable();
            }
        }

        protected List<T> _contextTemp; 
        protected List<T> ContextTemp => 
            _contextTemp ??
                (_contextTemp = Context.ToList());

        public void SaveChanges()
        {
            _context = _contextTemp;
            _contextTemp = null;
        }

        public abstract List<T> SampleData { get; }

        public int NewId() => 
            Context.Max(c => c.Id) + 1;

        public T Single(int id) => 
            Context.SingleOrDefault(c => c.Id == id);
        public T Single(string name) => 
            Context.FirstOrDefault(c => c.Name.Equals(name));
        
        public void Update(T entity)
        {
            if (entity == null) return;
            
            var copy = ContextTemp;

            if (entity.IsNew)
            {
                entity.Id = NewId();
                ContextTemp.Add(entity);
            }
            else
            {
                var existing = Single(entity.Id);
                existing.Map(entity);
            }
        }

        public void Delete(int id)
        {
            var entityToDelete = Single(id);
            if (entityToDelete == null) return;
            ContextTemp.Remove(entityToDelete);
        }
        public void Delete(T entity) => 
            Delete(entity.Id);    

        public IEnumerable<T> GetAll() => 
            Context;    

        public IEnumerable<T> Find(
            Func<T, bool> predicate) => 
            Context.Where(predicate);
    }
}