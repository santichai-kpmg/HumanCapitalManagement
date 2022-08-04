using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_PositionService : ServiceBase<TM_Position>
    {
        public TM_PositionService(IRepository<TM_Position> repo) : base(repo)
        {
            //
        }
        public TM_Position Find(int id)
        {
            var Return = Query().FirstOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public IEnumerable<TM_Position> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Position> GetPosition(string name, string status, string group_id)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.position_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.position_name_en + "").Trim().ToLower())));
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }

            if (!string.IsNullOrEmpty(group_id))
            {
                sQuery = sQuery.Where(w => w.TM_Divisions.division_code == group_id);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public bool CanSave(TM_Position TM_Position)
        {
            bool sCan = false;

            if (TM_Position.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.position_name_en + "").Trim() == (TM_Position.position_name_en + "").Trim() && w.TM_Divisions.Id == TM_Position.TM_Divisions.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.position_name_en + "").Trim() == (TM_Position.position_name_en + "").Trim() && w.Id != TM_Position.Id && w.TM_Divisions.Id == TM_Position.TM_Divisions.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Position s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Position s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.position_name_en = (s.position_name_en + "").Trim();
                _getData.position_short_name_en = (s.position_short_name_en + "").Trim();
                _getData.job_descriptions = s.job_descriptions;
                _getData.qualification_experience = s.qualification_experience;
                //_getData.TM_Divisions = s.TM_Divisions;
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
