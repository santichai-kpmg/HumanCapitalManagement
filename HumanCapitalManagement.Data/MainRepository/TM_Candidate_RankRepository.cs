using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Candidate_RankRepository : RepositoryBase<TM_Candidate_Rank>
    {
        public TM_Candidate_RankRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
