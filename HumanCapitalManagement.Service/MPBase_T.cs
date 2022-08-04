using HumanCapitalManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service
{
    public abstract class MPBase<T> : IService<T> where T : class
    {

        private readonly IRepository<T> repo;

        public MPBase(IRepository<T> repo)
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
    }
}
