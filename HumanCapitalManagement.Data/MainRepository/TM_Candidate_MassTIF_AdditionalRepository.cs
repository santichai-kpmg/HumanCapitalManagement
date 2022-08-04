using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Candidate_MassTIF_AdditionalRepository : RepositoryBase<TM_Candidate_MassTIF_Additional>
    {
        public TM_Candidate_MassTIF_AdditionalRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
