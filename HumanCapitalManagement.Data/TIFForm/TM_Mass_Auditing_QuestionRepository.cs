using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.TIFForm
{
    public class TM_Mass_Auditing_QuestionRepository : RepositoryBase<TM_Mass_Auditing_Question>
    {
        public TM_Mass_Auditing_QuestionRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
