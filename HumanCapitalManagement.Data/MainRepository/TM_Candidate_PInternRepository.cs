using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Candidate_PInternRepository : RepositoryBase<TM_Candidate_PIntern>
    {
        public TM_Candidate_PInternRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
