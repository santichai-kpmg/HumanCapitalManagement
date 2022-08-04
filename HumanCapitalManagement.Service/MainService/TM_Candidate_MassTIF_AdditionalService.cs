using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Candidate_MassTIF_AdditionalService : ServiceBase<TM_Candidate_MassTIF_Additional>
    {
        public TM_Candidate_MassTIF_AdditionalService(IRepository<TM_Candidate_MassTIF_Additional> repo) : base(repo)
        {
            //
        }


        public TM_Candidate_MassTIF_Additional FindForSave(int MassID , int nQusID)
        {
            return Query().SingleOrDefault(s => s.TM_Candidate_MassTIF.Id == MassID && s.TM_Additional_Questions.Id == nQusID);
        }
        public IEnumerable<TM_Candidate_MassTIF_Additional> GetListForUpdateAns(int MassID, int[] aQuestion)
        {
            var sQuery = Query();
            if (MassID != 0 && aQuestion.Length > 0)
            {
                sQuery = sQuery.Where(w => w.TM_Candidate_MassTIF.Id == MassID && aQuestion.Contains(w.TM_Additional_Questions.Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }


            return sQuery.ToList();
        }

        public int UpdateAnswer(List<TM_Candidate_MassTIF_Additional> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Candidate_MassTIF.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_Additional_Questions.Id).ToArray().Contains(w.TM_Additional_Questions.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.TM_Candidate_MassTIF != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_Additional_Questions.Id == item.TM_Additional_Questions.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_Additional_Questions.Id == item.TM_Additional_Questions.Id).ToList().ForEach(ed =>
                    {

                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.other_answer = item.other_answer + "";

                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
    }
}
