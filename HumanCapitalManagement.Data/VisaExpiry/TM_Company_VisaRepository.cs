using HumanCapitalManagement.Models.VisExpiry;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.VisaExpiry
{
    public class TM_Company_VisaRepository : RepositoryBase<TM_Company_Visa>
    {
        public TM_Company_VisaRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
