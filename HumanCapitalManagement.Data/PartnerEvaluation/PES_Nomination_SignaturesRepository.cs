using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class PES_Nomination_SignaturesRepository : RepositoryBase<PES_Nomination_Signatures>
    {
        public PES_Nomination_SignaturesRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
