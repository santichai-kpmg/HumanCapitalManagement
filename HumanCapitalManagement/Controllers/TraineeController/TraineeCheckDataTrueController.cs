using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TraineeSite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HumanCapitalManagement.Controllers.TraineeController.TraineeTrackingController;
using static HumanCapitalManagement.ViewModel.Trainee.Engagement;

namespace HumanCapitalManagement.Controllers.TraineeController
{
    public class TraineeCheckDataTrueController : BaseController
    {


        private TimeSheet_FormService _TimeSheet_FormService;
        private TimeSheet_DetailService _TimeSheet_DetailService;
        private Perdiem_TransportService _Perdiem_TransportService;
        private TM_Time_TypeService _TM_Time_TypeService;
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public TraineeCheckDataTrueController(
             TimeSheet_FormService TimeSheet_FormService
            , TimeSheet_DetailService TimeSheet_DetailService
            , Perdiem_TransportService Perdiem_TransportService
            , TM_Time_TypeService TM_Time_TypeService
            , TM_CandidatesService TM_CandidatesService
            , MailContentService MailContentService
           )
        {

            _TimeSheet_FormService = TimeSheet_FormService;
            _TimeSheet_DetailService = TimeSheet_DetailService;
            _Perdiem_TransportService = Perdiem_TransportService;
            _TM_Time_TypeService = TM_Time_TypeService;
            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;

        }
        // GET: TraineeCheckDataTrue
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TraineeCheckDataTrue()
        {
            return View();
        }

        public class vallforcheck : CResutlWebMethod
        {
            public List<allforcheck> lstData { get; set; }
            public List<vPerdiem_Transport> lstData_perdiem { get; set; }

        }

        public class allforcheck
        {
            public string first_name_en { get; set; }
            public string last_name_en { get; set; }
            public string candidate_TraineeNumber { get; set; }
            public string date_start { get; set; }
            public string date_end { get; set; }
            public string engagement_code { get; set; }
            public string remark { get; set; }
            public string hours { get; set; }
            public string TM_Time_Type_Id { get; set; }
            public string submit_status { get; set; }
            public string approve_status { get; set; }
            public string approve_user { get; set; }
            public string cost_center { get; set; }
            public string client_name { get; set; }
            public string approve_date { get; set; }
            public string submit_date { get; set; }
            public string review_date { get; set; }
            public string review_user { get; set; }
            public string paid_date { get; set; }
            public string paid_user { get; set; }
        }

        [HttpPost]
        public ActionResult LoadTraineeDataList(string StartDatetxt, string EndDatetxt, string Nametxt, string Lastnametxt, string Statusslt)
        {
            vallforcheck result = new vallforcheck();
            try
            {
                New_HRISEntities dbHr = new New_HRISEntities();
                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));

                Viztopia.ViztopiaSoapClient wsViztp = new Viztopia.ViztopiaSoapClient();
                List<Engagement_PR> lst_Engagement_ext = new List<Engagement_PR>();
                List<Engagement_PR> lst_Engagement_int = new List<Engagement_PR>();

