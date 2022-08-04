using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class PTR_Feedback_EmpRepository : RepositoryBase<PTR_Feedback_Emp>
    {
        public PTR_Feedback_EmpRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
