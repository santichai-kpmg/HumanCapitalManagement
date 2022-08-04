using HumanCapitalManagement.Models.eGreetings;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.eGreetingsRepository
{
    public class TM_eGreetings_QuestionRepository : RepositoryBase<TM_eGreetings_Question>
    {
        public TM_eGreetings_QuestionRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
