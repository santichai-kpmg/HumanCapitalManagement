using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class RequestTypeRepository : RepositoryBase<TM_Request_Type>
    {
        public RequestTypeRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
