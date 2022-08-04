using HumanCapitalManagement.Models._360Feedback.MiniHeart;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_MiniHeart_QuestionRepository : RepositoryBase<TM_MiniHeart_Question>
    {
        public TM_MiniHeart_QuestionRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
