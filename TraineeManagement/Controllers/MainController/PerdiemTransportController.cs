using HumanCapitalManagement.Models.TraineeSite;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TraineeSite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraineeManagement.App_Start;
using TraineeManagement.Controllers.CommonControllers;
using TraineeManagement.ViewModels.CommonVM;
using TraineeManagement.ViewModels.MainVM;
using static TraineeManagement.Controllers.MainController.TimeSheetController;

namespace TraineeManagement.Controllers.MainController
{
    public class PerdiemTransportController : BaseController
    {
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private Perdiem_TransportService _Perdiem_TransportService;

        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public PerdiemTransportController(TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , Perdiem_TransportService Perdiem_TransportService
            , TM_CandidatesService TM_CandidatesService
            , MailContentService MailContentService
                   )
        {
            _Perdiem_TransportService = Perdiem_TransportService;
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;
        }

        public List<Engagement_PR> lst_Engagement = new List<Engagement_PR>();
        public class data_sorce
        {
            public string Edit { get; set; }
            public int Id { get; set; }
            public int? seq { get; set; }
            public string date_start { get; set; }
            public string date_end { get; set; }

            public string daterange { get; set; }
            public string Engagement_Code { get; set; }

            public string remark { get; set; }

            public string Type_of_withdrawal { get; set; }

            public string Company { get; set; }

            public string Reimbursable { get; set; }

            public string Business_Purpose { get; set; }

            public string Description { get; set; }
            public decimal? Amount { get; set; }

            public string active_status { get; set; }
            public DateTime? trainee_create_date { get; set; }
            public int? trainee_create_user { get; set; }
            public DateTime? trainee_update_date { get; set; }
            public int? trainee_update_user { get; set; }

            public string submit_status { get; set; }

            public string approve_status { get; set; }

            public string Approve_user { get; set; }

            public string mgr_user_id { get; set; }
            public string mgr_user_no { get; set; }
            public string mgr_user_name { get; set; }
            public string mgr_user_rank { get; set; }
            public string mgr_unit_name { get; set; }
            public string status { get; set; }

            public List<data_sorce> lstData { get; set; }
        }

        public class vdata_sorce_Return : CResutlWebMethod
        {
            public List<data_sorce> lstData { get; set; }
        }
        // GET: PerdiemTransport

        public ActionResult PerdiemTransportView()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            data_sorce result = new data_sorce();


            New_HRISEntities dbHr = new New_HRISEntities();
            List<Employee> sQuery = dbHr.Employee.ToList();
            var engm = GetEngagement("");
            var getdata = _Perdiem_TransportService.GetDataForSelect().Where(w => w.trainee_create_user == CGlobal.UserInfo.nUserId).ToList();
            List<data_sorce> lst_setdata = new List<data_sorce>();
            foreach (var x in getdata)
            {
                data_sorce setdata = new data_sorce();
                setdata.Id = x.Id;
                if (x.approve_status == "N" || x.approve_status == "D")
                {
                    setdata.Edit = @"<a href=""#"" class=""btn btn-animated btn-sm btn-success"" href=""#"" onclick=""EditData('" + x.Id + @"');return false;"">Edit <i class=""fa fa-edit""></i></a>";
                }
                else
                {
                    setdata.Edit = @"<a href=""#"" class=""btn btn-animated btn-sm btn-warning"" href=""#"" onclick=""EditData('" + x.Id + @"');return false;"">View <i class=""fa fa-eye""></i></a>";
                }
                setdata.Type_of_withdrawal = x.Type_of_withdrawal;
                setdata.Company = x.Company;
                setdata.Reimbursable = x.Reimbursable;
                setdata.date_start = Convert.ToDateTime(x.date_start).ToString("dd/MM/yyyy");
                setdata.date_end = Convert.ToDateTime(x.date_end).ToString("dd/MM/yyyy");
                setdata.daterange = Convert.ToDateTime(x.date_start).ToString("dd MMM yyyy") + " - " + Convert.ToDateTime(x.date_end).ToString("dd MMM yyyy");
                setdata.Business_Purpose = x.Business_Purpose;
                setdata.Amount = x.Amount;

                setdata.mgr_user_no = x.Approve_user;

                var get = lst_Engagement.Where(w => w.id == x.Engagement_Code).FirstOrDefault();
                if (get != null)
                {
                    setdata.Engagement_Code = get.id + "|" + get.text;
                }
                setdata.Description = x.Description;

                if (!string.IsNullOrEmpty(x.Approve_user))
                {
                    var getall = sQuery.Where(w => w.Employeeno == x.Approve_user).FirstOrDefault();
                    setdata.Approve_user = getall.Employeename + " " + getall.Employeesurname;
                }

                setdata.approve_status = x.approve_status;

                lst_setdata.Add(setdata);
            }
            result.lstData = lst_setdata.OrderBy(o => o.trainee_update_date).ToList();
            return View(result);
        }

