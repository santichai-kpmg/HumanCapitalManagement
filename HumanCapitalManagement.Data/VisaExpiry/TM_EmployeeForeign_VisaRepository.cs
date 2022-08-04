using HumanCapitalManagement.Models.VisExpiry;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.VisaExpiry
{
    public class TM_EmployeeForeign_VisaRepository : RepositoryBase<TM_EmployeeForeign_Visa>
    {
        public TM_EmployeeForeign_VisaRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
