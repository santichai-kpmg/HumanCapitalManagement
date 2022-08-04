using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    
    public class TM_Trainee_HiringRatingRepository : RepositoryBase<TM_Trainee_HiringRating>
    {
        public TM_Trainee_HiringRatingRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
