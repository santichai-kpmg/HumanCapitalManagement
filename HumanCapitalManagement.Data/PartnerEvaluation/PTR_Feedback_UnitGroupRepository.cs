using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class PTR_Feedback_UnitGroupRepository : RepositoryBase<PTR_Feedback_UnitGroup>
    {
        public PTR_Feedback_UnitGroupRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
