using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class PTR_Evaluation_AuthorizedRepository : RepositoryBase<PTR_Evaluation_Authorized>
    {
        public PTR_Evaluation_AuthorizedRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
