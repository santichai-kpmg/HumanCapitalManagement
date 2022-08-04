using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class TM_WorkExperienceRepository : RepositoryBase<TM_WorkExperience>
    {
        public TM_WorkExperienceRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
