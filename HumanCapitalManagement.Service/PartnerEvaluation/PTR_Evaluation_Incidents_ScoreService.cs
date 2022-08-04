using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PTR_Evaluation_Incidents_ScoreService : ServiceBase<PTR_Evaluation_Incidents_Score>
    {
        public PTR_Evaluation_Incidents_ScoreService(IRepository<PTR_Evaluation_Incidents_Score> repo) : base(repo)
        {
            //
        }


        public IEnumerable<PTR_Evaluation_Incidents_Score> GetScored(
      int[] nID,
      bool isAdmin = false)//,bool isAdmin
        {
            //var sQuery = Query().Where(w => w.active_status == "Y" && (w.TM_PR_Status.Id == 3 || w.TM_PR_Status.Id == 2));
            var sQuery = Query().Where(w => w.active_status == "Y" && w.PTR_Evaluation_Approve_Id.HasValue /*&& w.PTR_Evaluation_Approve.Approve_status == "Y"*/);
            if ((nID != null && nID.Length > 0))
            {
                sQuery = sQuery.Where(w => nID.Contains((int)w.PTR_Evaluation_Approve_Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }
            return sQuery.ToList();
        }

        public int UpdateAnswer(List<PTR_Evaluation_Incidents_Score> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.PTR_Evaluation_Approve.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_PTR_Eva_Incidents.Id).ToArray().Contains(w.TM_PTR_Eva_Incidents.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.PTR_Evaluation_Approve != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_PTR_Eva_Incidents.Id == item.TM_PTR_Eva_Incidents.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_PTR_Eva_Incidents.Id == item.TM_PTR_Eva_Incidents.Id).ToList().ForEach(ed =>
                    {

                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.answer = item.answer + "";
                        ed.TM_PTR_Eva_Incidents_Score_Id = item.TM_PTR_Eva_Incidents_Score_Id;

                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }

    }
}
