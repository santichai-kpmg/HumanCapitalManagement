using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TIFForm
{
    public class TM_Mass_Auditing_QuestionService : ServiceBase<TM_Mass_Auditing_Question>
    {
        public TM_Mass_Auditing_QuestionService(IRepository<TM_Mass_Auditing_Question> repo) : base(repo)
        {
            //
        }
        public TM_Mass_Auditing_Question Find(int id)
        {
            var Return = Query().SingleOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public IEnumerable<TM_Mass_Auditing_Question> FindForTIFForm(int nType, int nTIF)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_Mass_Question_Type.Id == nType && w.TM_Mass_TIF_Form.Id == nTIF).OrderBy(o => o.seq);
            return sQuery.ToList();
        }
        public IEnumerable<TM_Mass_Auditing_Question> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
    }
}
