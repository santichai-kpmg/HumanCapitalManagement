using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Candidate_Status_CycleRepository : RepositoryBase<TM_Candidate_Status_Cycle>
    {
        public TM_Candidate_Status_CycleRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
