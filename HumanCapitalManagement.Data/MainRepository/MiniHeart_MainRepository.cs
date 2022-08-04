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
    public class MiniHeart_MainRepository : RepositoryBase<MiniHeart_Main>
    {
        public MiniHeart_MainRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
