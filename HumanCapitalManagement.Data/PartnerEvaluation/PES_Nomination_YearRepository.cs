using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class PES_Nomination_YearRepository : RepositoryBase<PES_Nomination_Year>
    {
        public PES_Nomination_YearRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
