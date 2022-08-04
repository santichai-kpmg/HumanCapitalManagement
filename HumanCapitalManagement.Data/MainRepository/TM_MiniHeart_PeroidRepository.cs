using HumanCapitalManagement.Models._360Feedback.MiniHeart;
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
    public class TM_MiniHeart_PeroidRepository : RepositoryBase<TM_MiniHeart_Peroid>
    {
        public TM_MiniHeart_PeroidRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
