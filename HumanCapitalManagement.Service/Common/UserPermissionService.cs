using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class UserPermissionService : ServiceBase<UserPermission>
    {
        public UserPermissionService(IRepository<UserPermission> repo) : base(repo)
        {
            //
        }
        #region Load Data
        public IEnumerable<UserPermission> GetAllUserPermissions()
        {
            return Query().ToList();
        }
        public UserPermission Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public UserPermission FindByEmpNo(string id)
        {
            return Query().FirstOrDefault(s => s.user_no == id);
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
        public IEnumerable<UserPermission> GetUserPermissionList(string[] code, int nGID, string status)//,bool isAdmin
        {
            var sQuery = Query();

            if (code != null)
            {
                sQuery = sQuery.Where(w => code.Contains(((w.user_id + "").Trim()).ToLower()));
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            if (nGID != 0)
            {
                sQuery = sQuery.Where(w => w.GroupPermission.Id == nGID);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }

        public IEnumerable<UserPermission> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }

        public bool CanSave(UserPermission UserPermission)
        {
            bool sCan = false;
            if (!string.IsNullOrEmpty(UserPermission.user_id))
            {
                if (UserPermission.Id == 0)
                {
                    var sCheck = Query().FirstOrDefault(w => ((w.user_id + "").Trim()).ToLower() == ((UserPermission.user_id + "").Trim()).ToLower());
                    if (sCheck == null)
                    {
                        sCan = true;
                    }
                }
                else
                {
                    var sCheck = Query().FirstOrDefault(w => ((w.user_id + "").Trim()).ToLower() == ((UserPermission.user_id + "").Trim()).ToLower() && w.Id != UserPermission.Id);
                    if (sCheck == null)
                    {
                        sCan = true;
                    }
                }
            }

            return sCan;
        }
        #endregion


        #region Save Edit Delect 
        public int CreateNew(ref UserPermission s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(UserPermission s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.GroupPermission = s.GroupPermission;
                _getData.User_description = s.User_description;
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion

    }
}
