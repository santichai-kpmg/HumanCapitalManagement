using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Candidate_MassTIF_Audit_QstService : ServiceBase<TM_Candidate_MassTIF_Audit_Qst>
    {
        public TM_Candidate_MassTIF_Audit_QstService(IRepository<TM_Candidate_MassTIF_Audit_Qst> repo) : base(repo)
        {
            //
        }

        public int UpdateAnswer(List<TM_Candidate_MassTIF_Audit_Qst> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Candidate_MassTIF.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.seq).ToArray().Contains(w.seq) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.TM_Candidate_MassTIF != null))
            {
                var Addnew = _getAdv.Where(w => w.seq == item.seq).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.seq == item.seq).ToList().ForEach(ed =>
                    {
                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.answer = item.answer + "";
                        ed.TM_Mass_Scoring = item.TM_Mass_Scoring;
                        ed.TM_Mass_Auditing_Question = item.TM_Mass_Auditing_Question;
                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
    }
}
