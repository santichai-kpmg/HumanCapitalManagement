using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TIFForm
{
    public class TM_TIF_RatingService : ServiceBase<TM_TIF_Rating>
    {
        public TM_TIF_RatingService(IRepository<TM_TIF_Rating> repo) : base(repo)
        {
            //
        }
        public TM_TIF_Rating Find(int id)
        {
            var Return = Query().SingleOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public IEnumerable<TM_TIF_Rating> GetDataForSelectAll()
        {
            var sQuery = Query().OrderBy(o => o.seq);
            return sQuery.ToList();
        }
        public IEnumerable<TM_TIF_Rating> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
        public IEnumerable<TM_TIF_Rating> GetDataForSelectOld()
        {
            var sQuery = Query().Where(w => w.active_status == "N").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
    }
}
