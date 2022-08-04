using HumanCapitalManagement.Models.StrengthFinderModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.TM_StrengthFinderRepository
{
    public class TM_StrengthFinderRepository : RepositoryBase<TM_StrenghtFinder>
    {
        public TM_StrengthFinderRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
