using HumanCapitalManagement.Models.VisaExpiry;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.VisaExpiry
{
    public class TM_EmployeeRepository : RepositoryBase<TM_Employee>
    {
        public TM_EmployeeRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
