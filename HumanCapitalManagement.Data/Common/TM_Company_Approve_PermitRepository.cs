using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class TM_Company_Approve_PermitRepository : RepositoryBase<TM_Company_Approve_Permit>
    {
        public TM_Company_Approve_PermitRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
