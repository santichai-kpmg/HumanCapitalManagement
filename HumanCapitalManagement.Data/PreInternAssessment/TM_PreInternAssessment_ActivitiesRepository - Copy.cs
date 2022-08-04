using HumanCapitalManagement.Models.PreInternAssessment;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PreInternAssessment
{
    public class TM_PreInternAssessment_ActivitiesRepository : RepositoryBase<TM_PInternAssessment_Activities>
    {
        public TM_PreInternAssessment_ActivitiesRepository(DbContext context) : base(context)
        {
            //
        }
    }
    
}
