using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;

namespace HumanCapitalManagement.Service.MainService
{

    public class TM_FY_DetailService : ServiceBase<TM_FY_Detail>
    {

        public TM_FY_DetailService(IRepository<TM_FY_Detail> repo) : base(repo)
        {
            //
        }

        public TM_FY_Detail Find(int id)
        {
            var Return = Query().FirstOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }

        public IEnumerable<TM_FY_Detail> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_FY_Detail> GetDataNow(int nYear)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_FY_Plan.fy_year.Value.Year == nYear);
            return sQuery.ToList();
        }
        public int UpdateAnswer(List<TM_FY_Detail> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_FY_Plan.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_Divisions.Id).ToArray().Contains(w.TM_Divisions.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.TM_FY_Plan != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_Divisions.Id == item.TM_Divisions.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_Divisions.Id == item.TM_Divisions.Id).ToList().ForEach(ed =>
                    {
                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.para = item.para;
                        ed.aa = item.aa;
                        ed.ad = item.ad;
                        ed.am = item.am;
                        ed.dir = item.dir;
                        ed.mgr = item.mgr;
                        ed.ptr = item.ptr;
                        ed.sr = item.sr;

                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }

    }





}
