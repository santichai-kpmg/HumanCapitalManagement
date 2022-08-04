using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Trainee_Eva_AnswerRepository : RepositoryBase<TM_Trainee_Eva_Answer>
    {
        public TM_Trainee_Eva_AnswerRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
