using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class TM_Recruitment_TeamRepository : RepositoryBase<TM_Recruitment_Team>
    {
        public TM_Recruitment_TeamRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
