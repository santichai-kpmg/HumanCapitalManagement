using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class PTR_Evaluation_KPIsRepository : RepositoryBase<PTR_Evaluation_KPIs>
    {
        public PTR_Evaluation_KPIsRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
