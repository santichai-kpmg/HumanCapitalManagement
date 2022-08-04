using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Candidate_TIF_AnswerRepository : RepositoryBase<TM_Candidate_TIF_Answer>
    {
        public TM_Candidate_TIF_AnswerRepository(DbContext context) : base(context)
        {
            //
        }
    }
}

