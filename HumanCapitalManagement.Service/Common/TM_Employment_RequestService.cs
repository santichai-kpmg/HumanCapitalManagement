using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class TM_Employment_RequestService : ServiceBase<TM_Employment_Request>
    {
        public TM_Employment_RequestService(IRepository<TM_Employment_Request> repo) : base(repo)
        {
            //
        }
        public TM_Employment_Request Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public int UpdateEmployment_Request(List<TM_Employment_Request> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Employment_Type.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_Request_Type.Id).ToArray().Contains(w.TM_Request_Type.Id) && w.TM_Employment_Type.Id == nEmID && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active
            _getAdv.Where(w => s.Select(s2 => s2.TM_Request_Type.Id).ToArray().Contains(w.TM_Request_Type.Id) && w.TM_Employment_Type.Id == nEmID && w.active_status == "N").ToList().ForEach(ed =>
            {
                ed.active_status = "Y";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            foreach (var item in s)
            {
                var Addnew = _getAdv.Where(w => w.TM_Employment_Type.Id == item.TM_Employment_Type.Id && w.TM_Request_Type.Id == item.TM_Request_Type.Id).FirstOrDefault();
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
