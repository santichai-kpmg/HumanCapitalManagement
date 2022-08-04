using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.TIFForm
{
    public class TM_TIF_RatingRepository : RepositoryBase<TM_TIF_Rating>
    {
        public TM_TIF_RatingRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
