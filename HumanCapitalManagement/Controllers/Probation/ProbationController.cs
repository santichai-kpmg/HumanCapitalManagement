using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.Probation;
using HumanCapitalManagement.Report.DataSet.Probation;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.ProbationVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Probation
{
    public class ProbationController : BaseController
    {
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        private Probation_FormService _Probation_FormService;
        private Probation_DetailService _Probation_DetailService;
        private TM_Probation_QuestionService _TM_Probation_QuestionService;
        private TM_Probation_Group_QuestionService _TM_Probation_Group_QuestionService;
        private Action_PlansService _Action_PlansService;
        private Probation_With_OutService _Probation_With_OutService;

        New_HRISEntities dbHr = new New_HRISEntities();
        public ProbationController(
           TM_CandidatesService TM_CandidatesService
            , MailContentService MailContentService
            , Probation_FormService Probation_FormService
            , Probation_DetailService Probation_DetailService
            , TM_Probation_QuestionService TM_Probation_QuestionService
            , TM_Probation_Group_QuestionService TM_Probation_Group_QuestionService
            , Action_PlansService Action_PlansService
            , Probation_With_OutService Probation_With_OutService
            )
        {

            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;
            _Probation_FormService = Probation_FormService;
            _Probation_DetailService = Probation_DetailService;
            _TM_Probation_QuestionService = TM_Probation_QuestionService;
            _TM_Probation_Group_QuestionService = TM_Probation_Group_QuestionService;
            _Action_PlansService = Action_PlansService;
            _Probation_With_OutService = Probation_With_OutService;
        }

        // GET: Probation

        #region parameter

        public int[] lstunitgroupauditK = { 14100058, 14100059, 14100060, 14100002, 14100063 };

        public class ResultLine
        {
            public string Pro_Id { get; set; }

            public string Company { get; set; }
            public string Cost_Center { get; set; }

            public string ST_user_name { get; set; }
            public string ST_user_no { get; set; }
            public string ST_Date { get; set; }
            public string ST_Rank { get; set; }

            public string PM_user_name { get; set; }
            public string PM_user_no { get; set; }
            public string PM_Date { get; set; }
            public string PM_Cost { get; set; }

            public string GH_user_name { get; set; }
            public string GH_user_no { get; set; }
            public string GH_Date { get; set; }

            public string HOP_user_name { get; set; }
            public string HOP_user_no { get; set; }
            public string HOP_Date { get; set; }

        }

        #endregion parameter

        #region View

        public ActionResult ProbationList(string qryStr)
        {
            var check = CheckLogin();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            //CGlobal.UserInfo.UserId = "wopasyanont";
            //CGlobal.UserInfo.EmployeeNo = "00002021";
            return View();
        }

        public ActionResult ProbationForm(string qryStr)
        {
            vProbation_Form_Return result = new vProbation_Form_Return();
            var check = CheckLogin();
            //CGlobal.UserInfo.UserId = "wopasyanont";
            //CGlobal.UserInfo.EmployeeNo = "00021282";

            result = getformbyid(qryStr);

            var setuser = new List<string>();
            setuser.Add(result.maindata.HOP_No);
            setuser.Add(result.maindata.GroupHead_No);
            setuser.Add(result.maindata.Staff_No);
            setuser.Add(result.maindata.PM_No);
            setuser.Add(result.maindata.HR_No);

            string[] UserAdmin = WebConfigurationManager.AppSettings["ProbationAdmin"].Split(';');
            if (UserAdmin.Length > 0 && UserAdmin.Contains(CGlobal.UserInfo.UserId))
            {
            }

            else if (setuser.Count() > 0 && setuser.Contains(CGlobal.UserInfo.EmployeeNo))
            {
            }
            else
            {
                return RedirectToAction("ErrorNopermission", "MasterPage");
            }




            Session["ssProForm"] = result;
            Session["ssGetFile"] = null;
            return View(result);
        }

        public ActionResult ProbationListPendingAndCompleted(string qryStr)
        {
            var check = CheckLogin();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            //CGlobal.UserInfo.UserId = "wopasyanont";
            //CGlobal.UserInfo.EmployeeNo = "00002021";
            return View();
        }

        public ActionResult ProbationManage_PeopleList()
        {

            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            string sSess = Request.Form["sSess"];
            Session[sSess] = null;
            return View();
        }

        public ActionResult ProbationManage_People(string qryStr)
        {
            ResultLine result = new ResultLine();
            try
            {

                if (!string.IsNullOrEmpty(qryStr))
                {
                    var getlinkid = SystemFunction.GetIntNullToZero(qryStr);

                    var getpro = _Probation_FormService.Find(getlinkid);

                    result.Pro_Id = getpro.Id.ToString();

                    var getstaff = wsHRis.getEmployeeInfoByEmpNo(getpro.Staff_No);



                    result.ST_user_no = getpro.Staff_No;
                    result.ST_user_name = getstaff.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");
                    result.ST_Date = getpro.Staff_Acknowledge_Date.ToString() + "";

                    result.PM_user_no = getpro.PM_No;
                    result.PM_user_name = wsHRis.getEmployeeInfoByEmpNo(getpro.PM_No).AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");
                    result.PM_Date = getpro.PM_Submit_Date.ToString() + "";


                    result.GH_user_no = getpro.GroupHead_No;
                    result.GH_user_name = wsHRis.getEmployeeInfoByEmpNo(getpro.GroupHead_No).AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");
                    result.GH_Date = getpro.GroupHead_Submit_Date.ToString() + "";

                    result.HOP_user_no = getpro.HOP_No;
                    result.HOP_user_name = wsHRis.getEmployeeInfoByEmpNo(getpro.HOP_No).AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");
                    result.HOP_Date = getpro.HOP_Submit_Date.ToString() + "";
                }

                string sSess = Request.Form["sSess"];
                Session[sSess] = null;

            }
            catch (Exception ex) { }
            return View(result);
        }

        public ActionResult Probation_With_Out(string qryStr)
        {
            vProbation_With_Out_Return result = new vProbation_With_Out_Return();
            //try
            //{

            //    List<vProbation_With_Out_obj> lstobj = new List<vProbation_With_Out_obj>();

            //    var getpro = _Probation_With_OutService.GetDataForSelect().ToList();
            //    foreach (var ex in getpro)
            //    {
            //        var getsv = wsHRis.getEmployeeInfoByEmpNo(ex.Staff_No);
            //        vProbation_With_Out_obj obj = new vProbation_With_Out_obj();
            //        obj.Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Delete('" + HCMFunc.Encrypt(ex.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>";
            //        obj.Id = ex.Id;
            //        obj.Staff_No = ex.Staff_No;
            //        obj.Staff_Name = getsv.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");
            //        obj.Company = getsv.AsEnumerable().FirstOrDefault().Field<string>("CompanyCode");
            //        obj.Cost_Center = getsv.AsEnumerable().FirstOrDefault().Field<string>("UnitGroup");
            //        obj.Join_Date = getsv.AsEnumerable().FirstOrDefault().Field<DateTime>("JoinDate").ToString("dd MMM yyyy");
            //        obj.Create_Date = ex.Create_Date.Value.ToString("dd MMM yyyy");
            //        obj.Create_User = ex.Create_User;
            //        obj.Update_Date = ex.Update_Date.Value.ToString("dd MMM yyyy");
            //        obj.Update_User = ex.Update_User;
            //        obj.Active_Status = ex.Active_Status;
            //        obj.Remark = ex.Remark;



            //        lstobj.Add(obj);
            //    }
            //    result.lstData = lstobj;
            //}
            //catch (Exception ex)
            //{

            //}
            return View(result);
        }

        #endregion View


        #region Ajax

        [HttpPost]
        public vProbation_Form_Return getformbyid(string qryStr)
        {
            vProbation_Form_Return result = new vProbation_Form_Return();
            try
            {

                Probation_Form proform = new Probation_Form();


                if (!string.IsNullOrEmpty(qryStr))
                {
                    var getlinkid = SystemFunction.GetIntNullToZero(qryStr);
                    if (getlinkid == 0)
                    {
                        var getobjlink = HCMFunc.DecryptUrl(qryStr);
                        getlinkid = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(getobjlink.id));
                    }
                    //vObjectMail objMaila = HCMFunc.DecryptUrl(qryStr);
                    //if (!String.IsNullOrEmpty(objMaila.id))
                    //{
                    //    getlinkid = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(objMaila.id));
                    //}
                    string winloginnamea = this.User.Identity.Name;
                    string EmpUserIDa = winloginnamea.Substring(winloginnamea.IndexOf("\\") + 1);
                    if (!string.IsNullOrEmpty(EmpUserIDa))
                    {
                        if (CGlobal.IsUserExpired())
                        {
                            DataTable staff = wsHRis.getActiveStaffByUserID(EmpUserIDa);
                            // DataTable staff = wsHRis.getActiveStaffByUserID("zjaffer");
                            if (staff != null)
                            {
                                //var CheckUser = staff.AsEnumerable().Where(w => w.Field<string>("EmpNo") == objMaila.emp_no + "").FirstOrDefault();
                                //if (CheckUser != null)
                                //{
                                //    var Login = HCMFunc.funcLogin(CheckUser.Field<string>("EmpNo") + "");
                                //}
                                //else
                                //{
                                var Login = HCMFunc.funcLogin(staff.Rows[0].Field<string>("EmpNo") + "");
                                //}
                            }
                        }
                    }
                    //.Where(w => w.Active_Status == "Y" && w.Probation_Form.Id == getlinkid)
                    var getqustionbyForm = _Probation_FormService.Find(getlinkid).Probation_Details.Where(w => w.Active_Status == "Y").ToList();
                    //var getqustionbyForms = _Probation_FormService.Find(getlinkid);
                    //var getqustionbyForm = _Probation_DetailService.GetDataForSelect().Where(w=> w.Probation_Form_Id == getqustionbyForms.Id);
                    var getqustion = getqustionbyForm.Select(s => s.TM_Probation_Question).ToList();


                    //var joinchoice = getqustion.Join(TM_Probation_Question );

                    List<vProbation_Detail_obj> setquestion = new List<vProbation_Detail_obj>();
                    foreach (var s in getqustionbyForm.ToList())
                    {
                        vProbation_Detail_obj item = new vProbation_Detail_obj();
                        item.Id = s.Id;
                        item.Seq = s.Seq;
                        item.Assessment = !string.IsNullOrEmpty(s.Assessment) ? s.Assessment : "";
                        item.Topic = s.TM_Probation_Question.Topic;
                        item.Content = s.TM_Probation_Question.Content;
                        item.Remark = s.Remark + "";
                        item.proposition = "<strong> " + s.TM_Probation_Question.Topic + "</strong>" + " <br> " + s.TM_Probation_Question.Content;
                        item.Ans_1 = !string.IsNullOrEmpty(s.Assessment) && s.Assessment == "Y" ? s.Assessment : "";
                        item.Ans_2 = !string.IsNullOrEmpty(s.Assessment) && s.Assessment == "N" ? s.Assessment : "";
                        item.Ans_3 = !string.IsNullOrEmpty(s.Assessment) && s.Assessment == "O" ? s.Assessment : "";

                        setquestion.Add(item);

                    }

                    //var setquestion = getqustionbyForm.Select(s => new vProbation_Detail_obj()
                    //{
                    //    Id = s.Id,
                    //    Seq = s.Seq,
                    //    Assessment = !string.IsNullOrEmpty(s.Assessment) ? s.Assessment:"",
                    //    Topic = s.TM_Probation_Question.Topic,
                    //    Content = s.TM_Probation_Question.Content,
                    //    Remark = s.Remark + "",
                    //    proposition = "<strong> " + s.TM_Probation_Question.Topic + "</strong>" + " <br> " + s.TM_Probation_Question.Content,
                    //    Ans_1 = !string.IsNullOrEmpty(s.Assessment) && s.Assessment == "Y" ? s.Assessment : "",
                    //    Ans_2 = !string.IsNullOrEmpty(s.Assessment) && s.Assessment == "N" ? s.Assessment : "",
                    //    Ans_3 = !string.IsNullOrEmpty(s.Assessment) && s.Assessment == "O" ? s.Assessment : "",


                    //    //Topic = s.Topic,
                    //    //Content = s.Content,
                    //    //icon = s.Icon,
                    //    //remark = ""


                    //}).ToList();

                    result.lstData_Detail = setquestion.OrderBy(o => o.Seq).ToList();

                    var getprolist = _Probation_FormService.Find(getlinkid);
                    vProbation_Form_obj newlist = new vProbation_Form_obj();
                    var get = wsHRis.getEmployeeInfoByEmpNo(getprolist.Staff_No).AsEnumerable().FirstOrDefault();
                    var adf = get.Field<DateTime>("JoinDate");
                    newlist.Id = getprolist.Id;
                    newlist.Assessment = getprolist.Assessment;
                    newlist.Remark = getprolist.Remark;
                    newlist.Staff_Name = get.Field<string>("EmpFullName");
                    newlist.Position = Regex.Replace(get.Field<string>("JobLevelText"), "[0-9]", "");
                    newlist.Staff_No = get.Field<string>("EmpNo");
                    newlist.Cost_Center = get.Field<string>("UnitGroup");
                    newlist.Date_Employed = getprolist.Start_Pro.Value.ToString("MMMM dd, yyyy");
                    newlist.Probationary_Period_End = getprolist.End_Pro.Value.ToString("MMMM dd, yyyy");

                    newlist.Start_Pro = getprolist.Start_Pro.ToString();
                    newlist.End_Pro = getprolist.End_Pro.ToString();
                    newlist.Count_Date_Pro = getprolist.Count_Date_Pro.ToString();

                    newlist.HR_No = !String.IsNullOrEmpty(getprolist.HR_No) ? wsHRis.getEmployeeInfoByEmpNo(getprolist.HR_No).AsEnumerable().FirstOrDefault().Field<string>("EmpNo") + "" : "";
                    newlist.PM_No = !String.IsNullOrEmpty(getprolist.PM_No) ? wsHRis.getEmployeeInfoByEmpNo(getprolist.PM_No).AsEnumerable().FirstOrDefault().Field<string>("EmpNo") + "" : "";
                    var getunitgroupid = get.Field<Int32>("UnitGroupID");
                    if (lstunitgroupauditK.Contains(get.Field<Int32>("UnitGroupID")) && get.Field<Int32>("RankPriority") >= 6)
                    {

                        newlist.HOP_No = "-";
                        newlist.HOP_Name = "-";
                    }
                    else
                    {
                        newlist.HOP_No = !String.IsNullOrEmpty(getprolist.HOP_No) ? wsHRis.getEmployeeInfoByEmpNo(getprolist.HOP_No).AsEnumerable().FirstOrDefault().Field<string>("EmpNo") + "" : "";
                        newlist.HOP_Name = !String.IsNullOrEmpty(getprolist.HOP_No) ? wsHRis.getEmployeeInfoByEmpNo(getprolist.HOP_No).AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") : "";
                    }
                    //if (!lstunitgroupauditK.Contains(getunitgroupid) && get.Field<Int32>("RankPriority") != 7)
                    //{
                    //    newlist.HOP_No = !String.IsNullOrEmpty(getprolist.HOP_No) ? wsHRis.getEmployeeInfoByEmpNo(getprolist.HOP_No).AsEnumerable().FirstOrDefault().Field<string>("EmpNo") + "" : "";
                    //    newlist.HOP_Name = !String.IsNullOrEmpty(getprolist.HOP_No) ? wsHRis.getEmployeeInfoByEmpNo(getprolist.HOP_No).AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") : "";
                    //}
                    //else
                    //{
                    //    newlist.HOP_No = "-";
                    //    newlist.HOP_Name = "-";
                    //}
                    newlist.GroupHead_No = !String.IsNullOrEmpty(getprolist.GroupHead_No) && getprolist.GroupHead_No != "00000000" ? wsHRis.getEmployeeInfoByEmpNo(getprolist.GroupHead_No).AsEnumerable().FirstOrDefault().Field<string>("EmpNo") + "" : "";

                    newlist.PM_Cost = wsHRis.getEmployeeInfoByEmpNo(getprolist.PM_No).AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") + "/" + wsHRis.getEmployeeInfoByEmpNo(getprolist.PM_No).AsEnumerable().FirstOrDefault().Field<string>("UnitGroup") + "";


                    newlist.HR_Name = !String.IsNullOrEmpty(getprolist.HR_No) ? wsHRis.getEmployeeInfoByEmpNo(getprolist.HR_No).AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") : "";
                    newlist.PM_Name = !String.IsNullOrEmpty(getprolist.PM_No) ? wsHRis.getEmployeeInfoByEmpNo(getprolist.PM_No).AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") : "";
                    newlist.GroupHead_Name = !String.IsNullOrEmpty(getprolist.GroupHead_No) && getprolist.GroupHead_No != "00000000" ? wsHRis.getEmployeeInfoByEmpNo(getprolist.GroupHead_No).AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") : "";



                    var getuserinfo = CGlobal.UserInfo.EmployeeNo;

                    string[] UserAdmin = WebConfigurationManager.AppSettings["ProbationAdmin"].Split(';');
                    var you_are = "";
                    if (UserAdmin.Contains(CGlobal.UserInfo.UserId))
                    {
                        var gerp = getprolist.Status;
                        if (gerp == "R")
                            gerp = "HR";

                        if (gerp == "GroupHead" && getprolist.Staff_Action == null)
                        {
                            gerp = "PM";
                        }

                        switch (gerp)
                        {
                            case "HR":
                                you_are = "PM";
                                newlist.PM_Action = CGlobal.UserInfo.EmployeeNo;
                                break;
                            case "Staff":
                                you_are = "GroupHead";
                                newlist.GroupHead_Action = CGlobal.UserInfo.EmployeeNo;
                                break;
                            case "PM":
                                you_are = "Staff";
                                newlist.Staff_Action = CGlobal.UserInfo.EmployeeNo;
                                break;
                            case "GroupHead":
                                you_are = "HOP";
                                newlist.HOP_Action = CGlobal.UserInfo.EmployeeNo;
                                break;
                            case "HOP":
                                you_are = "Completed";
                                break;
                            case "Completed":
                                you_are = "Completed";
                                break;
                            case "Complete":
                                you_are = "Completed";
                                break;

                        }


                    }
                    else
                    {
                        you_are = check_user_info(getuserinfo, getprolist);

                        switch (you_are)
                        {
                            //case "HR":
                            //    newlist.PM_Action = CGlobal.UserInfo.EmployeeNo;
                            //    break;
                            case "Staff":
                                newlist.Staff_Action = CGlobal.UserInfo.EmployeeNo;
                                break;
                            case "PM":
                                newlist.PM_Action = CGlobal.UserInfo.EmployeeNo;
                                break;
                            case "GroupHead":
                                newlist.GroupHead_Action = CGlobal.UserInfo.EmployeeNo;
                                break;
                            case "HOP":
                                newlist.HOP_Action = CGlobal.UserInfo.EmployeeNo;
                                break;

                        }
                    }



                    newlist.HR_Submit_Date = getprolist.HR_Submit_Date != null ? getprolist.HR_Submit_Date.Value.ToString("MMMM dd, yyyy") : "";
                    newlist.Staff_Acknowledge_Date = getprolist.Staff_Action != null && getprolist.Staff_Action != "00000000" && getprolist.Staff_Acknowledge_Date.HasValue ? getprolist.Staff_Acknowledge_Date.Value.ToString("MMMM dd, yyyy") : "-";

                    if (newlist.Assessment == "N")
                    {
                        newlist.Staff_Acknowledge_Date = "";
                        //newlist.Staff_Acknowledge_Date += "<br> (Auto Acknowledge)";

                    }

                    newlist.PM_Submit_Date = getprolist.PM_Submit_Date != null ? getprolist.PM_Submit_Date.Value.ToString("MMMM dd, yyyy") : "";
                    if (lstunitgroupauditK.Contains(get.Field<Int32>("UnitGroupID")) && get.Field<Int32>("RankPriority") >= 6)
                    {
                        newlist.HOP_Submit_Date = "-";
                    }
                    else
                    {
                        newlist.HOP_Submit_Date = getprolist.HOP_Submit_Date != null ? getprolist.HOP_Submit_Date.Value.ToString("MMMM dd, yyyy") : "";

                    }
                    newlist.GroupHead_Submit_Date = getprolist.GroupHead_Submit_Date != null ? getprolist.GroupHead_Submit_Date.Value.ToString("MMMM dd, yyyy") : "";
                    //if (getprolist.Status == "R")
                    //{
                    //    //newlist.GroupHead_Submit_Date = "<p class='text-danger'>REVISE</p>";
                    //    newlist.GroupHead_Submit_Date = "";
                    //    if (you_are != "GroupHead")
                    //    {
                    //        newlist.Staff_Acknowledge_Date = "";
                    //        newlist.PM_Submit_Date = "";
                    //    }
                    //}

                    newlist.You_Are = you_are;
                    string makeLeave = "";
                    var getleave = wsHRis.getLeaveHours(getprolist.Staff_No, "150000000000,150000000001", "", Convert.ToDateTime(newlist.Start_Pro), Convert.ToDateTime(newlist.End_Pro)).AsEnumerable();
                    if (getleave.Count() > 0)
                    {
                        foreach (var exl in getleave)
                        {

                            var gettypeleave = exl.Field<string>("InternalCodeNo");
                            var gettimeleave = exl.Field<decimal>("SumHour");
                            if (gettypeleave == "150000000000")
                            {
                                //vac leave
                                makeLeave += "Annual leave : " + gettimeleave + "<br>";
                            }
                            else if (gettypeleave == "150000000001")
                            {
                                //medical leave
                                makeLeave += "Medical leave : " + gettimeleave;
                            }
                            makeLeave += "";
                        }
                    }
                    else
                    {
                        makeLeave = "Annual leave : 0<br>Medical leave : 0";
                    }
                    newlist.Absent = makeLeave;

                    newlist.Status = getprolist.Status;
                    newlist.Extend_Form = getprolist.Extend_Form;
                    newlist.Extend_Status = getprolist.Extend_Status;
                    newlist.Extend_Period = getprolist.Extend_Period;
                    newlist.Remark_Revise = getprolist.Remark_Revise;




                    var _getDatalst = _Action_PlansService.GetDataForSelect().Where(w => w.Probation_Form.Id == getlinkid).ToList();
                    var genlistfordoc = "";
                    foreach (var _getData in _getDatalst)
                    {
                        if (_getData != null)
                        {
                            if (_getData.Probation_Form != null)
                            {

                                if (newlist.You_Are == "PM" && newlist.PM_Submit_Date == "")
                                {
                                    genlistfordoc += @"<button id=""btnEdit""  type=""button"" onclick=""LoadFile('" + HCMFunc.EncryptPES(_getData.Id + "")
                                        + @"')"" class=""btn btn-xs btn-success"" tooltip=""Download""><i class=""fa fa-download""></i></button> "
                                        //+ @" <button id=""btndel""  type=""button"" onclick=""DelFile('" + HCMFunc.EncryptPES(_getData.Id + "")
                                        //+ @"')"" class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button> "
                                        + _getData.sfile_oldname + "<br>";
                                }
                                else
                                {
                                    genlistfordoc += @"<button id=""btnEdit""  type=""button"" onclick=""LoadFile('" + HCMFunc.EncryptPES(_getData.Id + "")
                                        + @"')"" class=""btn btn-xs btn-success"" tooltip=""Download""><i class=""fa fa-download""></i></button> "
                                        + _getData.sfile_oldname + "<br>";
                                }

                            }
                            else
                            {

                            }
                        }


                    }

                    result.maindata = newlist;
                    result.maindata.File_Upload = genlistfordoc;

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        [HttpPost]
        public ActionResult Search_ProbationForm(string startFrom, string startUntil, string end, string status, string empno, string type, string without, string PageType)
        {
            vProbation_Form_Return result = new vProbation_Form_Return();
            List<vProbation_Form_obj> objFile = new List<vProbation_Form_obj>();
            //string sSess = Request.Form["sSess"];
            //objFile = Session[sSess] as List<vProbation_Form_obj>;
            var getprowithout = new List<Probation_With_Out>();
            List<vProbation_Form_obj> genNewWithout = new List<vProbation_Form_obj>();
            List<Probation_Form> getprolistExtend = new List<Probation_Form>();

            var getprolist = _Probation_FormService.GetDataWithOutExtendForSelect().ToList();
            getprolistExtend = _Probation_FormService.GetDataExtendForSelect().ToList();

            var getuserinfo = CGlobal.UserInfo.EmployeeNo;

            getprolist = getprolist.Where(w => !String.IsNullOrEmpty(w.Status) && w.PM_No == getuserinfo || w.Staff_No == getuserinfo || w.GroupHead_No == getuserinfo || w.HOP_No == getuserinfo).ToList();
            getprolistExtend = getprolistExtend.Where(w => !String.IsNullOrEmpty(w.Status) && w.PM_No == getuserinfo || w.Staff_No == getuserinfo || w.GroupHead_No == getuserinfo || w.HOP_No == getuserinfo).ToList();


            var newproextend = new List<Probation_Form>();
            if (getprolistExtend.Count() > 0)
            {
                foreach (var ex in getprolistExtend.Where(w => w.Start_Pro.Value.AddDays(118).AddDays(w.Extend_Period == "1" ? 16 : 31) <= DateTime.Now).ToList())
                {
                    //var checkex = _Probation_FormService.Find(SystemFunction.GetIntNullToZero(ex.Extend_Form));
                    //if (checkex.GroupHead_Submit_Date != null && checkex.Status != "R")
                    //{
                    //    newproextend.Add(ex);
                    //}

                    newproextend.Add(ex);

                }
                getprolistExtend = new List<Probation_Form>();
                if (newproextend.Count() > 0)
                {
                    getprolistExtend = newproextend;
                }
            }
            getprolist.AddRange(getprolistExtend);
            if (!String.IsNullOrEmpty(startFrom))
            {
                getprolist = getprolist.Where(w => w.Start_Pro.Value.Date >= Convert.ToDateTime(startFrom).Date).ToList();
                genNewWithout = genNewWithout.Where(w => Convert.ToDateTime(w.Start_Pro).Date >= Convert.ToDateTime(startFrom).Date).ToList();
                //getprolistExtend = getprolistExtend.Where(w => Convert.ToDateTime(w.Start_Pro.Value.AddMonths(SystemFunction.GetIntNullToZero(w.Extend_Period))).Date >= Convert.ToDateTime(startFrom).Date).ToList();


                //List<Probation_With_Out> newlist = new List<Probation_With_Out>();
                //foreach (var ex in getprolist)
                //{
                //    Probation_With_Out obj = new Probation_With_Out();
                //    if (ex.Extend_Status == "Y")
                //        if (Convert.ToDateTime(ex.Start_Pro).Date >= Convert.ToDateTime(startFrom).Date)
                //        { }
                //}
            }
            if (!String.IsNullOrEmpty(startUntil))
            {
                getprolist = getprolist.Where(w => w.Start_Pro.Value.Date <= Convert.ToDateTime(startUntil).Date).ToList();
                genNewWithout = genNewWithout.Where(w => Convert.ToDateTime(w.Start_Pro).Date <= Convert.ToDateTime(startUntil).Date).ToList();
                //getprolistExtend = getprolistExtend.Where(w => Convert.ToDateTime(w.Start_Pro.Value.AddMonths(SystemFunction.GetIntNullToZero(w.Extend_Period))).Date <= Convert.ToDateTime(startUntil).Date).ToList();
            }

            if (!String.IsNullOrEmpty(end))
            {
                getprolist = getprolist.Where(w => w.End_Pro.Value.Date <= Convert.ToDateTime(end).Date).ToList();

            }

            if (!String.IsNullOrEmpty(status))
            {
                if (status == "Completed")
                    getprolist = getprolist.Where(w => w.Status == "Completed").ToList();
                else if (status == "Incomplete")
                    getprolist = getprolist.Where(w => w.Status != "Completed").ToList();
            }


            if (!String.IsNullOrEmpty(empno))
            {
                getprolist = getprolist.Where(w => w.Staff_No == empno).ToList();
                //getprolistExtend = getprolistExtend.Where(w => w.Staff_No == empno).ToList();
            }



            var getuserrank = CGlobal.UserInfo.Rank;


            //var get_auth = check_user_info(getuserinfo,getprolist);

            string BackUrl = "";
            List<vProbation_Form_obj> setprolist = new List<vProbation_Form_obj>();
            foreach (var s in getprolist)
            {
                var getstaff = wsHRis.getEmployeeInfoByEmpNo(s.Staff_No);
                var getpm = wsHRis.getEmployeeInfoByEmpNo(s.PM_No);
                var getgh = wsHRis.getEmployeeInfoByEmpNo(s.GroupHead_No);
                var gethop = !String.IsNullOrEmpty(s.HOP_No) ? wsHRis.getEmployeeInfoByEmpNo(s.HOP_No):null;

                //string[] lstauditK = { "14100058", "14100059", "14100060", "14100002", "14100062" };

                vProbation_Form_obj obj = new vProbation_Form_obj();
                obj.View = @"<button id=""btnView""  type=""button"" onclick=""View('" + s.Id + @"')"" class=""btn btn-xs btn-success""><i class=""fa fa-eye""></i></button>";
                obj.Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + s.Id + @"')"" class=""btn btn-xs btn-primary""><i class=""fa fa-edit""></i></button>";
                obj.Update = @"<button id=""btnUpdate""  type=""button"" onclick=""Update('" + s.Id + @"')"" class=""btn btn-xs btn-warning""><i class=""fa fa-power-off""></i></button>";
                obj.Id = s.Id;
                obj.Assessment = String.IsNullOrEmpty(s.Assessment) ? "Pending" : s.Assessment == "Y" ? "Pass" : s.Assessment == "N" ? "Not Pass" : s.Assessment == "O" ? "Other" : "";
                obj.Company = getstaff.AsEnumerable().FirstOrDefault().Field<string>("CompanyCode");
                obj.Cost_Center = getstaff.AsEnumerable().FirstOrDefault().Field<string>("UnitGroup");
                obj.Staff_No = s.Staff_No;
                obj.Staff_Name = getstaff.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") + (s.Extend_Status == "Y" ? " (Extend)" : "");
                obj.Start_Pro = s.Start_Pro.Value.ToString("MMMM dd, yyyy");
                obj.End_Pro = s.End_Pro.Value.ToString("MMMM dd, yyyy");
                obj.PM_Name = !String.IsNullOrEmpty(s.PM_No) ? getpm.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") : "";
                obj.GroupHead_Name = !String.IsNullOrEmpty(s.GroupHead_No) ? getgh.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") + "" : "";
                obj.HOP_Name = lstunitgroupauditK.Contains(getstaff.AsEnumerable().FirstOrDefault().Field<Int32>("UnitGroupID"))
                && getstaff.AsEnumerable().FirstOrDefault().Field<Int32>("RankPriority") >= 6
                ? "-" : !String.IsNullOrEmpty(s.HOP_No) ? gethop.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") : "";
                obj.Create_Date = s.Create_Date.Value.ToString("MMMM dd, yyyy");
                obj.Update_Date = s.Update_Date.Value.ToString("MMMM dd, yyyy");

                obj.Status = s.Status == "R" ? "Revise" :
                    s.Status == "Completed" ? "Completed" :
                    s.Status == "HR" ? "PM" :
                    s.Status == "PM" ? "Staff" :
                    s.Status == "Staff" ? "Group Head" :
                    s.Status == "GroupHead" ? "HOP" :
                    s.Status == "HOP" ? "Completed" :
                    "";

                //s.HR_Submit_Date != null ? s.PM_Submit_Date != null 
                //? (!obj.Staff_Name.Contains("Resign") && s.Staff_Action != null) 
                //? s.GroupHead_Submit_Date != null 
                //? s.HOP_Submit_Date != null 
                //? "Completed" : 
                //(lstunitgroupauditK.Contains(getstaff.AsEnumerable().FirstOrDefault().Field<int>("UnitGroupID")) && getstaff.AsEnumerable().FirstOrDefault().Field<int>("RankPriority") >= 6) ? "Completed"
                //: "HOP" 
                //: "Group Head" 
                //: "Staff" 
                //: "PM" 
                //: "";

                obj.Absent = "Vac : 8 hrs.<br>Sick : 3 hrs.";
                obj.HR_No = s.HR_No;
                obj.PM_No = s.PM_No;
                obj.GroupHead_No = s.GroupHead_No;
                obj.HOP_No = s.HOP_No;
                obj.Extend_Status = s.Extend_Status;
                obj.Extend_Period = s.Extend_Period;

                setprolist.Add(obj);
            }


            if (genNewWithout.Count() > 0)
            {
                setprolist.AddRange(genNewWithout);
            }



            //setprolist = setprolist.Where(w=> w.);
            List<vProbation_Form_obj> setTast = new List<vProbation_Form_obj>();
            List<vProbation_Form_obj> setAll = new List<vProbation_Form_obj>();
            foreach (var ex in setprolist)
            {
                var currentstatus = ex.Status;
                var getyouard = "";
                var currentuserno = CGlobal.UserInfo.EmployeeNo;

                if (ex.Staff_No == currentuserno)
                {
                    getyouard = "Staff";
                }
                else if (ex.PM_No == currentuserno)
                {
                    getyouard = "PM";
                }
                else if (ex.GroupHead_No == currentuserno)
                {
                    getyouard = "GroupHead";
                }
                else if (ex.HOP_No == currentuserno)
                {
                    getyouard = "HOP";
                }

                var getstaff = wsHRis.getEmployeeInfoByEmpNo(ex.Staff_No);

                if (ex.Status == "Completed" || currentstatus.Replace(" ", "") != getyouard)
                {
                    setAll.Add(ex);
                }
                else
                {
                    setTast.Add(ex);
                }


                //if (getstaff.AsEnumerable().FirstOrDefault().Field<int>("Status") != 3 && ex.Status == "Completed")
                //{
                //    setAll.Add(ex);
                //}
                //else if (getstaff.AsEnumerable().FirstOrDefault().Field<int>("Status") != 3)
                //{
                //    setTast.Add(ex);
                //}
                //else if (currentstatus.Replace(" ", "") == getyouard && getstaff.AsEnumerable().FirstOrDefault().Field<int>("Status") != 3)
                //{
                //    setTast.Add(ex);
                //}
                //else
                //{
                //    setAll.Add(ex);
                //}

            }

            if (PageType == "M")
            {
                result.lstDataTask = setTast.OrderBy(o => o.Id).ToList();
            }
            else
            {
                result.lstData = setAll.OrderBy(o => o.Id).ToList();
            }
            //result.lstDataManage = setprolist;
            //Session[sSess] = setprolist;

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult Save_ProbationForm(vProbation_Form_Return itemdata)
        {
            vProbation_Form_Return result = new vProbation_Form_Return();
            try
            {
                var getmain = itemdata;

                List<Probation_Details> lstprode = new List<Probation_Details>();
                //lstprode = itemdata.lstData_Detail.Select(s => new Probation_Details()
                //{
                //    Id=s.Id,
                //    Assessment = s.Assessment,
                //    Remark = s.Remark
                //}).ToList();
                var getyour = itemdata.maindata.You_Are.Split('|').ToList();
                if (getyour[0] == "PM" || itemdata.maindata.Status == "R")
                {
                    foreach (var ex in itemdata.lstData_Detail)
                    {
                        if (!String.IsNullOrEmpty(ex.Assessment))
                        {
                            if (ex.Assessment != "Y" && String.IsNullOrEmpty(ex.Remark))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Please give a reason of “Not Pass” or “N / A”<br>" + ex.proposition;

                                return Json(new
                                {
                                    result
                                });
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Please Select " + ex.Topic;

                            return Json(new
                            {
                                result
                            });

                        }
                        #region Set Detail

                        Probation_Details inlist = new Probation_Details();
                        inlist = _Probation_DetailService.Find(ex.Id);

                        inlist.Assessment = ex.Assessment;
                        inlist.Remark = ex.Remark;
                        inlist.Update_Date = DateTime.Now;
                        inlist.Update_User = CGlobal.UserInfo.EmployeeNo;

                        lstprode.Add(inlist);

                        #endregion Set Detail
                    }

                    if (!String.IsNullOrEmpty(itemdata.maindata.Assessment))
                    {
                        #region Save Detail
                        foreach (var getprode in lstprode)
                        {
                            var savedetail = _Probation_DetailService.CreateNewOrUpdate(getprode);
                        }
                        #endregion Save Detail


                        if (itemdata.maindata.Assessment != "Y" && String.IsNullOrEmpty(itemdata.maindata.Remark))
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Please give a reason of “NO” or “Other”<br>Should the employment status be continued as a permanent staff?";

                            return Json(new
                            {
                                result
                            });
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Should the employment status be continued as a permanent staff?";

                        return Json(new
                        {
                            result
                        });

                    }
                }
                Probation_Form proform = new Probation_Form();

                proform.Id = itemdata.maindata.Id;

                var getolddata = _Probation_FormService.Find(proform.Id);


                #region New Fn
                //Not yet Complete
                //var _get_YouR = itemdata.maindata.You_Are.Split('|').ToList();
                //var _get_Asses = itemdata.maindata.Assessment;
                //var _get_Save_Type = itemdata.maindata.Save_Type;


                //var _set_Status = "";

                //foreach (var x in _get_YouR)
                //{
                //    _set_Status = x;

                //    var _is_u = x.ToUpper();
                //    if (_get_Save_Type == "S")
                //    {
                //        switch (_is_u)
                //        {
                //            case "PM":

                //                proform.Assessment = itemdata.maindata.Assessment;
                //                proform.Remark = itemdata.maindata.Remark;

                //                proform.PM_Submit_Date = DateTime.Now;
                //                proform.Send_Mail_Date = DateTime.Now;
                //                proform.PM_Action = itemdata.maindata.PM_Action;
                //                //set Group head
                //                proform.GroupHead_No = itemdata.maindata.GroupHead_No;

                //                break;
                //            case "STAFF":
                //                proform.Staff_Acknowledge_Date = DateTime.Now;
                //                proform.Staff_Action = itemdata.maindata.Staff_Action;

                //                if (itemdata.maindata.Cost_Center.Trim().ToLower().Contains("audit"))
                //                {
                //                    if (itemdata.maindata.GroupHead_Submit_Date != null)
                //                    {
                //                        _set_Status = "Completed";
                //                    }
                //                }

                //                //proform.Mail_Send = "N";

                //                if (getolddata.GroupHead_Submit_Date != null)
                //                {
                //                    _set_Status = "GroupHead";
                //                }
                //                break;
                //            case "GROUPHEAD":
                //                // code block
                //                break;
                //            case "HOP":
                //                // code block
                //                break;
                //            default:
                //                // code block
                //                break;
                //        }



                //    }
                //    else if (_get_Save_Type == "R")
                //    {

                //    }
                //}

                #endregion New Fn

                foreach (var yx in itemdata.maindata.You_Are.Split('|').ToList())
                {
                    var getempbyempno = wsHRis.getEmployeeInfoByEmpNo(itemdata.maindata.Staff_No).AsEnumerable().FirstOrDefault();

                    if (itemdata.maindata.Save_Type == "S")
                    {
                        proform.Status = yx;
                        if ((lstunitgroupauditK.Contains(getempbyempno.Field<Int32>("UnitGroupID")) && getempbyempno.Field<Int32>("RankPriority") >= 6) || getempbyempno.Field<Int32>("UnitGroupID") == 14100063)
                            proform.HOP_No = null;
                        else
                            proform.HOP_No = getolddata.HOP_No;

                        if (yx == "PM")
                        {
                            proform.Assessment = itemdata.maindata.Assessment;
                            proform.Remark = itemdata.maindata.Remark;

                            proform.PM_Submit_Date = DateTime.Now;
                            proform.Send_Mail_Date = DateTime.Now;
                            proform.PM_Action = itemdata.maindata.PM_Action;
                            //set Group head
                            proform.GroupHead_No = itemdata.maindata.GroupHead_No;

                            //proform.Mail_Send = "Y";

                            if (getempbyempno.Field<int>("Status") != 3)
                            {
                                proform.Status = "Staff";
                                proform.Staff_Action = itemdata.maindata.PM_Action;
                            }
                            else
                            {
                                proform.Status = "PM";
                            }
                            
                        }
                        else if (yx == "Staff")
                        {
                            proform.Staff_Acknowledge_Date = DateTime.Now;
                            proform.Staff_Action = itemdata.maindata.Staff_Action;

                            if (itemdata.maindata.Cost_Center.Trim().ToLower().Contains("audit"))
                            {
                                if (!String.IsNullOrEmpty(itemdata.maindata.GroupHead_Submit_Date))
                                {
                                    proform.Status = "Completed";
                                }
                            }



                            if (getolddata.GroupHead_Submit_Date != null)
                            {
                                proform.Status = "GroupHead";
                            }
                            if (getolddata.HOP_Submit_Date != null)
                            {
                                proform.Status = "Completed";
                            }
                        }
                        else if (yx == "GroupHead")
                        {
                            proform.GroupHead_Submit_Date = DateTime.Now;
                            proform.GroupHead_Action = itemdata.maindata.GroupHead_Action;
                            if (String.IsNullOrEmpty(getolddata.HOP_No) || (lstunitgroupauditK.Contains(getempbyempno.Field<Int32>("UnitGroupID")) && getempbyempno.Field<Int32>("RankPriority") >= 6) || getempbyempno.Field<Int32>("UnitGroupID") == 14100063)
                            {
                                if (getolddata.PM_Submit_Date != null && (!String.IsNullOrEmpty(getolddata.Staff_Action) || getolddata.Staff_Acknowledge_Date != null) || getolddata.Assessment == "N")
                                {
                                    proform.Status = "Completed";
                                }
                                else
                                {
                                    proform.Status = "GroupHead";
                                }
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(getolddata.Staff_Action) || getolddata.Staff_Acknowledge_Date != null)
                                {
                                    proform.Status = "GroupHead";
                                }
                                else
                                {
                                    proform.Status = "PM";
                                }
                            }
                        }
                        else if (yx == "HOP")
                        {

                            proform.HOP_Submit_Date = DateTime.Now;
                            proform.HOP_Action = itemdata.maindata.HOP_Action;
                            proform.HOP_No = itemdata.maindata.HOP_No;
                            if (String.IsNullOrEmpty(getolddata.Staff_Action) && getolddata.Staff_Acknowledge_Date == null)
                            {
                                proform.Status = "PM";
                            }
                            else if (String.IsNullOrEmpty(getolddata.GroupHead_Action) && getolddata.GroupHead_Submit_Date == null && proform.GroupHead_Submit_Date == null)
                            {
                                proform.Status = "Staff";
                            }
                            else
                            {
                                proform.Status = "Completed";
                            }
                        }


                    }
                    else
                    {
                        if (yx == "GroupHead")
                        {

                            proform.GroupHead_Submit_Date = DateTime.Now;
                            proform.GroupHead_Action = itemdata.maindata.GroupHead_Action;
                            //proform.Mail_Send = "N";
                            proform.Remark_Revise = itemdata.maindata.Remark_Revise;
                            //if (!itemdata.maindata.Cost_Center.Trim().ToLower().Contains("audit"))
                            //{
                            //    proform.Status = "Completed";
                            //}

                            if (itemdata.maindata.Save_Type == "R")
                            {
                                proform.Status = itemdata.maindata.Save_Type;

                                //proform.PM_Submit_Date = null;
                                //proform.Staff_Acknowledge_Date = null;
                                //proform.GroupHead_Submit_Date = null;
                                //proform.HOP_Submit_Date = null;
                            }





                        }
                    }
                }

                proform.Update_Date = DateTime.Now;
                proform.Update_User = CGlobal.UserInfo.EmployeeNo;

                proform.Active_Status = "Y";

                //proform.Probation_Details = lstprode;

                // new step
                //if (itemdata.maindata.Status != "R")
                //{
                if (!string.IsNullOrEmpty(itemdata.maindata.Assessment))
                {
                    if (itemdata.maindata.Assessment == "Y" || itemdata.maindata.Assessment == "N")
                    {
                        if (itemdata.maindata.Assessment == "N")
                        {
                            proform.Staff_Action = itemdata.maindata.PM_Action;
                            //proform.Staff_Acknowledge_Date = DateTime.Now;

                            if (proform.GroupHead_Submit_Date != null)
                                proform.Status = "GroupHead";
                            else
                                proform.Status = "Staff";
                        }

                        if (itemdata.maindata.Status == "R")
                        {
                            proform.Status = itemdata.maindata.You_Are == "PM|GroupHead" ? "PM" : itemdata.maindata.You_Are;
                            proform.Assessment = itemdata.maindata.Assessment;
                            proform.Remark = itemdata.maindata.Remark;

                            //proform.Staff_Acknowledge_Date = null;
                            //proform.GroupHead_Submit_Date = null;
                        }
                        proform.Extend_Period = itemdata.maindata.Extend_Period;
                        //if (itemdata.maindata.Staff_Name.Contains("(Resign)"))
                        //    proform.Status = "Completed";

                        var update = _Probation_FormService.CreateNewOrUpdate(proform);
                        if (update <= 0)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "error ";

                            return Json(new
                            {
                                result
                            });

                        }
                    }
                    else if (itemdata.maindata.Assessment == "O")
                    {
                        if (String.IsNullOrEmpty(itemdata.maindata.Extend_Period))
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Please select Extend Period";

                            return Json(new
                            {
                                result
                            });
                        }
                        else if (Session["ssGetFile"] == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Please select file (Action Plan)";

                            return Json(new
                            {
                                result
                            });

                        }

                        proform.Extend_Period = itemdata.maindata.Extend_Period;

                        proform.Status = itemdata.maindata.You_Are == "PM|GroupHead" || itemdata.maindata.You_Are == "PM|GroupHead|HOP" ? "PM" : itemdata.maindata.You_Are;
                        //proform.Status = itemdata.maindata.You_Are; 
                        if (itemdata.maindata.Status == "R")
                        {
                            proform.Status = itemdata.maindata.You_Are == "PM|GroupHead" || itemdata.maindata.You_Are == "PM|GroupHead|HOP" ? "PM" : itemdata.maindata.You_Are;
                            proform.Assessment = itemdata.maindata.Assessment;
                            proform.Remark = itemdata.maindata.Remark;

                            //proform.Staff_Acknowledge_Date = null;
                            //proform.GroupHead_Submit_Date = null;
                        }
                        var update = _Probation_FormService.CreateNewOrUpdate(proform);
                        if (update <= 0)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "error";

                            return Json(new
                            {
                                result
                            });

                        }
                        else
                        {

                            var getformservice = wsHRis.getEmployeeInfoByEmpNo(getolddata.Staff_No).AsEnumerable().FirstOrDefault();
                            //pm gen new form
                            if (itemdata.maindata.You_Are == "PM")
                            {

                                var gennewform = new Probation_Form();
                                //gennewform = getolddata;

                                gennewform.Assessment = null;
                                gennewform.Remark = null;
                                gennewform.Status = "HR";
                                gennewform.Remark_Revise = null;


                                gennewform.Extend_Form = proform.Id.ToString();
                                gennewform.Extend_Status = "Y";
                                gennewform.Extend_Period = itemdata.maindata.Extend_Period;

                                gennewform.Active_Status = "Y";
                                gennewform.Probation_Active = "Y";

                                //var start_D = Convert.ToDateTime(getolddata.End_Pro).AddDays(1);
                                //var end_D = start_D.AddMonths(Convert.ToInt32(getolddata.Extend_Period));
                                var checkextend = _Probation_FormService.GetDataForSelect().Where(w => w.Extend_Status == "Y" && w.Staff_No == getolddata.Staff_No && w.Assessment == "O").ToList();
                                var start_D = Convert.ToDateTime(getolddata.Start_Pro);
                                var end_D = getolddata.Start_Pro.Value.AddDays(118).AddMonths(Convert.ToInt32(getolddata.Extend_Period));
                                if (checkextend.Count() > 0)
                                {
                                    end_D = checkextend.OrderByDescending(o => o.Id).FirstOrDefault().End_Pro.Value.AddMonths(Convert.ToInt32(getolddata.Extend_Period));
                                }

                                gennewform.Start_Pro = start_D;
                                gennewform.End_Pro = end_D;

                                gennewform.Count_Date_Pro = Convert.ToInt32((end_D - start_D).TotalDays);

                                gennewform.Provident_Fund = getolddata.Provident_Fund;
                                gennewform.Mail_Send = "N";

                                gennewform.PM_Submit_Date = null;
                                gennewform.GroupHead_Submit_Date = null;
                                gennewform.Staff_Acknowledge_Date = null;
                                gennewform.HOP_Submit_Date = null;
                                gennewform.HR_Submit_Date = DateTime.Now;

                                gennewform.Create_Date = DateTime.Now;
                                gennewform.Create_User = CGlobal.UserInfo.EmployeeNo;
                                gennewform.Update_Date = DateTime.Now;
                                gennewform.Update_User = CGlobal.UserInfo.EmployeeNo;


                                gennewform.HR_No = itemdata.maindata.HR_No;
                                gennewform.Staff_No = itemdata.maindata.Staff_No;
                                gennewform.PM_No = getformservice.Field<string>("PM_No");
                                var getcost = getformservice.Field<string>("UnitGroup");
                                int[] taxlegallst = { 2, 5 };
                                if (lstunitgroupauditK.Contains(getformservice.Field<Int32>("PoolID")))
                                {
                                    var getforauditpool = wsHRis.getEmployeeInfoByEmpNo(gennewform.PM_No).AsEnumerable().FirstOrDefault();
                                    gennewform.GroupHead_No = getforauditpool.Field<string>("GroupHead_No");
                                }
                                else
                                {
                                    gennewform.GroupHead_No = getformservice.Field<string>("GroupHead_No");
                                }

                                if ((lstunitgroupauditK.Contains(getformservice.Field<Int32>("UnitGroupID")) && getformservice.Field<Int32>("RankPriority") >= 6) || getformservice.Field<Int32>("UnitGroupID") == 14100063)
                                {
                                    gennewform.HOP_No = null;
                                }
                                else
                                {
                                    gennewform.HOP_No = getformservice.Field<string>("HOP_No");

                                }


                                gennewform.Probation_Details = null;

                                var updatenewform = _Probation_FormService.CreateNew(ref gennewform);

                                var getolddetail = _Probation_DetailService.GetDataForSelect().Where(w => w.Probation_Form_Id == itemdata.maindata.Id).ToList();
                                var getoldform = _Probation_FormService.GetDataForSelect().Where(w => w.Extend_Form == itemdata.maindata.Id.ToString()).OrderByDescending(o => o.Create_Date).FirstOrDefault();

                                if (updatenewform <= 0)
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "error ";

                                    return Json(new
                                    {
                                        result
                                    });

                                }
                                else
                                {
                                    List<Probation_Details> lstgennew = new List<Probation_Details>();

                                    foreach (var adddetail in getolddata.Probation_Details.ToList())
                                    {
                                        Probation_Details gennewdetail = new Probation_Details();

                                        gennewdetail.Seq = adddetail.Seq;
                                        gennewdetail.Active_Status = "Y";

                                        gennewdetail.TM_Probation_Question = adddetail.TM_Probation_Question;
                                        gennewdetail.TM_Probation_Question_Id = adddetail.TM_Probation_Question_Id;

                                        gennewdetail.Probation_Form = getoldform;
                                        gennewdetail.Probation_Form_Id = getoldform.Id;

                                        gennewdetail.Create_Date = DateTime.Now;
                                        gennewdetail.Create_User = CGlobal.UserInfo.EmployeeNo;
                                        gennewdetail.Update_Date = DateTime.Now;
                                        gennewdetail.Update_User = CGlobal.UserInfo.EmployeeNo;

                                        //lstgennew.Add(gennewdetail);
                                        var adddetailnew = _Probation_DetailService.CreateNew(ref gennewdetail);

                                    }
                                    //gennewform.Probation_Details = lstgennew;


                                }
                            }
                        }

                    }
                }
                else
                {
                    var update = _Probation_FormService.CreateNewOrUpdate(proform);
                    if (update <= 0)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "error ";

                        return Json(new
                        {
                            result
                        });

                    }
                }


                if (Session["ssGetFile"] != null && (itemdata.maindata.You_Are == "PM|GroupHead" || itemdata.maindata.You_Are == "PM"))
                {
                    //upload file
                    var getfiletosave = (List<HttpPostedFileBase>)Session["ssGetFile"];
                    int runnum = 1;
                    foreach (var file in getfiletosave)
                    {

                        var fileContent = file;
                        if (fileContent != null && fileContent.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(fileContent.FileName);

                            #region savefiletopath
                            //var gen_namefile = itemdata.maindata.Staff_No + "-AP" + proform.Id + "_" + runnum.ToString("00#") + "." + fileName.Split('.')[1];

                            //var path = HttpContext.Server.MapPath("~/Attachment/Action_Plans/");
                            //fileContent.SaveAs(Path.Combine(path, gen_namefile));
                            #endregion savefiletopath

                            var filedata = new byte[] { };
                            string[] aType = new string[] { ".pdf", ".docx", ".doc", ".xlsx", ".xls" };
                            // get a stream
                            var stream = fileContent.InputStream;
                            // and optionally write the file to disk
                            var fileType = Path.GetExtension(fileContent.FileName).ToLower() + "";
                            if (aType.Contains(fileType))
                            {
                                using (var binaryReader = new BinaryReader(stream))
                                {
                                    filedata = binaryReader.ReadBytes(fileContent.ContentLength);
                                }

                            }
                            var getform = _Probation_FormService.Find(proform.Id);

                            Action_Plans objSave = new Action_Plans()
                            {
                                Probation_Form = getform,
                                Update_User = CGlobal.UserInfo.EmployeeNo,
                                Update_Date = DateTime.Now,
                                Create_User = CGlobal.UserInfo.EmployeeNo,
                                Create_Date = DateTime.Now,
                                Active_Status = "Y",
                                sfile64 = filedata,
                                sfile_oldname = fileName,
                                sfileType = fileType,
                                Description = "",
                            };
                            var sComplect = _Action_PlansService.CreateNew(ref objSave);
                        }

                    }

                    runnum++;
                }




                //send mail
                bool sSendMail = true;
                //IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                //var getbyno = sQuery.Where(w => w.Employeeno == itemdata.maindata.Staff_No).FirstOrDefault();
                //var getbyno = wsHRis.getEmployeeInfoByEmpNo(itemdata.maindata.Staff_No);

                string pathUrl = Url.Action("ProbationForm", "Probation", null, Request.Url.Scheme);
                //production
                //pathUrl = pathUrl.Replace("TraineeEvaluation", "application/HR/HumanCapitalManagement");
                string genlink = HCMFunc.GetUrl("submit", itemdata.maindata.Id.ToString(), pathUrl, CGlobal.UserInfo.EmployeeNo);
                //if (proform.Mail_Send == "Y")
                //{

                var getyou = itemdata.maindata.You_Are;
                try
                {
                    if (getyou == "PM" || getyou == "PM|GroupHead")
                    {                //send to staff
                        if (itemdata.maindata.Assessment != "N" && !itemdata.maindata.Staff_Name.Contains("(Resign)"))
                        {
                            genlink = HCMFunc.GetUrl("submit", itemdata.maindata.Id.ToString(), pathUrl, itemdata.maindata.PM_No);
                            sSendMail = send_mail("Probation to Staff", itemdata.maindata.Staff_No, "", genlink);
                        }

                    }
                    if (getyou == "Staff")
                    {
                        //send to group head
                        //sSendMail = send_mail("Probation to Group Head", itemdata.maindata.GroupHead_No, "", genlink, itemdata.maindata.Staff_Name, itemdata.maindata.PM_Name, itemdata.maindata.PM_No);
                    }
                    if (getyou == "GroupHead")
                    {


                        if (itemdata.maindata.Save_Type == "R")
                        {
                            genlink = HCMFunc.GetUrl("submit", itemdata.maindata.Id.ToString(), pathUrl, itemdata.maindata.HOP_No);
                            sSendMail = send_mail("Probation Revise", itemdata.maindata.PM_No, "", genlink, itemdata.maindata.Staff_Name, itemdata.maindata.Remark_Revise);
                        }
                    }
                    if (getyou == "HOP")
                    {

                        //send to HR
                        //sSendMail = send_mail("Probation to Staff", itemdata.maindata.HR_No, "", genlink);
                    }
                }
                catch (Exception ex)
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = ex.Message;
                }
                //}
                if (!sSendMail)
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Submit Success. But Error in Email";
                    return Json(new
                    {
                        result
                    });
                }



                result.Status = SystemFunction.process_Success;
                result.Msg = "Success ";


            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }



            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult Reset_ProbationForm(vProbation_Form_Return itemdata)
        {
            vProbation_Form_Return result = new vProbation_Form_Return();
            try
            {
                var getmain = itemdata;

                List<Probation_Details> lstprode = new List<Probation_Details>();

                var getyour = itemdata.maindata.You_Are.Split('|').ToList();
                if (getyour[0] == "PM" || itemdata.maindata.Status == "R")
                {
                    // set new form
                    Probation_Form proform = new Probation_Form();



                    proform.Id = itemdata.maindata.Id;

                    proform.Status = "HR";

                    proform.Update_User = CGlobal.UserInfo.EmployeeNo;




                    var update = _Probation_FormService.Reset(proform);


                    // set new detail
                    var getdetail = _Probation_DetailService.GetDataForSelect().Where(w => w.Probation_Form_Id == itemdata.maindata.Id).ToList();
                    foreach (var ex in getdetail)
                    {

                        ex.Assessment = null;
                        ex.Update_Date = DateTime.Now;
                        ex.Update_User = CGlobal.UserInfo.EmployeeNo;
                        //Probation_Details inlist = new Probation_Details();
                        //inlist.Seq = ex.Seq;
                        //inlist.Active_Status = "Y";
                        //inlist.Assessment = "";
                        //inlist.Create_Date = DateTime.Now;
                        //inlist.Create_User = CGlobal.UserInfo.EmployeeNo;
                        //inlist.Update_Date = DateTime.Now;
                        //inlist.Update_User = CGlobal.UserInfo.EmployeeNo;
                        //inlist.TM_Probation_Question_Id = ex.TM_Probation_Question_Id;
                        //inlist.Probation_Form_Id = getmaxid;
                        //lstprode.Add(inlist);
                        var savedetail = _Probation_DetailService.CreateNewOrUpdate(ex);
                    }




                    result = itemdata;
                    result.idEncype = itemdata.maindata.Id.ToString();

                    result.Status = SystemFunction.process_Success;
                    result.Msg = "Success ";

                    //return RedirectToAction("ProbationForm", new { qryStr = itemdata.maindata.Id.ToString() });
                    return Json(new
                    {
                        result
                    });

                }
            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }



            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult Search_ProbationManage(string startFrom, string startUntil, string end, string status, string empno, string type, string without, string PageType)
        {
            vProbation_Form_Return result = new vProbation_Form_Return();
            try
            {
                List<vProbation_Form_obj> objFile = new List<vProbation_Form_obj>();
                string sSess = Request.Form["sSess"];
                objFile = Session[sSess] as List<vProbation_Form_obj>;
                var getprowithout = new List<Probation_With_Out>();
                List<vProbation_Form_obj> genNewWithout = new List<vProbation_Form_obj>();
                List<Probation_Form> getprolistExtendold = new List<Probation_Form>();

                var getprolist = _Probation_FormService.GetDataWithOutExtendForSelect().ToList();
                getprolistExtendold = _Probation_FormService.GetDataExtendForSelect().ToList();



                var getuserinfo = CGlobal.UserInfo.EmployeeNo;

                string[] UserAdmin = WebConfigurationManager.AppSettings["ProbationAdmin"].Split(';');
                if (UserAdmin.Contains(CGlobal.UserInfo.UserId))
                {

                    if (without == "true")
                    {
                        getprowithout = _Probation_With_OutService.GetDataForSelect().ToList();
                        List<vProbation_Form_obj> lstwo = new List<vProbation_Form_obj>();
                        foreach (var s in getprowithout)
                        {
                            vProbation_Form_obj nw = new vProbation_Form_obj();
                            var getservice = wsHRis.getEmployeeInfoByEmpNo(s.Staff_No).AsEnumerable().FirstOrDefault();


                            nw.Staff_No = s.Staff_No;
                            nw.Status = "";
                            nw.Company = getservice.Field<string>("CompanyCode");
                            nw.Cost_Center = getservice.Field<string>("UnitGroup");
                            nw.Staff_Name = getservice.Field<string>("EmpFullName");
                            nw.Start_Pro = getservice.Field<DateTime>("JoinDate").ToString("MMMM dd, yyyy");
                            nw.End_Pro = "";
                            nw.PM_No = getservice.Field<string>("PM_No");
                            nw.PM_Name = getservice.Field<string>("PM_Name");
                            nw.GroupHead_Name = getservice.Field<string>("GroupHead_Name");
                            nw.HOP_Name = (lstunitgroupauditK.Contains(getservice.Field<Int32>("UnitGroupID"))
                                && getservice.Field<Int32>("RankPriority") >= 6)
                             ? "-" : !String.IsNullOrEmpty(getservice.Field<string>("HOP_Name")) ?"": getservice.Field<string>("HOP_Name");

                            nw.Create_Date = s.Create_Date.Value.ToString("MMMM dd, yyyy");
                            nw.Update_Date = s.Update_Date.Value.ToString("MMMM dd, yyyy");

                            nw.Assessment = "Without";
                            lstwo.Add(nw);
                        }
                        genNewWithout = lstwo;

                    }


                }
                var getprolistExtend = new List<Probation_Form>();
                var newproextend = new List<Probation_Form>();
                if (getprolistExtendold.Count() > 0)
                {
                    foreach (var ex in getprolistExtendold.Where(w => w.Start_Pro.Value.AddDays(118).AddDays(w.Extend_Period == "1" ? 16 : 31) <= DateTime.Now).ToList())
                    {
                        //var checkex = _Probation_FormService.Find(SystemFunction.GetIntNullToZero(ex.Extend_Form));
                        //if (checkex.GroupHead_Submit_Date != null && checkex.Status != "R")
                        //{
                        //    newproextend.Add(ex);
                        //}

                        newproextend.Add(ex);

                    }

                    if (newproextend.Count() > 0)
                    {
                        getprolistExtend = newproextend;
                    }
                }

                if (!String.IsNullOrEmpty(empno))
                {
                    getprolist = getprolist.Where(w => w.Staff_No == empno).ToList();
                    getprolistExtend = getprolistExtend.Where(w => w.Staff_No == empno).ToList();
                }

                //getprolist.AddRange(getprolistExtend);
                if (!String.IsNullOrEmpty(startFrom))
                {
                    var newpro = new List<Probation_Form>();
                    foreach (var w in getprolistExtend)
                    {
                        Probation_Form getprolistExtendbyadmin = new Probation_Form();
                        getprolistExtendbyadmin = (_Probation_FormService.Find(SystemFunction.GetIntNullToZero(w.Extend_Form)));
                        if (getprolistExtendbyadmin != null)
                        {
                            var geta = getprolistExtendbyadmin;


                            int getoldextend = 0;
                            if (geta != null)
                            {
                                Probation_Form getb = new Probation_Form();
                                getb = _Probation_FormService.Find(SystemFunction.GetIntNullToZero(geta.Extend_Form));
                                getoldextend += SystemFunction.GetIntNullToZero(geta.Extend_Period);

                                if (getb != null)
                                {
                                    Probation_Form getc = new Probation_Form();
                                    getc = _Probation_FormService.Find(SystemFunction.GetIntNullToZero(getb.Extend_Form));
                                    getoldextend += SystemFunction.GetIntNullToZero(getb.Extend_Period);

                                    if (getc != null)
                                        getoldextend += SystemFunction.GetIntNullToZero(getc.Extend_Period);
                                }

                            }



                            if (!string.IsNullOrEmpty(w.Extend_Form))
                            {
                                if (Convert.ToDateTime(w.Start_Pro.Value.AddMonths(getoldextend)) >= Convert.ToDateTime(startFrom).Date)
                                {
                                    newpro.Add(w);
                                }
                            }
                        }
                        //else
                        //{
                        //    if (Convert.ToDateTime(w.Start_Pro.Value.AddMonths(SystemFunction.GetIntNullToZero(w.Extend_Period))) >= Convert.ToDateTime(startFrom).Date)
                        //        newpro.Add(w);
                        //}
                    }

                    getprolist = getprolist.Where(w => w.Start_Pro.Value.Date >= Convert.ToDateTime(startFrom).Date).ToList();
                    genNewWithout = genNewWithout.Where(w => Convert.ToDateTime(w.Start_Pro).Date >= Convert.ToDateTime(startFrom).Date).ToList();
                    getprolistExtend = newpro;




                    //List<Probation_With_Out> newlist = new List<Probation_With_Out>();
                    //foreach (var ex in getprolist)
                    //{
                    //    Probation_With_Out obj = new Probation_With_Out();
                    //    if (ex.Extend_Status == "Y")
                    //        if (Convert.ToDateTime(ex.Start_Pro).Date >= Convert.ToDateTime(startFrom).Date)
                    //        { }
                    //}
                }
                if (!String.IsNullOrEmpty(startUntil))
                {
                    var newpro = new List<Probation_Form>();
                    foreach (var w in getprolistExtend)
                    {
                        Probation_Form getprolistExtendbyadmin = new Probation_Form();
                        getprolistExtendbyadmin = (_Probation_FormService.Find(SystemFunction.GetIntNullToZero(w.Extend_Form)));
                        if (getprolistExtendbyadmin != null)
                        {
                            var geta = getprolistExtendbyadmin;


                            int getoldextend = 0;
                            if (geta != null)
                            {
                                Probation_Form getb = new Probation_Form();
                                getb = _Probation_FormService.Find(SystemFunction.GetIntNullToZero(geta.Extend_Form));
                                getoldextend += SystemFunction.GetIntNullToZero(geta.Extend_Period);

                                if (getb != null)
                                {
                                    Probation_Form getc = new Probation_Form();
                                    getc = _Probation_FormService.Find(SystemFunction.GetIntNullToZero(getb.Extend_Form));
                                    getoldextend += SystemFunction.GetIntNullToZero(getb.Extend_Period);

                                    if (getc != null)
                                        getoldextend += SystemFunction.GetIntNullToZero(getc.Extend_Period);
                                }

                            }



                            if (!string.IsNullOrEmpty(w.Extend_Form))
                            {
                                if (Convert.ToDateTime(w.Start_Pro.Value.AddMonths(getoldextend)) <= Convert.ToDateTime(startUntil).Date)
                                {
                                    newpro.Add(w);
                                }
                            }
                        }
                        //else
                        //{
                        //    if (Convert.ToDateTime(w.Start_Pro.Value.AddMonths(SystemFunction.GetIntNullToZero(w.Extend_Period))) >= Convert.ToDateTime(startFrom).Date)
                        //        newpro.Add(w);
                        //}
                    }

                    getprolist = getprolist.Where(w => w.Start_Pro.Value.Date <= Convert.ToDateTime(startUntil).Date).ToList();
                    genNewWithout = genNewWithout.Where(w => Convert.ToDateTime(w.Start_Pro).Date <= Convert.ToDateTime(startUntil).Date).ToList();
                    getprolistExtend = newpro;
                }

                if (!String.IsNullOrEmpty(end))
                {
                    getprolist = getprolist.Where(w => w.End_Pro.Value.Date <= Convert.ToDateTime(end).Date).ToList();

                }

                if (!String.IsNullOrEmpty(status))
                {
                    if (status == "Completed")
                        getprolist = getprolist.Where(w => w.Status == "Completed").ToList();
                    else if (status == "Incomplete")
                        getprolist = getprolist.Where(w => w.Status != "Completed").ToList();
                }





                getprolist.AddRange(getprolistExtend);

                var getuserrank = CGlobal.UserInfo.Rank;


                //var get_auth = check_user_info(getuserinfo,getprolist);

                string BackUrl = "";
                List<vProbation_Form_obj> setprolist = new List<vProbation_Form_obj>();

                foreach (var s in getprolist)
                {
                    var getstaff = wsHRis.getEmployeeInfoByEmpNo(s.Staff_No);
                    var getpm = wsHRis.getEmployeeInfoByEmpNo(s.PM_No);
                    var getgh = wsHRis.getEmployeeInfoByEmpNo(s.GroupHead_No);
                    DataTable gethop = new DataTable();
                    if (!String.IsNullOrEmpty(s.HOP_No))
                        gethop = wsHRis.getEmployeeInfoByEmpNo(s.HOP_No);

                    //string[] lstauditK = { "14100058", "14100059", "14100060", "14100002", "14100062" };

                    vProbation_Form_obj obj = new vProbation_Form_obj();
                    obj.View = @"<button id=""btnView""  type=""button"" onclick=""View('" + s.Id + @"')"" class=""btn btn-xs btn-success""><i class=""fa fa-eye""></i></button>";
                    obj.Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + s.Id + @"')"" class=""btn btn-xs btn-primary""><i class=""fa fa-edit""></i></button>";
                    obj.Update = @"<button id=""btnUpdate""  type=""button"" onclick=""Update('" + s.Id + @"')"" class=""btn btn-xs btn-warning""><i class=""fa fa-power-off""></i></button>";
                    obj.Id = s.Id;
                    obj.Assessment = String.IsNullOrEmpty(s.Assessment) ? "Pending" : s.Assessment == "Y" ? "Pass" : s.Assessment == "N" ? "Not Pass" : s.Assessment == "O" ? "Other" : "";
                    obj.Company = getstaff.AsEnumerable().FirstOrDefault().Field<string>("CompanyCode");
                    obj.Cost_Center = getstaff.AsEnumerable().FirstOrDefault().Field<string>("UnitGroup");
                    obj.Staff_No = s.Staff_No;
                    obj.Staff_Name = getstaff.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") + (s.Extend_Status == "Y" ? " (Extend)" : "");
                    obj.Start_Pro = s.Start_Pro.Value.ToString("MMMM dd, yyyy");
                    obj.End_Pro = s.End_Pro.Value.ToString("MMMM dd, yyyy");
                    obj.PM_Name = !String.IsNullOrEmpty(s.PM_No) ? getpm.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") : "";
                    obj.GroupHead_Name = !String.IsNullOrEmpty(s.GroupHead_No) && s.GroupHead_No != "00000000" ? getgh.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") + "" : "";


                    obj.HOP_Name =
                        (lstunitgroupauditK.Contains(getstaff.AsEnumerable().FirstOrDefault().Field<Int32>("UnitGroupID"))
                        && getstaff.AsEnumerable().FirstOrDefault().Field<Int32>("RankPriority") >= 6)
                    ? "-" : !String.IsNullOrEmpty(s.HOP_No) ? gethop.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName") : "";
                    obj.Create_Date = s.Create_Date.Value.ToString("MMMM dd, yyyy");
                    obj.Update_Date = s.Update_Date.Value.ToString("MMMM dd, yyyy");
                    //obj.Status = s.Status == "R" ? "Revise" : s.Status == "Completed" ? "Completed" : s.HR_Submit_Date != null ? s.PM_Submit_Date != null ? (!obj.Staff_Name.Contains("Resign") && s.Staff_Acknowledge_Date != null) ? s.GroupHead_Submit_Date != null ? s.HOP_Submit_Date != null ? "Completed" : (lstunitgroupauditK.Contains(getstaff.AsEnumerable().FirstOrDefault().Field<int>("UnitGroupID")) && getstaff.AsEnumerable().FirstOrDefault().Field<int>("RankPriority") >= 6) ? "Completed" : "HOP" : "Group Head" : "Staff" : "PM" : "";
                    obj.Status = s.Status == "R" ? "Revise" :
                                        s.Status == "Completed" ? "Completed" :
                                        s.Status == "HR" ? "PM" :
                                        s.Status == "PM" ? "Staff" :
                                        s.Status == "Staff" ? "Group Head" :
                                        s.Status == "GroupHead" ? "HOP" :
                                        s.Status == "HOP" ? "Completed" :
                                        "";
                    obj.Absent = "Vac : 8 hrs.<br>Sick : 3 hrs.";
                    obj.HR_No = s.HR_No;
                    obj.PM_No = s.PM_No;
                    obj.GroupHead_No = s.GroupHead_No;
                    obj.HOP_No = s.HOP_No;
                    obj.Extend_Status = s.Extend_Status;
                    obj.Extend_Period = s.Extend_Period;

                    setprolist.Add(obj);
                }

                if (genNewWithout.Count() > 0)
                {
                    setprolist.AddRange(genNewWithout);
                }

                result.lstDataManage = setprolist;
                Session[sSess] = setprolist;
            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult Save_ManagePeople(string pro_id, string pm_no, string grouphaed_no, string hop_no)
        {
            vProbation_Form_Return result = new vProbation_Form_Return();
            try
            {
                var forchange = 0;

                Probation_Form proform = new Probation_Form();
                proform = _Probation_FormService.Find(SystemFunction.GetIntNullToZero(pro_id));

                if (proform.PM_No != pm_no && proform.PM_Submit_Date == null)
                {
                    proform.PM_No = pm_no;
                    forchange++;
                }
                if (proform.GroupHead_No != grouphaed_no && proform.GroupHead_Submit_Date == null)
                {
                    proform.GroupHead_No = grouphaed_no;
                    forchange++;
                }
                if (proform.HOP_No != hop_no && proform.HOP_Submit_Date == null)
                {
                    proform.HOP_No = hop_no;
                    forchange++;
                }

                proform.Update_Date = DateTime.Now;
                proform.Update_User = CGlobal.UserInfo.EmployeeNo;


                if (forchange > 0)
                {
                    var update = _Probation_FormService.CreateNewOrUpdate(proform);
                    if (update <= 0)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "error ";

                        return Json(new
                        {
                            result
                        });

                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Success;
                    result.Msg = "Successful recording but no data changes.";
                    return Json(new
                    {
                        result
                    });
                }


                //send mail
                //bool sSendMail = true;
                //IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                //var getbyno = sQuery.Where(w => w.Employeeno == itemdata.maindata.Staff_No).FirstOrDefault();
                //string pathUrl = Url.Action("ProbationForm", "Probation", null, Request.Url.Scheme);
                ////production
                ////pathUrl = pathUrl.Replace("TraineeEvaluation", "application/HR/HumanCapitalManagement");
                //string genlink = HCMFunc.GetUrl("submit", itemdata.maindata.Id.ToString(), pathUrl, CGlobal.UserInfo.EmployeeNo);
                ////if (proform.Mail_Send == "Y")
                ////{
                //if (itemdata.maindata.You_Are == "PM")
                //{                //send to staff
                //    if (itemdata.maindata.Assessment != "N")
                //        sSendMail = send_mail("Probation to Staff", itemdata.maindata.Staff_No, "", genlink);
                //}
                //else if (itemdata.maindata.You_Are == "Staff")
                //{
                //    //send to group head
                //    //sSendMail = send_mail("Probation to Group Head", itemdata.maindata.GroupHead_No, "", genlink, itemdata.maindata.Staff_Name, itemdata.maindata.PM_Name, itemdata.maindata.PM_No);
                //}
                //else if (itemdata.maindata.You_Are == "GroupHead")
                //{


                //    if (itemdata.maindata.Status == "R")
                //        sSendMail = send_mail("Probation Revise", itemdata.maindata.PM_No, "", genlink, itemdata.maindata.Staff_Name, "");
                //}
                //else if (itemdata.maindata.You_Are == "HOP")
                //{

                //    //send to HR
                //    //sSendMail = send_mail("Probation to Staff", itemdata.maindata.HR_No, "", genlink);
                //}
                ////}
                //if (!sSendMail)
                //{
                //    result.Status = SystemFunction.process_Failed;
                //    result.Msg = "Error, Submit Success. But Error in Email";
                //    return Json(new
                //    {
                //        result
                //    });
                //}



                result.Status = SystemFunction.process_Success;
                result.Msg = "Success ";


            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }



            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult Search_Probation_With_Out(string startUntil, string endUntil)
        {
            vProbation_With_Out_Return result = new vProbation_With_Out_Return();
            try
            {

                List<vProbation_With_Out_obj> lstobj = new List<vProbation_With_Out_obj>();

                var getpro = _Probation_With_OutService.GetDataForSelect().ToList();

                foreach (var ex in getpro)
                {
                    var getsv = wsHRis.getEmployeeInfoByEmpNo(ex.Staff_No);
                    vProbation_With_Out_obj obj = new vProbation_With_Out_obj();
                    obj.Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Delete('" + HCMFunc.Encrypt(ex.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""fa fa-trash""></i></button>";
                    obj.Id = ex.Id;
                    obj.Staff_No = ex.Staff_No;
                    obj.Staff_Name = getsv.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");
                    obj.Company = getsv.AsEnumerable().FirstOrDefault().Field<string>("CompanyCode");
                    obj.Cost_Center = getsv.AsEnumerable().FirstOrDefault().Field<string>("UnitGroup");
                    obj.Join_Date = getsv.AsEnumerable().FirstOrDefault().Field<DateTime>("JoinDate").ToString("dd MMM yyyy");
                    obj.Create_Date = ex.Create_Date.Value.ToString("dd MMM yyyy");
                    obj.Create_User = ex.Create_User;
                    obj.Update_Date = ex.Update_Date.Value.ToString("dd MMM yyyy");
                    obj.Update_User = ex.Update_User;
                    obj.Active_Status = ex.Active_Status;
                    obj.Remark = ex.Remark;



                    lstobj.Add(obj);
                }

                if (!string.IsNullOrEmpty(startUntil))
                {
                    var setdate = Convert.ToDateTime(startUntil);
                    var enddate = Convert.ToDateTime(endUntil);
                    if (setdate != null)
                    {
                        lstobj = lstobj.Where(w => Convert.ToDateTime(w.Join_Date).Month >= setdate.Month && Convert.ToDateTime(w.Join_Date).Year == setdate.Year).ToList();
                    }
                    if (enddate != null)
                    {
                        lstobj = lstobj.Where(w => Convert.ToDateTime(w.Join_Date).Month <= enddate.Month && Convert.ToDateTime(w.Join_Date).Year == enddate.Year).ToList();

                    }
                }

                result.lstData = lstobj;
            }
            catch (Exception ex)
            {

            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult Save_People_With_Out(string Staff_No)
        {
            vProbation_With_Out_Return result = new vProbation_With_Out_Return();
            try
            {
                var checkdata = _Probation_With_OutService.FindByEmpno(Staff_No);
                if (checkdata != null)
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "This person already exists in the system.";

                    return Json(new
                    {
                        result
                    });
                }

                Probation_With_Out item = new Probation_With_Out();
                item.Staff_No = Staff_No;
                item.Active_Status = "Y";
                item.Create_User = CGlobal.UserInfo.EmployeeNo;
                item.Create_Date = DateTime.Now;
                item.Update_User = CGlobal.UserInfo.EmployeeNo;
                item.Update_Date = DateTime.Now;
                var forchange = _Probation_With_OutService.CreateNewOrUpdate(item);

                if (forchange < 1)
                {

                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "error ";

                    return Json(new
                    {
                        result
                    });


                }
                else
                {
                    result.Status = SystemFunction.process_Success;
                    result.Msg = "Success. Record successfully recorded.";
                    return Json(new
                    {
                        result
                    });
                }





            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }



            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult Delete_People_With_Out(string nid)
        {
            vProbation_With_Out_Return result = new vProbation_With_Out_Return();
            try
            {

                var getid = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(nid + ""));
                var forchange = _Probation_With_OutService.SetInActive(getid, CGlobal.UserInfo.EmployeeNo, "N");

                if (forchange < 1)
                {

                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "error ";

                    return Json(new
                    {
                        result
                    });


                }
                else
                {
                    result.Status = SystemFunction.process_Success;
                    result.Msg = "Success. Deleted.";
                    return Json(new
                    {
                        result
                    });
                }





            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }



            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult UploadFileBypass(vProbation_Form_Return objModel)
        {
            vProbation_Form_Return result = new vProbation_Form_Return();
            if (Request != null)
            {
                result = (vProbation_Form_Return)Session["ssProForm"];

                if (Request.Files.Count > 0)
                {
                    var getfiletosession = Request.Files;

                    List<HttpPostedFileBase> lstfile = new List<HttpPostedFileBase>();
                    foreach (string file in Request.Files)
                    {
                        lstfile.Add(Request.Files[file]);

                        //var fileContent = Request.Files[file];
                        //if (fileContent != null && fileContent.ContentLength > 0)
                        //{
                        //    // get a stream
                        //    var stream = fileContent.InputStream;
                        //    // and optionally write the file to disk
                        //    var fileName = Path.GetFileName(file);


                        //}
                    }
                    Session["ssGetFile"] = lstfile;
                    //result.Status = SystemFunction.process_Success;
                    //result.Msg = "Success.";

                }
                else
                {
                    //result.Status = SystemFunction.process_Failed;
                    //result.Msg = "Incorrect template, please try again.";
                }
            }


            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult InactiveForm(string nid)
        {
            vProbation_With_Out_Return result = new vProbation_With_Out_Return();
            try
            {
                var getid = SystemFunction.GetIntNullToZero(nid);
                var Inactive = _Probation_FormService.SetInActive(getid, CGlobal.UserInfo.EmployeeNo, "N");
                if (Inactive >= 1)
                {
                    result.Status = SystemFunction.process_Success;
                    result.Msg = "Inactive form completed.";
                    return Json(new
                    {
                        result
                    });
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "error ";

                    return Json(new
                    {
                        result
                    });

                }
            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }



            return Json(new
            {
                result
            });
        }

        #endregion Ajax

        #region Fn

        public string getvalnull(string value)
        {
            string returnvalue = "";
            if (!String.IsNullOrEmpty(value))
            {
                returnvalue = value;
            }
            return returnvalue;
        }

        public ActionResult CreatAction_Plans_File(string id)
        {
            var sCheck = pesCheckLoginAndPermission(true);
            if (sCheck != null)
            {
                return sCheck;
            }
            Stream stream = new MemoryStream();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(id + ""));
                if (nId != 0)
                {
                    var _getData = _Action_PlansService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.Probation_Form != null)
                        {
                            //if (_getData.PTR_Evaluation.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user.Contains(CGlobal.UserInfo.EmployeeNo)) || CGlobal.UserIsAdminPES())
                            //{
                            if (_getData.sfileType == ".docx" || _getData.sfileType == ".doc")
                            {
                                return File(_getData.sfile64, "application/octet-stream", _getData.sfile_oldname + _getData.sfileType);
                            }
                            else if (_getData.sfileType == ".xls" || _getData.sfileType == ".xlsx")
                            {
                                return File(_getData.sfile64, "application/excel", _getData.sfile_oldname + _getData.sfileType);
                            }
                            else if (_getData.sfileType == ".pdf")
                            {
                                return File(_getData.sfile64, "application/pdf", _getData.sfile_oldname + _getData.sfileType);
                            }
                            //}
                            //else
                            //{
                            //    return JavaScript("CloseTab();");
                            //}
                        }
                        else
                        {
                            return JavaScript("CloseTab();");
                        }

                        //stream = new MemoryStream(_getData.sfile64);
                        //// Get content of your Excel file
                        ////var stream = wBook.WriteXLSX(); // Return a MemoryStream (Using DTG.Spreadsheet)

                        //stream.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
            return JavaScript("CloseTab();");
        }

        public bool send_mail(string mailcontent_name, string reciepver, string sender, string genlink, string param1 = "", string param2 = "", string param3 = "")
        {
            bool returnbool = true;
            try
            {
                string msg = "";
                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                var get = wsHRis.getEmployeeInfoByEmpNo(reciepver).AsEnumerable().FirstOrDefault();
                var getbyno = sQuery.Where(w => w.Employeeno == reciepver).FirstOrDefault();
                string pathUrl = Url.Action("ProbationForm", "Probation", null, Request.Url.Scheme);
                //production
                pathUrl = pathUrl.Replace("TraineeEvaluation", "application/HR/HumanCapitalManagement");


                var MailRequest = _MailContentService.GetMailContent(mailcontent_name, "Y").FirstOrDefault();
                //string genlink = HCMFunc.GetUrl("submit", itemdata.maindata.Id.ToString(), pathUrl, CGlobal.UserInfo.EmployeeNo);

                string sContent = MailRequest.content;
                string from = MailRequest.sender_name;
                string subject = MailRequest.mail_header;

                sContent = (sContent + "").Replace("$linkto", genlink);
                sContent = (sContent + "").Replace("$datetimenow", DateTime.Now.ToString("MMMM dd, yyyy"));
                sContent = (sContent + "").Replace("$sender", CGlobal.UserInfo.FullName);
                sContent = (sContent + "").Replace("$staff", param1);
                sContent = (sContent + "").Replace("$pm", param2);
                sContent = (sContent + "").Replace("$reciepver", get.Field<string>("EmpFullName"));
                sContent = (sContent + "").Replace("$Appraisee", param1);
                sContent = (sContent + "").Replace("$remarkrevise", param2);

                string getemailcc = "";
                if (!String.IsNullOrEmpty(param3))
                {
                    var getcc = sQuery.Where(w => w.Employeeno == param3).FirstOrDefault();
                    getemailcc = getcc.Email;
                }

                var objMail = new vObjectMail_Send();
                objMail.mail_from = "hcmthailand@kpmg.co.th";
                objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                objMail.mail_to = get.Field<string>("Email");
                objMail.mail_cc = !String.IsNullOrEmpty(getemailcc) ? CGlobal.UserInfo.EMail + "," + getemailcc : CGlobal.UserInfo.EMail;
                objMail.mail_subject = subject;
                objMail.mail_content = sContent;

                var sSendMail = HCMFunc.SendMail(objMail, ref msg);

                returnbool = sSendMail;
            }
            catch (Exception ex)
            {
                returnbool = false;
            }
            return returnbool;
        }

        public string check_user_info(string getuserinfo, Probation_Form getprolist)
        {
            var you_are = "";
            if (getuserinfo == getprolist.HR_No)
            {
                you_are = "HR";
            }
            if (getuserinfo == getprolist.PM_No)
            {
                if (!String.IsNullOrEmpty(you_are))
                    you_are += "|";

                you_are += "PM";
            }
            if (getuserinfo == getprolist.Staff_No)
            {
                if (!String.IsNullOrEmpty(you_are))
                    you_are += "|";

                you_are += "Staff";
            }
            if (getuserinfo == getprolist.GroupHead_No)
            {
                if (!String.IsNullOrEmpty(you_are))
                    you_are += "|";

                you_are += "GroupHead";
            }
            if (getuserinfo == getprolist.HOP_No)
            {
                if (!String.IsNullOrEmpty(you_are))
                    you_are += "|";

                you_are += "HOP";
            }
            return you_are;
        }

        public ActionResult LetterExport(string empno, string enddatepro)
        {

            try
            {
                enddatepro = Convert.ToDateTime(enddatepro).AddDays(1).ToString("MMMM dd, yyyy");
                var get = wsHRis.getEmployeeInfoByEmpNo(empno).AsEnumerable().FirstOrDefault();

                exProbation_Letter reportLetter = new exProbation_Letter();
                var stream = new MemoryStream();
                XrTemp xrTemp = new XrTemp();

                xrTemp.CreateDocument();

                XRLabel xrLBCompanyEng = (XRLabel)reportLetter.FindControl("xrLBCompanyEng", true);
                xrLBCompanyEng.Text = get.Field<string>("Company");

                XRLabel xrLBCompanyTh = (XRLabel)reportLetter.FindControl("xrLBCompanyTh", true);
                xrLBCompanyTh.Text = get.Field<string>("CompanyTH");

                XRLabel xrLBEffective_Date = (XRLabel)reportLetter.FindControl("xrLBEffective_Date", true);
                xrLBEffective_Date.Text = enddatepro;

                XRLabel xrLBFullName = (XRLabel)reportLetter.FindControl("xrLBFullName", true);
                xrLBFullName.Text = get.Field<string>("EmpTitle") + " " + get.Field<string>("EmpFullName");

                XRLabel xrLBPosition = (XRLabel)reportLetter.FindControl("xrLBPosition", true);
                xrLBPosition.Text = Regex.Replace(get.Field<string>("JobLevelText"), "[0-9]", "");

                XRLabel xrLBCostCenter = (XRLabel)reportLetter.FindControl("xrLBCostCenter", true);
                xrLBCostCenter.Text = get.Field<string>("UnitGroup");

                XRLabel xrLBName = (XRLabel)reportLetter.FindControl("xrLBName", true);
                xrLBName.Text = get.Field<string>("EmpTitle") + " " + get.Field<string>("EmpFirstName") + ",";

                XRLabel xrLBEffectiveDate2 = (XRLabel)reportLetter.FindControl("xrLBEffectiveDate2", true);
                xrLBEffectiveDate2.Text = xrLBEffective_Date.Text;

                XRLabel xrLBStaffID = (XRLabel)reportLetter.FindControl("xrLBStaffID", true);
                xrLBStaffID.Text = Convert.ToInt64(get.Field<string>("EmpNo")).ToString();

                XRLabel xrLBForFooter = (XRLabel)reportLetter.FindControl("xrLBForFooter", true);
                xrLBForFooter.Text = xrLBCompanyEng.Text;
                xrLBForFooter.Text += " a Thai limited liability company and a member firm of the KPMG network of independent member firms affiliated with KPMG International Cooperative(\"KPMG International\"), a Swiss entity.";

                var compcode = get.Field<string>("CompanyCode");

                XRLabel xrLBFloorTH = (XRLabel)reportLetter.FindControl("xrLBFloorTH", true);
                switch (compcode)
                {
                    case "KPA":
                        xrLBFloorTH.Text = "ชั้น 50 เอ็มไพร์ทาวเวอร์";
                        break;
                    case "KPT":
                        xrLBFloorTH.Text = "ชั้น 49 เอ็มไพร์ทาวเวอร์";
                        break;
                    case "KPBA":
                        xrLBFloorTH.Text = "ชั้น 48 เอ็มไพร์ทาวเวอร์";
                        break;
                    case "KPL":
                        xrLBFloorTH.Text = "ชั้น 49 เอ็มไพร์ทาวเวอร์";
                        break;
                }


                XRLabel xrLBFloor = (XRLabel)reportLetter.FindControl("xrLBFloor", true);
                switch (compcode)
                {
                    case "KPA":
                        xrLBFloor.Text = "50   Floor, Empire Tower";
                        break;
                    case "KPT":
                        xrLBFloor.Text = "49   Floor, Empire Tower";
                        break;
                    case "KPBA":
                        xrLBFloor.Text = "48   Floor, Empire Tower";
                        break;
                    case "KPL":
                        xrLBFloor.Text = "49   Floor, Empire Tower";
                        break;
                }

                XRPictureBox xrPicCheck = (XRPictureBox)reportLetter.FindControl("xrPictureBox1", true);
                xrPicCheck.ImageUrl = "~\\Image\\KPMG-RGB.png";

                XRPictureBox xrPicCheck2 = (XRPictureBox)reportLetter.FindControl("xrPictureBox2", true);
                xrPicCheck2.ImageUrl = "~\\Image\\since-by-WannaP.png";

                reportLetter.CreateDocument();

                xrTemp.Pages.AddRange(reportLetter.Pages);
                PdfExportOptions pdfOptions = xrTemp.ExportOptions.Pdf;


                xrTemp.ExportToPdf(stream, pdfOptions);
                stream.Seek(0, SeekOrigin.Begin);
                byte[] breport = stream.ToArray();
                return File(breport, "application/pdf", "Letter of Successful Probation Period " + get.Field<string>("EmpNo") + DateTime.Now.ToString("ddMMyyyy") + ".pdf");

            }
            catch (Exception ex)
            {

            }

            return Json("");
        }

        public ActionResult SetVariable(string key, string value)
        {
            Session[key] = value;

            return this.Json(new { success = true });
        }

        public ActionResult ProbationExport()
        {
            vProbation_Form_Return result = new vProbation_Form_Return();

            try
            {
                var dNow = DateTime.Now;
                List<vProbation_Form_obj> objFile = new List<vProbation_Form_obj>();
                string sSess = Request.Form["sSess"];
                objFile = Session[sSess] as List<vProbation_Form_obj>;
                if (objFile != null)
                {
                    //var export = this.ExportToExcel(ItemData, ItemData.subject);
                    dxProbationList reportMemo = new dxProbationList();


                    var stream = new MemoryStream();
                    XrTemp xrTemp = new XrTemp();

                    xrTemp.CreateDocument();

                    #region Memo
                    xrTemp.CreateDocument();
                    if (objFile.Count() > 0)
                    {


                        var setAll = objFile.Select(s => new
                        {
                            Assessment = !String.IsNullOrEmpty(s.Extend_Period) && s.Assessment == "Other" ? s.Assessment + "(Extend " + s.Extend_Period + " month.)" : s.Assessment,
                            Staff_No = !string.IsNullOrEmpty(s.Staff_No) ? s.Staff_No + "" : "",
                            Staff_Name = !string.IsNullOrEmpty(s.Staff_Name) ? s.Staff_Name : "",
                            Start_Pro = !string.IsNullOrEmpty(s.Start_Pro) ? Convert.ToDateTime(s.Start_Pro).ToString("dd-MMMM-yyyy") + "" : "",
                            End_Pro = !string.IsNullOrEmpty(s.End_Pro) ? Convert.ToDateTime(s.End_Pro).ToString("dd-MMMM-yyyy") + "" : "",
                            PM_Name = !string.IsNullOrEmpty(s.PM_Name) ? s.PM_Name : "",
                            GroupHead_Name = !string.IsNullOrEmpty(s.GroupHead_Name) ? s.GroupHead_Name : "",
                            HOP_Name = !string.IsNullOrEmpty(s.HOP_Name) ? s.HOP_Name : "",
                            Create_Date = !string.IsNullOrEmpty(s.Create_Date) ? Convert.ToDateTime(s.Create_Date).ToString("dd-MMMM-yyyy") + "" : "",
                            Update_Date = !string.IsNullOrEmpty(s.Update_Date) ? Convert.ToDateTime(s.Update_Date).ToString("dd-MMMM-yyyy") + "" : "",
                            Status = !string.IsNullOrEmpty(s.Status) ? s.Status + "" : "",
                            Company = !string.IsNullOrEmpty(s.Company) ? s.Company + "" : "",
                            Cost_Center = !string.IsNullOrEmpty(s.Cost_Center) ? s.Cost_Center + "" : "",
                            Rank = !string.IsNullOrEmpty(s.Staff_No) ? wsHRis.getEmployeeInfoByEmpNo(s.Staff_No).AsEnumerable().FirstOrDefault().Field<string>("Rank") + "" : "",
                            PM_No = !string.IsNullOrEmpty(s.PM_No) ? s.PM_No + "" : "",
                            PM_Cost = (!string.IsNullOrEmpty(s.PM_No.Trim()) && s.PM_No != "00000000") ? wsHRis.getEmployeeInfoByEmpNo(s.PM_No).AsEnumerable().FirstOrDefault().Field<string>("UnitGroup") + "" : "",

                        }).ToList();
                        System.Data.DataTable dtR = SystemFunction.LinqToDataTable(setAll);
                        dsProbation abc = new dsProbation();
                        abc.dtProbation.Merge(dtR);


                        reportMemo.DataSource = abc;
                        reportMemo.CreateDocument();




                    }
                    #endregion memo


                    #region Export 



                    //Add page
                    xrTemp.Pages.AddRange(reportMemo.Pages);


                    XlsxExportOptions xlsxOptions = xrTemp.ExportOptions.Xlsx;
                    xlsxOptions.ShowGridLines = true;
                    xlsxOptions.SheetName = "Tab_";
                    xlsxOptions.TextExportMode = TextExportMode.Text;
                    xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;

                    xrTemp.ExportToXlsx(stream, xlsxOptions);
                    stream.Seek(0, SeekOrigin.Begin);
                    byte[] breport = stream.ToArray();
                    return File(breport, "application/excel", "Probation List" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".xlsx");
                    #endregion Export
                }
                else
                {
                    result.Msg = "Error, No Item For Save";
                    result.Status = SystemFunction.process_Failed;
                    Json(new { result });
                }
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message + "";
                result.Status = SystemFunction.process_Failed;
            }
            return Json(new { result });
        }
        #endregion Fn

    }
}