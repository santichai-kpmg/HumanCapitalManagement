using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.OldTable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PreInternAssessment
{
    public class ActivityRepository : RepositoryBase<TM_PreInternAssessment_ActivitiesRepository>
    {
        public ActivityRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
