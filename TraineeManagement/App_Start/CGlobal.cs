using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace TraineeManagement.App_Start
{
    public class CGlobal
    {
        public static User UserInfo
        {
            get { return (User)HttpContext.Current.Session["UserInfo"]; }
            set { HttpContext.Current.Session["UserInfo"] = value; }
        }
        public static bool IsUserExpired()
        {
            return !(HttpContext.Current.Session["UserInfo"] is User);
        }
        public static bool UserValidate()
        {

            try
            {
                if (CGlobal.UserInfo == null) throw new Exception("UserInfo == null");
                if (string.IsNullOrEmpty(CGlobal.UserInfo.UserId)) throw new Exception("USER_ID IsNullOrEmpty");
                // if (string.IsNullOrEmpty(CGlobal.UserInfo.MAPDEPTCODE)) throw new Exception("MAPDEPTCODE IsNullOrEmpty");
            }
            catch (Exception ex)
            {
                // var err_msg = DataObjects.UtilDao.Error_Log(CGlobal.UserInfo, ex.Message + " : " + ex.StackTrace.ToString(), HttpContext.Current.Request.Url.AbsoluteUri);
                return false;
            }

            return true;

        }
        public static bool UserIsAdmin()
        {
            bool bReturn = false;
            try
            {
                string[] UserAdmin = WebConfigurationManager.AppSettings["UserAdmin"].Split(';');
                //bReturn = db.UserPermissions.Any(w => w.user_no == CGlobal.UserInfo.EmployeeNo && w.active_status == "Y");
                if (UserAdmin.Length > 0 && UserAdmin.Contains(CGlobal.UserInfo.UserId))
                {
                    bReturn = true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return bReturn;
        }
    }
}