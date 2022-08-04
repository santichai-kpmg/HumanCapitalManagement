using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanCapitalManagement.Models.MainModels;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_FY_PlanRepository : RepositoryBase<TM_FY_Plan>
    {
        public TM_FY_PlanRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
