using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using HumanCapitalManagement.ViewModel.Feedbacks.MiniHeart_Main2021;
using HumanCapitalManagement.Models._360Feedback.MiniHeart2021;
using HumanCapitalManagement.ViewModel.Feedbacks.vMiniHeart_Detail2021;

namespace HumanCapitalManagement.Controllers.FeedbacksController
{
    public class MiniHeart2021Controller : BaseController
    {
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        private MiniHeart_Main2021Service _MiniHeart_Main2021Service;
        private MiniHeart_Detail2021Service _MiniHeart_Detail2021Service;
        private TM_MiniHeart_Peroid2021Service _TM_MiniHeart_Peroid2021Service;
        private TM_MiniHeart_Question2021Service _TM_MiniHeart_Question2021Service;
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public MiniHeart2021Controller(
            MiniHeart_Main2021Service MiniHeart_Main2021Service
            , TM_CandidatesService TM_CandidatesService
            , MiniHeart_Detail2021Service MiniHeart_Detail2021Service
            , TM_MiniHeart_Peroid2021Service TM_MiniHeart_Peroid2021Service
            , TM_MiniHeart_Question2021Service TM_MiniHeart_Question2021Service
            , MailContentService MailContentService)
        {
            _MiniHeart_Main2021Service = MiniHeart_Main2021Service;
            _MiniHeart_Detail2021Service = MiniHeart_Detail2021Service;
            _TM_MiniHeart_Peroid2021Service = TM_MiniHeart_Peroid2021Service;
            _TM_MiniHeart_Question2021Service = TM_MiniHeart_Question2021Service;
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
        public ActionResult MiniHeartView2021()
        {
            var datefrom = SystemFunction.ConvertStringToDateTime("01-01-2021", "", "dd-MM-yyyy");
            var dateend = SystemFunction.ConvertStringToDateTime("25-02-2021", "", "dd-MM-yyyy");

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

            vMiniHeart_Main2021_Return result = new vMiniHeart_Main2021_Return();

            try
            {
                List<TM_MiniHeart_Peroid2021> getpr = new List<TM_MiniHeart_Peroid2021>();

                getpr = _TM_MiniHeart_Peroid2021Service.GetDataForSelect().ToList();
                var myrank = wsHRis.getEmployeeInfoByEmpNo(CGlobal.UserInfo.EmployeeNo).AsEnumerable().FirstOrDefault().Field<int>("RankPriority").ToString();
                result.maindata = new vMiniHeart_Main2021_obj_save();
                result.maindata.image_path = getMyImage(CGlobal.UserInfo.EmployeeNo);

                if (getpr.Count != 0)
                {
                    var getdatanow = _MiniHeart_Main2021Service.FindByCondition(CGlobal.UserInfo.EmployeeNo, getpr.FirstOrDefault().Id);
                    var getdataemp = wsHRis.getEmployeeInfoByEmpNo(CGlobal.UserInfo.EmployeeNo).AsEnumerable().FirstOrDefault();

                    result.maindata.Emp_No = CGlobal.UserInfo.EmployeeNo;

                    result.maindata.End_Peroid = getpr.FirstOrDefault().End_Peroid.Value.ToString("yyyy-MM-dd");
                    result.maindata.receive = _MiniHeart_Detail2021Service.ContReceiveByEmpNo(CGlobal.UserInfo.EmployeeNo, getpr.FirstOrDefault().Id);
                    result.maindata.emp_name = getdataemp.Field<string>("EmpFullName");
                    result.maindata.emp_costcenter = getdataemp.Field<string>("UnitGroup");
                    if (getdatanow != null)
                    {
                        result.maindata.Id = getdatanow.Id.ToString();
                        result.maindata.Remaining_Rights = getdatanow.Remaining_Rights.ToString();

                        vMiniHeart_Detail2021_obj obj_detail = new vMiniHeart_Detail2021_obj();

                        var getset = new List<vMiniHeart_Detail2021_obj>();


                        if (CGlobal.UserInfo.EmployeeNo == "00001445" && DateTime.Now.Date >= datefrom && DateTime.Now.Date <= dateend)
                        {


                        }
                        else
                        {
                            var getdetail = _MiniHeart_Detail2021Service.FindByCondition(CGlobal.UserInfo.EmployeeNo, getdatanow.Id).ToList();
                            foreach (var ex in getdetail)
                            {
                                var sub = new vMiniHeart_Detail2021_obj();
                                sub.Id = ex.Id;
                                sub.Rank = ex.Rank;

                                sub.Group_Text = ex.TM_MiniHeart_Question.Topic;
                                sub.Reason = ex.Reason;
                                sub.Active_Status = ex.Active_Status;
                                sub.Create_Date = ex.Create_Date;
                                sub.Create_User = ex.Create_User;
                                sub.Update_Date = ex.Update_Date;
                                sub.Update_User = ex.Update_User;
                                sub.MiniHeart_Main_Id = ex.MiniHeart_Main_Id;
                                sub.Img_Emp = getMyImage(ex.Emp_No);
                                sub.Show_Name = ex.Show_Name;

                                var getinfoempdetail = wsHRis.getEmployeeInfoByEmpNo(ex.Emp_No).AsEnumerable().FirstOrDefault();

                                var theyrank = getinfoempdetail.Field<int>("RankPriority").ToString();
                                if (ex.Show_Name == "Y")
                                {
                                    sub.Emp_Name = getinfoempdetail.Field<string>("EmpFullName");
                                    sub.Emp_No = ex.Emp_No;
                                }

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

                    var getreason = _MiniHeart_Detail2021Service.GetReasonByEmpNo(CGlobal.UserInfo.EmployeeNo, getpr.FirstOrDefault().Id).ToList();

                    var getsetreason = new List<vMiniHeart_Detail2021_obj>();
                    foreach (var ex in getreason)
                    {
                        var sub = new vMiniHeart_Detail2021_obj();
                        sub.Group_Text = ex.TM_MiniHeart_Question.Topic;
                        sub.Reason = ex.Reason;
                        sub.Emp_No = ex.MiniHeart_Main.Emp_No;
                        sub.Show_Name = ex.Show_Name;
                        sub.Stamp_Name = wsHRis.getEmployeeInfoByEmpNo(ex.MiniHeart_Main.Emp_No).AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");


                        getsetreason.Add(sub);
                    }
                    result.maindata.MiniHeart_Reason = getsetreason;
                }

                var getreasonforcer = _MiniHeart_Detail2021Service.GetReasonByEmpNoforcer(CGlobal.UserInfo.EmployeeNo).ToList();

                var getsetreasonforcer = new List<vMiniHeart_Detail2021_obj>();
                foreach (var ex in getreasonforcer)
                {
                    var sub = new vMiniHeart_Detail2021_obj();
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
                var getquestion = _TM_MiniHeart_Question2021Service.GetDataMainActive().ToList();
                List<vMiniHeart_Question2021_obj> lstdataquestion = new List<vMiniHeart_Question2021_obj>();
                foreach (var ex in getquestion)
                {
                    vMiniHeart_Question2021_obj data = new vMiniHeart_Question2021_obj();
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
        public ActionResult MiniHeartManageGive2021()
        {

            vMiniHeart_Main2021_Return result = new vMiniHeart_Main2021_Return();
            try
            {

            }
            catch (Exception ex) { }
            return View(result);
        }
        public ActionResult MiniHeartManageRecive2021()
        {

            vMiniHeart_Main2021_Return result = new vMiniHeart_Main2021_Return();
            try
            {

            }
            catch (Exception ex) { }
            return View(result);
        }
        [HttpPost]
        public ActionResult Save_Give_MiniHeart2021(string main_id, string emp_no, string reason, string group, string showname)
        {
            vMiniHeart_Main2021_obj_save result = new vMiniHeart_Main2021_obj_save();
            var getMain = new MiniHeart_Main2021();
            try
            {
                var datefrom = SystemFunction.ConvertStringToDateTime("01-02-2021", "", "dd-MM-yyyy");
                var dateend = SystemFunction.ConvertStringToDateTime("24-02-2021", "", "dd-MM-yyyy");

                MiniHeart_Main2021 maindata = new MiniHeart_Main2021();
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
                    var getpr = _TM_MiniHeart_Peroid2021Service.GetDataForSelect().ToList();
                    maindata.Emp_No = CGlobal.UserInfo.EmployeeNo;
                    maindata.Active_Status = "Y";
                    maindata.Create_User = CGlobal.UserInfo.EmployeeNo;
                    maindata.Create_Date = DateTime.Now;
                    maindata.Update_User = CGlobal.UserInfo.EmployeeNo;
                    maindata.Update_Date = DateTime.Now;
                    maindata.TM_MiniHeart_Peroid = getpr.FirstOrDefault();

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
                    getMain = _MiniHeart_Main2021Service.Find(SystemFunction.GetIntNullToZero(main_id));
                    maindata = new MiniHeart_Main2021();
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

                    if (SystemFunction.GetNumberNullToZero(getrankMe) > SystemFunction.GetNumberNullToZero(getrankgiveemp))
                    {

                        check_rank = _MiniHeart_Detail2021Service.FindByCondition(CGlobal.UserInfo.EmployeeNo, maindata.Id)
                       .Where(w => SystemFunction.GetNumberNullToZero(getrankMe) > SystemFunction.GetNumberNullToZero(w.Rank)
                       )
                       .Count();
                    }
                    else if (SystemFunction.GetNumberNullToZero(getrankMe) == SystemFunction.GetNumberNullToZero(getrankgiveemp))
                    {
                        check_rank = _MiniHeart_Detail2021Service.FindByCondition(CGlobal.UserInfo.EmployeeNo, maindata.Id)
                    .Where(w => SystemFunction.GetNumberNullToZero(getrankMe) == SystemFunction.GetNumberNullToZero(w.Rank)
                    )
                    .Count();
                    }
                    else if (SystemFunction.GetNumberNullToZero(getrankMe) < SystemFunction.GetNumberNullToZero(getrankgiveemp))
                    {
                        check_rank = _MiniHeart_Detail2021Service.FindByCondition(CGlobal.UserInfo.EmployeeNo, maindata.Id)
                     .Where(w => SystemFunction.GetNumberNullToZero(getrankMe) < SystemFunction.GetNumberNullToZero(w.Rank)
                     )
                     .Count();
                    }

                    check_rank = 0;

                    //check_rank = _MiniHeart_Detail2021Service.FindByCondition(CGlobal.UserInfo.EmployeeNo, maindata.Id)
                    //    .Where(w => SystemFunction.GetNumberNullToZero(w.Rank) <= SystemFunction.GetNumberNullToZero(getrankgiveemp)
                    //  || SystemFunction.GetNumberNullToZero(w.Rank) == SystemFunction.GetNumberNullToZero(getrankgiveemp)
                    //  || SystemFunction.GetNumberNullToZero(w.Rank) >= SystemFunction.GetNumberNullToZero(getrankgiveemp)
                    //    )
                    //.Count();


                }
                if (DateTime.Now.Date > datefrom && DateTime.Now.Date < dateend)
                {
                    check_rank = 0;
                }
                if (check_rank < 1)
                {
                    //var getdup = _MiniHeart_Detail2021Service.GetDataForSelect().Where(w => w.Emp_No == emp_no && w.Active_Status == "Y" && w.MiniHeart_Main == getMain).ToList();
                    //if (getdup.Count() >= 1)
                    //{
                    //    result.Status = SystemFunction.process_Failed;
                    //    result.Msg = "Cannot give a mini-heart for someone who has already given it.";

                    //    return Json(new
                    //    {
                    //        result
                    //    });
                    //}
                    var setnew = _MiniHeart_Main2021Service.CreateNewOrUpdateMain(maindata);
                    MiniHeart_Detail2021 MiniHeart_Detail2021 = new MiniHeart_Detail2021();
                    if (setnew > 0)
                    {
                        if (!String.IsNullOrEmpty(main_id))
                        {
                            MiniHeart_Detail2021.MiniHeart_Main_Id = maindata.Id;
                        }
                        else
                        {
                            var getmaxid = _MiniHeart_Main2021Service.GetDataForSelect().Max(m => m.Id);
                            MiniHeart_Detail2021.MiniHeart_Main_Id = getmaxid;
                        }

                    }


                    MiniHeart_Detail2021.Emp_No = emp_no;
                    MiniHeart_Detail2021.TM_MiniHeart_Question_Id = SystemFunction.GetIntNullToZero(group);
                    MiniHeart_Detail2021.Reason = reason;
                    MiniHeart_Detail2021.Rank = getrankgiveemp;

                    MiniHeart_Detail2021.Active_Status = "Y";
                    MiniHeart_Detail2021.Create_User = CGlobal.UserInfo.EmployeeNo;
                    MiniHeart_Detail2021.Create_Date = DateTime.Now;
                    MiniHeart_Detail2021.Update_User = CGlobal.UserInfo.EmployeeNo;
                    MiniHeart_Detail2021.Update_Date = DateTime.Now;
                    MiniHeart_Detail2021.Show_Name = showname == "True" ? "Y" : "N";

                    var setdetailnew = _MiniHeart_Detail2021Service.CreateNewOrUpdate(MiniHeart_Detail2021);
                    if (setdetailnew > 0)
                    {
                        string pathUrl = Url.Action("MiniHeartView2021", "MiniHeart2021", null, Request.Url.Scheme);
                        var senttocol = send_mail("MiniHeart To Colleague 2021", emp_no, "", pathUrl);
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
        public ActionResult Search_MiniHeart_Give2021(string start, string end)
        {
            vMiniHeart_Main2021_Return result = new vMiniHeart_Main2021_Return();
            try
            {
                result.mainlstdata = new List<vMiniHeart_Main2021_obj>();
                result.mainlstdatarecive = new List<vMiniHeart_Main2021_obj>();

                var getde = _MiniHeart_Detail2021Service.GetDataForSelect().ToList();

                var conditon = getde.Where(w => w.MiniHeart_Main.TM_MiniHeart_Peroid.Start_Peroid.Value.Date >= SystemFunction.ConvertStringToDateTime(start, "", "dd MMM yyyy").Date
                && w.MiniHeart_Main.TM_MiniHeart_Peroid.End_Peroid.Value.Date <= SystemFunction.ConvertStringToDateTime(end, "", "dd MMM yyyy").Date).OrderBy(o => o.Emp_No);

                var group = conditon.GroupBy(g => g.MiniHeart_Main.Emp_No).ToList();
                var i = 0;
                foreach (var x in group)
                {
                    i++;
                    //var fine = conditon.Where(w=> w.Emp_No == x.FirstOrDefault().MiniHeart_Main.Emp_No).Count();
                    if (i > 135)
                    {
                        var sdfa = x.FirstOrDefault().MiniHeart_Main;
                    }
                    var nObj = new vMiniHeart_Main2021_obj();
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
        public ActionResult Search_MiniHeart_Recive2021(string start, string end)
        {
            vMiniHeart_Main2021_Return result = new vMiniHeart_Main2021_Return();
            try
            {
                result.mainlstdata = new List<vMiniHeart_Main2021_obj>();
                result.mainlstdatarecive = new List<vMiniHeart_Main2021_obj>();

                var getde = _MiniHeart_Detail2021Service.GetDataForSelect().ToList();

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

                    var nObj = new vMiniHeart_Main2021_obj();
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

                var abc = wsHRis.getActiveStaffByEmpNo(reciepver);
                var MailRequest = _MailContentService.GetMailContent(mailcontent_name, "Y").FirstOrDefault();
                string keyword = "";


                switch (abc.Rows[0].Field<string>("CompanyCode"))
                {
                    case "KPMG Lao":
                        keyword = "Bok Huk";
                        break;
                    case "KPMG MM":
                        keyword = "Chit Kyaung Pyaw";
                        break;
                    default:
                        keyword = "Bok Ruk";
                        break;
                }

                if (true) { }

                string sContent = MailRequest.content;
                string from = MailRequest.sender_name;
                string subject = MailRequest.mail_header;

                sContent = (sContent + "").Replace("$linkto", genlink);
                sContent = (sContent + "").Replace("$reciepver", abc.Rows[0].Field<string>("EmpFullName"));
                sContent = (sContent + "").Replace("$keyword$", keyword);

                //string getemailcc = "";
                //if (!String.IsNullOrEmpty(param3))
                //{
                //    var getcc = sQuery.Where(w => w.Employeeno == param3).FirstOrDefault();
                //    getemailcc = getcc.Email;
                //}

                var objMail = new vObjectMail_Send();
                objMail.mail_from = "hcmthailand@kpmg.co.th";
                objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                objMail.mail_to = abc.Rows[0].Field<string>("Email");
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