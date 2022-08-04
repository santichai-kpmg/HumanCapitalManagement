using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{

    public class TM_Evaluation_QuestionRepository : RepositoryBase<TM_Evaluation_Question>
    {
        public TM_Evaluation_QuestionRepository(DbContext context) : base(context)
        {
            //
        }
    }

}
