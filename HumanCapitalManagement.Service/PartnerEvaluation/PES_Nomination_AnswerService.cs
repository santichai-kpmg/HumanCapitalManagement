using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PES_Nomination_AnswerService : ServiceBase<PES_Nomination_Answer>
    {
        public PES_Nomination_AnswerService(IRepository<PES_Nomination_Answer> repo) : base(repo)
        {
            //
        }
        public int UpdateAnswer(List<PES_Nomination_Answer> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.PES_Nomination.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_PES_NMN_Questions_Id).ToArray().Contains(w.TM_PES_NMN_Questions_Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.PES_Nomination != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_PES_NMN_Questions_Id == item.TM_PES_NMN_Questions_Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_PES_NMN_Questions_Id == item.TM_PES_NMN_Questions_Id).ToList().ForEach(ed =>
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
        public int UpdatePlanAnswer(List<PES_Nomination_Answer> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.PES_Nomination.Id == nEmID && w.TM_PES_NMN_Questions.questions_type == "P").ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_PES_NMN_Questions.Id).ToArray().Contains(w.TM_PES_NMN_Questions.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.PES_Nomination != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_PES_NMN_Questions.Id == item.TM_PES_NMN_Questions.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_PES_NMN_Questions.Id == item.TM_PES_NMN_Questions.Id).ToList().ForEach(ed =>
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
