using HumanCapitalManagement.Models.TraineeSite;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.Service.TraineeSite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using TraineeManagement.App_Start;
using TraineeManagement.Controllers.CommonControllers;
using TraineeManagement.ViewModels.CommonVM;
using TraineeManagement.ViewModels.MainVM;

namespace TraineeManagement.Controllers.MainController
{
    public class TimeSheetController : BaseController
    {
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TimeSheet_FormService _TimeSheet_FormService;
        private TimeSheet_DetailService _TimeSheet_DetailService;
        private TM_Time_TypeService _TM_Time_TypeService;
        private TM_Eva_RatingService _TM_Eva_RatingService;
        private TM_Trainee_EvaService _TM_Trainee_EvaService;
        private TM_Trainee_Eva_AnswerService _TM_Trainee_Eva_AnswerService;
        private TM_TraineeEva_StatusService _TM_TraineeEva_StatusService;
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public TimeSheetController(TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TimeSheet_FormService TimeSheet_FormService
            , TimeSheet_DetailService TimeSheet_DetailService
            , TM_Time_TypeService TM_Time_TypeService
            , TM_Eva_RatingService TM_Eva_RatingService
            , TM_Trainee_EvaService TM_Trainee_EvaService
            , TM_Trainee_Eva_AnswerService TM_Trainee_Eva_AnswerService
            , TM_TraineeEva_StatusService TM_TraineeEva_StatusService
            , TM_CandidatesService TM_CandidatesService
            , MailContentService MailContentService
                   )
        {
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TimeSheet_FormService = TimeSheet_FormService;
            _TimeSheet_DetailService = TimeSheet_DetailService;
            _TM_Time_TypeService = TM_Time_TypeService;
            _TM_Eva_RatingService = TM_Eva_RatingService;
            _TM_Trainee_EvaService = TM_Trainee_EvaService;
            _TM_Trainee_Eva_AnswerService = TM_Trainee_Eva_AnswerService;
            _TM_TraineeEva_StatusService = TM_TraineeEva_StatusService;
            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;
        }
        public class Engagement_PR
        {
            public string id { get; set; }
            public string name { get; set; }
            public string text { get; set; }
            public string value { get; set; }
        }

        public class ResultLine
        {

            public ResultLine() { }

            public string date { get; set; }
            public string time { get; set; }
            public string Quantity { get; set; }
            public string type { get; set; }
        }

        public List<Engagement_PR> lst_Engagement = new List<Engagement_PR>();

        // GET: TimeSheet
        public ActionResult TimeSheetView(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            qryStr = CGlobal.UserInfo.UserId;
            vTimeSheet_obj_View result = new vTimeSheet_obj_View();

            New_HRISEntities dbHr = new New_HRISEntities();
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));


            if (!string.IsNullOrEmpty(qryStr))
            {
                result.IdEncrypt = qryStr;
                int nId = Convert.ToInt32(qryStr);
                //int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _TM_CandidatesService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.Id + "" != CGlobal.UserInfo.UserId)
                        {
                            return RedirectToAction("TimeSheetView", "TimeSheet");
                        }

                        int nID = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId);

                        var _GetCanMap = _TM_PR_Candidate_MappingService.GetDataForTimeSheet_CDD(nID);
                        var _getList = _TimeSheet_FormService.FindByCandidateID(nID).Where(w => w.active_status == "Y").OrderByDescending(o => o.update_date);
                        //var _getList = _TimeSheet_FormService.FindByCandidateID(nID);

