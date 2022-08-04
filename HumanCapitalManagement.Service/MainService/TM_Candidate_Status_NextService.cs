using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Candidate_Status_NextService : ServiceBase<TM_Candidate_Status_Next>
    {
        public TM_Candidate_Status_NextService(IRepository<TM_Candidate_Status_Next> repo) : base(repo)
        {
            //
        }
        public int UpdateNext_Status(List<TM_Candidate_Status_Next> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Candidate_Status.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.next_status_id).ToArray().Contains(w.next_status_id)  && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active
            _getAdv.Where(w => s.Select(s2 => s2.next_status_id).ToArray().Contains(w.next_status_id) && w.active_status == "N").ToList().ForEach(ed =>
            {
                ed.active_status = "Y";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            foreach (var item in s)
            {
                var Addnew = _getAdv.Where(w => w.next_status_id == item.next_status_id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
    }
}
