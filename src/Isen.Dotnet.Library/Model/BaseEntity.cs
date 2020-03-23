using System;
using System.Dynamic;

namespace Isen.Dotnet.Library.Model
{
    public abstract class BaseEntity<T>
        where T : BaseEntity<T>
    {
        public virtual int Id { get;set;}

        public virtual bool IsNew => Id <= 0;

        public virtual string Display => 
            $"[{this.GetType()}] Id={Id}";

        public override string ToString() 
            => Display;

        public virtual void Map(T copy)
        {
        }

        public virtual dynamic ToDynamic()
        {
            dynamic response = new ExpandoObject();
            response.id = Id;
            return response;
        }
    }
}