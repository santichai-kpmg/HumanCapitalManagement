using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PreInternAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PreInternAssessment
{
    public class TM_PIntern_RatingService : ServiceBase<TM_PIntern_Rating>
    {
        public TM_PIntern_RatingService(IRepository<TM_PIntern_Rating> repo) : base(repo)
        {
            //
        }
        public TM_PIntern_Rating Find(int id)
        {
            var Return = Query().SingleOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public IEnumerable<TM_PIntern_Rating> GetDataForSelectAll()
        {
            var sQuery = Query().OrderBy(o => o.seq);
            return sQuery.ToList();
        }
        public IEnumerable<TM_PIntern_Rating> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
        public IEnumerable<TM_PIntern_Rating> GetDataForSelectOld()
        {
            var sQuery = Query().Where(w => w.active_status == "N").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
    }
}
