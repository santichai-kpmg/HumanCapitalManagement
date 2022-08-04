using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.TIFForm
{
    public class TM_Eva_RatingRepository : RepositoryBase<TM_Eva_Rating>
    {
        public TM_Eva_RatingRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
