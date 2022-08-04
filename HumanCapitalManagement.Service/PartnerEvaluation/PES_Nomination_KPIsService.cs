using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PES_Nomination_KPIsService : ServiceBase<PES_Nomination_KPIs>
    {
        public PES_Nomination_KPIsService(IRepository<PES_Nomination_KPIs> repo) : base(repo)
        {
            //
        }

        public List<PES_Nomination_KPIs> FindFor3YearKPI(string id, int nYear)
        {

            return Query().Where(s => s.user_id == id && s.PES_Final_Rating_Year.evaluation_year.Value.Year == nYear && s.active_status == "Y").ToList();
        }

        public int Update(PES_Nomination_KPIs s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateKPIs(List<PES_Nomination_KPIs> s, int nEmID, string user_no, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.PES_Final_Rating_Year.Id == nEmID && w.user_id == user_no).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_KPIs_Base_Id).ToArray().Contains(w.TM_KPIs_Base_Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.PES_Final_Rating_Year != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_KPIs_Base_Id == item.TM_KPIs_Base_Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_KPIs_Base_Id == item.TM_KPIs_Base_Id).ToList().ForEach(ed =>
                    {

                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.actual = item.actual;
                        ed.target = item.target;

                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }

        public int ImportKPIs(List<PES_Nomination_KPIs> s, int year_id, string user_no, string UserUpdate, DateTime dNow, PES_Final_Rating_Year pES_Final_Rating_Year)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.PES_Final_Rating_Year.Id == year_id && w.user_id == user_no).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_KPIs_Base_Id).ToArray().Contains(w.TM_KPIs_Base_Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.PES_Final_Rating_Year != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_KPIs_Base_Id == item.TM_KPIs_Base_Id).FirstOrDefault();

                if (Addnew == null)
                {
                    var get = Query().Where(w => w.PES_Final_Rating_Year.Id == year_id).ToList();
                    item.PES_Final_Rating_Year = pES_Final_Rating_Year;

                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_KPIs_Base_Id == item.TM_KPIs_Base_Id).ToList().ForEach(ed =>
                    {

                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.actual = item.actual;
                        ed.target = item.target;

                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
    }
}
