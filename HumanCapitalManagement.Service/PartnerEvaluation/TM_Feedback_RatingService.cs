using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class TM_Feedback_RatingService : ServiceBase<TM_Feedback_Rating>
    {
        public TM_Feedback_RatingService(IRepository<TM_Feedback_Rating> repo) : base(repo)
        {
            //
        }
        public TM_Feedback_Rating Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_Feedback_Rating> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
    }
}
