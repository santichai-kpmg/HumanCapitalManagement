using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class TM_Annual_RatingService : ServiceBase<TM_Annual_Rating>
    {
        public TM_Annual_RatingService(IRepository<TM_Annual_Rating> repo) : base(repo)
        {
            //
        }
        public TM_Annual_Rating Find(int id)
        {
            var Return = Query().SingleOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public IEnumerable<TM_Annual_Rating> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y").OrderBy(o => o.seq);
            return sQuery.ToList();
        } 
        
        public IEnumerable<TM_Annual_Rating> GetDataForSelect22()
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.rating_code =="2022").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
    }
}
