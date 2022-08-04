using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class PES_Final_RatingRepository : RepositoryBase<PES_Final_Rating>
    {
        public PES_Final_RatingRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
