using HumanCapitalManagement.Models._360Feedback;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class FeedbackRepository : RepositoryBase<Feedback>
    {
        public FeedbackRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
