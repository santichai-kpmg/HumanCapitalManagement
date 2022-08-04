using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Candidate_MassTIF_Adnl_AnswerRepository : RepositoryBase<TM_Candidate_MassTIF_Adnl_Answer>
    {
        public TM_Candidate_MassTIF_Adnl_AnswerRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
