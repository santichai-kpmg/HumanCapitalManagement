using HumanCapitalManagement.Models.PartnerEvaluation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class TM_PTR_Eva_ApproveStepRepository : RepositoryBase<TM_PTR_Eva_ApproveStep>
    {
        public TM_PTR_Eva_ApproveStepRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
