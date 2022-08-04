using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Common
{
    public class LoginController : BaseController
    {
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        // GET: Login
        public ActionResult Login(string qry, string action, string controller)
        {
            vLogin_obj result = new vLogin_obj();
            Session["UserInfo"] = null;
            result.lstEmp = new List<vSelect_Lg>() { new vSelect_Lg { id = "", name = " - Select - ", } };
            string winloginname = this.User.Identity.Name;

            string EmpUserID = winloginname.Substring(winloginname.IndexOf("\\") + 1);


            if (!string.IsNullOrEmpty(EmpUserID))
            {
                try
                {

                    DataTable staff = wsHRis.getEmployeeInfoByUserID(EmpUserID);
                    var getlogin = WebConfigurationManager.AppSettings["IsLogin"];
                    if (getlogin == "0")
                        staff = wsHRis.getEmployeeInfoByUserID(WebConfigurationManager.AppSettings["IsUserID"].Trim());

                    if (staff != null)
                    {
                        if (staff.Rows.Count > 0)
                        {
                            if (staff.Rows.Count == 1)
                            {
                                var Login = HCMFunc.funcLogin(staff.Rows[0].Field<string>("EmpNo") + "");
                                if (!Login.sSucc)
                                {
                                    ViewBag.UserName = CGlobal.UserInfo.FullName;
                                    ViewBag.IsAdmin = CGlobal.UserIsAdmin();
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    return RedirectToAction("ErrorNopermission", "MasterPage");
                                }
                            }
                            else
                            {
                                List<vSelect_Lg> lstEmp = staff.AsEnumerable().Select(s => new vSelect_Lg
                                {
                                    id = staff.Columns.Contains("EmpNo") ? HCMFunc.Encrypt(s.Field<string>("EmpNo")) : "",
                                    name = s.Field<string>("EmpNo") + " : " + s.Field<string>("EmpName") + "(" + s.Field<string>("UnitGroup") + ")",
                                }).ToList();
                                result.lstEmp.AddRange(lstEmp);
                            }
                        }
                        else
                        {
                            return RedirectToAction("ErrorNopermission", "MasterPage");
                        }
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error500", "MasterPage");

                }

            }
            if (string.IsNullOrEmpty(qry))
            {
                return View(result);
            }
            else
            {
                return RedirectToAction(action, controller, new { qryStr = qry });
            }

        }

        //public vObjectLogin_return funcLogin(string emp_no)
        //{
        //    vObjectLogin_return bReturn = new vObjectLogin_return();
        //    string winloginname = this.User.Identity.Name;
        //    string EmpUserID = winloginname.Substring(winloginname.IndexOf("\\") + 1);
        //    bReturn.sSucc = false;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(emp_no))
        //        {
        //            DataTable staff = wsHRis.getActiveStaffByEmpNo(emp_no);
        //            if (staff != null)
        //            {
        //                if (staff.Rows.Count == 1)
        //                {
        //                    New_HRISEntities dbHr = new New_HRISEntities();
        //                    var userID = staff.Columns.Contains("UserID") ? staff.Rows[0].Field<string>("UserID") + "" : "";
        //                    var UserNo = staff.Columns.Contains("EmpNo") ? staff.Rows[0].Field<string>("EmpNo") + "" : "";
        //                    if (UserNo != "" && userID != "" && UserNo == emp_no && userID == EmpUserID)
        //                    {
        //                        User newUser = new User(userID);
        //                        //string[] UserAdmin = WebConfigurationManager.AppSettings["UserAdmin"].Split(';');
        //                        newUser.UserId = staff.Columns.Contains("UserID") ? staff.Rows[0].Field<string>("UserID") + "" : "";
        //                        newUser.EmployeeNo = staff.Columns.Contains("EmpNo") ? staff.Rows[0].Field<string>("EmpNo") + "" : "";
        //                        newUser.FirstName = staff.Columns.Contains("EmpFirstName") ? staff.Rows[0].Field<string>("EmpFirstName") + "" : "";
        //                        newUser.LastName = staff.Columns.Contains("EmpSurname") ? staff.Rows[0].Field<string>("EmpSurname") + "" : "";
        //                        newUser.FullName = staff.Columns.Contains("EmpName") ? staff.Rows[0].Field<string>("EmpName") + "" : "";
        //                        newUser.EMail = staff.Columns.Contains("Email") ? staff.Rows[0].Field<string>("Email") + "" : "";
        //                        newUser.OfficePhone = staff.Columns.Contains("OfficePhone") ? staff.Rows[0].Field<string>("OfficePhone") + "" : "";
        //                        newUser.Company = staff.Columns.Contains("CompanyCode") ? staff.Rows[0].Field<string>("CompanyCode") + "" : "";
        //                        newUser.PI = staff.Columns.Contains("PI") ? staff.Rows[0].Field<string>("PI") + "" : "";
        //                        newUser.Pool = staff.Columns.Contains("Pool") ? staff.Rows[0].Field<string>("Pool") + "" : "";
        //                        newUser.Division = staff.Columns.Contains("Division") ? staff.Rows[0].Field<string>("Division") + "" : "";
        //                        newUser.UnitGroup = staff.Columns.Contains("UnitGroup") ? staff.Rows[0].Field<string>("UnitGroup") + "" : "";
        //                        newUser.Rank = staff.Columns.Contains("RankCode") ? staff.Rows[0].Field<string>("RankCode") + "" : "";
        //                        newUser.lstDivision = new List<lstDivision>();
        //                        var CheckDivi = dbHr.vw_Unit.Where(w => w.UnitID + "" == newUser.Division + "").FirstOrDefault();
        //                        if (CheckDivi != null)
        //                        {
        //                            newUser.lstDivision.Add(new lstDivision
        //                            {
        //                                sID = CheckDivi.UnitGroupID + "",// dbHr.vw_Unit.Where(w => w.UnitID == newUser.Division).Select(s => s.UnitGroupID + "").FirstOrDefault(),
        //                                sName = newUser.UnitGroup,
        //                                from_role = "Y",

        //                            });
        //                        }
        //                        var CheckCEO = dbHr.tbMaster_CompanyHead.Where(w => w.EmployeeNo == newUser.EmployeeNo).ToList();
        //                        var CheckPool = dbHr.tbMaster_PoolHead.Where(w => w.EmployeeNo == newUser.EmployeeNo).ToList();
        //                        var CheckGroupH = dbHr.tbMaster_UnitGroupHead.Where(w => w.EmployeeNo == newUser.EmployeeNo && w.RankID == 0).ToList();
        //                        if (CheckCEO.Any())
        //                        {
        //                            newUser.lstDivision.AddRange(dbHr.vw_Unit.Where(w => (CheckCEO.Select(s => s.CompanyID).ToArray()).Contains(w.CompanyID)).Select(s => new lstDivision
        //                            {
        //                                sID = s.UnitGroupID + "",
        //                                sName = s.UnitName,
        //                                from_role = "Y",
        //                            }).ToList());
        //                        }
        //                        if (CheckPool.Any())
        //                        {
        //                            newUser.lstDivision.AddRange(dbHr.vw_Unit.Where(w => (CheckPool.Select(s => s.PoolID).ToArray()).Contains(w.PoolID)).Select(s => new lstDivision
        //                            {
        //                                sID = s.UnitGroupID + "",
        //                                sName = s.UnitName,
        //                                from_role = "Y",
        //                            }).ToList());
        //                        }
        //                        if (CheckGroupH.Any())
        //                        {
        //                            newUser.lstDivision.AddRange(dbHr.vw_Unit.Where(w => (CheckGroupH.Select(s => s.UnitGroupID).ToArray()).Contains(w.UnitGroupID)).Select(s => new lstDivision
        //                            {
        //                                sID = s.UnitGroupID + "",
        //                                sName = s.UnitName,
        //                                from_role = "Y",
        //                            }).ToList());
        //                        }
        //                        Session["UserInfo"] = newUser;
        //                        ViewBag.UserName = newUser.FullName;
        //                        ViewBag.IsAdmin = CGlobal.UserIsAdmin();
        //                    }
        //                    else
        //                    {
        //                        bReturn.sSucc = true;
        //                    }

        //                }
        //                else
        //                {
        //                    bReturn.sSucc = true;
        //                }
        //            }
        //            else
        //            {
        //                bReturn.sSucc = true;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        bReturn.sSucc = true;
        //        // throw;
        //        return bReturn;
        //    }
        //    if (!string.IsNullOrEmpty(Session["UserInfo"] as string))
        //    {
        //        bReturn.sSucc = true;
        //    }
        //    return bReturn;
        //}
        #region ajax
        [HttpPost]
        public ActionResult ajLogin(vLogin_obj ItemData)
        {
            vLogin_Return result = new vLogin_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.emp_no))
                {
                    string sEmp_no = HCMFunc.Decrypt(ItemData.emp_no);
                    var objLogin = HCMFunc.funcLogin(sEmp_no);
                    if (!objLogin.sSucc)
                    {
                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Can't Login Plz Call Admin.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, select employee no.";
                }
            }
            return Json(new
            {
                result
            });
        }
        #endregion
        public class vObjectLogin_return
        {
            public bool sSucc { get; set; }
            public string msg { get; set; }

        }
    }
}