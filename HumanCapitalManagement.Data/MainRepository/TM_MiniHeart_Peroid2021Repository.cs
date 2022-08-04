using HumanCapitalManagement.Models._360Feedback.MiniHeart2021;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.Probation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_MiniHeart_Peroid2021Repository : RepositoryBase<TM_MiniHeart_Peroid2021>
    {
        public TM_MiniHeart_Peroid2021Repository(DbContext context) : base(context)
        {
            //
        }
    }
}
