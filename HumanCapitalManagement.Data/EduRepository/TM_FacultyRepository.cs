using HumanCapitalManagement.Models.EducationModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.EduRepository
{
    public class TM_FacultyRepository : RepositoryBase<TM_Faculty>
    {
        public TM_FacultyRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
