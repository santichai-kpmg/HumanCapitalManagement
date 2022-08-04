using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class PTR_EvaluationRepository : RepositoryBase<PTR_Evaluation>
    {
        public PTR_EvaluationRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
