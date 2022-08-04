using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class TM_PTR_E_Mail_HistoryRepository : RepositoryBase<TM_PTR_E_Mail_History>
    {
        public TM_PTR_E_Mail_HistoryRepository(DbContext context) : base(context)
        {
            //
        }
    }

}
