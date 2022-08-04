using HumanCapitalManagement.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using HumanCapitalManagement.Service.eGreetingsService;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Models.eGreetings;
using HumanCapitalManagement.ViewModel.eGreetingsVM;
using HumanCapitalManagement.ViewModel.CommonVM;

namespace HumanCapitalManagement.Controllers.eGreetings
{
    public class eGreetingsController : BaseController
    {
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        private eGreetings_MainService _eGreetings_MainService;
        private eGreetings_DetailService _eGreetings_DetailService;
        private TM_eGreetings_PeroidService _TM_eGreetings_PeroidService;
        private TM_eGreetings_QuestionService _TM_eGreetings_QuestionService;
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public eGreetingsController(
            eGreetings_MainService eGreetings_MainService
            , TM_CandidatesService TM_CandidatesService
            , eGreetings_DetailService eGreetings_DetailService
            , TM_eGreetings_PeroidService TM_eGreetings_PeroidService
            , TM_eGreetings_QuestionService TM_eGreetings_QuestionService
            , MailContentService MailContentService)
        {
            _eGreetings_MainService = eGreetings_MainService;
            _eGreetings_DetailService = eGreetings_DetailService;
            _TM_eGreetings_PeroidService = TM_eGreetings_PeroidService;
            _TM_eGreetings_QuestionService = TM_eGreetings_QuestionService;
            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;
        }
        public class ForGroup
        {
            public string Count_All { get; set; }
            public string Count_Give { get; set; }
            public string Count_Recive { get; set; }
        }
        public class ForRank
        {
            public string RankID { get; set; }
            public string RankName { get; set; }
            public string RankPiority { get; set; }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult eGreetingsView()
        {
            var datefrom = SystemFunction.ConvertStringToDateTime("01-01-", "", "dd-MM-yyyy");
            var dateend = SystemFunction.ConvertStringToDateTime("25-02-", "", "dd-MM-yyyy");

            string winloginname = this.User.Identity.Name;
            string EmpUserID = winloginname.Substring(winloginname.IndexOf("\\") + 1);
            if (!string.IsNullOrEmpty(EmpUserID))
            {
                if (CGlobal.IsUserExpired())
                {
                    DataTable staff = wsHRis.getActiveStaffByUserID(EmpUserID);
                    // DataTable staff = wsHRis.getActiveStaffByUserID("zjaffer");
                    if (staff != null)
                    {
                        var CheckUser = staff.AsEnumerable().Where(w => w.Field<string>("EmpNo") == "").FirstOrDefault();
                        if (CheckUser != null)
                        {
                            var Login = HCMFunc.funcLogin(CheckUser.Field<string>("EmpNo") + "");
                        }
                        else
                        {
                            var Login = HCMFunc.funcLogin(staff.Rows[0].Field<string>("EmpNo") + "");
                        }
                    }
                }
            }
            //CGlobal.UserInfo.EmployeeNo = "95000023";

            veGreetings_Main_Return result = new veGreetings_Main_Return();

            try
            {
                List<TM_eGreetings_Peroid> getpr = new List<TM_eGreetings_Peroid>();

                getpr = _TM_eGreetings_PeroidService.GetDataForSelect().ToList();
                var myrank = wsHRis.getEmployeeInfoByEmpNo(CGlobal.UserInfo.EmployeeNo).AsEnumerable().FirstOrDefault().Field<int>("RankPriority").ToString();
                result.maindata = new veGreetings_Main_obj_save();
                result.maindata.image_path = getMyImage(CGlobal.UserInfo.EmployeeNo);

                if (getpr.Count != 0)
                {
                    var getdatanow = _eGreetings_MainService.FindByCondition(CGlobal.UserInfo.EmployeeNo, getpr.FirstOrDefault().Id);
                    var getdataemp = wsHRis.getEmployeeInfoByEmpNo(CGlobal.UserInfo.EmployeeNo).AsEnumerable().FirstOrDefault();

                    result.maindata.Emp_No = CGlobal.UserInfo.EmployeeNo;

                    result.maindata.End_Peroid = getpr.FirstOrDefault().End_Peroid.Value.ToString("yyyy-MM-dd");
                    result.maindata.receive = _eGreetings_DetailService.ContReceiveByEmpNo(CGlobal.UserInfo.EmployeeNo, getpr.FirstOrDefault().Id);
                    result.maindata.emp_name = getdataemp.Field<string>("EmpFullName");
                    result.maindata.emp_costcenter = getdataemp.Field<string>("UnitGroup");
                    if (getdatanow != null)
                    {
                        result.maindata.Id = getdatanow.Id.ToString();
                        result.maindata.Remaining_Rights = getdatanow.Remaining_Rights.ToString();

                        veGreetings_Detail_obj obj_detail = new veGreetings_Detail_obj();

                        var getset = new List<veGreetings_Detail_obj>();


                        if (CGlobal.UserInfo.EmployeeNo == "00001445" && DateTime.Now.Date >= datefrom && DateTime.Now.Date <= dateend)
                        {


                        }
                        else
                        {
                            var getdetail = _eGreetings_DetailService.FindByCondition(CGlobal.UserInfo.EmployeeNo, getdatanow.Id).ToList();
                            foreach (var ex in getdetail)
                            {
                                var sub = new veGreetings_Detail_obj();
                                sub.Id = ex.Id;
                                sub.Rank = ex.Rank;

                                sub.Group_Text = ex.TM_eGreetings_Question.Topic;
                                sub.Reason = ex.Reason;
                                sub.Active_Status = ex.Active_Status;
                                sub.Create_Date = ex.Create_Date;
                                sub.Create_User = ex.Create_User;
                                sub.Update_Date = ex.Update_Date;
                                sub.Update_User = ex.Update_User;
                                sub.eGreetings_Main_Id = ex.eGreetings_Main_Id;
                                sub.Img_Emp = getMyImage(ex.Emp_No);
                                sub.Show_Name = ex.Show_Name;

                                var getinfoempdetail = wsHRis.getEmployeeInfoByEmpNo(ex.Emp_No);
                                var getos = dbHr.TB_OutSource.FirstOrDefault(f => f.employeeno == ex.Emp_No);
                                if (getinfoempdetail == null)
                                {
                                    sub.Emp_Name = getos.employeename + " " + getos.employeesurname;
                                }
                                else
                                {
                                    sub.Emp_Name = getinfoempdetail.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");

                                }
                                sub.Emp_No = ex.Emp_No;


                                getset.Add(sub);
                            }
                        }

                        result.maindata.eGreetings_Detail = getset;


                    }
                    else
                    {
                        //if (myrank == "1" || myrank == "7")
                        //    result.maindata.Remaining_Rights = "2";
                        //else
                        result.maindata.Remaining_Rights = "5";
                    }

                    var getreason = _eGreetings_DetailService.GetReasonByEmpNo(CGlobal.UserInfo.EmployeeNo, getpr.FirstOrDefault().Id).ToList();

                    var getsetreason = new List<veGreetings_Detail_obj>();
                    foreach (var ex in getreason)
                    {
                        var sub = new veGreetings_Detail_obj();
                        sub.Group_Text = ex.TM_eGreetings_Question.Topic;
                        sub.Reason = ex.Reason;
                        sub.Emp_No = ex.eGreetings_Main.Emp_No;
                        sub.Show_Name = ex.Show_Name;

                        var getinfoempdetail = wsHRis.getEmployeeInfoByEmpNo(ex.Create_User);
                       
                        if (getinfoempdetail == null)
                        {
                            var getos = dbHr.TB_OutSource.FirstOrDefault(f => f.employeeno == ex.Create_User);
                            sub.Stamp_Name = getos.employeename + " " + getos.employeesurname;
                        }
                        else
                        {
                            sub.Stamp_Name = getinfoempdetail.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");

                        }
                        getsetreason.Add(sub);
                    }
                    result.maindata.eGreetings_Reason = getsetreason;
                }

                var getreasonforcer = _eGreetings_DetailService.GetReasonByEmpNoforcer(CGlobal.UserInfo.EmployeeNo).ToList();

                var getsetreasonforcer = new List<veGreetings_Detail_obj>();
                foreach (var ex in getreasonforcer)
                {
                    var sub = new veGreetings_Detail_obj();
                    sub.Group_Text = ex.TM_eGreetings_Question.Topic;
                    sub.Reason = ex.Reason;





                    getsetreasonforcer.Add(sub);
                }
                result.maindata.eGreetings_Certificate = getsetreasonforcer.OrderBy(o => o.Group_Text).ToList();

                //get question
                var getquestion = _TM_eGreetings_QuestionService.GetDataMainActive().ToList();
                List<veGreetings_Question_obj> lstdataquestion = new List<veGreetings_Question_obj>();
                foreach (var ex in getquestion)
                {
                    veGreetings_Question_obj data = new veGreetings_Question_obj();
                    data.Id = ex.Id;
                    if (ex.Id == 27)
                        data.Topic = "<span  data-toggle='tooltip' title='" + ex.Content + "' style='cursor: help;color:#0091DA; '>" + ex.Topic + "</span> ";
                    else if (ex.Id == 28)
                        data.Topic = "<span  data-toggle='tooltip' title='" + ex.Content + "' style='cursor: help;color:#00BAB3; '>" + ex.Topic + "</span> ";
                    else if (ex.Id == 29)
                        data.Topic = "<span  data-toggle='tooltip' title='" + ex.Content + "' style='cursor: help;color:#470A68; '>" + ex.Topic + "</span> ";
                    else if (ex.Id == 30)
                        data.Topic = "<span  data-toggle='tooltip' title='" + ex.Content + "' style='cursor: help;color:#005EB8; '>" + ex.Topic + "</span> ";
                    else if (ex.Id == 31)
                        data.Topic = "<span  data-toggle='tooltip' title='" + ex.Content + "' style='cursor: help;color:#00338D; '>" + ex.Topic + "</span> ";
                    else
                        data.Topic = "<span  data-toggle='tooltip' title='" + ex.Content + "' style='cursor: help;'>" + ex.Topic + "</span> ";

                    data.Group = ex.Group;

                    lstdataquestion.Add(data);
                }

                result.lstDataquestion = lstdataquestion;
            }
            catch (Exception ex) { }
            return View(result);
        }
        public ActionResult eGreetingsManageGive()
        {

            veGreetings_Main_Return result = new veGreetings_Main_Return();
            try
            {

            }
            catch (Exception ex) { }
            return View(result);
        }
        public ActionResult eGreetingsManageRecive()
        {

            veGreetings_Main_Return result = new veGreetings_Main_Return();
            try
            {

            }
            catch (Exception ex) { }
            return View(result);
        }
        [HttpPost]
        public ActionResult Save_Give_eGreetings(string main_id, string emp_no, string reason, string group, string showname)
        {
            veGreetings_Main_obj_save result = new veGreetings_Main_obj_save();
            var getMain = new eGreetings_Main();
            try
            {
                var datefrom = SystemFunction.ConvertStringToDateTime("01-02-", "", "dd-MM-yyyy");
                var dateend = SystemFunction.ConvertStringToDateTime("24-02-", "", "dd-MM-yyyy");

                eGreetings_Main maindata = new eGreetings_Main();
                if (String.IsNullOrEmpty(emp_no))
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Please fill in the staff information.";

                    return Json(new
                    {
                        result
                    });
                }
                else if (CGlobal.UserInfo.EmployeeNo == emp_no)
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "You can't give e-Greetings to Yourself.";

                    return Json(new
                    {
                        result
                    });
                }

