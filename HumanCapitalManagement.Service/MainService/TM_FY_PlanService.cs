using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;


namespace HumanCapitalManagement.Service.MainService
{
    public class TM_FY_PlanService : ServiceBase<TM_FY_Plan>
    {
        public TM_FY_PlanService(IRepository<TM_FY_Plan> repo) : base(repo)
        {
            //
        }
        public TM_FY_Plan Find(int id)
        {
            var Return = Query().FirstOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public TM_FY_Plan FindbyYear(int nYear)
        {
            var Return = Query().FirstOrDefault(s => s.fy_year.Value.Year == nYear && s.active_status == "Y");
            return Return != null ? Return : null;
        }
        public int GetActiveYear()
        {
            DateTime dNow = DateTime.Now;
            var Return = Query().FirstOrDefault(s => s.fy_year.Value.Year == dNow.Year && s.active_status == "Y");
            return Return != null ? Return.Id : 0;
        }
        public int GetYear(int nYear)
        {
            DateTime dNow = DateTime.Now;
            var Return = Query().FirstOrDefault(s => s.fy_year.Value.Year == nYear && s.active_status == "Y");
            return Return != null ? Return.Id : 0;
        }
        public IEnumerable<TM_FY_Plan> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_FY_Plan> GetFYPlanList(int nYear)
        {
            var sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y");

            if (nYear != 0)
            {
                sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y" && w.Id == nYear);
            }
            return sQuery.ToList();
        }
        public TM_FY_Plan GetFYPlanNow(int nYear)
        {
            var sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y");
            if (nYear != 0)
            {
                sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y" && w.fy_year.Value.Year == nYear);
            }
            return sQuery.FirstOrDefault();
        }
        public bool CanSave(TM_FY_Plan TM_FY_Plan)
        {
            bool sCan = false;

            if (TM_FY_Plan.Id == 0)
            {
                if (Query().Any())
                {
                    var sCheck = Query().FirstOrDefault(w => w.fy_year.Value.Year == TM_FY_Plan.fy_year.Value.Year);
                    if (sCheck == null)
                    {
                        sCan = true;
                    }
                }
                else
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => w.fy_year.Value.Year == TM_FY_Plan.fy_year.Value.Year && w.Id != TM_FY_Plan.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(ref TM_FY_Plan s)
        {
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_FY_Plan s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
