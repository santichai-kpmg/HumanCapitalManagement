using HumanCapitalManagement.Models.PartnerEvaluation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class TM_PTR_Eva_StatusRepository : RepositoryBase<TM_PTR_Eva_Status>
    {
        public TM_PTR_Eva_StatusRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
