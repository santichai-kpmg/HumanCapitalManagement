using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PTR_Evaluation_AnswerService : ServiceBase<PTR_Evaluation_Answer>
    {
        public PTR_Evaluation_AnswerService(IRepository<PTR_Evaluation_Answer> repo) : base(repo)
        {
            //
        }
        public int UpdateAnswer(List<PTR_Evaluation_Answer> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.PTR_Evaluation.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_PTR_Eva_Questions.Id).ToArray().Contains(w.TM_PTR_Eva_Questions.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.PTR_Evaluation != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_PTR_Eva_Questions.Id == item.TM_PTR_Eva_Questions.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_PTR_Eva_Questions.Id == item.TM_PTR_Eva_Questions.Id).ToList().ForEach(ed =>
                    {

                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.answer = item.answer + "";

                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
        public int UpdatePlanAnswer(List<PTR_Evaluation_Answer> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.PTR_Evaluation.Id == nEmID && w.TM_PTR_Eva_Questions.questions_type == "P").ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_PTR_Eva_Questions.Id).ToArray().Contains(w.TM_PTR_Eva_Questions.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.PTR_Evaluation != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_PTR_Eva_Questions.Id == item.TM_PTR_Eva_Questions.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_PTR_Eva_Questions.Id == item.TM_PTR_Eva_Questions.Id).ToList().ForEach(ed =>
                    {

                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.answer = item.answer + "";

                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
    }
}
