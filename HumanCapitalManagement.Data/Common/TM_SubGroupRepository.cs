using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class TM_SubGroupRepository : RepositoryBase<TM_SubGroup>
    {
        public TM_SubGroupRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
