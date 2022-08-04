using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.StrengthFinderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.StrengthFinder
{
   public class StrengthFinderService : ServiceBase<TM_StrenghtFinder>
    {
        public StrengthFinderService(IRepository<TM_StrenghtFinder> repo) : base(repo)
        {
            //
        }
    }
}
