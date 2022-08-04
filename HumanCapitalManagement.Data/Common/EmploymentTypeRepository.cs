using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class EmploymentTypeRepository : RepositoryBase<TM_Employment_Type>
    {
        public EmploymentTypeRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