        [HttpPost]
        public ActionResult SavePerdiem_Transport(data_sorce ItemData, string TypeSave)
        {
            List<Perdiem_Transport> datasave = new List<Perdiem_Transport>();
            vdata_sorce_Return result = new vdata_sorce_Return();
            try
            {
                if (TypeSave != "S")
                {

                    var get_map = _TM_PR_Candidate_MappingService.GetDataForSelect();
                    Perdiem_Transport data = new Perdiem_Transport();
                    data.Id = ItemData.Id;
                    data.Type_of_withdrawal = ItemData.Type_of_withdrawal;
                    data.Company = ItemData.Company;
                    data.Reimbursable = ItemData.Reimbursable;
                    data.Business_Purpose = ItemData.Business_Purpose;
                    var startdate = ItemData.daterange.Trim().Split('-')[0].Split('/');
                    var enddate = ItemData.daterange.Trim().Split('-')[1].Split('/');
                    data.date_start = Convert.ToDateTime(Convert.ToDateTime(startdate[1] + "-" + startdate[0] + "-" + startdate[2]));
                    data.date_end = Convert.ToDateTime(Convert.ToDateTime(enddate[1] + "-" + enddate[0] + "-" + enddate[2]));
                    data.Description = ItemData.Description;
                    if (ItemData.Amount != null || ItemData.Amount != 0)
                    {
                        data.Amount = ItemData.Amount;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Plese check data again !";
                        return Json(new
                        {
                            result
                        });
                    }
                    data.Engagement_Code = ItemData.Engagement_Code;
                    data.Approve_user = ItemData.mgr_user_no;
                    data.approve_status = "D";
                    data.trainee_create_date = DateTime.Now;
                    data.trainee_create_user = CGlobal.UserInfo.nUserId;
                    data.trainee_update_date = DateTime.Now;
                    data.trainee_update_user = CGlobal.UserInfo.nUserId;
                    data.active_status = "Y";
                    data.submit_status = "N";

                    data.TM_PR_Candidate_Mapping_Id = get_map.Where(w => w.TM_Candidates.Id == CGlobal.UserInfo.nUserId).OrderByDescending(o => o.trainee_start).FirstOrDefault().Id;

                    datasave.Add(data);
                }
                else
                {
                    var dataget_ = _Perdiem_TransportService.GetDataForSelect().Where(w => w.trainee_create_user.ToString() == CGlobal.UserInfo.UserId && w.active_status == "Y" && w.submit_status == "N" && w.approve_status == "D").ToList();
                    foreach (var setsubmit in dataget_)
                    {
                        setsubmit.submit_status = "Y";
                        setsubmit.approve_status = "N";
                        setsubmit.submit_date = DateTime.Now;
                        datasave.Add(setsubmit);
                    }

                }
                var save = 0;
                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                foreach (var tosave_ in datasave)
                {
                    save = _Perdiem_TransportService.CreateNewOrUpdate(tosave_);
                    if (TypeSave == "S")
                    {


                        var get_mgr = sQuery.Where(w => w.Employeeno == tosave_.Approve_user).FirstOrDefault();

                        string msg = "";

                        var Mail1 = _MailContentService.GetMailContent("Trainee Perdeim/Transport Submit", "Y").FirstOrDefault();
                        string pathUrl = Url.Action("TraineePerdiemTransportList", "TraineePerdiemTransport", null, Request.Url.Scheme);
                        //production
                        pathUrl = pathUrl.Replace("TraineeEvaluation", "application/HR/HumanCapitalManagement");

                        string genlink = HCMFunc.GetUrl(Mail1.mail_type, CGlobal.UserInfo.UserId + "", pathUrl, get_mgr.Employeeno);

                        if (save > 0)
                        {
                            string sContent = Mail1.content;
                            sContent = (sContent + "").Replace("$emailto", get_mgr.Email);
                            sContent = (sContent + "").Replace("$pm", get_mgr.Employeename + " " + get_mgr.Employeesurname);
                            sContent = (sContent + "").Replace("$trainee", CGlobal.UserInfo.FullName);
                            sContent = (sContent + "").Replace("$linkto", genlink);
                            var objMail = new vObjectMail_Send();

                            objMail.mail_from = "hcmthailand@kpmg.co.th";
                            objMail.title_mail_from = "HCM System";
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
                var getdata = _Perdiem_TransportService.GetDataForSelect().Where(w => w.trainee_create_user == CGlobal.UserInfo.nUserId).ToList();
                List<data_sorce> lst_setdata = new List<data_sorce>();
                foreach (var x in getdata)
                {
                    data_sorce setdata = new data_sorce();
                    setdata.Id = x.Id;
                    if (x.submit_status == "N")
                    {
                        setdata.Edit = @"<a href=""#"" class=""btn btn-animated btn-sm btn-primary"" href=""#"" onclick=""EditData('" + x.Id + @"');return false;"">Edit <i class=""fa fa-edit""></i></a>";
                    }
                    else
                    {
                        setdata.Edit = @"<a href=""#"" class=""btn btn-animated btn-sm btn-warning"" href=""#"" onclick=""EditData('" + x.Id + @"');return false;"">View <i class=""fa fa-edit""></i></a>";
                    }
                    setdata.Type_of_withdrawal = x.Type_of_withdrawal;
                    setdata.Company = x.Company;
                    setdata.date_start = Convert.ToDateTime(x.date_start).ToString("MM/dd/yyyy");
                    setdata.date_end = Convert.ToDateTime(x.date_end).ToString("MM/dd/yyyy");
                    setdata.daterange = Convert.ToDateTime(x.date_start).ToString("dd MMM yyyy") + " - " + Convert.ToDateTime(x.date_end).ToString("dd MMM yyyy");
                    setdata.Business_Purpose = x.Business_Purpose;
                    setdata.Amount = x.Amount;
                    setdata.Engagement_Code = x.Engagement_Code;
                    setdata.Description = x.Description;
                    setdata.Approve_user = x.Approve_user;
                    setdata.approve_status = x.approve_status;

                    lst_setdata.Add(setdata);
                }
                result.lstData = lst_setdata;



                result.Status = SystemFunction.process_Success;
                result.Msg = "Success, Submit Success.";

            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, " + ex.Message;
            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult Update_Cancle(string Id)
        {
            vdata_sorce_Return result = new vdata_sorce_Return();

            Perdiem_Transport data = new Perdiem_Transport();
            data.Id = SystemFunction.GetIntNullToZero(Id);
            data.approve_status = "C";
            data.trainee_update_date = DateTime.Now;

            var update = _Perdiem_TransportService.UpdateApprove_Status(data);

            if (update > 0)
            {
                result.Status = SystemFunction.process_Success;
                result.Msg = "Success, Withdrawn.";
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
        public JsonResult GetEngagement(string university_id)
        {
            Viztopia.ViztopiaSoapClient wsViztp = new Viztopia.ViztopiaSoapClient();

            string ChargeCode = "";
            string CompName = "";

            var data = wsViztp.GetAllEngagementFromSSA("", "", "2,3", "", "", "", "", "", "", "", "", "");

            lst_Engagement = new List<Engagement_PR>();
            lst_Engagement = data.AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("EngagementNo"), text = dataRow.Field<string>("EngagementName"), value = dataRow.Field<string>("EngagementManagerNo") }).ToList();


            var indata = wsViztp.Get_Internal_Chargecode("", "");
            var a = new List<Engagement_PR>();
            a = indata.Tables[0].AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("Code"), text = dataRow.Field<string>("Description") }).ToList();


            lst_Engagement.AddRange(a);


            return Json(lst_Engagement, JsonRequestBehavior.AllowGet);
        }

    }
}