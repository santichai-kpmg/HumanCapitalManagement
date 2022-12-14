using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service
{
    public interface IService<T> where T : class
    {
        IQueryable<T> Query();

        int SaveChanges();
    }
}
