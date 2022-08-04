using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Candidate_MassTIF_CoreService : ServiceBase<TM_Candidate_MassTIF_Core>
    {
        public TM_Candidate_MassTIF_CoreService(IRepository<TM_Candidate_MassTIF_Core> repo) : base(repo)
        {
            //
        }
        public int UpdateAnswer(List<TM_Candidate_MassTIF_Core> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Candidate_MassTIF.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_MassTIF_Form_Question.Id).ToArray().Contains(w.TM_MassTIF_Form_Question.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.TM_Candidate_MassTIF != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_MassTIF_Form_Question.Id == item.TM_MassTIF_Form_Question.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_MassTIF_Form_Question.Id == item.TM_MassTIF_Form_Question.Id).ToList().ForEach(ed =>
                    {

                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.evidence = item.evidence + "";
                        ed.TM_Mass_Scoring = item.TM_Mass_Scoring;
                        ed.TM_TIF_Rating_Id = item.TM_TIF_Rating_Id;
                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
    }
}