                var extdata = wsViztp.GetAllEngagementFromSSA("", "", "", "", "", "", "", "", "", "", (DateTime.Now.Year - 3).ToString(), "");
                lst_Engagement_ext = extdata.AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("EngagementNo"), text = dataRow.Field<string>("EngagementName"), comp = dataRow.Field<string>("ClientName") }).ToList();

                var intdata = wsViztp.Get_Internal_Chargecode("", "");
                lst_Engagement_int = intdata.Tables[0].AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("Code"), text = dataRow.Field<string>("Description"), comp = dataRow.Field<string>("CompDesc") }).ToList();


                var approve_user = CGlobal.UserInfo.EmployeeNo;

                var _getFirstLists = _TimeSheet_DetailService.GetDataForSelect().ToList();
                var _getFirstLists_Perdiems = _Perdiem_TransportService.GetDataForSelect().ToList();

                DateTime SetStartDate = SystemFunction.ConvertStringToDateTime(StartDatetxt, "", "dd MMMM yyyy");
                DateTime SetEndDate = SystemFunction.ConvertStringToDateTime(EndDatetxt, "", "dd MMMM yyyy");

                var _getFirstList = _getFirstLists.Select(s => s).Where(w => w.date_start.Value.Date >= SetStartDate.Date && w.date_start.Value.Date <= SetEndDate.Date).ToList();
                var __getFirstLists_Perdiem = _getFirstLists_Perdiems.Select(s => s).Where(w => w.date_start.Value.Date >= SetStartDate.Date && w.date_start.Value.Date <= SetEndDate.Date).ToList();

                if (!string.IsNullOrEmpty(Nametxt))
                {
                    //var getname = _getFirstList.Select(s => s.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + s.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en).ToList();
                    var setname = Nametxt.Trim().ToLower();
                    _getFirstList = _getFirstList.Where(w => setname.Contains((w.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en).Trim().ToLower())).ToList();
                    __getFirstLists_Perdiem = __getFirstLists_Perdiem.Where(w => setname.Contains((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en).Trim().ToLower())).ToList();

                }
                if (!string.IsNullOrEmpty(Lastnametxt))
                {
                    //var getname = _getFirstList.Select(s => s.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + s.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en).ToList();
                    var setname = Lastnametxt.Trim().ToLower();
                    _getFirstList = _getFirstList.Where(w => setname.Contains((w.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en).Trim().ToLower())).ToList();
                    __getFirstLists_Perdiem = __getFirstLists_Perdiem.Where(w => setname.Contains((w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en).Trim().ToLower())).ToList();

                }
                if (!string.IsNullOrEmpty(Statusslt))
                {
                    if (Statusslt != "All")
                    {
                        if (Statusslt == "D" || Statusslt == "S")
                        {
                            _getFirstList = _getFirstList.Select(s => s).Where(w => w.submit_status == Statusslt).ToList();
                            var forstatusperdiem = "";

                            if (Statusslt == "D")
                                forstatusperdiem = "N";
                            else if (Statusslt == "S")
                                forstatusperdiem = "Y";

                            __getFirstLists_Perdiem = __getFirstLists_Perdiem.Select(s => s).Where(w => w.submit_status == forstatusperdiem).ToList();
                        }
                        else
                        {
                            _getFirstList = _getFirstList.Select(s => s).Where(w => w.approve_status == Statusslt).ToList();
                            __getFirstLists_Perdiem = __getFirstLists_Perdiem.Select(s => s).Where(w => w.approve_status == Statusslt).ToList();
                        }
                    }

                }

                List<allforcheck> lstdataall = new List<allforcheck>();
                foreach (var ex in _getFirstList)
                {
                    allforcheck setnewdata = new allforcheck();
                    setnewdata.candidate_TraineeNumber = ex.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.candidate_TraineeNumber;
                    setnewdata.first_name_en = ex.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en;
                    setnewdata.last_name_en = ex.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                    setnewdata.date_start = ex.date_start.Value.ToString("dd MMM yyyy");
                    setnewdata.engagement_code = ex.Engagement_Code;
                    setnewdata.TM_Time_Type_Id = ex.TM_Time_Type.type_name_en;
                    setnewdata.hours = ex.hours.Replace(':', '.');
                    setnewdata.submit_status = ex.submit_status;
                    setnewdata.approve_status = ex.approve_status;
                    var get_mgr = sQuery.Where(w => w.Employeeno == ex.Approve_user).FirstOrDefault();
                    if (get_mgr != null)
                    {
                        setnewdata.approve_user = get_mgr.Employeename + " " + get_mgr.Employeesurname;
                    }
                    //setnewdata.approve_user = ex.Approve_user;
                    setnewdata.cost_center = ex.TimeSheet_Form.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en;

                    setnewdata.submit_date = ex.submit_date != null ? ex.submit_date.Value.ToString("dd MMM yyyy") : "";
                    if (ex.approve_status == "Y")
                        setnewdata.approve_date = ex.approve_date != null ? ex.approve_date.Value.ToString("dd MMM yyyy") : "";
                    else
                        setnewdata.approve_date = ex.trainee_update_date != null ? ex.trainee_update_date.Value.ToString("dd MMM yyyy") : "";

                    //setnewdata.client_name = ex.TimeSheet_Form.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.;

                    var check_ext = lst_Engagement_ext.Where(w => w.id == ex.Engagement_Code).ToList();
                    if (check_ext != null)
                    {
                        if (check_ext.Count > 0)
                        {
                            setnewdata.client_name = check_ext.First().comp;


                        }
                        else
                        {
                            var check_int = lst_Engagement_int.Where(w => w.id == ex.Engagement_Code).ToList();
                            if (check_int != null)
                            {
                                if (check_int.Count() > 0)
                                {
                                    setnewdata.client_name = check_int.First().text;

                                }
                            }
                            else
                            {
                                setnewdata.client_name = "NULL";
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(ex.review_user) && ex.review_date != null)
                    {
                        var get_update_user = sQuery.Where(w => w.Employeeno == ex.review_user).FirstOrDefault();
                        if (ex.review_date != null)
                            setnewdata.review_date = ex.review_date.Value.ToString("dd MMM yyyy");
                        else
                            setnewdata.review_date = "";

                        setnewdata.review_user = get_update_user.Employeename + " " + get_update_user.Employeesurname;
                    }
                    if (!String.IsNullOrEmpty(ex.paid_user) && ex.paid_date != null)
                    {
                        var get_update_user = sQuery.Where(w => w.Employeeno == ex.paid_user).FirstOrDefault();
                        if (ex.paid_date != null)
                            setnewdata.paid_date = ex.paid_date.Value.ToString("dd MMM yyyy");
                        else
                            setnewdata.paid_date = "";
                        setnewdata.paid_user = get_update_user.Employeename + " " + get_update_user.Employeesurname;
                    }


                    lstdataall.Add(setnewdata);

                }

                List<vPerdiem_Transport> lstdataallperdiem = new List<vPerdiem_Transport>();


                foreach (var ex in __getFirstLists_Perdiem)
                {
                    vPerdiem_Transport perdiem_Transports = new vPerdiem_Transport();
                    var get_mgr = sQuery.Where(w => w.Employeeno == ex.Approve_user).FirstOrDefault();
                    perdiem_Transports.Trainee_Code = ex.TM_PR_Candidate_Mapping.TM_Candidates.candidate_TraineeNumber.ToString();
                    perdiem_Transports.first_name_en = ex.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en;
                    perdiem_Transports.last_name_en = ex.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                    perdiem_Transports.date_start = ex.date_start.Value.ToString("dd MMM yyyy");
                    perdiem_Transports.date_end = ex.date_end.Value.ToString("dd MMM yyyy");
                    perdiem_Transports.Amount = ex.Amount;
                    perdiem_Transports.Engagement_Code = ex.Engagement_Code;
                    perdiem_Transports.submit_status = ex.submit_status;
                    perdiem_Transports.approve_status = ex.approve_status;

                    if (get_mgr != null)
                    {
                        perdiem_Transports.Approve_user = get_mgr.Employeename + " " + get_mgr.Employeesurname;
                    }
                    //setnewdata.approve_user = ex.Approve_user;
                    perdiem_Transports.Cost_Center = ex.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en;

                    perdiem_Transports.submit_date = ex.submit_date != null ? ex.submit_date.Value.ToString("dd MMM yyyy") : "";
                    if (ex.approve_status == "Y")
                        perdiem_Transports.approve_date = ex.approve_date != null ? ex.approve_date.Value.ToString("dd MMM yyyy") : "";
                    else
                        perdiem_Transports.approve_date = ex.trainee_update_date != null ? ex.trainee_update_date.Value.ToString("dd MMM yyyy") : "";

                    //setnewdata.client_name = ex.TimeSheet_Form.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.;

                    var check_ext = lst_Engagement_ext.Where(w => w.id == ex.Engagement_Code).ToList();
                    if (check_ext != null)
                    {
                        if (check_ext.Count > 0)
                        {
                            perdiem_Transports.client_name = check_ext.First().comp;


                        }
                        else
                        {
                            var check_int = lst_Engagement_int.Where(w => w.id == ex.Engagement_Code).ToList();
                            if (check_int.Count() > 0)
                            {
                                perdiem_Transports.client_name = check_int.First().text;

                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(ex.review_user) && ex.review_date != null)
                    {
                        var get_update_user = sQuery.Where(w => w.Employeeno == ex.review_user).FirstOrDefault();


                        if (ex.review_date != null)
                            perdiem_Transports.review_date = ex.review_date.Value.ToString("dd MMM yyyy");
                        else
                            perdiem_Transports.review_date = DateTime.Now.ToString("dd MMM yyyy");


                        perdiem_Transports.review_user = get_update_user.Employeename + " " + get_update_user.Employeesurname;
                    }
                    if (!String.IsNullOrEmpty(ex.paid_user) && ex.paid_date != null)
                    {
                        var get_update_user = sQuery.Where(w => w.Employeeno == ex.paid_user).FirstOrDefault();
                        if (ex.paid_date != null)
                            perdiem_Transports.paid_date = ex.paid_date.Value.ToString("dd MMM yyyy");
                        else
                            perdiem_Transports.paid_date = "";

                        perdiem_Transports.paid_user = get_update_user.Employeename + " " + get_update_user.Employeesurname;
                    }
                    lstdataallperdiem.Add(perdiem_Transports);

                }


                result.lstData = lstdataall;
                result.lstData_perdiem = lstdataallperdiem;
            }
            catch (Exception ex)
            {
            }
            return Json(new
            {
                result
            });

        }

    }
}