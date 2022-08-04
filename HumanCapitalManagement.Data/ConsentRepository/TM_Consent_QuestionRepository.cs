
using HumanCapitalManagement.Models.ConsentModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Consent_QuestionRepository : RepositoryBase<TM_Consent_Question>
    {
        public TM_Consent_QuestionRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
