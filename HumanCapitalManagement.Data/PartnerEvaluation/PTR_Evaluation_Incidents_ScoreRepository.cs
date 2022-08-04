using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class PTR_Evaluation_Incidents_ScoreRepository : RepositoryBase<PTR_Evaluation_Incidents_Score>
    {
        public PTR_Evaluation_Incidents_ScoreRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
