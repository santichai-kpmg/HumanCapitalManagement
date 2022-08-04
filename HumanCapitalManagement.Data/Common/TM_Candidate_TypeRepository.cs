using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class TM_Candidate_TypeRepository : RepositoryBase<TM_Candidate_Type>
    {
        public TM_Candidate_TypeRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
