using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class TM_PES_NMN_StatusRepository : RepositoryBase<TM_PES_NMN_Status>
    {
        public TM_PES_NMN_StatusRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