                if (main_id == null)
                {
                    var getpr = _TM_eGreetings_PeroidService.GetDataForSelect().ToList();
                    maindata.Emp_No = CGlobal.UserInfo.EmployeeNo;
                    maindata.Active_Status = "Y";
                    maindata.Create_User = CGlobal.UserInfo.EmployeeNo;
                    maindata.Create_Date = DateTime.Now;
                    maindata.Update_User = CGlobal.UserInfo.EmployeeNo;
                    maindata.Update_Date = DateTime.Now;
                    maindata.TM_eGreetings_Peroid = getpr.FirstOrDefault();

                    //if (DateTime.Now.Date > datefrom && DateTime.Now.Date < dateend)
                    //{
                    //maindata.Remaining_Rights = 0;
                    //}
                    //else
                    //{
                    //if (getrankMe == "1" || getrankMe == "7")
                    //    maindata.Remaining_Rights = 1;
                    //else
                    maindata.Remaining_Rights = 9999;
                    //}
                }
                else
                {
                    getMain = _eGreetings_MainService.Find(SystemFunction.GetIntNullToZero(main_id));
                    maindata = new eGreetings_Main();
                    maindata = getMain;

                    maindata.Update_User = CGlobal.UserInfo.EmployeeNo;
                    maindata.Update_Date = DateTime.Now;
                    //if (DateTime.Now.Date > datefrom && DateTime.Now.Date < dateend)
                    //{
                    //    maindata.Remaining_Rights = 0;
                    //}
                    //else
                    //{
                    maindata.Remaining_Rights = getMain.Remaining_Rights - 1;
                    //}




                }

