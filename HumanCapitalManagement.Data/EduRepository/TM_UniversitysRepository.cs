using HumanCapitalManagement.Models.EducationModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.EduRepository
{
    public class TM_UniversitysRepository : RepositoryBase<TM_Universitys>
    {
        public TM_UniversitysRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
