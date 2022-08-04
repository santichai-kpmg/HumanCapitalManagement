using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class TM_SubGroupService : ServiceBase<TM_SubGroup>
    {
        public TM_SubGroupService(IRepository<TM_SubGroup> repo) : base(repo)
        {
            //
        }
        public TM_SubGroup Find(int id)
        {
            var Return = Query().FirstOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public IEnumerable<TM_SubGroup> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_SubGroup> GetSubGroupForSave(int[] aID)//,bool isAdmin
        {
            var sQuery = Query();
            if (aID != null)
            {
                sQuery = sQuery.Where(w => aID.Contains(w.Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }
        public IEnumerable<TM_SubGroup> GetSubGroup(string name, string group_id, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.sub_group_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.sub_group_name_en + "").Trim().ToLower())));
            }

            if (!string.IsNullOrEmpty(group_id))
            {
                sQuery = sQuery.Where(w => w.TM_Divisions.division_code == group_id);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public bool CanSave(TM_SubGroup TM_SubGroup)
        {
            bool sCan = false;

            if (TM_SubGroup.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.sub_group_name_en + "").Trim() == (TM_SubGroup.sub_group_name_en + "").Trim() && w.TM_Divisions.Id == TM_SubGroup.TM_Divisions.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.sub_group_name_en + "").Trim() == (TM_SubGroup.sub_group_name_en + "").Trim() && w.Id != TM_SubGroup.Id && w.TM_Divisions.Id == TM_SubGroup.TM_Divisions.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_SubGroup s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_SubGroup s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.sub_group_name_en = (s.sub_group_name_en + "").Trim();
                _getData.sub_group_short_name_en = (s.sub_group_short_name_en + "").Trim();
                _getData.sub_group_description = s.sub_group_description;
                _getData.TM_Divisions = s.TM_Divisions;
                _getData.head_user_id = s.head_user_id;
                _getData.head_user_no = s.head_user_no;
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
