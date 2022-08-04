using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class TM_Employment_RequestRepository : RepositoryBase<TM_Employment_Request>
    {
        public TM_Employment_RequestRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
