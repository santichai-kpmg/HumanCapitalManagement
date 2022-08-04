using HumanCapitalManagement.Models.StrengthFinderModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.TB_StrengthFinderHistoryRepository
{
    public class TB_StrengthFinderHistoryRepository : RepositoryBase<TB_StrengthFinderHistory>
    {
        public TB_StrengthFinderHistoryRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
