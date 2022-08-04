using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Step_ApproveRepository : RepositoryBase<TM_Step_Approve>
    {
        public TM_Step_ApproveRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
