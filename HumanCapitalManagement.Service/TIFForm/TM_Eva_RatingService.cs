using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TIFForm
{
    public class TM_Eva_RatingService : ServiceBase<TM_Eva_Rating>
    {
        public TM_Eva_RatingService(IRepository<TM_Eva_Rating> repo) : base(repo)
        {
            //
        }
        public TM_Eva_Rating Find(int id)
        {
            var Return = Query().SingleOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public IEnumerable<TM_Eva_Rating> GetDataForSelectAll()
        {
            var sQuery = Query().OrderBy(o => o.seq);
            return sQuery.ToList();
        }
        public IEnumerable<TM_Eva_Rating> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
        public IEnumerable<TM_Eva_Rating> GetActiveAndInactiveForSelect()
        {
            var sQuery = Query().OrderBy(o => o.seq);
            return sQuery.ToList();
        }

        public IEnumerable<TM_Eva_Rating> GetDataForSelectOld()
        {
            var sQuery = Query().Where(w => w.active_status == "N").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
    }
}
