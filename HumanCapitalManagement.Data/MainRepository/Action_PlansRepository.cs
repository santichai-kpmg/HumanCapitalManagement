using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.Probation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class Action_PlansRepository : RepositoryBase<Action_Plans>
    {
        public Action_PlansRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
