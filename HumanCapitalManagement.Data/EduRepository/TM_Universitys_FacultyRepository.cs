using HumanCapitalManagement.Models.EducationModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.EduRepository
{
    public class TM_Universitys_FacultyRepository : RepositoryBase<TM_Universitys_Faculty>
    {
        public TM_Universitys_FacultyRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
