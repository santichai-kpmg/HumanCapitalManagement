using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.PartnerEvaluation
{
    public class PTR_Evaluation_AnswerRepository : RepositoryBase<PTR_Evaluation_Answer>
    {
        public PTR_Evaluation_AnswerRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
