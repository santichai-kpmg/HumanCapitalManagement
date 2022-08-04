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
    public class MiniHeart_Main2021Repository : RepositoryBase<MiniHeart_Main2021>
    {
        public MiniHeart_Main2021Repository(DbContext context) : base(context)
        {
            //
        }
    }
}
