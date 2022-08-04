using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class TM_PES_NMN_CompetenciesRepository : RepositoryBase<TM_PES_NMN_Competencies>
    {
        public TM_PES_NMN_CompetenciesRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
