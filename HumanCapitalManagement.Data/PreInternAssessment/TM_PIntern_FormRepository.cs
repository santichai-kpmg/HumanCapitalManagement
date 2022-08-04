using HumanCapitalManagement.Models.PreInternAssessment;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PreInternAssessment
{
    public class TM_PIntern_FormRepository : RepositoryBase<TM_PIntern_Form>
    {
        public TM_PIntern_FormRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
