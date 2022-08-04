using HumanCapitalManagement.Models.PartnerEvaluation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class TM_Annual_RatingRepository : RepositoryBase<TM_Annual_Rating>
    {
        public TM_Annual_RatingRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
