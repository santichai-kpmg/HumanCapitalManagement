using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.TIFForm
{
    public class TM_MassTIF_StatusRepository : RepositoryBase<TM_MassTIF_Status>
    {
        public TM_MassTIF_StatusRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
