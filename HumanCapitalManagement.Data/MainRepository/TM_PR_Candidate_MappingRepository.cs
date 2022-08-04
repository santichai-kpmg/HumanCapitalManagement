using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_PR_Candidate_MappingRepository : RepositoryBase<TM_PR_Candidate_Mapping>
    {
        public TM_PR_Candidate_MappingRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
