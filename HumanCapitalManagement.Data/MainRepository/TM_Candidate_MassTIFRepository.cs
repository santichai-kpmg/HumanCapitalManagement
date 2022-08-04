using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Candidate_MassTIFRepository : RepositoryBase<TM_Candidate_MassTIF>
    {
        public TM_Candidate_MassTIFRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