                        var count_CanMap = _GetCanMap.Count();
                        var count_Form = _getList.Count();
                        if (count_Form < count_CanMap)
                        {
                            result.Add = "Y";
                        }
                        if (_getList != null && _getList.Any(a => a.active_status + "" == "Y"))
                        {
                            List<vTimeSheet_obj> mainlstdata = new List<vTimeSheet_obj>();
                            vTimeSheet_obj lstdata = new vTimeSheet_obj();
                            foreach (var s in _getList)
                            {
                                var sf = s.TM_PR_Candidate_Mapping;
                                lstdata = new vTimeSheet_obj();
                                lstdata.acknowledge_user = s.acknowledge_user;
                                lstdata.active_status = s.TM_PR_Candidate_Mapping.trainee_end.Value >= DateTime.Now.Date ?  s.active_status: "N";
                                //lstdata.create_date = s.create_date.HasValue ? s.create_date.Value.DateTimebyCulture() : "";
                                lstdata.create_date = s.create_date.HasValue ? s.TM_PR_Candidate_Mapping.trainee_start.Value.DateTimebyCulture() + " - " + s.TM_PR_Candidate_Mapping.trainee_end.Value.DateTimebyCulture()+" ( "+s.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en+" )" : "";
                                var get_mgr = sQuery.Where(w => w.Employeeno == s.Approve_user).FirstOrDefault();
                                if (get_mgr != null)
                                {
                                    lstdata.Approve_user = get_mgr.Employeename + " " + get_mgr.Employeesurname;
                                }
                                lstdata.Edit_timesheet = @"<a href=""#"" class=""btn btn-animated btn-info btn-sm"" href=""#"" onclick=""Edit_timesheet('" + HCMFunc.Encrypt(s.Id + "") + @"');return false;"">Internship Period <i class=""fa fa-calendar""></i></a>";
                                lstdata.Edit = @"<a href=""#"" class=""btn btn-animated btn-  btn-sm"" href=""#"" onclick=""Edit('" + HCMFunc.Encrypt(s.Id + "") + @"');return false;"">Edit <i class=""fa fa-edit""></i></a>";
                                mainlstdata.Add(lstdata);
                            }
                            result.lstData = mainlstdata;
                            //result.lstData = _getList.Where(w => w.active_status + "" == "Y").Select(s => new vTimeSheet_obj
                            //{
                            //    acknowledge_user = s.acknowledge_user,
                            //    create_date = s.create_date.HasValue ? s.create_date.Value.DateTimebyCulture() : "",
                            //    active_status = s.active_status,

                            //    Edit = @"<a href=""#"" class=""btn btn-animated btn-info btn-sm"" href=""#"" onclick=""Edit('" + HCMFunc.Encrypt(s.Id + "") + @"');return false;"">Edit <i class=""fa fa-arrow-right""></i></a>"

                            //}).ToList();
                        }
                    }
                    else
                    {
                        return RedirectToAction("TimeSheetView", "TimeSheet");
                    }
                }

            }
            else
            {
                //return RedirectToAction("TimeSheetForm", "TimeSheet");
            }

            return View(result);
        }
        public ActionResult TimeSheetCreate(string qryStr)
        {
            //qryStr = CGlobal.UserInfo.UserId;
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vTimeSheet_obj_Save result = new vTimeSheet_obj_Save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                result.IdEncrypt = qryStr;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                //int nId = Convert.ToInt32(qryStr + "");
                if (nId != 0)
                {
                    var getData = _TimeSheet_FormService.Find(nId);
                    if (getData != null)
                    {

                        IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                        var get_mgr = sQuery.Where(w => w.Employeeno == getData.Approve_user).FirstOrDefault();
                        if (get_mgr != null)
                        {
                            var _getEmp = dbHr.AllInfo_WS.Where(w => get_mgr.UserID.Contains(w.UserID)).ToList();
                            var lstUnit = _getEmp.Where(w => w.EmpNo == get_mgr.Employeeno).DefaultIfEmpty(new AllInfo_WS()).FirstOrDefault();

                            result.mgr_user_name = get_mgr.Employeename + " " + get_mgr.Employeesurname;
                            result.mgr_user_no = getData.Approve_user;
                            result.mgr_unit_name = lstUnit.UnitGroup;
                            result.mgr_user_rank = lstUnit.Rank;
                        }
                    }
                    else
                    {
                        return RedirectToAction("TimeSheetView", "TimeSheet");
                    }
                }

            }
            else
            {

            }

            return View(result);
        }
        public ActionResult TimeSheetEdit(string qryStr)
        {

            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vTimeSheet_Detail_obj_Save result = new vTimeSheet_Detail_obj_Save();


            if (!string.IsNullOrEmpty(qryStr))
            {

                result.IdEncrypt = qryStr;


                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                var _getList = _TimeSheet_FormService.FindByCandidateID(Convert.ToInt32(CGlobal.UserInfo.UserId));
                //var _getbymapping = _TM_PR_Candidate_MappingService.GetDataForTimeSheet_id();
                if (nId != 0)
                {
                    result.TimeSheet_Form_Id = _getList.Where(w=> w.Id == nId).FirstOrDefault().Id.ToString();
                    //result.cost_center = "Audit Pool";
                    var getlist_detail = _getList.Where(w => w.Id == nId).FirstOrDefault();
                    result.active_status = getlist_detail.TM_PR_Candidate_Mapping.trainee_end.Value.Date >= DateTime.Now.Date ?getlist_detail.active_status:  "N"   ;

                    //var getcandidate = _TM_PR_Candidate_MappingService.GetDataForSelect().Where(w => w.TM_Candidates.Id == Convert.ToInt32(CGlobal.UserInfo.UserId)).FirstOrDefault();
                    var getcandidate = getlist_detail.TM_PR_Candidate_Mapping;
                    var lastdate = getlist_detail.TimeSheet_Detail.Where(w => w.active_status == "Y" && w.approve_status == "Y" && w.Source_Type != "R").ToList().Select(s => s.date_start).Max();
                    var count_Approve = getlist_detail.TimeSheet_Detail.Where(w => w.active_status == "Y" && w.approve_status == "N").Count();
                    if (count_Approve == 0)
                    {
                        result.TimeSheet_Form_Approve = "Y";
                    }
                    var approvedate = getlist_detail.Approve_date;
                    var startofmonth = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01");
                    var endofmonth = DateTime.Now.AddMonths(1).ToString("yyyy-MM-" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month).ToString());
                    result.date_start = startofmonth;
                    result.date_end = endofmonth;
                    result.name = CGlobal.UserInfo.FullName;
                    result.cost_center = _getList.Where(w => w.Id == nId).FirstOrDefault().TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en;
                    result.division_code = _getList.Where(w => w.Id == nId).FirstOrDefault().TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code;
                    result.account_no = _getList.Where(w => w.Id == nId).FirstOrDefault().TM_PR_Candidate_Mapping.TM_Candidates.candidate_BankAccountNumber;
                    //if (DateTime.Now.Day <= 15)
                    //{
                    //    result.date_start = startofmonth;
                    //    result.date_end = DateTime.Now.ToString("yyyy-MM-15");
                    //}
                    //else
                    //{
                    //    result.date_start = DateTime.Now.ToString("yyyy-MM-16"); ;
                    //    result.date_end = endofmonth;
                    //}

                    if (SystemFunction.ConvertStringToDateTime(result.date_start, "", "yyyy-MM-dd").Date < getcandidate.trainee_start.Value)
                    {
                        result.date_start = getcandidate.trainee_start.AsDateTimeWithTimebyCulture("yyyy-MM-dd");
                    }
                    //if (lastdate != null && Convert.ToDateTime(result.date_start) < lastdate)
                    //{
                    //    result.date_start = Convert.ToDateTime(lastdate).AddDays(1).AsDateTimeWithTimebyCulture("yyyy-MM-dd");
                    //}
                    if (SystemFunction.ConvertStringToDateTime(result.date_end, "", "yyyy-MM-dd").Date > getcandidate.trainee_end.Value.Date)
                    {
                        result.date_end = getcandidate.trainee_end.AsDateTimeWithTimebyCulture("yyyy-MM-dd");
                    }

                    //foreach (var a in getlist_detail.TimeSheet_Detail)
                    //{
                    //    result.lstData.Add(new vTimeSheet_Detail_obj() { start = a.date_start.ToString() });
                    //}
                    //edit
                    List<vTimeSheet_Detail_obj> maindata = new List<vTimeSheet_Detail_obj>();
                    List<Calandar_Even> mainlstdata = new List<Calandar_Even>();

                    foreach (var s in getlist_detail.TimeSheet_Detail.Where(w => w.active_status == "Y"))
                    {
                        string colors = "";
                        if (!string.IsNullOrEmpty(s.Source_Type) && s.Source_Type == "R")
                        {
                            colors = "#bfe87c";
                        }
                        else
                        {
                            if (s.submit_status == "S" && s.approve_status == "N")
                            {
                                colors = "#a3ada3";
                            }

                            if (s.approve_status == "R")
                            {
                                colors = "#8B0000";
                            }
                            else if (s.approve_status == "Y")
                            {
                                colors = "#bfe87c";
                            }

                            else if (s.submit_status == "D" && s.approve_status == "N")
                            {
                                colors = _TM_Time_TypeService.Find(Convert.ToInt32(s.TM_Time_Type_Id)).colors;
                            }
                        }
                        if (s.approve_status == "A")
                        {
                            colors = "#c78be6";
                        }
                        else if (s.approve_status == "P")
                        {
                            colors = "#f380ef";
                        }


                        Calandar_Even lstdata = new Calandar_Even();
                        lstdata.id = s.Id.ToString();
                        lstdata.title = s.title;
                        lstdata.start = Convert.ToDateTime(s.date_start).ToString("yyyy-MM-dd HH:mm");
                        lstdata.end = Convert.ToDateTime(s.date_end).ToString("yyyy-MM-dd HH:mm");
                        //lstdata.allDay = "false";
                        lstdata.backgroundColor = colors;
                        lstdata.status = s.approve_status;
                        mainlstdata.Add(lstdata);

                        vTimeSheet_Detail_obj lstmain = new vTimeSheet_Detail_obj();
                        lstmain.title = s.title;
                        lstmain.TM_Time_Type_Id = s.TM_Time_Type_Id.ToString();
                        lstmain.start = Convert.ToDateTime(s.date_start).ToString("yyyy-MM-dd HH:mm");
                        lstmain.end = Convert.ToDateTime(s.date_end).ToString("yyyy-MM-dd HH:mm");
                        lstmain.hour = s.hours;
                        lstmain.remark = s.remark;
                        lstmain.approve_status = s.approve_status;
                        maindata.Add(lstmain);
                    }
                    result.lstData = maindata;
                    result.lstData_cld = mainlstdata;


                    wsHRIS.HRISSoap Hris = new wsHRIS.HRISSoapClient();
                    var holiday = Hris.Get_Holiday(DateTime.Now.Year.ToString());
                    var _Getholiday = holiday.AsEnumerable().Select(s => s).ToList();
                    List<holiday_date> lst_holiday = new List<holiday_date>();
                    foreach (var hol in _Getholiday)
                    {
                        holiday_date hol_date = new holiday_date();
                        hol_date.date = Convert.ToDateTime(hol.ItemArray[4]).ToString("dd-MM-yyyy");

                        lst_holiday.Add(hol_date);
                    }

                    result.lstData_holiday = lst_holiday;

                }
                else
                {
                    return RedirectToAction("TimeSheetView", "TimeSheet");
                }
            }
            else
            {
                return RedirectToAction("TimeSheetView", "TimeSheet");
            }

            //vTimeSheet_Detail_obj_Save returnresult = new vTimeSheet_Detail_obj_Save();

            //try
            //{
            //    returnresult.Add(new Calandar_Even()
            //    {
            //        title = "08:00 test sent",
            //        //start = DateTime.Now.ToString("yyyy,MM,dd"),
            //        start = "2019-03-16 08:00",
            //        end = "2019-03-16 17:00",
            //        allDay = "false",
            //        backgroundColor = "#dede14",
            //        //borderColor = "#00c0ef"
            //    });

            //}
            //catch (Exception ex)
            //{

            //}

            return View(
                result
            );
        }

        [HttpPost]
        public ActionResult GetEvenOfCalendar(string ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            List<Calandar_Even> returnresult = new List<Calandar_Even>();
            try
            {
                returnresult.Add(new Calandar_Even()
                {
                    title = "test sent",
                    //start = DateTime.Now.ToString("yyyy,MM,dd"),
                    start = "2019-03-16",
                    end = "2019-03-16",
                    //allDay = "true",
                    backgroundColor = "#00c0ef",
                    //borderColor = "#00c0ef"
                });

            }
            catch (Exception ex)
            {

            }
            return Json(new
            {
                returnresult
            });

        }


        #region Ajax Function

        [HttpPost]
        public JsonResult GetEngagement(string university_id)
        {
            Viztopia.ViztopiaSoapClient wsViztp = new Viztopia.ViztopiaSoapClient();

            string ChargeCode = "";
            string CompName = "";

            var data = wsViztp.GetAllEngagementFromSSA("", "", "2,3", "", "", "", "", "", "", "", "", "");

            lst_Engagement = new List<Engagement_PR>();
            lst_Engagement = data.AsEnumerable().Select(dataRow => new Engagement_PR
            {
                id = dataRow.Field<string>("EngagementNo")
            ,
                text = dataRow.Field<string>("EngagementName")
            ,
                value = dataRow.Field<string>("EngagementManagerNo")
            ,
                name = dataRow.Field<string>("EngagementManagerName")
            }).ToList();


            var indata = wsViztp.Get_Internal_Chargecode("", "");
            var a = new List<Engagement_PR>();
            a = indata.Tables[0].AsEnumerable().Select(dataRow => new Engagement_PR
            {
                id = dataRow.Field<string>("Code")
            ,
                text = dataRow.Field<string>("Description")
            }).ToList();


            lst_Engagement.AddRange(a);


            return Json(lst_Engagement, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateTimeSheetForm(vTimeSheet_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTimeSheet_Return result = new vTimeSheet_Return();


            if (ItemData != null)
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                DateTime dNow = DateTime.Now;
                ItemData.Id = HCMFunc.Decrypt(ItemData.IdEncrypt);
                List<vTimeSheet_obj> tm = new List<vTimeSheet_obj>();
                vTimeSheet_obj subtm = new vTimeSheet_obj();
                subtm.Id = ItemData.IdEncrypt;
                tm.Add(subtm);
                result.lstData = tm;
                int nId = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId);
                if (nId != 0)
                {

                    if (string.IsNullOrEmpty(ItemData.mgr_user_no))
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please select Performance Manager.";
                        return Json(new
                        {
                            result
                        });

                    }
                    var lstData_mapping = _TM_PR_Candidate_MappingService.GetDataForTimeSheet_CDD(nId).OrderByDescending(o => o.Id).FirstOrDefault();
                    TimeSheet_Form objSave = new TimeSheet_Form()
                    {
                        Id = SystemFunction.GetIntNullToZero(ItemData.Id),
                        update_user = CGlobal.UserInfo.FullName,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.FullName,
                        create_date = dNow,
                        trainee_create_user = nId,
                        trainee_create_date = dNow,
                        trainee_update_user = nId,
                        trainee_update_date = dNow,
                        active_status = "Y",
                        submit_status = "Y",
                        Approve_status = "N",
                        Approve_user = ItemData.mgr_user_no,
                        TM_PR_Candidate_Mapping = lstData_mapping
                    };


                    var deldetail = _TimeSheet_FormService.UpdateInactive(objSave);

                    var sComplect = _TimeSheet_FormService.CreateNewOrUpdate(objSave);
                    if (sComplect > 0)
                    {
                        result.Status = SystemFunction.process_Success;
                        result.Msg = "Success, Save Success.";

                        IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                        var get_mgr = sQuery.Where(w => w.Employeeno == ItemData.mgr_user_no).FirstOrDefault();

                        string msg = "";

                        var Mail1 = _MailContentService.GetMailContent("Time Sheet Change PM", "Y").FirstOrDefault();
                        string sContent = Mail1.content;
                        sContent = (sContent + "").Replace("$emailto", get_mgr.Email);
                        sContent = (sContent + "").Replace("$pm", get_mgr.Employeename + " " + get_mgr.Employeesurname);
                        sContent = (sContent + "").Replace("$trainee", CGlobal.UserInfo.FullName);

                        var objMail = new vObjectMail_Send();

                        objMail.mail_from = "hcmthailand@kpmg.co.th";
                        objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                        objMail.mail_to = get_mgr.Email;
                        objMail.mail_cc = CGlobal.UserInfo.EMail;
                        objMail.mail_subject = Mail1.mail_header;
                        objMail.mail_content = sContent;

                        //objMail.mail_from = "hcmthailand@kpmg.co.th";
                        //objMail.title_mail_from = Mail1.sender_name;
                        //objMail.mail_subject = Mail1.mail_header;
                        //objMail.mail_content = sContent;
                        //objMail.mail_to = get_mgr.Email;

                        var sSendMail = HCMFunc.SendMail(objMail, ref msg);
                        if (!sSendMail)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Submit Success. But Error in Email : " + msg;
                            return Json(new
                            {
                                result
                            });
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Time Sheet Not Found.";
                    }


                }

            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SaveTimeSheet(vTimeSheet_Detail_obj_Save ItemData, string types)
        {

            return SaveTimeSheet_Detail(ItemData, types);
        }
        public ActionResult SaveTimeSheet_Detail(vTimeSheet_Detail_obj_Save ItemData, string types)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTimeSheet_Return result = new vTimeSheet_Return();
            bool savelist = true;
            //var sComplect = _TimeSheet_FormService.GetDataForSelect();
            var appreverSet = "";

            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TimeSheet_FormService.Find(nId);

                    //var _getData = _TimeSheet_DetailService.Find(nId);
                    //var _getRealData = _getData.TimeSheet_Form;

                    if (_getData != null)
                    {


                        List<TimeSheet_Detail> lst_map_detail = new List<TimeSheet_Detail>();
                        List<TimeSheet_Detail> lst_map_detail_All = new List<TimeSheet_Detail>();
                        TimeSheet_Detail objSave = new TimeSheet_Detail();
                        //var groups = ItemData.lstData_cld.GroupBy(g=> Convert.ToDateTime(g.start).ToString("dd-MM-yyyy"));


                        //IEnumerable<Calandar_Even> smths = groups.SelectMany(group => group);
                        //List<Calandar_Even> newList = smths.ToList();

                        if (ItemData.lstData_cld != null)
                        {
                            var resultst = ItemData.lstData_cld.GroupBy(g => Convert.ToDateTime(g.start.Split(' ')[0])).Select(g => new ResultLine
                            {
                                date = g.Key.ToString("dd MMMM yyyy"),
                                time = g.Sum(x => Convert.ToDecimal(x.title.Split('|')[0].Replace(':', '.'))).ToString()
                            }).ToList();

                            var setvaltolist = new List<ResultLine>();
                            foreach (var set in ItemData.lstData_cld)
                            {
                                var setobj = new ResultLine();
                                setobj.date = set.start.Split(' ')[0];
                                setobj.type = set.title.Split('|')[1];
                                setobj.time = set.title.Split('|')[0].Replace(':', '.');
                                setvaltolist.Add(setobj);
                            }
                            var grouptolist = setvaltolist.Where(w => w.type != "OT").GroupBy(g => new { g.date }).Select(s => new ResultLine
                            {
                                date = s.Key.date,
                                time = s.Sum(x => Convert.ToDecimal(x.time)).ToString()

                            }).ToList();


                            //                List<ResultLine> resultsts = ItemData.lstData_cld
                            //.GroupBy(g => Convert.ToDateTime(g.start.Split(' ')[0]))
                            //.SelectMany(cl => cl.Select(
                            //    csLine => new ResultLine
                            //    {
                            //        date = Convert.ToDateTime(csLine.start).ToString("dd-MM-yyyy"),
                            //        Quantity = cl.Count().ToString(),
                            //        time = cl.Sum(c => Convert.ToDecimal(c.title.Split('|')[0].Replace(':', '.'))).ToString(),
                            //        type = csLine.title.Split('|')[1]
                            //    })).ToList<ResultLine>();


                            if (ItemData.cost_center != "Audit Pool")
                            {
                                foreach (var check in grouptolist)
                                {
                                    //if (check.type == "Normal Hour" && Convert.ToDecimal(check.time) < 8)
                                    //{
                                    //result.Status = SystemFunction.process_Failed;
                                    //result.Msg = "Error," + check.date + " The total time must not be less than 8 hours.";

                                    //return Json(new
                                    //{
                                    //    result
                                    //});
                                    //}
                                    //else 

                                    if (Convert.ToDecimal(check.time) > 24)
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error," + check.date + " The total time must not be more than 24 hours.";

                                        return Json(new
                                        {
                                            result
                                        });
                                    }
                                    if (types == "Submit")
                                    {
                                        if (Convert.ToDecimal(check.time) != 8)
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Error," + check.date + "(" + check.time + ") Total hours of Normal, Leave and Training must be 8 hours.";

                                            return Json(new
                                            {
                                                result
                                            });
                                        }
                                    }

                                }
                            }

                            GetEngagement("");
                            if (!string.IsNullOrEmpty(ItemData.date_start) && !string.IsNullOrEmpty(ItemData.date_end))
                            {
                                var getwhere = ItemData.lstData_cld.Where(w => w.status == "N" || String.IsNullOrEmpty(w.status)).ToList();
                                //    .Where(w =>
                                //Convert.ToDateTime(w.start).Date >= SystemFunction.ConvertStringToDateTime(ItemData.date_start,"","yyyy-MM-dd").Date 
                                //&& Convert.ToDateTime(w.end).Date <= SystemFunction.ConvertStringToDateTime(ItemData.date_end, "", "yyyy-MM-dd").Date
                                //).ToList();
                                foreach (var xp in getwhere)
                                {
                                    //if (!string.IsNullOrEmpty(xp.id))
                                    //{
                                    //    IEnumerable<TimeSheet_Detail> dupList = _getData.TimeSheet_Detail.Where(s => s.Id == Convert.ToInt32(xp.id));
                                    //}

                                    var spilt_title = xp.title.Split('|')[0];
                                    var hours_ = xp.title.Split('|')[0];
                                    var type_ = "";
                                    var Engagement_ = "";
                                    var Remark_ = "";
                                    if (!string.IsNullOrEmpty(hours_) && hours_.Split(':').Length > 0)
                                    {
                                        if (xp.title.Split('|').Length >= 2)
                                            type_ = xp.title.Split('|')[1];
                                        if (xp.title.Split('|').Length == 3)
                                            Engagement_ = xp.title.Split('|')[2];
                                        if (xp.title.Split('|').Length == 4)
                                            Remark_ = xp.title.Split('|')[3];
                                    }
                                    var type_id = 0;

                                    var Time_type_ = _TM_Time_TypeService.FindByName(type_);
                                    if (Time_type_ != null)
                                        type_id = Time_type_.Id;


                                    Engagement_ = xp.title.Split('|')[2];

                                    var check_egm_mgr = lst_Engagement.Where(w => w.id == Engagement_).FirstOrDefault();
                                    //เพิ่มเงื่อนไขสำหรับ audit


                                    if (check_egm_mgr == null)
                                    {
                                        if (Engagement_ != "150000000001" && Engagement_ != "150000000000")
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Error, Engagement does not exist. => " + Engagement_;
                                            return Json(new
                                            {
                                                result
                                            });
                                        }
                                    }


                                    if (ItemData.cost_center != "Audit Pool")
                                    {
                                        if (String.IsNullOrEmpty(_getData.Approve_user) || _getData.Approve_user == "00000000")
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Error, Approver does not exist";
                                            return Json(new
                                            {
                                                result
                                            });
                                        }
                                        appreverSet = _getData.Approve_user;
                                    }
                                    else
                                    {


                                        var getmgr = check_egm_mgr.value;
                                        //เอาค่ามาจาก engatementMgr
                                        appreverSet = getmgr;
                                    }

                                    if (string.IsNullOrEmpty(xp.id))
                                    {
                                        objSave = new TimeSheet_Detail()
                                        {
                                            date_start = Convert.ToDateTime(xp.start),
                                            date_end = Convert.ToDateTime(xp.end),
                                            title = xp.title,
                                            hours = hours_,
                                            TM_Time_Type_Id = type_id,
                                            Engagement_Code = Engagement_,
                                            remark = Remark_,
                                            active_status = "Y",
                                            trainee_create_date = DateTime.Now,
                                            trainee_create_user = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId),
                                            trainee_update_date = DateTime.Now,
                                            trainee_update_user = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId),
                                            TimeSheet_Form = _getData,
                                            submit_status = types == "Draft" ? "D" : "S",
                                            approve_status = "N",
                                            Approve_user = appreverSet


                                        };
                                        lst_map_detail_All.Add(objSave);
                                    }
                                    else
                                    {

                                        objSave = new TimeSheet_Detail()
                                        {
                                            Id = Convert.ToInt32(SystemFunction.GetIntNullToZero(xp.id)),
                                            date_start = Convert.ToDateTime(xp.start),
                                            date_end = Convert.ToDateTime(xp.end),
                                            title = xp.title,
                                            hours = hours_,
                                            TM_Time_Type_Id = type_id,
                                            Engagement_Code = Engagement_,
                                            remark = Remark_,
                                            TimeSheet_Form = _getData,
                                            submit_status = types == "Draft" ? "D" : "S",
                                            submit_date = DateTime.Now,
                                            Approve_user = appreverSet,
                                            trainee_create_date = DateTime.Now,
                                            trainee_create_user = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId),
                                            trainee_update_date = DateTime.Now,
                                            trainee_update_user = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId),


                                        };
                                        lst_map_detail_All.Add(objSave);

                                    }

                                    if (!string.IsNullOrEmpty(xp.title) && type_id != 0)
                                    {
                                        var change = _TimeSheet_DetailService.CreateNewOrUpdateCheck(objSave);
                                        if (change > 0)
                                        {
                                            lst_map_detail.Add(objSave);
                                        }
                                        var update_form = _TimeSheet_FormService.UpdateApproveFrom(new TimeSheet_Form() { Id = nId, Approve_status = "N", trainee_update_user = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId), });
                                        //if (change <= 0)
                                        //{
                                        //    savelist = false;
                                        //    result.Status = SystemFunction.process_Failed;
                                        //    result.Msg = "Error, Can't Save " + objSave.title;
                                        //    return Json(new
                                        //    {
                                        //        result
                                        //    });
                                        //}
                                        //else
                                        //{

                                        //}
                                    }

                                }
                            }


                            if (savelist)
                            {
                                result.Status = SystemFunction.process_Success;
                                result.Msg = types == "Draft" ? "Success, Save Success." : "Success, Submit Success.";


                                if (types == "Submit")
                                {
                                    var Mail1 = _MailContentService.GetMailContent("Time Sheet Submit", "Y").FirstOrDefault();
                                    string pathUrl = Url.Action("TimeSheetApprove", "TraineeTSheet", null, Request.Url.Scheme);
                                    //production
                                    pathUrl = pathUrl.Replace("TraineeEvaluation", "application/HR/HumanCapitalManagement");

                                    string genlink = HCMFunc.GetUrl(Mail1.mail_type, _getData.Id + "", pathUrl, _getData.Approve_user);

                                    IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                                    if (ItemData.cost_center == "Audit Pool")
                                    {
                                        try
                                        {

                                            //var a = lst_map_detail.Select(s => new TimeSheet_Detail() { Approve_user = s.Approve_user }).ToList();
                                            //var groupmgr = a.GroupBy(g => g.Approve_user).Select(s => s.ToList()).ToList();

                                            var a = lst_map_detail.Select(s => new TimeSheet_Detail() { TimeSheet_Form = s.TimeSheet_Form, Engagement_Code = s.Engagement_Code }).ToList();
                                            var groupmgr = a.GroupBy(g => new { g.TimeSheet_Form, g.Engagement_Code }).ToList();

                                            foreach (var x in groupmgr)
                                            {
                                                var check_egm_mgr = lst_Engagement.Where(w => w.id == x.Key.Engagement_Code).FirstOrDefault();

                                                var approve_u = x.First().Approve_user;
                                                if (check_egm_mgr == null)
                                                { }
                                                var get_mgr_lst = sQuery.Where(w => w.Employeeno == check_egm_mgr.value).FirstOrDefault();
                                                string msg = "";

                                                string sContent = Mail1.content;
                                                sContent = (sContent + "").Replace("$emailto", get_mgr_lst.Email);
                                                sContent = (sContent + "").Replace("$pm", get_mgr_lst.Employeename + " " + get_mgr_lst.Employeesurname);
                                                sContent = (sContent + "").Replace("$trainee", CGlobal.UserInfo.FullName);
                                                sContent = (sContent + "").Replace("$linkto", genlink);

                                                var objMail = new vObjectMail_Send();



                                                objMail.mail_from = "hcmthailand@kpmg.co.th";
                                                objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                                                objMail.mail_to = get_mgr_lst.Email;
                                                objMail.mail_cc = CGlobal.UserInfo.EMail;
                                                objMail.mail_subject = Mail1.mail_header;
                                                objMail.mail_content = sContent;


                                                var sSendMail = HCMFunc.SendMail(objMail, ref msg);
                                                if (!sSendMail)
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = "Error, Submit Success. But Error in Email : " + msg;
                                                    return Json(new
                                                    {
                                                        result
                                                    });
                                                }


                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Error, Submit Success. But Error in Email : " + ex.Message;
                                            return Json(new
                                            {
                                                result
                                            });
                                        }
                                    }

                                    else
                                    {

                                        var get_mgr = sQuery.Where(w => w.Employeeno == appreverSet).FirstOrDefault();

                                        string msg = "";

                                        string sContent = Mail1.content;
                                        sContent = (sContent + "").Replace("$emailto", get_mgr.Email);
                                        sContent = (sContent + "").Replace("$pm", get_mgr.Employeename + " " + get_mgr.Employeesurname);
                                        sContent = (sContent + "").Replace("$trainee", CGlobal.UserInfo.FullName);
                                        sContent = (sContent + "").Replace("$linkto", genlink);

                                        var objMail = new vObjectMail_Send();

                                        objMail.mail_from = "hcmthailand@kpmg.co.th";
                                        objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                                        objMail.mail_to = get_mgr.Email;
                                        objMail.mail_cc = CGlobal.UserInfo.EMail;
                                        objMail.mail_subject = Mail1.mail_header;
                                        objMail.mail_content = sContent;

                                        var sSendMail = HCMFunc.SendMail(objMail, ref msg);
                                        if (!sSendMail)
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Error, Submit Success. But Error in Email : " + msg;
                                            return Json(new
                                            {
                                                result
                                            });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Save Failed.";
                            }

                            var lst_main = _getData.TimeSheet_Detail.Where(w =>
                            w.active_status != "N" && (w.approve_status == "N" ||w.approve_status == "R" || String.IsNullOrEmpty(w.approve_status))
                            //&& Convert.ToDateTime(w.date_start).Date >= SystemFunction.ConvertStringToDateTime(ItemData.date_start,"","yyyy-MM-dd").Date
                            //&& Convert.ToDateTime(w.date_end).Date <= SystemFunction.ConvertStringToDateTime(ItemData.date_end, "", "yyyy-MM-dd").Date
                            ).ToList();
                            var lst_sub = lst_map_detail_All.ToList();
                            var remove_lst = lst_main.Where(p => !lst_sub.Any(p2 => p2.Id == p.Id)).ToList();

                            foreach (var inac in remove_lst)
                            {
                                if (inac.approve_status != "Y")
                                {
                                    var del = _TimeSheet_DetailService.UpdateInactive(inac);
                                    if (del <= 0)
                                    {
                                        savelist = false;
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, Can'n Save" + inac.title;
                                        return Json(new
                                        {
                                            result
                                        });
                                    }
                                }
                            }

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Success;
                            result.Msg = "Error, Data not found.";

                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, TimeSheet Not Found.";
                    }

                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditTimeSheetForm(vTimeSheet_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTimeSheet_Return result = new vTimeSheet_Return();
            if (ItemData != null)
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    if (string.IsNullOrEmpty(ItemData.mgr_user_no))
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please select Performance Manager.";
                        return Json(new
                        {
                            result
                        });

                    }


                    var _getData = _TM_Trainee_EvaService.Find(nId);
                    if (_getData != null)
                    {
                        var _getEmpMrg = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && w.Employeeno == ItemData.mgr_user_no).FirstOrDefault();
                        if (_getEmpMrg == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error,Performance Manager Not Found.";
                            return Json(new
                            {
                                result
                            });
                        }

                        var _getMap = _TM_PR_Candidate_MappingService.Find(_getData.TM_PR_Candidate_Mapping.Id);

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, TimeSheet Not Found.";
                    }

                }

            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SaveEditTimeSheet(vTimeSheet_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTimeSheet_Return result = new vTimeSheet_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {

                    var _getData = _TM_Trainee_EvaService.Find(nId);
                    if (_getData != null)
                    {
                        var _getMap = _TM_PR_Candidate_MappingService.Find(_getData.TM_PR_Candidate_Mapping.Id);
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, TimeSheet Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult UpdateCandidate(vTimeSheet_obj_View ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTimeSheet_Return result = new vTimeSheet_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    DateTime? dtarget_period = null, dtarget_period_to = null;

                    var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getData != null)
                    {
                        //var _getCandidate = _TM_CandidatesService.Find(_getData.TM_Candidates.Id);
                        //if (_getCandidate != null)
                        //{
                        //    _getCandidate.candidate_TraineeNumber = ItemData.trainee_no;
                        //    _getCandidate.candidate_NickName = ItemData.nick_name;
                        //    if (!string.IsNullOrEmpty(ItemData.target_end))
                        //    {
                        //        dtarget_period_to = SystemFunction.ConvertStringToDateTime(ItemData.target_end, "");
                        //    }
                        //    if (!string.IsNullOrEmpty(ItemData.target_start))
                        //    {
                        //        dtarget_period = SystemFunction.ConvertStringToDateTime(ItemData.target_start, "");
                        //    }
                        //    _getData.trainee_start = dtarget_period;
                        //    _getData.trainee_end = dtarget_period_to;
                        //    //id null = first time Save data
                        //    var sComplect = _TM_PR_Candidate_MappingService.Update(_getData);
                        //    if (sComplect > 0)
                        //    {
                        //        result.Status = SystemFunction.process_Success;

                        //    }
                        //    sComplect = _TM_CandidatesService.Update(_getCandidate);
                        //    if (sComplect > 0)
                        //    {
                        //        result.Status = SystemFunction.process_Success;
                        //    }
                        //    result.Status = SystemFunction.process_Success;
                        //}
                        //else
                        //    {
                        //        result.Status = SystemFunction.process_Failed;
                        //        result.Msg = "Error, Trainee Not Found.";
                        //    }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, TimeSheet Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }
            return Json(new
            {
                result
            });
        }
        #endregion
        public bool IsBusinessDay(DateTime value, string month, string year)
        {

            if (value.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            else if (value.DayOfWeek == DayOfWeek.Saturday)
            {
                return false;
            }
            else
            {
                wsHRIS.HRISSoap Hris = new wsHRIS.HRISSoapClient();
                var holiday = Hris.Get_Holiday(year);
                var _Getholiday = holiday.AsEnumerable().Select(s => s).ToList();
                foreach (var hol in _Getholiday)
                {
                    if (value.ToString("dd/MM/yyyy") == Convert.ToDateTime(hol.ItemArray[4]).ToString("dd/MM/yyyy")) return false;
                }
            }

            return true;
        }
    }
}