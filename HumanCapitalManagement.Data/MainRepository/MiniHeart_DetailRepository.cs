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
    public class MiniHeart_DetailRepository : RepositoryBase<MiniHeart_Detail>
    {
        public MiniHeart_DetailRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
