using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class TM_Pool_RankRepository : RepositoryBase<TM_Pool_Rank>
    {
        public TM_Pool_RankRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
