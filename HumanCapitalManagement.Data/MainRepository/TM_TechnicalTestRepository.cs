using System;
using HumanCapitalManagement.Models.MainModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace HumanCapitalManagement.Data.MainRepository
{

    public class TM_TechnicalTestRepository : RepositoryBase<TM_TechnicalTest>
    {
        public TM_TechnicalTestRepository(DbContext context) : base(context)
        {
            
        }
    }

}
