using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class GroupPermissionService : ServiceBase<GroupPermission>
    {
        public GroupPermissionService(IRepository<GroupPermission> repo) : base(repo)
        {
            //
        }
        #region Load Data
        public GroupPermission Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public int SelectMax()
        {
            int nReturn;
            var sCheck = Query().FirstOrDefault();
            if (sCheck != null)
            {
                nReturn = Query().Max(m => m.Id) + 1;
            }
            else
            {
                nReturn = 1;
            }
            return nReturn;
        }
        public IEnumerable<GroupPermission> GetGroupPermissionList(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.group_name_th + "").Trim().ToLower() + (w.group_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.group_name_th + "").Trim().ToLower() + (w.group_name_en + "").Trim().ToLower())));

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

        public IEnumerable<GroupPermission> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }

        public bool CanSave(GroupPermission GroupPermission)
        {
            bool sCan = false;

            if (GroupPermission.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.group_name_en + "").Trim() == (GroupPermission.group_name_en + "").Trim());
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.group_name_en + "").Trim() == (GroupPermission.group_name_en + "").Trim() && w.Id != GroupPermission.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #endregion


        #region Save Edit Delect 
        public int CreateNew(GroupPermission s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(GroupPermission s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.group_name_en = (s.group_name_en + "").Trim();
                _getData.group_name_th = (s.group_name_th + "").Trim();
                _getData.group_description = s.group_description;
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
