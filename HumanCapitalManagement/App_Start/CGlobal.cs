using HumanCapitalManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace HumanCapitalManagement.App_Start
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
        public static bool UserIsHRAdmin()
        {
            bool bReturn = false;
            try
            {
                string[] UserAdmin = WebConfigurationManager.AppSettings["UserHRAdmin"].Split(';');
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
        public static bool IsRecruitmentOrAdmin()
        {
            bool bReturn = false;
            StoreDb db = new StoreDb();
            try
            {
                string[] UserAdmin = WebConfigurationManager.AppSettings["UserAdmin"].Split(';');
                //bReturn = db.UserPermissions.Any(w => w.user_no == CGlobal.UserInfo.EmployeeNo && w.active_status == "Y");

                if (UserAdmin.Length > 0 && UserAdmin.Contains(CGlobal.UserInfo.UserId) || db.TM_Recruitment_Team.Any(a => a.user_no.Contains(CGlobal.UserInfo.EmployeeNo) && a.active_status == "Y"))
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
        public static bool UserIsAdminPES()
        {
            bool bReturn = false;
            try
            {
                string[] UserAdmin = WebConfigurationManager.AppSettings["PESAdmin"].Split(';');
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
        public static List<lstDivision> GetDivision()
        {
            StoreDb db = new StoreDb();
            List<lstDivision> lstReturn = new List<lstDivision>();
            if (CGlobal.UserInfo.lstDivision.Any())
            {
                lstReturn = CGlobal.UserInfo.lstDivision.ToList();
            }
            string[] aCode = lstReturn.Select(s => s.sID).ToArray();
            var GetDivisionFromSystem = db.UserPermission.Where(w => w.user_no == CGlobal.UserInfo.EmployeeNo && w.active_status == "Y").FirstOrDefault();
            string[] UserAdmin = WebConfigurationManager.AppSettings["UserAdmin"].Split(';');
            if ((GetDivisionFromSystem != null && GetDivisionFromSystem.UserUnitGroup != null && GetDivisionFromSystem.UserUnitGroup.Any()) || (UserAdmin.Length > 0 && UserAdmin.Contains(CGlobal.UserInfo.UserId)))
            {
                if (UserAdmin.Length > 0 && UserAdmin.Contains(CGlobal.UserInfo.UserId))
                {
                    lstReturn.AddRange(db.TM_Divisions.Where(w => !aCode.Contains(w.division_code + "") && w.active_status == "Y").Select(s => new lstDivision
                    {
                        sID = s.division_code,
                        sName = s.division_name_en,
                        from_role = "N",
                        sCompany_code = s.TM_Pool.TM_Company.Company_code + "",
                    }).ToList());
                }
                else
                {
                    lstReturn.AddRange(GetDivisionFromSystem.UserUnitGroup.Where(w => !aCode.Contains(w.TM_Divisions.division_code + "") && w.active_status == "Y" && w.TM_Divisions.active_status + "" == "Y").Select(s => new lstDivision
                    {
                        sID = s.TM_Divisions.division_code,
                        sName = s.TM_Divisions.division_name_en,
                        from_role = "N",
                        sCompany_code = s.TM_Divisions.TM_Pool.TM_Company.Company_code + "",
                    }).ToList());
                }

            }
            return lstReturn;
        }
    
    }
}