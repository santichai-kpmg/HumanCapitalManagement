﻿using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class UserUnitGroupService : ServiceBase<UserUnitGroup>
    {
        public UserUnitGroupService(IRepository<UserUnitGroup> repo) : base(repo)
        {
            //
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
        public int UpdateUserUnitGroupService(List<UserUnitGroup> s, int nUserID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.UserPermission.Id == nUserID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_Divisions.Id).ToArray().Contains(w.TM_Divisions.Id) && w.UserPermission.Id == nUserID && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active
            _getAdv.Where(w => s.Select(s2 => s2.TM_Divisions.Id).ToArray().Contains(w.TM_Divisions.Id) && w.UserPermission.Id == nUserID && w.active_status == "N").ToList().ForEach(ed =>
            {
                ed.active_status = "Y";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            foreach (var item in s)
            {
                var Addnew = _getAdv.Where(w => w.UserPermission.Id == item.UserPermission.Id && w.TM_Divisions.Id == item.TM_Divisions.Id).FirstOrDefault();
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
