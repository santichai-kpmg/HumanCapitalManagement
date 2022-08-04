using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class TM_Feedback_RatingRepository : RepositoryBase<TM_Feedback_Rating>
    {
        public TM_Feedback_RatingRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
