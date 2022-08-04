using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();

        T Add(T item);

        T Remove(T item);

        int SaveChanges();

    }
}
