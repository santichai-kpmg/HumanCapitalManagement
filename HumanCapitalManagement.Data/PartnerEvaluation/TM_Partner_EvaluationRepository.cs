using HumanCapitalManagement.Models.PartnerEvaluation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class TM_Partner_EvaluationRepository : RepositoryBase<TM_Partner_Evaluation>
    {
        public TM_Partner_EvaluationRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
