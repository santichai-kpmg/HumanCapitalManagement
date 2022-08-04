using HumanCapitalManagement.Models.EducationModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.EduRepository
{
    public class TM_Education_DegreeRepository : RepositoryBase<TM_Education_Degree>
    {
        public TM_Education_DegreeRepository(DbContext context) : base(context)
        {
            //
        }
    }

}
