using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class MenuActionService : ServiceBase<MenuAction>
    {
        public MenuActionService(IRepository<MenuAction> repo) : base(repo)
        {
            //
        }
        #region Load Data
        public int GetActionMenu(string Controller, string Action)
        {
            int nReturn = 0;
            var sQuery = Query().Where(w => (w.Controller + "").Trim().ToLower() == (Controller + "").Trim().ToLower() &&
                (w.Action + "").Trim().ToLower() == (Action + "").Trim().ToLower()).FirstOrDefault();
            if (sQuery != null)
            {
                nReturn = sQuery.Menu.Id;
            }
            return nReturn;
        }
        public IEnumerable<MenuAction> SelectMenuActionByaID(int[] mAc_id, int[] aAction_type)
        {
            return Query().Where(w => mAc_id.Contains(w.Menu.Id) && aAction_type.Contains(w.Action_Type)).ToList();
        }
        public IEnumerable<MenuAction> GetMenuActionByMenuAndType(int? mAc_id, int aAction_type)
        {
            if (mAc_id.HasValue)
            {
                return Query().Where(w => w.Menu.Id == mAc_id.Value && w.Action_Type == aAction_type).ToList();
            }
            else
            {
                return new List<MenuAction>();
            }

        }
        #endregion
    }
}
