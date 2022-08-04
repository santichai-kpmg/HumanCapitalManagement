using HumanCapitalManagement.Models.VisExpiry;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.VisaExpiry
{
    public class TM_Prefix_VisaRepository : RepositoryBase<TM_Prefix_Visa>
    {
        public TM_Prefix_VisaRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
