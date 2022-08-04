using HumanCapitalManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service
{
    public abstract class ServiceBase<T> : IService<T> where T : class
    {

        private readonly IRepository<T> repo;

        public ServiceBase(IRepository<T> repo)
        {
            this.repo = repo;
        }

        public IQueryable<T> Query()
        {
            return repo.Query();
        }


        public int SaveChanges()
        {
            return repo.SaveChanges();
        }

        public T Add(T item)
        {
            return repo.Add(item);
        }

        public T Remove(T item)
        {
            return repo.Remove(item);
        }
    }
}
