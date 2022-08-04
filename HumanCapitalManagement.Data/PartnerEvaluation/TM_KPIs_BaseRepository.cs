using HumanCapitalManagement.Models.PartnerEvaluation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class TM_KPIs_BaseRepository : RepositoryBase<TM_KPIs_Base>
    {
        public TM_KPIs_BaseRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
