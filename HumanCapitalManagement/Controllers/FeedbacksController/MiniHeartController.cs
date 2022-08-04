using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.ViewModel.Feedbacks.MiniHeart_Main;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using HumanCapitalManagement.Models.Probation;
using HumanCapitalManagement.Models._360Feedback.MiniHeart;
using HumanCapitalManagement.ViewModel.Feedbacks.vMiniHeart_Detail;

namespace HumanCapitalManagement.Controllers.FeedbacksController
{
    public class MiniHeartController : BaseController
    {
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        private MiniHeart_MainService _MiniHeart_MainService;
        private MiniHeart_DetailService _MiniHeart_DetailService;
        private TM_MiniHeart_PeroidService _TM_MiniHeart_PeroidService;
        private TM_MiniHeart_QuestionService _TM_MiniHeart_QuestionService;
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public MiniHeartController(
            MiniHeart_MainService MiniHeart_MainService
            , TM_CandidatesService TM_CandidatesService
            , MiniHeart_DetailService MiniHeart_DetailService
            , TM_MiniHeart_PeroidService TM_MiniHeart_PeroidService
            , TM_MiniHeart_QuestionService TM_MiniHeart_QuestionService
            , MailContentService MailContentService)
        {
            _MiniHeart_MainService = MiniHeart_MainService;
            _MiniHeart_DetailService = MiniHeart_DetailService;
            _TM_MiniHeart_PeroidService = TM_MiniHeart_PeroidService;
            _TM_MiniHeart_QuestionService = TM_MiniHeart_QuestionService;
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
        public ActionResult MiniHeartView()
        {
            var datefrom = SystemFunction.ConvertStringToDateTime("10-02-2020", "", "dd-MM-yyyy");
            var dateend = SystemFunction.ConvertStringToDateTime("29-02-2020", "", "dd-MM-yyyy");

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

            vMiniHeart_Main_Return result = new vMiniHeart_Main_Return();

            try
            {
                List<TM_MiniHeart_Peroid> getpr = new List<TM_MiniHeart_Peroid>();

                getpr = _TM_MiniHeart_PeroidService.GetDataForSelect().ToList();
                var myrank = wsHRis.getEmployeeInfoByEmpNo(CGlobal.UserInfo.EmployeeNo).AsEnumerable().FirstOrDefault().Field<int>("RankPriority").ToString();



                if (getpr != null && getpr.Count() != 0)
                {
                    var getdatanow = _MiniHeart_MainService.FindByCondition(CGlobal.UserInfo.EmployeeNo, getpr.FirstOrDefault().Id);
                   var getdataemp =  wsHRis.getEmployeeInfoByEmpNo(CGlobal.UserInfo.EmployeeNo).AsEnumerable().FirstOrDefault();
                    result.maindata = new vMiniHeart_Main_obj_save();
                    result.maindata.Emp_No = CGlobal.UserInfo.EmployeeNo;
                    result.maindata.image_path = getMyImage(CGlobal.UserInfo.EmployeeNo);
                    result.maindata.End_Peroid = getpr.FirstOrDefault().End_Peroid.Value.ToString("yyyy-MM-dd");
                    result.maindata.receive = _MiniHeart_DetailService.ContReceiveByEmpNo(CGlobal.UserInfo.EmployeeNo, getpr.FirstOrDefault().Id);
                    result.maindata.emp_name = getdataemp.Field<string>("EmpFullName");
                    result.maindata.emp_costcenter = getdataemp.Field<string>("UnitGroup");
                    if (getdatanow != null)
                    {
                        result.maindata.Id = getdatanow.Id.ToString();
                        result.maindata.Remaining_Rights = getdatanow.Remaining_Rights.ToString();

                        vMiniHeart_Detail_obj obj_detail = new vMiniHeart_Detail_obj();

                        var getset = new List<vMiniHeart_Detail_obj>();


                        if (CGlobal.UserInfo.EmployeeNo == "00001445" && DateTime.Now.Date >= datefrom && DateTime.Now.Date <= dateend)
                        {


                        }
                        else
                        {
                            var getdetail = _MiniHeart_DetailService.FindByCondition(CGlobal.UserInfo.EmployeeNo, getdatanow.Id).ToList();
                            foreach (var ex in getdetail)
                            {
                                var sub = new vMiniHeart_Detail_obj();
                                sub.Id = ex.Id;
                                sub.Rank = ex.Rank;
                                sub.Emp_No = ex.Emp_No;
                                sub.Group_Text = ex.TM_MiniHeart_Question.Topic;
                                sub.Reason = ex.Reason;
                                sub.Active_Status = ex.Active_Status;
                                sub.Create_Date = ex.Create_Date;
                                sub.Create_User = ex.Create_User;
                                sub.Update_Date = ex.Update_Date;
                                sub.Update_User = ex.Update_User;
                                sub.MiniHeart_Main_Id = ex.MiniHeart_Main_Id;
                                sub.Img_Emp = getMyImage(ex.Emp_No);

                                var getinfoempdetail = wsHRis.getEmployeeInfoByEmpNo(ex.Emp_No).AsEnumerable().FirstOrDefault();

                                var theyrank = getinfoempdetail.Field<int>("RankPriority").ToString();
                                sub.Emp_Name = getinfoempdetail.Field<string>("EmpFullName");

                                if (SystemFunction.GetIntNullToZero(myrank) > SystemFunction.GetIntNullToZero(theyrank))
                                {
                                    sub.Rank_Level = "Hight";
                                }
                                else if (SystemFunction.GetIntNullToZero(myrank) == SystemFunction.GetIntNullToZero(theyrank))
                                {
                                    sub.Rank_Level = "Same";
                                }
                                else if (SystemFunction.GetIntNullToZero(myrank) < SystemFunction.GetIntNullToZero(theyrank))
                                {
                                    sub.Rank_Level = "Lower";
                                }

                                getset.Add(sub);
                            }
                        }

                        result.maindata.MiniHeart_Detail = getset;


                    }
                    else
                    {
                        //if (myrank == "1" || myrank == "7")
                        //    result.maindata.Remaining_Rights = "2";
                        //else
                        result.maindata.Remaining_Rights = "5";
                    }

                    var getreason = _MiniHeart_DetailService.GetReasonByEmpNo(CGlobal.UserInfo.EmployeeNo, getpr.FirstOrDefault().Id).ToList();

                    var getsetreason = new List<vMiniHeart_Detail_obj>();
                    foreach (var ex in getreason)
                    {
                        var sub = new vMiniHeart_Detail_obj();
                        sub.Group_Text = ex.TM_MiniHeart_Question.Topic;
                        sub.Reason = ex.Reason;
                        sub.Emp_No = ex.MiniHeart_Main.Emp_No;

                        getsetreason.Add(sub);
                    }
                    result.maindata.MiniHeart_Reason = getsetreason;
                }

                var getreasonforcer = _MiniHeart_DetailService.GetReasonByEmpNoforcer(CGlobal.UserInfo.EmployeeNo).ToList();

                var getsetreasonforcer = new List<vMiniHeart_Detail_obj>();
                foreach (var ex in getreasonforcer)
                {
                    var sub = new vMiniHeart_Detail_obj();
                    sub.Group_Text = ex.TM_MiniHeart_Question.Topic;
                    sub.Reason = ex.Reason;

                    var getinfoempdetail = wsHRis.getEmployeeInfoByEmpNo(ex.MiniHeart_Main.Emp_No).AsEnumerable().FirstOrDefault();

                    var theyrank = getinfoempdetail.Field<int>("RankPriority").ToString();

                    if (SystemFunction.GetIntNullToZero(myrank) > SystemFunction.GetIntNullToZero(theyrank))
                    {
                        sub.Rank_Level = "Hight";
                    }
                    else if (SystemFunction.GetIntNullToZero(myrank) == SystemFunction.GetIntNullToZero(theyrank))
                    {
                        sub.Rank_Level = "Same";
                    }
                    else if (SystemFunction.GetIntNullToZero(myrank) < SystemFunction.GetIntNullToZero(theyrank))
                    {
                        sub.Rank_Level = "Lower";
                    }

                    getsetreasonforcer.Add(sub);
                }
                result.maindata.MiniHeart_Certificate = getsetreasonforcer.OrderBy(o => o.Group_Text).ToList();

                //get question
                var getquestion = _TM_MiniHeart_QuestionService.GetDataMainActive().ToList();
                List<vMiniHeart_Question_obj> lstdataquestion = new List<vMiniHeart_Question_obj>();
                foreach (var ex in getquestion)
                {
                    vMiniHeart_Question_obj data = new vMiniHeart_Question_obj();
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
        public ActionResult MiniHeartManageGive()
        {

            vMiniHeart_Main_Return result = new vMiniHeart_Main_Return();
            try
            {

            }
            catch (Exception ex) { }
            return View(result);
        }
        public ActionResult MiniHeartManageRecive()
        {

            vMiniHeart_Main_Return result = new vMiniHeart_Main_Return();
            try
            {

            }
            catch (Exception ex) { }
            return View(result);
        }
        [HttpPost]
        public ActionResult Save_Give_MiniHeart(string main_id, string emp_no, string reason, string group)
        {
            vMiniHeart_Main_obj_save result = new vMiniHeart_Main_obj_save();
            var getMain = new MiniHeart_Main();
            try
            {
                var datefrom = SystemFunction.ConvertStringToDateTime("10-02-2020", "", "dd-MM-yyyy");
                var dateend = SystemFunction.ConvertStringToDateTime("29-02-2020", "", "dd-MM-yyyy");

                MiniHeart_Main maindata = new MiniHeart_Main();
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
                    result.Msg = "You can't give Mini Heart to Yourself.";

                    return Json(new
                    {
                        result
                    });
                }
                var getrankgiveemp = wsHRis.getEmployeeInfoByEmpNo(emp_no).AsEnumerable().FirstOrDefault().Field<int>("RankPriority").ToString();

                var getrankalls = wsHRis.getAllRank().AsEnumerable().Select(s => new ForRank()
                {
                    RankID = s.Field<int>("RankID").ToString(),
                    RankName = s.Field<string>("RankName"),
                    RankPiority = s.Field<int>("RankPiority").ToString()
                }).ToList();


                var getrankMe = getrankalls.Where(w => w.RankID.ToString() == CGlobal.UserInfo.RankID).FirstOrDefault().RankPiority;


                var check_rank = 0;

                if (main_id == null)
                {
                    var getpr = _TM_MiniHeart_PeroidService.GetDataForSelect().ToList();
                    maindata.Emp_No = CGlobal.UserInfo.EmployeeNo;
                    maindata.Active_Status = "Y";
                    maindata.Create_User = CGlobal.UserInfo.EmployeeNo;
                    maindata.Create_Date = DateTime.Now;
                    maindata.Update_User = CGlobal.UserInfo.EmployeeNo;
                    maindata.Update_Date = DateTime.Now;
                    maindata.TM_MiniHeart_Peroid = getpr.FirstOrDefault();
                    if (DateTime.Now.Date >= datefrom && DateTime.Now.Date <= dateend)
                    {
                        maindata.Remaining_Rights = 0;
                    }
                    else
                    {
                        //if (getrankMe == "1" || getrankMe == "7")
                        //    maindata.Remaining_Rights = 1;
                        //else
                        maindata.Remaining_Rights = 4;
                    }
                }
                else
                {
                    getMain = _MiniHeart_MainService.Find(SystemFunction.GetIntNullToZero(main_id));
                    maindata = new MiniHeart_Main();
                    maindata = getMain;

                    maindata.Update_User = CGlobal.UserInfo.EmployeeNo;
                    maindata.Update_Date = DateTime.Now;
                    if (DateTime.Now.Date >= datefrom && DateTime.Now.Date <= dateend)
                    {
                        maindata.Remaining_Rights = 0;
                    }
                    else
                    {
                        maindata.Remaining_Rights = getMain.Remaining_Rights - 1;
                    }

                    if (SystemFunction.GetNumberNullToZero(getrankMe) > SystemFunction.GetNumberNullToZero(getrankgiveemp))
                    {

                        check_rank = _MiniHeart_DetailService.FindByCondition(CGlobal.UserInfo.EmployeeNo, maindata.Id)
                       .Where(w => SystemFunction.GetNumberNullToZero(getrankMe) > SystemFunction.GetNumberNullToZero(w.Rank)
                       )
                       .Count();
                    }
                    else if (SystemFunction.GetNumberNullToZero(getrankMe) == SystemFunction.GetNumberNullToZero(getrankgiveemp))
                    {
                        check_rank = _MiniHeart_DetailService.FindByCondition(CGlobal.UserInfo.EmployeeNo, maindata.Id)
                    .Where(w => SystemFunction.GetNumberNullToZero(getrankMe) == SystemFunction.GetNumberNullToZero(w.Rank)
                    )
                    .Count();
                    }
                    else if (SystemFunction.GetNumberNullToZero(getrankMe) < SystemFunction.GetNumberNullToZero(getrankgiveemp))
                    {
                        check_rank = _MiniHeart_DetailService.FindByCondition(CGlobal.UserInfo.EmployeeNo, maindata.Id)
                     .Where(w => SystemFunction.GetNumberNullToZero(getrankMe) < SystemFunction.GetNumberNullToZero(w.Rank)
                     )
                     .Count();
                    }

                    check_rank = 0;

                    //check_rank = _MiniHeart_DetailService.FindByCondition(CGlobal.UserInfo.EmployeeNo, maindata.Id)
                    //    .Where(w => SystemFunction.GetNumberNullToZero(w.Rank) <= SystemFunction.GetNumberNullToZero(getrankgiveemp)
                    //  || SystemFunction.GetNumberNullToZero(w.Rank) == SystemFunction.GetNumberNullToZero(getrankgiveemp)
                    //  || SystemFunction.GetNumberNullToZero(w.Rank) >= SystemFunction.GetNumberNullToZero(getrankgiveemp)
                    //    )
                    //.Count();


                }
                if (DateTime.Now.Date >= datefrom && DateTime.Now.Date <= dateend)
                {
                    check_rank = 0;
                }
                if (check_rank < 1)
                {
                    var getdup = _MiniHeart_DetailService.GetDataForSelect().Where(w => w.Emp_No == emp_no && w.Active_Status == "Y" && w.MiniHeart_Main == getMain).ToList();
                    if (getdup.Count() >= 1)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Cannot give a mini-heart for someone who has already given it.";

                        return Json(new
                        {
                            result
                        });
                    }
                    var setnew = _MiniHeart_MainService.CreateNewOrUpdateMain(maindata);
                    MiniHeart_Detail miniHeart_Detail = new MiniHeart_Detail();
                    if (setnew > 0)
                    {
                        if (!String.IsNullOrEmpty(main_id))
                        {
                            miniHeart_Detail.MiniHeart_Main_Id = maindata.Id;
                        }
                        else
                        {
                            var getmaxid = _MiniHeart_MainService.GetDataForSelect().Max(m => m.Id);
                            miniHeart_Detail.MiniHeart_Main_Id = getmaxid;
                        }

                    }


                    miniHeart_Detail.Emp_No = emp_no;
                    miniHeart_Detail.TM_MiniHeart_Question_Id = SystemFunction.GetIntNullToZero(group);
                    miniHeart_Detail.Reason = reason;
                    miniHeart_Detail.Rank = getrankgiveemp;

                    miniHeart_Detail.Active_Status = "Y";
                    miniHeart_Detail.Create_User = CGlobal.UserInfo.EmployeeNo;
                    miniHeart_Detail.Create_Date = DateTime.Now;
                    miniHeart_Detail.Update_User = CGlobal.UserInfo.EmployeeNo;
                    miniHeart_Detail.Update_Date = DateTime.Now;

                    var setdetailnew = _MiniHeart_DetailService.CreateNewOrUpdate(miniHeart_Detail);
                    if (setdetailnew > 0)
                    {
                        string pathUrl = Url.Action("MiniHeartView", "MiniHeart", null, Request.Url.Scheme);
                        var senttocol = send_mail("MiniHeart To Colleague", emp_no, "", pathUrl);
                        result.Status = SystemFunction.process_Success;
                        result.Msg = "Save Success.";
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Save Error.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "You can give one mini heart for each rank tier only ";

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
        public ActionResult Search_MiniHeart_Give(string start, string end)
        {
            vMiniHeart_Main_Return result = new vMiniHeart_Main_Return();
            try
            {
                result.mainlstdata = new List<vMiniHeart_Main_obj>();
                result.mainlstdatarecive = new List<vMiniHeart_Main_obj>();

                var getde = _MiniHeart_DetailService.GetDataForSelect().ToList();

                var conditon = getde.Where(w => w.MiniHeart_Main.TM_MiniHeart_Peroid.Start_Peroid.Value.Date >= SystemFunction.ConvertStringToDateTime(start, "", "dd MMM yyyy").Date
                && w.MiniHeart_Main.TM_MiniHeart_Peroid.End_Peroid.Value.Date <= SystemFunction.ConvertStringToDateTime(end, "", "dd MMM yyyy").Date).OrderBy(o => o.Emp_No);

                var group = conditon.GroupBy(g => g.MiniHeart_Main.Emp_No).ToList();

                foreach (var x in group)
                {

                    //var fine = conditon.Where(w=> w.Emp_No == x.FirstOrDefault().MiniHeart_Main.Emp_No).Count();

                    var nObj = new vMiniHeart_Main_obj();
                    nObj.Emp_No = x.FirstOrDefault().MiniHeart_Main.Emp_No;
                    nObj.Give = x.Count().ToString();
                    var getinfo = wsHRis.getEmployeeInfoByEmpNo(x.FirstOrDefault().MiniHeart_Main.Emp_No).AsEnumerable().FirstOrDefault();
                    nObj.Emp_Name = getinfo.Field<string>("EmpFullName");
                    nObj.Group = getinfo.Field<string>("UnitGroup");
                    nObj.Remark = getinfo.Field<string>("Rank");

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
        public ActionResult Search_MiniHeart_Recive(string start, string end)
        {
            vMiniHeart_Main_Return result = new vMiniHeart_Main_Return();
            try
            {
                result.mainlstdata = new List<vMiniHeart_Main_obj>();
                result.mainlstdatarecive = new List<vMiniHeart_Main_obj>();

                var getde = _MiniHeart_DetailService.GetDataForSelect().ToList();

                var conditon = getde.Where(w => w.MiniHeart_Main.TM_MiniHeart_Peroid.Start_Peroid.Value.Date >= SystemFunction.ConvertStringToDateTime(start, "", "dd MMM yyyy").Date
                && w.MiniHeart_Main.TM_MiniHeart_Peroid.End_Peroid.Value.Date <= SystemFunction.ConvertStringToDateTime(end, "", "dd MMM yyyy").Date).OrderBy(o => o.Emp_No);

                var group = conditon.GroupBy(g => g.MiniHeart_Main.Emp_No).ToList();

                var grouprecive = conditon.GroupBy(g => g.Emp_No).ToList();
                foreach (var x in grouprecive)
                {

                    //var getmain = conditon.Where(w=> w.MiniHeart_Main.Emp_No == x.Key).ToList();

                    //var countgive = 0;
                    //if (getmain != null && getmain.Count != 0)
                    //{
                    //    countgive = getmain.Count();
                    //}

                    var nObj = new vMiniHeart_Main_obj();
                    //nObj.Id = ge.FirstOrDefault().Id;
                    nObj.Emp_No = x.Key;
                    //nObj.Give = countgive.ToString();
                    nObj.Recive = x.Count().ToString();
                    var getinfo = wsHRis.getEmployeeInfoByEmpNo(x.Key).AsEnumerable().FirstOrDefault();
                    nObj.Emp_Name = getinfo.Field<string>("EmpFullName");
                    nObj.Group = getinfo.Field<string>("UnitGroup");
                    nObj.Remark = getinfo.Field<string>("Rank");




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
                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
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
                sContent = (sContent + "").Replace("$reciepver", getbyno.Employeename + " " + getbyno.Employeesurname);

                string getemailcc = "";
                if (!String.IsNullOrEmpty(param3))
                {
                    var getcc = sQuery.Where(w => w.Employeeno == param3).FirstOrDefault();
                    getemailcc = getcc.Email;
                }

                var objMail = new vObjectMail_Send();
                objMail.mail_from = "hcmthailand@kpmg.co.th";
                objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                objMail.mail_to = getbyno.Email;
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