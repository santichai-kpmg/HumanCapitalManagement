using HumanCapitalManagement.Models.PreInternAssessment;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PreInternAssessment
{
    public class TM_PIntern_StatusRepository : RepositoryBase<TM_PIntern_Status>
    {
        public TM_PIntern_StatusRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
