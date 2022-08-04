using HumanCapitalManagement.Models.PreInternAssessment;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PreInternAssessment
{
 
    public class TM_PIntern_Mass_Form_QuestionRepository : RepositoryBase<TM_PIntern_Mass_Form_Question>
    {
        public TM_PIntern_Mass_Form_QuestionRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