                var setnew = _eGreetings_MainService.CreateNewOrUpdateMain(maindata);
                eGreetings_Detail eGreetings_Detail = new eGreetings_Detail();
                if (setnew > 0)
                {
                    if (!String.IsNullOrEmpty(main_id))
                    {
                        eGreetings_Detail.eGreetings_Main_Id = maindata.Id;
                    }
                    else
                    {
                        var getmaxid = _eGreetings_MainService.GetDataForSelect().Max(m => m.Id);
                        eGreetings_Detail.eGreetings_Main_Id = getmaxid;
                    }

                }


                eGreetings_Detail.Emp_No = emp_no;
                eGreetings_Detail.TM_eGreetings_Question_Id = SystemFunction.GetIntNullToZero(group);
                eGreetings_Detail.Reason = reason;
                eGreetings_Detail.Rank = "0";

                eGreetings_Detail.Active_Status = "Y";
                eGreetings_Detail.Create_User = CGlobal.UserInfo.EmployeeNo;
                eGreetings_Detail.Create_Date = DateTime.Now;
                eGreetings_Detail.Update_User = CGlobal.UserInfo.EmployeeNo;
                eGreetings_Detail.Update_Date = DateTime.Now;
                eGreetings_Detail.Show_Name = showname == "True" ? "Y" : "N";

