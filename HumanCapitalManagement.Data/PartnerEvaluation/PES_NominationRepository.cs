using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class PES_NominationRepository : RepositoryBase<PES_Nomination>
    {
        public PES_NominationRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
