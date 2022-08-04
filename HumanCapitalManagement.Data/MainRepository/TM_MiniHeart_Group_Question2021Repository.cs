using HumanCapitalManagement.Models._360Feedback.MiniHeart2021;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_MiniHeart_Group_Question2021Repository : RepositoryBase<TM_MiniHeart_Group_Question2021>
    {
        public TM_MiniHeart_Group_Question2021Repository(DbContext context) : base(context)
        {
            //
        }
    }
}
