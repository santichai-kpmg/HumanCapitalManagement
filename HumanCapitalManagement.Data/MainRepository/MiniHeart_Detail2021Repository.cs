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
    public class MiniHeart_Detail2021Repository : RepositoryBase<MiniHeart_Detail2021>
    {
        public MiniHeart_Detail2021Repository(DbContext context) : base(context)
        {
            //
        }
    }
}
