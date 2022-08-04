using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Candidate_PIntern_AnswerService : ServiceBase<TM_Candidate_PIntern_Answer>
    {
        public TM_Candidate_PIntern_AnswerService(IRepository<TM_Candidate_PIntern_Answer> repo) : base(repo)
        {
            //
        }
        public int UpdateAnswer(List<TM_Candidate_PIntern_Answer> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Candidate_PIntern.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_PIntern_Form_Question.Id).ToArray().Contains(w.TM_PIntern_Form_Question.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.TM_Candidate_PIntern != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_PIntern_Form_Question.Id == item.TM_PIntern_Form_Question.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_PIntern_Form_Question.Id == item.TM_PIntern_Form_Question.Id).ToList().ForEach(ed =>
                    {

                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.answer = item.answer + "";
                        ed.TM_PIntern_Rating = item.TM_PIntern_Rating;
                        ed.active_status = item.active_status;
                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
        public int InactiveAnswer( int nPInternID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Candidate_PIntern.Id == nPInternID).ToList();
            //set old to inactive
            _getAdv.Where(w =>  w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active
            sResult = SaveChanges();
            return sResult;
        }
    }
}
