using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Candidate_MassTIF_Adnl_AnswerService : ServiceBase<TM_Candidate_MassTIF_Adnl_Answer>
    {
        public TM_Candidate_MassTIF_Adnl_AnswerService(IRepository<TM_Candidate_MassTIF_Adnl_Answer> repo) : base(repo)
        {
            //
        }

        public int UpdateAnswer(List<TM_Candidate_MassTIF_Adnl_Answer> s, int MassID, int qID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Candidate_MassTIF_Additional.Id == MassID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_Additional_Answers.Id).ToArray().Contains(w.TM_Additional_Answers.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.TM_Candidate_MassTIF_Additional != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_Additional_Answers.Id == item.TM_Additional_Answers.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_Additional_Answers.Id == item.TM_Additional_Answers.Id).ToList().ForEach(ed =>
                    {

                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.active_status = "Y";
                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }

        public int UpdateLstAns(List<TM_Candidate_MassTIF_Adnl_Answer> s, int AdditionalID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Candidate_MassTIF_Additional.Id == AdditionalID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_Additional_Answers.Id).ToArray().Contains(w.TM_Additional_Answers.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.TM_Candidate_MassTIF_Additional != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_Additional_Answers.Id == item.TM_Additional_Answers.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_Additional_Answers.Id == item.TM_Additional_Answers.Id).ToList().ForEach(ed =>
                    {
                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.active_status = "Y";
                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
    }
}
