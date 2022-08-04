using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TIFForm
{
    public class TM_Mass_Question_TypeService : ServiceBase<TM_Mass_Question_Type>
    {
        public TM_Mass_Question_TypeService(IRepository<TM_Mass_Question_Type> repo) : base(repo)
        {
            //
        }
        public TM_Mass_Question_Type Find(int id)
        {
            var Return = Query().SingleOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public IEnumerable<TM_Mass_Question_Type> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
    }
}
