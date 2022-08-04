using HumanCapitalManagement.Models.VisaExpiry;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.VisaExpiry
{
    public class TM_Document_EmployeeRepository : RepositoryBase<TM_Document_Employee>
    {
        public TM_Document_EmployeeRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