                var setdetailnew = _eGreetings_DetailService.CreateNewOrUpdate(eGreetings_Detail);
                if (setdetailnew > 0)
                {
                    string pathUrl = Url.Action("eGreetingsView", "eGreetings", null, Request.Url.Scheme);
                    var senttocol = send_mail("eGreetings To Colleague", emp_no, showname == "True" ? "K."+CGlobal.UserInfo.FullName : " from an anonymous colleague.", pathUrl, _TM_eGreetings_QuestionService.Find(SystemFunction.GetIntNullToZero(group)).Content, reason);
                    if (senttocol)
                    {
                        result.Status = SystemFunction.process_Success;
                        result.Msg = "Save Success.";
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Send E-mail Error.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Save Error.";
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
        public ActionResult Search_eGreetings_Give(string start, string end)
        {
            veGreetings_Main_Return result = new veGreetings_Main_Return();
            try
            {
                result.mainlstdata = new List<veGreetings_Main_obj>();
                result.mainlstdatarecive = new List<veGreetings_Main_obj>();

                var getde = _eGreetings_DetailService.GetDataForSelect().ToList();

                var conditon = getde.Where(w => w.eGreetings_Main.TM_eGreetings_Peroid.Start_Peroid.Value.Date >= SystemFunction.ConvertStringToDateTime(start, "", "dd MMM yyyy").Date
                && w.eGreetings_Main.TM_eGreetings_Peroid.End_Peroid.Value.Date <= SystemFunction.ConvertStringToDateTime(end, "", "dd MMM yyyy").Date).OrderBy(o => o.Emp_No);

                var group = conditon.GroupBy(g => g.eGreetings_Main.Emp_No).ToList();
                var i = 0;
                foreach (var x in group)
                {
                    i++;
                    //var fine = conditon.Where(w=> w.Emp_No == x.FirstOrDefault().eGreetings_Main.Emp_No).Count();
                    if (i > 135)
                    {
                        var sdfa = x.FirstOrDefault().eGreetings_Main;
                    }
                    var nObj = new veGreetings_Main_obj();
                    nObj.Emp_No = x.FirstOrDefault().eGreetings_Main.Emp_No;
                    nObj.Give = x.Count().ToString();

                    var getinfoempdetail = wsHRis.getEmployeeInfoByEmpNo(x.FirstOrDefault().eGreetings_Main.Emp_No);

                    if (getinfoempdetail == null)
                    {
                        var getos = dbHr.TB_OutSource.FirstOrDefault(f => f.employeeno == x.FirstOrDefault().eGreetings_Main.Emp_No);
                        nObj.Group = "IT Support";
                        nObj.Remark = "OutSource";
                        nObj.Emp_Name = getos.employeename + " " + getos.employeesurname;
                    }
                    else
                    {
                        nObj.Emp_Name = getinfoempdetail.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");
                        nObj.Group = getinfoempdetail.AsEnumerable().FirstOrDefault().Field<string>("UnitGroup");
                        nObj.Remark = getinfoempdetail.AsEnumerable().FirstOrDefault().Field<string>("Rank");
                    }
                    result.mainlstdata.Add(nObj);
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
        public ActionResult Search_eGreetings_Recive(string start, string end)
        {
            veGreetings_Main_Return result = new veGreetings_Main_Return();
            try
            {
                result.mainlstdata = new List<veGreetings_Main_obj>();
                result.mainlstdatarecive = new List<veGreetings_Main_obj>();

                var getde = _eGreetings_DetailService.GetDataForSelect().ToList();

                var conditon = getde.Where(w => w.eGreetings_Main.TM_eGreetings_Peroid.Start_Peroid.Value.Date >= SystemFunction.ConvertStringToDateTime(start, "", "dd MMM yyyy").Date
                && w.eGreetings_Main.TM_eGreetings_Peroid.End_Peroid.Value.Date <= SystemFunction.ConvertStringToDateTime(end, "", "dd MMM yyyy").Date).OrderBy(o => o.Emp_No);

                var group = conditon.GroupBy(g => g.eGreetings_Main.Emp_No).ToList();

                var grouprecive = conditon.GroupBy(g => g.Emp_No).ToList();
                foreach (var x in grouprecive)
                {

                    //var getmain = conditon.Where(w=> w.eGreetings_Main.Emp_No == x.Key).ToList();

                    //var countgive = 0;
                    //if (getmain != null && getmain.Count != 0)
                    //{
                    //    countgive = getmain.Count();
                    //}

                    var nObj = new veGreetings_Main_obj();
                    //nObj.Id = ge.FirstOrDefault().Id;
                    nObj.Emp_No = x.Key;
                    //nObj.Give = countgive.ToString();
                    nObj.Recive = x.Count().ToString();
                    var getinfoempdetail = wsHRis.getEmployeeInfoByEmpNo(x.Key);
                    if (getinfoempdetail == null || x.Key.Contains("OS"))
                    {
                        IQueryable<TB_OutSource> getos = dbHr.TB_OutSource.Where(f => f.employeeno == x.Key);
                        nObj.Group = "IT Support";
                        nObj.Remark = "OutSource";
                        nObj.Emp_Name = getos.FirstOrDefault().employeename + " " + getos.FirstOrDefault().employeesurname;
                    }
                    else
                    {
                        nObj.Emp_Name = getinfoempdetail.AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");
                        nObj.Group = getinfoempdetail.AsEnumerable().FirstOrDefault().Field<string>("UnitGroup");
                        nObj.Remark = getinfoempdetail.AsEnumerable().FirstOrDefault().Field<string>("Rank");
                    }




                    result.mainlstdatarecive.Add(nObj);

                }


                //result.MainUse = group.Count().ToString();
                //result.DetailUse = conditon.GroupBy(g=> g.Emp_No).Count().ToString();

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



        public string getMyImage(string empNo)
        {

            image_obj result = new image_obj();

            New_HRISEntities dbHr = new New_HRISEntities();
            var GetPic = dbHr.vw_StaffPhoto_FileName.Where(w => w.Employeeno == empNo).FirstOrDefault();
            try
            {

                string sPicturePath = System.Web.Configuration.WebConfigurationManager.AppSettings["PathPic"];
                var absolutePath = HttpContext.Server.MapPath("~/Image/ic_noimage.png");
                var patha = System.Web.Configuration.WebConfigurationManager.AppSettings["PathPic"];
                if (GetPic != null)
                {
                    sPicturePath = (sPicturePath + GetPic.PhotoName + ".jpg").Replace("/", "\\");
                }
                else
                {
                    sPicturePath = absolutePath;
                }

                result.path = sPicturePath;

            }
            catch (Exception ex)
            {
            }


            return result.path;
        }

        public class image_obj
        {
            public string path { get; set; }
        }

        [HttpPost]
        public ActionResult getImageEmp(string empNo)
        {

            image_obj result = new image_obj();

            New_HRISEntities dbHr = new New_HRISEntities();
            var GetPic = dbHr.vw_StaffPhoto_FileName.Where(w => w.Employeeno == empNo).FirstOrDefault();
            try
            {

                string sPicturePath = System.Web.Configuration.WebConfigurationManager.AppSettings["PathPic"];
                var absolutePath = HttpContext.Server.MapPath("~/Image/ic_noimage.png");
                var patha = System.Web.Configuration.WebConfigurationManager.AppSettings["PathPic"];
                if (GetPic != null)
                {
                    sPicturePath = (sPicturePath + GetPic.PhotoName + ".jpg").Replace("/", "\\");
                }
                else
                {
                    sPicturePath = absolutePath;
                }

                result.path = sPicturePath;

            }
            catch (Exception ex)
            {
            }


            return Json(new { result });
        }

        public bool send_mail(string mailcontent_name, string reciepver, string sender, string genlink, string param1 = "", string param2 = "", string param3 = "")
        {
            bool returnbool = true;
            try
            {
                string msg = "";
                var getmeilrec = "";
                var getnamerec = "";

                if (!reciepver.Contains("OS"))
                {
                    var abc = wsHRis.getActiveStaffByEmpNo(reciepver);
                    getmeilrec = abc.Rows[0].Field<string>("Email");
                    getnamerec = "K."+abc.Rows[0].Field<string>("EmpFullName");
                }
                else
                {
                    var abc = dbHr.TB_OutSource.FirstOrDefault(s => s.employeeno == reciepver);
                    getmeilrec = abc.email;
                    getnamerec = "K." +abc.employeename + " " + abc.employeesurname;
                }
                var MailRequest = _MailContentService.GetMailContent(mailcontent_name, "Y").FirstOrDefault();
                string keyword = "";


                //switch (abc.Rows[0].Field<string>("CompanyCode"))
                //{
                //    case "KPMG Lao":
                //        keyword = "Bok Huk";
                //        break;
                //    case "KPMG MM":
                //        keyword = "Chit Kyaung Pyaw";
                //        break;
                //    default:
                //        keyword = "Bok Ruk";
                //        break;
                //}

                if (true) { }

                string sContent = MailRequest.content;
                string from = MailRequest.sender_name;
                string subject = MailRequest.mail_header;

                sContent = (sContent + "").Replace("$sender$", sender);
                sContent = (sContent + "").Replace("$reciver$", getnamerec);
                sContent = (sContent + "").Replace("$header$", param1);
                sContent = (sContent + "").Replace("$reason$", param2);

                //string getemailcc = "";
                //if (!String.IsNullOrEmpty(param3))
                //{
                //    var getcc = sQuery.Where(w => w.Employeeno == param3).FirstOrDefault();
                //    getemailcc = getcc.Email;
                //}

                var objMail = new vObjectMail_Send();
                objMail.mail_from = "hcmthailand@kpmg.co.th";
                objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                objMail.mail_to = getmeilrec;
                objMail.mail_cc = "";
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

    }
}