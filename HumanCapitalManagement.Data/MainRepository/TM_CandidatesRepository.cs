using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_CandidatesRepository : RepositoryBase<TM_Candidates>
    {
        public TM_CandidatesRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
