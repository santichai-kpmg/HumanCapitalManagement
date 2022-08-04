using DevExpress.Office.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.TraineeSite;
using HumanCapitalManagement.Report.DataSet.Trainee.Intern_Allowance;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TraineeSite;
using HumanCapitalManagement.ViewModel.MainVM;
using HumanCapitalManagement.ViewModel.Trainee;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static HumanCapitalManagement.Controllers.TraineeController.TraineePerdiemTransportController;
using static HumanCapitalManagement.ViewModel.Trainee.vPerdiemTransport;
using static HumanCapitalManagement.ViewModel.Trainee.vTraineePayment;

namespace HumanCapitalManagement.Controllers.TraineeController
{
    public class TraineePayMentController : BaseController
    {
        #region
        // GET: TraineePayMent
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TimeSheet_FormService _TimeSheet_FormService;
        private TimeSheet_DetailService _TimeSheet_DetailService;
        private TM_Time_TypeService _TM_Time_TypeService;
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        private TM_Company_TraineeService _TM_Company_TraineeService;
        private Perdiem_TransportService _Perdiem_TransportService;
        public TraineePayMentController(TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TimeSheet_FormService TimeSheet_FormService
            , TimeSheet_DetailService TimeSheet_DetailService
            , TM_Time_TypeService TM_Time_TypeService
            , TM_CandidatesService TM_CandidatesService
            , MailContentService MailContentService
            , Perdiem_TransportService Perdiem_TransportService
            , TM_Company_TraineeService TM_Company_TraineeService

                   )
        {
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TimeSheet_FormService = TimeSheet_FormService;
            _TimeSheet_DetailService = TimeSheet_DetailService;
            _TM_Time_TypeService = TM_Time_TypeService;
            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;
            _Perdiem_TransportService = Perdiem_TransportService;
            _TM_Company_TraineeService = TM_Company_TraineeService;
        }
        #endregion

        #region
        public class dsSummary
        {

            public string company { get; set; }
            public string cost_center { get; set; }
            public string trainee_code { get; set; }
            public string name { get; set; }
            public string client_name { get; set; }
            public string code { get; set; }
            public decimal client { get; set; }
            public decimal office { get; set; }
            public decimal nomal_hours { get; set; }
            public decimal nomal_amount { get; set; }
            public decimal nomal_amount_hd { get; set; }
            public decimal ot_amount_hd { get; set; }
            public decimal ot_hours { get; set; }
            public decimal ot_amount { get; set; }
            public decimal grand_total { get; set; }
        }

        public List<Engagement_PR> lst_Engagement = new List<Engagement_PR>();

        #endregion

        #region view
        public ActionResult TraineePayMent()
        {
            List<ResultLine> result = new List<ResultLine>();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            //ExportListFromTsv();
            string sSess = Request.Form["sSess"];
            Session[sSess] = null;
            return View(result);
        }

        #endregion view

        #region

        [HttpPost]
        public ActionResult TraineePayMentList(string preoidtxt, string startD, string endD, string name, string group, string status)
        {
            ResultLine result = new ResultLine();

            if (string.IsNullOrEmpty(startD) || string.IsNullOrEmpty(endD))
            {
                return Json(new
                {
                    result
                });
            }
            else if (SystemFunction.ConvertStringToDateTime(startD, "", "dd MMMM yyyy") > SystemFunction.ConvertStringToDateTime(endD, "", "dd MMMM yyyy"))
            {
                return Json(new
                {
                    result
                });
            }
            DateTime SetStartDate = SystemFunction.ConvertStringToDateTime(startD, "", "dd MMMM yyyy");
            DateTime SetEndDate = SystemFunction.ConvertStringToDateTime(endD, "", "dd MMMM yyyy");



            ResultLine objFile = new ResultLine();
            string sSess = Request.Form["sSess"];
            objFile = Session[sSess] as ResultLine;

            var companylist = _TM_Company_TraineeService.GetDataForSelect();

            Viztopia.ViztopiaSoapClient wsViztp = new Viztopia.ViztopiaSoapClient();
            List<Engagement_PR> lst_Engagement_ext = new List<Engagement_PR>();
            List<Engagement_PR> lst_Engagement_int = new List<Engagement_PR>();

            var extdata = wsViztp.GetAllEngagementFromSSA("", "", "2,3", "", "", "", "", "", "", "", "", "");
            lst_Engagement_ext = extdata.AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("EngagementNo"), text = dataRow.Field<string>("EngagementName"), comp = dataRow.Field<string>("ClientName") }).ToList();

            var intdata = wsViztp.Get_Internal_Chargecode("", "");
            lst_Engagement_int = intdata.Tables[0].AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("Code"), text = dataRow.Field<string>("Description"), comp = dataRow.Field<string>("CompDesc") }).ToList();



            wsHRIS.HRISSoap Hris = new wsHRIS.HRISSoapClient();
            var holiday = Hris.Get_Holiday(DateTime.Now.Year.ToString());

            var _getdetail = _TimeSheet_DetailService.GetDataForSelect().Where(w => w.TM_Time_Type.type_short_name_en == "NH" || w.TM_Time_Type.type_short_name_en == "TN" || w.TM_Time_Type.type_short_name_en == "OT").ToList();

          
            
            //if (string.IsNullOrEmpty(preoidtxt))
            //{
            //    preoidtxt = DateTime.Now.ToString("MMMM yyyy");
            //}

            _getdetail = _getdetail.Select(s => s).Where(w => w.date_start.Value.Date >= SetStartDate.Date && w.date_start.Value.Date <= SetEndDate.Date).ToList();
            var get_sub = new List<TimeSheet_Detail>();
            //add condition
            if (!string.IsNullOrEmpty(name))
            {
                var getname = _getdetail.Select(s => s.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + s.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en).ToList();
                var setname = name.Trim().ToLower();
                _getdetail = _getdetail.Where(w => setname.Contains((w.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + w.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en).Trim().ToLower())).ToList();

            }
            if (!string.IsNullOrEmpty(group))
            {
                _getdetail = _getdetail.Select(s => s).Where(w => (w.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + w.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en).Trim().ToLower() == name.Trim().ToLower()).ToList();

            }
            if (!string.IsNullOrEmpty(status))
            {
                _getdetail = _getdetail.Select(s => s).Where(w => w.approve_status == status).ToList();

            }
            List<vTimeSheet_Detail> new_TimeSheet_Detail = new List<vTimeSheet_Detail>();

            var vTimeSheet_Detail_ = _getdetail.Select(s => new vTimeSheet_Detail()
            {
                Id = s.Id.ToString()
            });
            new_TimeSheet_Detail = vTimeSheet_Detail_.ToList();
            result.lstTimeSheet_Detail_All = new_TimeSheet_Detail;

            #region OT
            var _getdetail_OT = _getdetail.Where(w => w.TM_Time_Type.type_short_name_en == "OT");
            var _groupname_OT = _getdetail_OT.GroupBy(g => g.TimeSheet_Form.TM_PR_Candidate_Mapping).ToList();


            List<ResultLine_OT> lst_ot = new List<ResultLine_OT>();

            foreach (var exp in _groupname_OT)
            {

                ResultLine_OT subresult = new ResultLine_OT();

                var st = exp.GroupBy(g => g.date_start.Value.ToString("dd/MM/yyyy")).Select(s => new TimeSheet_Detail()
                {
                    title = s.Key,
                    hours = s.Sum(g => SystemFunction.GetNumberNullToZero(g.hours.Replace(':', '.') + "")).ToString(),
                }).ToList();

                string fname = exp.Select(s => s.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en).FirstOrDefault();
                string lname = exp.Select(s => s.TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en).FirstOrDefault();
                decimal totalhours = 0;
                decimal rateone = 0;
                decimal ratetwo = 0;
                decimal ratethree = 0;

                decimal daily_wage = exp.Select(s => s.TimeSheet_Form.TM_PR_Candidate_Mapping.daily_wage.Value).FirstOrDefault();
                decimal daily_wagesub = daily_wage / 8;

                subresult.name = fname + " " + lname;
                var _date_start = DateTime.Now;
                foreach (var subexp in st)
                {

                    string _hours = subexp.hours;
                    _date_start = DateTime.ParseExact(subexp.title, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var valhours = SystemFunction.GetNumberNullToZero(_hours + "");
                    totalhours += valhours;
                    //check date set rate

                    if (IsBusinessDay(_date_start, "", _date_start.ToString("yyyy")))
                    {
                        rateone = rateone + valhours;
                    }
                    else
                    {
                        var settime = valhours;
                        if (settime <= 8)
                        {
                            ratetwo += valhours;
                        }
                        else
                        {
                            var contime = settime - 8;
                            ratetwo += settime - contime;
                            ratethree += contime;

                        }

                    }

                }

                decimal total = 0;

                //r1
                total += (daily_wagesub * (rateone * (decimal)1.5));


                //r2
                total += (daily_wagesub * (ratetwo * 2));


                //r3
                total += (daily_wagesub * (ratethree * 3));

                subresult.daily_wage = daily_wage.ToString("#,##0.00");
                subresult.month = _date_start.ToString("MMMM");
                subresult.rate1 = rateone.ToString();
                subresult.rate2 = ratetwo.ToString();
                subresult.rate3 = ratethree.ToString();
                subresult.totalhrs = (rateone + ratetwo + ratethree).ToString("#,##0.00");
                subresult.total = Math.Round(total, MidpointRounding.AwayFromZero).ToString("#,##0.00");
                lst_ot.Add(subresult);
                //Math.Round(, MidpointRounding.AwayFromZero)

            }
            result.lstData_OT = lst_ot.OrderBy(o => o.name).ToList();

            #endregion OT

            #region Perdiem 

            var getPerdiem_Transport = _Perdiem_TransportService.GetDataForSelect().Where(w => w.approve_status == status).ToList();
            var lst_perdiem_transport = new List<Perdiem_Transport>();

            if (!String.IsNullOrEmpty(name))
            {
                var nameofperdiem = name.Trim().ToLower();
                lst_perdiem_transport = getPerdiem_Transport.Where(w => nameofperdiem.Contains((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en).Trim().ToLower())).ToList();
            }
            else
            {
                lst_perdiem_transport = getPerdiem_Transport;
            }
            //&& (w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en).Trim().ToLower() == nameofperdiem
            lst_perdiem_transport = lst_perdiem_transport.Where(w => w.trainee_create_date.Value.Date >= SetStartDate.Date && w.trainee_create_date.Value.Date <= SetEndDate.Date).ToList();


            List<vPerdiem_Transport> new_Perdiem_Transport = new List<vPerdiem_Transport>();
            new_Perdiem_Transport = lst_perdiem_transport.Select(s => new vPerdiem_Transport()
            {
                Id = s.Id
            }).ToList();
            result.lstPerdiem_Transport_All = new_Perdiem_Transport;


            var slst_perdiem_transport = lst_perdiem_transport.GroupBy(g => new { g.trainee_create_user }).Select(
                s => new ResultLine_Sum_Perdiem()
                {
                    Name = s.FirstOrDefault().TM_PR_Candidate_Mapping.TM_Candidates.first_name_en,
                    //trainee_code = s.FirstOrDefault().trainee_code,
                    //name = s.FirstOrDefault().name,
                    //code = s.FirstOrDefault().code,
                    //client_name = s.FirstOrDefault().client_name,
                    //ot_hours = s.Sum(x => SystemFunction.GetNumberNullToZero(x.ot_hours)) == 0 ? "" : s.Sum(x => SystemFunction.GetNumberNullToZero(x.ot_hours)).ToString(),
                    //nomal_hours = s.Sum(x => SystemFunction.GetNumberNullToZero(x.nomal_hours)) == 0 ? "" : s.Sum(x => SystemFunction.GetNumberNullToZero(x.nomal_hours)).ToString(),
                    //ot_amount = s.Sum(x => SystemFunction.GetNumberNullToZero(x.ot_amount)) == 0 ? "" : s.Sum(x => SystemFunction.GetNumberNullToZero(x.ot_amount)).ToString("#,##0.00"),
                    //nomal_amount = s.Sum(x => SystemFunction.GetNumberNullToZero(x.nomal_amount)) == 0 ? "" : s.Sum(x => SystemFunction.GetNumberNullToZero(x.nomal_amount)).ToString("#,##0.00"),
                    //grand_total = s.Sum(x => SystemFunction.GetNumberNullToZero(x.grand_total)).ToString("#,##0.00"),
                    //client = s.Sum(x => SystemFunction.GetNumberNullToZero(x.client)) == 0 ? "" : s.Sum(x => SystemFunction.GetNumberNullToZero(x.client)).ToString("#,##0.00"),
                    Amount = s.Sum(x => x.Amount) == 0 ? 0 : s.Sum(x => x.Amount)

                }).ToList();


            //        var desired = result.lstData_All.Join(
            //lst_perdiem_transport,
            //a => a.trainee_create_user,
            //b => b.trainee_create_user,
            //(a, b) => new { AValue = a, BValue = b });



            var engm = GetEngagement("");
            List<ResultLine_Sum_Perdiem> lst_pt = new List<ResultLine_Sum_Perdiem>();

            foreach (var pt in lst_perdiem_transport)
            {
                var get_can = _TM_CandidatesService.Find(pt.trainee_create_user.Value);
                ResultLine_Sum_Perdiem data_pt = new ResultLine_Sum_Perdiem();
                data_pt.Id = pt.Id;
                data_pt.Type_of_withdrawal = pt.Type_of_withdrawal;
                data_pt.Cost_Center = get_can.TM_PR_Candidate_Mapping.OrderByDescending(o => o.Id).FirstOrDefault().TM_Divisions != null ? get_can.TM_PR_Candidate_Mapping.OrderByDescending(o => o.Id).FirstOrDefault().TM_Divisions.division_name_en : get_can.TM_PR_Candidate_Mapping.OrderByDescending(o => o.create_date).Select(s => s.PersonnelRequest).Select(s => s.TM_Divisions).Select(s => s.division_name_en).FirstOrDefault(); 
                data_pt.Trainee_Code = get_can.candidate_TraineeNumber;
                data_pt.Name = get_can.first_name_en + " " + get_can.last_name_en;
                data_pt.Amount = pt.Amount;
                data_pt.trainee_create_date = pt.trainee_create_date;
                data_pt.Create_Date = pt.date_start.Value.ToString("dd MMM yyyy") + "-" + pt.date_end.Value.ToString("dd MMM yyyy");
                var get = lst_Engagement.Where(w => w.id == pt.Engagement_Code).FirstOrDefault();
                if (get != null)
                {
                    data_pt.Engagement_Code = get.id;
                    data_pt.Engagement_Name = get.comp;
                }


                lst_pt.Add(data_pt);
            }



            result.lstData_PerdiemTransport = lst_pt.OrderBy(o => o.Cost_Center).ThenBy(s => s.Trainee_Code).ThenBy(t => t.date_start).ToList();
            #endregion Perdiem

            #region All and Autopay
            decimal Hstandard = 0;
            decimal Hot = 0;
            decimal Htotal = 0;
            decimal Astandard = 0;
            decimal Aot = 0;
            decimal Aperdiem = 0;
            decimal Atotal = 0;
            var _getdetail_All = _getdetail;
            var _groupname_All = _getdetail_All.GroupBy(g => g.TimeSheet_Form.TM_PR_Candidate_Mapping).ToList();
            var _groupPT = lst_perdiem_transport.GroupBy(g => g.trainee_create_user).ToList();


            List<ResultLine_All> lst_all = new List<ResultLine_All>();
            List<ResultLine_AutoPay> lst_autopay = new List<ResultLine_AutoPay>();
            var runnum = 1;
            List<Perdiem_Transport> withoutperdiem = new List<Perdiem_Transport>();
            foreach (var forall in _groupname_All)
            {
                var checekperdiem = lst_perdiem_transport.Where(w => w.trainee_create_user == forall.FirstOrDefault().trainee_create_user).ToList();
                withoutperdiem.AddRange(checekperdiem);



                var getcandidate_ = forall.Key.TM_Candidates;
                ResultLine_All subresult_all = new ResultLine_All();
                subresult_all.id = (runnum += 1).ToString();
                var get_company = getcandidate_.TM_PR_Candidate_Mapping.Select(s => s.PersonnelRequest).Select(s => s.TM_Divisions).Select(s => s.division_code).FirstOrDefault();
                //subresult_all.id =
                subresult_all.company = companylist.Where(w => w.UnitGroupID.Contains(get_company)).Select(s => s.Company_Short).FirstOrDefault();
                subresult_all.cost_center = forall.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Divisions != null ? forall.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Divisions.division_name_en : forall.First().TimeSheet_Form.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en ;
                subresult_all.trainee_code = getcandidate_.candidate_TraineeNumber;
                subresult_all.name = getcandidate_.first_name_en + " " + getcandidate_.last_name_en;
                subresult_all.hours_standard = forall.Where(w => w.TM_Time_Type.type_short_name_en == "NH" || w.TM_Time_Type.type_short_name_en == "TN").Sum(s => SystemFunction.GetNumberNullToZero(s.hours.Replace(':', '.') + "")).ToString();
                subresult_all.hours_ot = forall.Where(w => w.TM_Time_Type.type_short_name_en == "OT").Sum(s => SystemFunction.GetNumberNullToZero(s.hours.Replace(':', '.') + "")).ToString();
                subresult_all.hours_total = (SystemFunction.GetNumberNullToZero(subresult_all.hours_standard) + SystemFunction.GetNumberNullToZero(subresult_all.hours_ot)).ToString("#0.00");

                //var sumNormalnew = forall.Where(w => w.TM_Time_Type.type_short_name_en == "NH" || w.TM_Time_Type.type_short_name_en == "TN").Sum(s => (Math.Round(Convert.ToDouble(SystemFunction.GetNumberNullToZero(s.hours.Replace(':', '.') + "") * (forall.Key.daily_wage / 8)), MidpointRounding.AwayFromZero))).ToString();
                var sumnormallist = forall.Where(w => w.TM_Time_Type.type_short_name_en == "NH" || w.TM_Time_Type.type_short_name_en == "TN").ToList();
                var sumnormalgroup = sumnormallist.GroupBy(g => g.Engagement_Code).ToList();

                List<decimal> newlist = new List<decimal>();
                foreach (var exgroup in sumnormalgroup)
                {
                    decimal sumall = 0;
                    foreach (var expsub in exgroup)
                    {
                        sumall += SystemFunction.GetNumberNullToZero(expsub.hours.Replace(':', '.') + "");
                    }
                    newlist.Add(sumall);
                }

                decimal dalywag = (forall.Key.daily_wage.Value / 8);
                decimal sumstandard = 0;
                foreach (var s in newlist)
                {
                    sumstandard += s * dalywag;
                }


                subresult_all.amount_standard = Math.Round(SystemFunction.GetNumberNullToZero(sumstandard.ToString()), MidpointRounding.AwayFromZero).ToString("#,##0.00");
                subresult_all.amount_ot = Math.Round(SystemFunction.GetNumberNullToZero(lst_ot.Where(w => w.name == subresult_all.name).Select(s => s.total).FirstOrDefault()), MidpointRounding.AwayFromZero).ToString("#,##0.00");
                subresult_all.amount_perdiem = Math.Round(lst_perdiem_transport.Where(w => w.trainee_create_user == forall.FirstOrDefault().trainee_create_user).Sum(s => s.Amount).Value, MidpointRounding.AwayFromZero).ToString("#,##0.00");
                subresult_all.amount_total = Math.Round((SystemFunction.GetNumberNullToZero(subresult_all.amount_standard) + SystemFunction.GetNumberNullToZero(subresult_all.amount_ot) + SystemFunction.GetNumberNullToZero(subresult_all.amount_perdiem)), MidpointRounding.AwayFromZero).ToString("#,##0.00");

                subresult_all.bank = getcandidate_.candidate_BankAccountBranchNumber;
                subresult_all.id_book_bank = getcandidate_.candidate_BankAccountNumber;
                subresult_all.note = "";
                subresult_all.trainee_create_user = getcandidate_.Id;

                lst_all.Add(subresult_all);

                ResultLine_AutoPay subresult_autopay = new ResultLine_AutoPay();
                subresult_autopay.Bank_Code = subresult_all.bank;
                subresult_autopay.Account_Number = subresult_all.id_book_bank;
                subresult_autopay.Vendor_Name = subresult_all.name;
                subresult_autopay.Amount = subresult_all.amount_total;
                subresult_autopay.Bene_Ref = subresult_all.trainee_code;
                subresult_autopay.WHT = "N";
                subresult_autopay.Advice = "N";
                subresult_autopay.SMS = "N";
                subresult_autopay.Cost_Center = subresult_all.cost_center;
                subresult_autopay.Company = subresult_all.company;

                lst_autopay.Add(subresult_autopay);
            }

            var outlistper = lst_perdiem_transport.Where(w => !withoutperdiem.Contains(w)).ToList();
            var groupper = outlistper.GroupBy(g => g.trainee_create_user).ToList();
            foreach (var forperdiem in groupper)
            {
                var getcandidate_ = forperdiem.First().TM_PR_Candidate_Mapping.TM_Candidates;
                ResultLine_All subresult_all = new ResultLine_All();
                subresult_all.id = (runnum += 1).ToString();
                var get_company = getcandidate_.TM_PR_Candidate_Mapping.Select(s => s.PersonnelRequest).Select(s => s.TM_Divisions).Select(s => s.division_name_en).FirstOrDefault();
                //subresult_all.id =
                subresult_all.company = companylist.Where(w => w.UnitGroupName.Contains(get_company)).Select(s => s.Company_Short).FirstOrDefault();
                //subresult_all.cost_center = forperdiem.First().TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en;
                subresult_all.cost_center = forperdiem.First().TM_PR_Candidate_Mapping.TM_Divisions != null ? forperdiem.First().TM_PR_Candidate_Mapping.TM_Divisions.division_name_en : forperdiem.First().TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en;
                subresult_all.trainee_code = getcandidate_.candidate_TraineeNumber;
                subresult_all.name = getcandidate_.first_name_en + " " + getcandidate_.last_name_en;
                subresult_all.hours_standard = "0.00";
                subresult_all.hours_ot = "0.00";
                subresult_all.hours_total = "0.00";

                subresult_all.amount_standard = "0.00";
                subresult_all.amount_ot = "0.00";
                subresult_all.amount_perdiem = Math.Round(forperdiem.Sum(s => s.Amount).Value, MidpointRounding.AwayFromZero).ToString("#,##0.00");
                subresult_all.amount_total = Math.Round((SystemFunction.GetNumberNullToZero(subresult_all.amount_standard) + SystemFunction.GetNumberNullToZero(subresult_all.amount_ot) + SystemFunction.GetNumberNullToZero(subresult_all.amount_perdiem)), MidpointRounding.AwayFromZero).ToString("#,##0.00");

                subresult_all.bank = getcandidate_.candidate_BankAccountBranchNumber;
                subresult_all.id_book_bank = getcandidate_.candidate_BankAccountNumber;
                subresult_all.note = "";
                subresult_all.trainee_create_user = getcandidate_.Id;

                lst_all.Add(subresult_all);

                ResultLine_AutoPay subresult_autopay = new ResultLine_AutoPay();
                subresult_autopay.Bank_Code = "004";
                subresult_autopay.Account_Number = subresult_all.id_book_bank;
                subresult_autopay.Vendor_Name = subresult_all.name;
                subresult_autopay.Amount = subresult_all.amount_total;
                subresult_autopay.Bene_Ref = subresult_all.trainee_code;
                subresult_autopay.WHT = "N";
                subresult_autopay.Advice = "N";
                subresult_autopay.SMS = "N";
                subresult_autopay.Cost_Center = subresult_all.cost_center;
                subresult_autopay.Company = subresult_all.company;

                lst_autopay.Add(subresult_autopay);
            }

            result.lstData_All = lst_all.OrderBy(o => o.cost_center).ThenBy(t => t.trainee_code).ToList();
            result.lstData_AutoPay = lst_autopay.OrderBy(o => o.Bene_Ref).ToList();
            #endregion All and Autopay

            #region Sum

            var _getdetail_Sum = _getdetail.Where(w => w.TM_Time_Type.type_short_name_en == "NH" || w.TM_Time_Type.type_short_name_en == "TN");
            var _getdetail_Sum_OT = _getdetail.Where(w => w.TM_Time_Type.type_short_name_en == "OT");

            List<ResultLine_Sum> lst_Sum = new List<ResultLine_Sum>();
            List<ResultLine_Sum> lst_Sum_OT = new List<ResultLine_Sum>();


            var grousp = _getdetail_Sum_OT.GroupBy(x => new { x.trainee_create_user, x.Engagement_Code, x.date_start.Value.Date })
                .Select(s => new
                {
                    Key = s.Key,
                    date_start = s.Key.Date,
                    Hour = s.Sum(x => SystemFunction.GetNumberNullToZero(x.hours.Replace(":", "."))),
                    Trainee_Code = s.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.candidate_TraineeNumber,
                    Name = s.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + s.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                    Company = s.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.candidate_Company,
                    Cost_center = s.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Divisions != null ? s.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Divisions.division_name_en: s.First().TimeSheet_Form.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en,
                    TimeType = s.First().TM_Time_Type.type_short_name_en,
                    Daily_Wage = s.First().TimeSheet_Form.TM_PR_Candidate_Mapping.daily_wage
                }).ToList();

            foreach (var a in grousp)
            {
                ResultLine_Sum line_sum = new ResultLine_Sum();
                line_sum.cost_center = a.Cost_center;
                line_sum.trainee_code = a.Trainee_Code;
                line_sum.name = a.Name;

                line_sum.code = a.Key.Engagement_Code;

                lst_Sum.Add(line_sum);

                line_sum.ot_hours = a.Hour.ToString("00.00");

                var setdate = a.Key.Date;

                decimal totalhours = 0;
                decimal rateone = 0;
                decimal ratetwo = 0;
                decimal ratethree = 0;

                decimal daily_wage = SystemFunction.GetNumberNullToZero(a.Daily_Wage + "");
                decimal daily_wagesub = daily_wage / 8;

                string _hours = a.Hour.ToString("00.00");
                DateTime _date_start = Convert.ToDateTime(a.Key.Date);
                var valhours = SystemFunction.GetNumberNullToZero(_hours + "");
                totalhours += valhours;
                //check date set rate

                if (IsBusinessDay(_date_start, "", _date_start.ToString("yyyy")))
                {
                    rateone = rateone + valhours;
                }
                else
                {
                    var settime = valhours;
                    if (settime <= 8)
                    {
                        ratetwo += valhours;
                    }
                    else
                    {
                        var contime = settime - 8;
                        ratetwo += settime - contime;
                        ratethree += contime;

                    }

                }



                decimal total = 0;

                //r1
                total += (daily_wagesub * (rateone * (decimal)1.5));


                //r2
                total += (daily_wagesub * (ratetwo * 2));


                //r3
                total += (daily_wagesub * (ratethree * 3));


                //subresult.total = total.ToString("#,##0.00");


                //line_sum.ot_amount = Math.Round(total, MidpointRounding.AwayFromZero).ToString("#,##0.00");
                //line_sum.grand_total = Math.Round(total, MidpointRounding.AwayFromZero).ToString("#,##0.00");
                line_sum.ot_amount = total.ToString("#,##0.00");
                line_sum.grand_total = total.ToString("#,##0.00");
                //var check_ext = lst_Engagement_ext.Where(w => w.id == line_sum.code).ToList();
                //if (check_ext != null)
                //{

                //    if (check_ext.Count > 0)
                //    {
                //        line_sum.client_name = check_ext.First().comp;
                //        line_sum.client = line_sum.grand_total;

                //    }
                //    else
                //    {
                //        var check_int = lst_Engagement_int.Where(w => w.id == line_sum.code).ToList();
                //        if (check_int.Count() > 0)
                //        {
                //            line_sum.client_name = check_int.First().comp;
                //            line_sum.office = line_sum.grand_total;
                //        }
                //    }
                //}
            }

            result.lstData_Sum = lst_Sum;
            
            var _group_sum = _getdetail_Sum.Where(w => w.active_status != "N" && w.approve_status == status).GroupBy(d => new { d.trainee_create_user, d.Engagement_Code })
        .Select(
        g => new
        {
            Key = g.Key,
            Hour = g.Sum(s => SystemFunction.GetNumberNullToZero(s.hours.Replace(":", "."))),
            //Office = g.Sum(s => SystemFunction.GetNumberNullToZero(s.hours.Replace(":","."))),
            Trainee_Code = g.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.candidate_TraineeNumber,
            Name = g.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + g.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
            Cost_center = g.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Divisions != null ? g.First().TimeSheet_Form.TM_PR_Candidate_Mapping.TM_Divisions.division_name_en : g.First().TimeSheet_Form.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en,
            TimeType = g.First().TM_Time_Type.type_short_name_en,
            Daily_Wage = g.First().TimeSheet_Form.TM_PR_Candidate_Mapping.daily_wage
            //Category = g.First().Category
        }).ToList();
            foreach (var ot in _group_sum)
            {

                ResultLine_Sum line_sum = new ResultLine_Sum();
                line_sum.cost_center = ot.Cost_center;
                line_sum.trainee_code = ot.Trainee_Code;
                line_sum.name = ot.Name;
                line_sum.code = ot.Key.Engagement_Code;

                line_sum.nomal_hours = ot.Hour.ToString("00.00");
                line_sum.nomal_amount = SystemFunction.GetNumberNullToZero(((ot.Daily_Wage / 8) * ot.Hour).ToString()).ToString("#,##0.00");

                line_sum.grand_total = SystemFunction.GetNumberNullToZero(line_sum.nomal_amount) + SystemFunction.GetNumberNullToZero(line_sum.ot_amount).ToString("###0.00");
                //var check_ext = lst_Engagement_ext.Where(w => w.id == line_sum.code).ToList();
                //if (check_ext != null)
                //{
                //    if (check_ext.Count > 0)
                //    {
                //        line_sum.client_name = check_ext.First().comp;
                //        line_sum.client = line_sum.grand_total;

                //    }
                //    else
                //    {
                //        var check_int = lst_Engagement_int.Where(w => w.id == line_sum.code).ToList();
                //        if (check_int.Count() > 0)
                //        {
                //            line_sum.client_name = check_int.First().text;
                //            line_sum.office = line_sum.grand_total;
                //        }
                //    }
                //}

                lst_Sum.Add(line_sum);
            }
            result.lstData_Sum = lst_Sum;

            var slst_Sum = result.lstData_Sum.GroupBy(g => new { g.code, g.name }).Select(
                s => new ResultLine_Sum()
                {
                    cost_center = s.FirstOrDefault().cost_center,
                    trainee_code = s.FirstOrDefault().trainee_code,
                    name = s.FirstOrDefault().name,
                    code = s.FirstOrDefault().code,
                    client_name = s.FirstOrDefault().client_name,
                    ot_hours = s.Sum(x => SystemFunction.GetNumberNullToZero(x.ot_hours)) == 0 ? "0.0" : s.Sum(x => SystemFunction.GetNumberNullToZero(x.ot_hours)).ToString("0.0"),
                    nomal_hours = s.Sum(x => SystemFunction.GetNumberNullToZero(x.nomal_hours)) == 0 ? "0.0" : s.Sum(x => SystemFunction.GetNumberNullToZero(x.nomal_hours)).ToString("0.0"),
                    ot_amount = s.Sum(x => SystemFunction.GetNumberNullToZero(x.ot_amount)).ToString("#,##0.00"),
                    nomal_amount = s.Sum(x => SystemFunction.GetNumberNullToZero(x.nomal_amount)).ToString("#,##0.00"),
                    grand_total = (Math.Round(s.Sum(x => SystemFunction.GetNumberNullToZero(x.ot_amount)), MidpointRounding.AwayFromZero) + s.Sum(x => SystemFunction.GetNumberNullToZero(x.nomal_amount))).ToString("#,##0.00"),
                    client = s.Sum(x => SystemFunction.GetNumberNullToZero(x.client)).ToString("#,##0.00"),
                    office = s.Sum(x => SystemFunction.GetNumberNullToZero(x.office)).ToString("#,##0.00"),

                }).ToList();
            result.lstData_Sum = new List<ResultLine_Sum>();

            List<ResultLine_Sum> lstnewsum = new List<ResultLine_Sum>();
            foreach (var en in slst_Sum)
            {

                ResultLine_Sum genEg = new ResultLine_Sum();
                genEg = en;
                var check_ext = lst_Engagement_ext.Where(w => w.id == en.code).ToList();
                if (check_ext != null)
                {
                    if (check_ext.Count > 0)
                    {
                        genEg.client_name = check_ext.First().comp;
                        genEg.client = genEg.grand_total;

                    }
                    else
                    {
                        var check_int = lst_Engagement_int.Where(w => w.id == en.code).ToList();
                        if (check_int.Count() > 0)
                        {
                            genEg.client_name = check_int.First().text;
                            genEg.office = genEg.grand_total;
                        }
                    }
                }

                lstnewsum.Add(genEg);
            }
            List<ResultLine_Sum> lstsumgroup = new List<ResultLine_Sum>();
            var lastname = "";
            foreach (var s in lstnewsum)
            {
                ResultLine_Sum obj = new ResultLine_Sum();
                obj = s;

                if (s.name != lastname)
                {
                    var da = slst_Sum.Where(w => w.name == s.name).Sum(sum => SystemFunction.GetNumberNullToZero(sum.nomal_amount)).ToString("#,##0.00");
                    obj.nomal_amount_hd = da;
                    var ot = slst_Sum.Where(w => w.name == s.name).Sum(sum => SystemFunction.GetNumberNullToZero(sum.ot_amount)).ToString("#,##0.00");
                    obj.ot_amount_hd = ot;
                    lastname = s.name;
                }
                else
                {
                    obj.nomal_amount_hd = "0";
                    obj.ot_amount_hd = "0";
                }

                lstsumgroup.Add(obj);
            }



            result.lstData_Sum = lstsumgroup.OrderBy(o => o.cost_center).ThenBy(s => s.trainee_code).ToList();
            #endregion Sum

            #region Memo


            result.from = CGlobal.UserInfo.FirstName;
            result.date = DateTime.Now.ToString("dd-MMM-yyyy");
            //var dayinmonth = Convert.ToDateTime("1 " + preoidtxt);
            //var lastDayOfMonth = DateTime.DaysInMonth(dayinmonth.Year, dayinmonth.Month);
            result.subject = "Intern Allowance " + startD + " - " + endD;
            result.approvedate = "Please approve intern allowance for the period of " + startD + " - " + endD;

            List<ResultLine_Memo> lst_Memo = new List<ResultLine_Memo>();
            var lst_for_memo = result.lstData_All;

            var group_memo_cos = lst_for_memo.GroupBy(g => g.cost_center).ToList();

            List<ResultLine_Memo> group_memo = lst_for_memo
    .GroupBy(l => l.cost_center)
    .Select(cl => new ResultLine_Memo
    {
        company = cl.First().company,
        cost_center = cl.First().cost_center,
        standard = cl.Sum(c => SystemFunction.GetNumberNullToZero(c.amount_standard.Replace(",", ""))).ToString(),
        ot = cl.Sum(c => SystemFunction.GetNumberNullToZero(c.amount_ot.Replace(",", ""))).ToString(),
        perdiem = cl.Sum(c => SystemFunction.GetNumberNullToZero(c.amount_perdiem.Replace(",", ""))).ToString(),
        total = cl.Sum(c => SystemFunction.GetNumberNullToZero(c.amount_total.Replace(",", ""))).ToString(),
    }).ToList();

            result.lstData_Memo = group_memo.OrderBy(o => o.company).ThenBy(t => t.cost_center).ToList();
            #endregion Memo 

            //ExportToExcel(lst_all, preoidtxt.Split(' ')[0] + DateTime.Now.Day);

            //var getlstall = result.lstTimeSheet_Detail_All;
            

            result.type_save = status;
            Session[sSess] = result;

            //result.lstTimeSheet_Detail_All = new List<vTimeSheet_Detail>();


            var jsonResult = Json(new
            {
                result
            }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpPost]
        public ActionResult TraineePayMentSave(string Typesave)
        {
            ResultLine_Save result = new ResultLine_Save();
            try
            {
                var dNow = DateTime.Now;
                ResultLine ItemData = new ResultLine();
                string sSess = Request.Form["sSess"];
                ItemData = Session[sSess] as ResultLine;
                if (ItemData != null)
                {
                    TimeSheet_Detail set_timesheet = new TimeSheet_Detail();

                    var get_new_timesheet = ItemData.lstTimeSheet_Detail_All.ToList();

                    var get_timesheet = _TimeSheet_DetailService.GetDataForSelect().ToList();

                    var query = from seta in get_timesheet
                                join setb in get_new_timesheet on seta.Id.ToString() equals setb.Id
                                select seta;

                    foreach (var newitem in query)
                    {
                        newitem.approve_status = Typesave;
                        if (Typesave == "A")
                        {
                            newitem.review_user = CGlobal.UserInfo.EmployeeNo;
                            newitem.review_date = DateTime.Now;
                        }
                        else if (Typesave == "P")
                        {
                            newitem.paid_user = CGlobal.UserInfo.EmployeeNo;
                            newitem.paid_date = DateTime.Now;
                        }
                        var check = _TimeSheet_DetailService.UpdateApproveDetail(newitem);
                    }

                    //update perdiem
                    var get_new_perdiem = ItemData.lstData_PerdiemTransport.ToList();
                    List<Perdiem_Transport> lstperd = new List<Perdiem_Transport>();
                    foreach (var ex in get_new_perdiem)
                    {
                        Perdiem_Transport pd = new Perdiem_Transport();
                        pd.Id = ex.Id;
                        pd.approve_status = Typesave;
                        pd.remark = ex.remark;
                        pd.trainee_update_user = ex.trainee_update_user;
                        if (Typesave == "A")
                        {
                            pd.review_user = CGlobal.UserInfo.EmployeeNo;
                            pd.review_date = DateTime.Now;
                        }
                        else if (Typesave == "P")
                        {
                            pd.paid_user = CGlobal.UserInfo.EmployeeNo;
                            pd.paid_date = DateTime.Now;
                        }
                        lstperd.Add(pd);

                    }

                    //var setnewperdiem = get_new_perdiem.Select(s => new Perdiem_Transport()
                    //{
                    //    Id = s.Id,
                    //    approve_status = Typesave,
                    //    remark = s.remark,
                    //    trainee_update_user = s.trainee_update_user,
                    //    review_user = Typesave == "A" ? CGlobal.UserInfo.EmployeeNo : null,
                    //    review_date = Typesave == "A" ? DateTime.Now : null,
                    //    paid_user = Typesave == "P" ? CGlobal.UserInfo.EmployeeNo : null,
                    //    paid_date = Typesave == "A" ? DateTime.Now : null,
                    //}).ToList();

                    foreach (var pd in lstperd)
                    {

                        var updateperdiem = _Perdiem_TransportService.UpdateApprove_Status(pd);
                    }

                    //var export = ExportToExcel(ItemData, ItemData.subject);


                    result.Msg = "Success, Save Success";
                    result.Status = SystemFunction.process_Success;
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

        public ActionResult TraineePayMentExport()
        {
            ResultLine_Save result = new ResultLine_Save();
            try
            {
                var dNow = DateTime.Now;
                ResultLine objFile = new ResultLine();
                string sSess = Request.Form["sSess"];
                objFile = Session[sSess] as ResultLine;
                if (objFile != null)
                {
                    //var export = this.ExportToExcel(ItemData, ItemData.subject);
                    epPayMemo reportMemo = new epPayMemo();
                    epPayAll reportAll = new epPayAll();
                    epPaySumSTOT reportSTOT = new epPaySumSTOT();
                    exPayPerdiem reportPDTS = new exPayPerdiem();
                    exPayOT reportOT = new exPayOT();
                    exPayAutopay reportAutopay = new exPayAutopay();

                    var stream = new MemoryStream();
                    XrTemp xrTemp = new XrTemp();

                    xrTemp.CreateDocument();

                    #region Memo
                    xrTemp.CreateDocument();
                    if (objFile.lstData_Memo.Count() > 0)
                    {
                        var setAll = objFile.lstData_Memo.Select(s => new
                        {
                            company = s.company,
                            cost_center = s.cost_center,
                            standard = SystemFunction.GetNumberNullToZero(s.standard + ""),
                            ot = SystemFunction.GetNumberNullToZero(s.ot + ""),
                            perdiem = SystemFunction.GetNumberNullToZero(s.perdiem + ""),
                            total = SystemFunction.GetNumberNullToZero(s.total + "")
                        }).ToList();
                        System.Data.DataTable dtR = SystemFunction.LinqToDataTable(setAll);
                        dsMemo abc = new dsMemo();
                        abc.dsMain.Merge(dtR);

                        XRLabel Dateexport = (XRLabel)reportMemo.FindControl("Dateexport", true);
                        Dateexport.Text = dNow.ToString("dd MMM yyyy");

                        XRLabel Preoids = (XRLabel)reportMemo.FindControl("Preoids", true);
                        Preoids.Text = objFile.subject;

                        var splitdate = objFile.subject.Split();
                        XRLabel Preoidfullmonths = (XRLabel)reportMemo.FindControl("Preoidfullmonths", true);
                        Preoidfullmonths.Text = (splitdate[2] + " " + splitdate[3] + " " + splitdate[4] + " - " + splitdate[6] + " " + splitdate[7] + " " + splitdate[8]);

                        reportMemo.DataSource = abc;
                        reportMemo.CreateDocument();




                    }
                    #endregion memo

                    #region All
                    xrTemp.CreateDocument();
                    if (objFile.lstData_All.Count() > 0)
                    {

                        var i = 1;
                        var setAll = objFile.lstData_All.Select(s => new
                        {
                            id = (i++).ToString(),
                            company = s.company,
                            cost_center = s.cost_center,
                            trainee_code = s.trainee_code,
                            name = s.name,
                            hours_standard = SystemFunction.GetNumberNullToZero(s.hours_standard + ""),
                            hours_ot = SystemFunction.GetNumberNullToZero(s.hours_ot + ""),
                            hours_total = SystemFunction.GetNumberNullToZero(s.hours_total + ""),
                            amount_standard = SystemFunction.GetNumberNullToZero(s.amount_standard + ""),
                            amount_ot = SystemFunction.GetNumberNullToZero(s.amount_ot + ""),
                            amount_perdiem = SystemFunction.GetNumberNullToZero(s.amount_perdiem + ""),
                            amount_total = SystemFunction.GetNumberNullToZero(s.amount_total + ""),
                            bank = s.bank,
                            id_book_bank = s.id_book_bank,
                            note = s.note,
                            trainee_create_user = s.trainee_create_user
                        }).ToList();
                        System.Data.DataTable dtR = SystemFunction.LinqToDataTable(setAll);
                        dsAll ds = new dsAll();
                        ds.dsMain.Merge(dtR);

                        //XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                        //xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;
                        reportAll.DataSource = ds;
                        reportAll.CreateDocument();


                    }
                    #endregion All

                    #region SumSTOT
                    xrTemp.CreateDocument();
                    if (objFile.lstData_Sum.Count() > 0)
                    {


                        var i = 1;
                        var setAll = objFile.lstData_Sum.Select(s => new
                        {
                            company = s.company,
                            cost_center = s.cost_center,
                            trainee_code = s.trainee_code,
                            name = s.name,
                            client_name = s.client_name,
                            code = s.code,
                            client = SystemFunction.GetNumberNullToZero(s.client + ""),
                            office = SystemFunction.GetNumberNullToZero(s.office + ""),
                            nomal_hours = SystemFunction.GetNumberNullToZero(s.nomal_hours + ""),
                            nomal_amount = SystemFunction.GetNumberNullToZero(s.nomal_amount + ""),
                            ot_amount_hd = Math.Round(SystemFunction.GetNumberNullToZero(s.ot_amount + ""), MidpointRounding.AwayFromZero),
                            nomal_amount_hd = Math.Round(SystemFunction.GetNumberNullToZero(s.nomal_amount + ""), MidpointRounding.AwayFromZero),
                            ot_hours = SystemFunction.GetNumberNullToZero(s.ot_hours + ""),
                            ot_amount = SystemFunction.GetNumberNullToZero(s.ot_amount + ""),
                            grand_total = SystemFunction.GetNumberNullToZero(s.nomal_amount + "") + SystemFunction.GetNumberNullToZero(s.ot_amount + "")
                        }).ToList();

                        var setsub = objFile.lstData_Sum.GroupBy(g => g.name).Select(s => new
                        {
                            company = s.First().company,
                            cost_center = s.First().cost_center,
                            trainee_code = s.First().trainee_code,
                            name = s.First().name,
                            client_name = s.First().client_name,
                            code = s.First().code,
                            client = SystemFunction.GetNumberNullToZero(s.First().client + ""),
                            office = SystemFunction.GetNumberNullToZero(s.First().office + ""),
                            nomal_hours = SystemFunction.GetNumberNullToZero(s.First().nomal_hours + ""),
                            nomal_amount = Math.Round(s.Sum(su => SystemFunction.GetNumberNullToZero(su.nomal_amount + "")), MidpointRounding.AwayFromZero),
                            ot_hours = SystemFunction.GetNumberNullToZero(s.First().ot_hours + ""),
                            ot_amount = Math.Round(s.Sum(su => SystemFunction.GetNumberNullToZero(su.ot_amount + "")), MidpointRounding.AwayFromZero),
                            grand_total = Math.Round(s.Sum(su => SystemFunction.GetNumberNullToZero(su.grand_total + "")))
                        }).ToList();

                        List<dsSummary> lstsumgroup = new List<dsSummary>();
                        var lastname = "";
                        foreach (var s in setAll)
                        {
                            dsSummary obj = new dsSummary();
                            obj.company = s.company;
                            obj.cost_center = s.cost_center;
                            obj.trainee_code = s.trainee_code;
                            obj.name = s.name;
                            obj.client_name = s.client_name;
                            obj.code = s.code;
                            obj.client = SystemFunction.GetNumberNullToZero(s.client + "");
                            obj.office = SystemFunction.GetNumberNullToZero(s.office + "");
                            obj.nomal_hours = SystemFunction.GetNumberNullToZero(s.nomal_hours + "");
                            obj.nomal_amount = SystemFunction.GetNumberNullToZero(s.nomal_amount + "");

                            if (s.name != lastname)
                            {
                                var da = setsub.Where(w => w.name == s.name).Sum(sum => sum.nomal_amount);
                                obj.nomal_amount_hd = da;
                                var ot = setsub.Where(w => w.name == s.name).Sum(sum => sum.ot_amount);
                                obj.ot_amount_hd = ot;
                                lastname = s.name;
                            }
                            else
                            {
                                obj.nomal_amount_hd = 0;
                                obj.ot_amount_hd = 0;
                            }


                            obj.ot_hours = SystemFunction.GetNumberNullToZero(s.ot_hours + "");
                            obj.ot_amount = SystemFunction.GetNumberNullToZero(s.ot_amount + "");
                            obj.grand_total = SystemFunction.GetNumberNullToZero(s.nomal_amount + "") + SystemFunction.GetNumberNullToZero(s.ot_amount + "");

                            lstsumgroup.Add(obj);
                        }


                        System.Data.DataTable dtR = SystemFunction.LinqToDataTable(lstsumgroup);
                        dsSum ds = new dsSum();
                        ds.dsMain.Merge(dtR);

                        var sumnomal = setsub.Sum(s => s.nomal_amount);
                        var sumot = setsub.Sum(s => s.ot_amount);
                        var sumgrand = sumnomal + sumot;

                        reportSTOT.Parameters["SummaryNormal"].Value = sumnomal;
                        reportSTOT.Parameters["SummaryOT"].Value = sumot;
                        reportSTOT.Parameters["GrandTotal"].Value = sumgrand;

                        reportSTOT.DataSource = ds;
                        reportSTOT.CreateDocument();

                    }
                    #endregion SumSTOT

                    #region Perdiem
                    xrTemp.CreateDocument();
                    if (objFile.lstData_PerdiemTransport.Count() > 0)
                    {

                        var i = 1;
                        var setAll = objFile.lstData_PerdiemTransport.Select(s => new
                        {
                            Cost_Center = s.Cost_Center,
                            Type_of_withdrawal = s.Type_of_withdrawal == "P" ? "Perdiem" : "Transport",
                            Trainee_Code = s.Trainee_Code,
                            Name = s.Name,
                            Engagement_Name = s.Engagement_Name,
                            Engagement_Code = s.Engagement_Code,
                            Amount = SystemFunction.GetNumberNullToZero(s.Amount + ""),
                            Create_Date = s.Create_Date,

                        }).ToList();
                        System.Data.DataTable dtR = SystemFunction.LinqToDataTable(setAll);
                        dsPerdiemTransport ds = new dsPerdiemTransport();
                        ds.dsMain.Merge(dtR);

                        //XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                        //xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;
                        reportPDTS.DataSource = ds;
                        reportPDTS.CreateDocument();


                    }
                    #endregion Perdiem

                    #region OT
                    xrTemp.CreateDocument();
                    if (objFile.lstData_OT.Count() > 0)
                    {

                        var i = 1;
                        var setAll = objFile.lstData_OT.Select(s => new
                        {
                            name = s.name,
                            month = s.month,
                            daily_wage = SystemFunction.GetNumberNullToZero(s.daily_wage + ""),
                            rate1 = SystemFunction.GetNumberNullToZero(s.rate1 + ""),
                            rate2 = SystemFunction.GetNumberNullToZero(s.rate2 + ""),
                            rate3 = SystemFunction.GetNumberNullToZero(s.rate3 + ""),
                            totalhrs = SystemFunction.GetNumberNullToZero(s.totalhrs + ""),
                            total = SystemFunction.GetNumberNullToZero(s.total + "")
                        }).ToList();
                        System.Data.DataTable dtR = SystemFunction.LinqToDataTable(setAll);
                        dsOT ds = new dsOT();
                        ds.dsMain.Merge(dtR);

                        //XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                        //xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;
                        reportOT.DataSource = ds;
                        reportOT.CreateDocument();


                    }
                    #endregion OT

                    #region AutoPay
                    xrTemp.CreateDocument();
                    if (objFile.lstData_AutoPay.Count() > 0)
                    {

                        var i = 1;
                        var setAll = objFile.lstData_AutoPay.Select(s => new
                        {
                            Vendor_ID = s.Vendor_ID,
                            Bank_Code = s.Bank_Code,
                            Account_Number = s.Account_Number,
                            Vendor_Name = s.Vendor_Name,
                            Amount = SystemFunction.GetNumberNullToZero(s.Amount + ""),
                            Bene_Ref = s.Bene_Ref,
                            WHT = s.WHT,
                            Advice = s.Advice,
                            SMS = s.SMS,
                            Payment_Detail = s.Payment_Detail,
                            Invoice = s.Invoice,
                            Branch_Code = s.Branch_Code,
                            Cost_Center = s.Cost_Center
                        }).ToList();
                        System.Data.DataTable dtR = SystemFunction.LinqToDataTable(setAll);
                        dsAutopay ds = new dsAutopay();
                        ds.dsMain.Merge(dtR);

                        //XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                        //xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;
                        reportAutopay.DataSource = ds;
                        reportAutopay.CreateDocument();


                    }
                    #endregion AutoPay

                    #region Export 

                    var gettype = objFile.type_save;
                    var fortypeexprot = "";
                    switch (gettype)
                    {
                        case "Y":
                            fortypeexprot = "Approve";
                            break;
                        case "A":
                            fortypeexprot = "Review";
                            break;
                        case "P":
                            fortypeexprot = "Paid";
                            break;
                    }
                    //Add page
                    xrTemp.Pages.AddRange(reportMemo.Pages);
                    xrTemp.Pages.AddRange(reportAll.Pages);
                    xrTemp.Pages.AddRange(reportSTOT.Pages);
                    xrTemp.Pages.AddRange(reportPDTS.Pages);
                    xrTemp.Pages.AddRange(reportOT.Pages);
                    xrTemp.Pages.AddRange(reportAutopay.Pages);

                    XlsxExportOptions xlsxOptions = xrTemp.ExportOptions.Xlsx;
                    xlsxOptions.ShowGridLines = true;
                    xlsxOptions.SheetName = "Tab_";
                    xlsxOptions.TextExportMode = TextExportMode.Text;
                    xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;

                    xrTemp.ExportToXlsx(stream, xlsxOptions);
                    stream.Seek(0, SeekOrigin.Begin);
                    byte[] breport = stream.ToArray();
                    return File(breport, "application/excel", "Intern Allowance_" + DateTime.Now.ToString("ddMMyyyyHHmm") + "_" + fortypeexprot + ".xlsx");
                    #endregion Export
                    //result.Msg = export;
                    //result.Status = SystemFunction.process_Success;
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

        #endregion

        #region
        //not use
        public string ExportToExcel(ResultLine list_, string epMonth)
        {
            epMonth = epMonth.Trim();
            var status = true;
            var msg = "";
            string fileName = "";


            var root = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Intern Allowance";
            var subroot = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Intern Allowance\\" + DateTime.Now.ToString("ddMMyyyyHHmm");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            if (!Directory.Exists(subroot))
            {
                Directory.CreateDirectory(subroot);
            }


            #region Memo

            // Define filename
            fileName = string.Format(@"{0}\Memo_" + epMonth + ".xlsx", subroot);

            // Load Excel application
            Microsoft.Office.Interop.Excel.Application excel_Memo = new Microsoft.Office.Interop.Excel.Application();

            // Create empty workbook
            excel_Memo.Workbooks.Add();
            // Create Worksheet from active sheet
            Microsoft.Office.Interop.Excel._Worksheet workSheet_Memo = excel_Memo.ActiveSheet;
            workSheet_Memo.Name = "Memo";
            // I created Application and Worksheet objects before try/catch,
            // so that i can close them in finnaly block.
            // It's IMPORTANT to release these COM objects!!
            try
            {
                // ------------------------------------------------
                // Creation of header cells
                // ------------------------------------------------
                workSheet_Memo.Cells[9, "A"] = "Company";
                workSheet_Memo.Cells[9, "B"] = "Cost Center";
                workSheet_Memo.Cells[9, "C"] = "Standard";
                workSheet_Memo.Cells[9, "D"] = "OT";
                workSheet_Memo.Cells[9, "E"] = "Per Diem";
                workSheet_Memo.Cells[9, "F"] = "Total";

                var groupMemo = list_.lstData_Memo.GroupBy(g => g.company).ToList();
                int rowAll = 2;


                workSheet_Memo.Cells[1, "A"] = "To : ";
                workSheet_Memo.Cells[2, "A"] = "From : ";
                workSheet_Memo.Cells[3, "A"] = "Date : ";
                workSheet_Memo.Cells[4, "A"] = "Subject : ";

                workSheet_Memo.Cells[1, "B"] = "K.Wanna / K.Manisa";
                workSheet_Memo.Cells[2, "B"] = CGlobal.UserInfo.FirstName;
                workSheet_Memo.Cells[3, "B"] = DateTime.Now.ToString("dd-MMM-yyyy");
                workSheet_Memo.Cells[4, "B"] = epMonth;

                workSheet_Memo.Cells[5, "A"] = "_________________________________________________________________________";
                var f =
                workSheet_Memo.Cells[7, "A"] = "Please approve intern allowance for the period of " + SystemFunction.ConvertStringToDateTime(epMonth.Split(' ')[2] + " - " + epMonth.Split(' ')[3] + "-" + epMonth.Split(' ')[4], "", "dd-MMM-yyyy").ToString("dd MMMM yyyy");

                int rowsub = 10;
                decimal Ast = 0;
                decimal Aot = 0;
                decimal Apd = 0;
                decimal Att = 0;

                workSheet_Memo.Range["A9"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

                foreach (var ex in groupMemo)
                {
                    decimal st = 0;
                    decimal ot = 0;
                    decimal pd = 0;
                    decimal tt = 0;
                    foreach (ResultLine_Memo rs in ex)
                    {
                        workSheet_Memo.Cells[rowsub, "A"] = rs.company;
                        workSheet_Memo.Cells[rowsub, "B"] = rs.cost_center;
                        workSheet_Memo.Cells[rowsub, "C"] = SystemFunction.GetNumberNullToZero(rs.standard).ToString("#,###");
                        workSheet_Memo.Cells[rowsub, "D"] = SystemFunction.GetNumberNullToZero(rs.ot).ToString("#,###");
                        workSheet_Memo.Cells[rowsub, "E"] = SystemFunction.GetNumberNullToZero(rs.perdiem).ToString("#,###");
                        workSheet_Memo.Cells[rowsub, "F"] = SystemFunction.GetNumberNullToZero(rs.total).ToString("#,###");
                        rowsub++;

                        st += SystemFunction.GetNumberNullToZero(rs.standard + "");
                        ot += SystemFunction.GetNumberNullToZero(rs.ot + "");
                        pd += SystemFunction.GetNumberNullToZero(rs.perdiem + "");
                        tt += SystemFunction.GetNumberNullToZero(rs.total + "");
                    }

                    Ast += st;
                    Aot += ot;
                    Apd += pd;
                    Att += tt;

                    workSheet_Memo.Cells[rowsub, "A"] = "Total " + ex.First().company;
                    workSheet_Memo.Cells[rowsub, "B"] = "";
                    workSheet_Memo.Cells[rowsub, "C"] = st.ToString("#,###");
                    workSheet_Memo.Cells[rowsub, "D"] = ot.ToString("#,###");
                    workSheet_Memo.Cells[rowsub, "E"] = pd.ToString("#,###");
                    workSheet_Memo.Cells[rowsub, "F"] = tt.ToString("#,###");

                    workSheet_Memo.get_Range("A" + rowsub, "F" + rowsub).Interior.Color = XlRgbColor.rgbLightSkyBlue;
                    workSheet_Memo.get_Range("A" + rowsub, "F" + rowsub).Font.Bold = true;
                    rowsub++;
                }
                workSheet_Memo.Cells[rowsub, "A"] = "Grand Total";
                workSheet_Memo.Cells[rowsub, "B"] = "";
                workSheet_Memo.Cells[rowsub, "C"] = Ast.ToString("#,###");
                workSheet_Memo.Cells[rowsub, "D"] = Aot.ToString("#,###");
                workSheet_Memo.Cells[rowsub, "E"] = Apd.ToString("#,###");
                workSheet_Memo.Cells[rowsub, "F"] = Att.ToString("#,###");



                workSheet_Memo.get_Range("A9", "F9").Interior.Color = XlRgbColor.rgbSkyBlue;
                workSheet_Memo.get_Range("A" + rowsub, "F" + rowsub).Interior.Color = XlRgbColor.rgbSkyBlue;
                workSheet_Memo.get_Range("A" + rowsub, "F" + rowsub).Font.Bold = true;
                workSheet_Memo.get_Range("A9", "F" + rowsub).Borders.Color = XlRgbColor.rgbBlack;


                //add Footer
                rowsub += 2;
                workSheet_Memo.Cells[rowsub, "A"] = "Reviewed : ";
                workSheet_Memo.Cells[rowsub, "B"] = "_____________________";
                workSheet_Memo.Cells[rowsub, "C"] = "Manisa M.";
                workSheet_Memo.Cells[rowsub, "D"] = "_____________________";
                workSheet_Memo.Cells[rowsub, "E"] = "Date";

                rowsub += 2;
                workSheet_Memo.Cells[rowsub, "A"] = "Reviewed : ";
                workSheet_Memo.Cells[rowsub, "B"] = "_____________________";
                workSheet_Memo.Cells[rowsub, "C"] = "Wanna P.";
                workSheet_Memo.Cells[rowsub, "D"] = "_____________________";
                workSheet_Memo.Cells[rowsub, "E"] = "Date";


                workSheet_Memo.get_Range("A5", "F5").Merge();

                // Save this data as a file
                workSheet_Memo.SaveAs(fileName);

                status = true;
                msg += string.Format("\nThe file '{0}' is saved successfully!", fileName);

            }
            catch (Exception exception)
            {
                status = false;
                msg += "\nThere was a PROBLEM saving Excel file! [" + fileName + "]  \n" + exception.Message;
            }
            finally
            {
                // Quit Excel application
                excel_Memo.Quit();

                // Release COM objects (very important!)
                if (excel_Memo != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_Memo);

                if (workSheet_Memo != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet_Memo);

                // Empty variables
                excel_Memo = null;
                workSheet_Memo = null;

                // Force garbage collector cleaning
                GC.Collect();
            }
            #endregion Momo

            #region All

            // Define filename
            fileName = string.Format(@"{0}\All_" + epMonth + ".xlsx", subroot);

            // Load Excel application
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            // Create empty workbook
            excel.Workbooks.Add();
            // Create Worksheet from active sheet
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excel.ActiveSheet;
            workSheet.Name = "All";
            // I created Application and Worksheet objects before try/catch,
            // so that i can close them in finnaly block.
            // It's IMPORTANT to release these COM objects!!
            try
            {
                // ------------------------------------------------
                // Creation of header cells
                // ------------------------------------------------
                workSheet.Cells[1, "A"] = "No.";
                workSheet.Cells[1, "B"] = "Cost Center";
                workSheet.Cells[1, "C"] = "Trainee Code";
                workSheet.Cells[1, "D"] = "Name";
                workSheet.Cells[1, "E"] = "Hours";
                workSheet.Cells[1, "H"] = "Amount (Baht)";
                workSheet.Cells[1, "L"] = "Bank";
                workSheet.Cells[1, "M"] = "ID Book Bank";
                workSheet.Cells[1, "N"] = "Note";

                workSheet.Cells[2, "E"] = "Standard";
                workSheet.Cells[2, "F"] = "OT";
                workSheet.Cells[2, "G"] = "Total Hours";
                workSheet.Cells[2, "H"] = "Standard";
                workSheet.Cells[2, "I"] = "OT";
                workSheet.Cells[2, "J"] = "Per diem";
                workSheet.Cells[2, "K"] = "Total Amount";

                workSheet.Range["A1", "N2"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

                var groupAll = list_.lstData_All.GroupBy(g => g.cost_center).ToList();

                int row = 3; // start row (in row 1 are header cells)
                var runnum = 1;
                decimal thst = 0;
                decimal thot = 0;
                decimal thtt = 0;
                decimal tast = 0;
                decimal taot = 0;
                decimal tapd = 0;
                decimal tatt = 0;
                foreach (var ex in groupAll)
                {
                    decimal hst = 0;
                    decimal hot = 0;
                    decimal htt = 0;
                    decimal ast = 0;
                    decimal aot = 0;
                    decimal apd = 0;
                    decimal att = 0;
                    foreach (ResultLine_All rs in ex)
                    {
                        workSheet.Cells[row, "A"] = (runnum);
                        workSheet.Cells[row, "B"] = rs.cost_center;
                        workSheet.Cells[row, "C"] = rs.trainee_code;
                        workSheet.Cells[row, "D"] = rs.name;
                        workSheet.Cells[row, "E"] = rs.hours_standard;
                        workSheet.Cells[row, "F"] = rs.hours_ot;
                        workSheet.Cells[row, "G"] = rs.hours_total;
                        workSheet.Cells[row, "H"] = rs.amount_standard;
                        workSheet.Cells[row, "I"] = rs.amount_ot;
                        workSheet.Cells[row, "J"] = rs.amount_perdiem;
                        workSheet.Cells[row, "K"] = rs.amount_total;
                        workSheet.Cells[row, "L"] = rs.bank;
                        workSheet.Cells[row, "M"] = rs.id_book_bank;
                        workSheet.Cells[row, "N"] = rs.note;

                        runnum++;
                        row++;

                        hst += SystemFunction.GetNumberNullToZero(rs.hours_standard.Replace(",", "") + "");
                        hot += SystemFunction.GetNumberNullToZero(rs.hours_ot.Replace(",", "") + "");
                        htt += SystemFunction.GetNumberNullToZero(rs.hours_total.Replace(",", "") + "");
                        ast += SystemFunction.GetNumberNullToZero(rs.amount_standard.Replace(",", "") + "");
                        aot += SystemFunction.GetNumberNullToZero(rs.amount_ot.Replace(",", "") + "");
                        apd += SystemFunction.GetNumberNullToZero(rs.amount_perdiem.Replace(",", "") + "");
                        att += SystemFunction.GetNumberNullToZero(rs.amount_total.Replace(",", "") + "");
                    }
                    thst += hst;
                    thot += hot;
                    thtt += htt;
                    tast += ast;
                    taot += aot;
                    tapd += apd;
                    tatt += att;

                    workSheet.Cells[row, "B"] = ex.First().cost_center + " Total";
                    workSheet.Cells[row, "E"] = hst.ToString("#,###.00");
                    workSheet.Cells[row, "F"] = hot.ToString("#,###.00");
                    workSheet.Cells[row, "G"] = htt.ToString("#,###.00");
                    workSheet.Cells[row, "H"] = ast.ToString("#,###.00");
                    workSheet.Cells[row, "I"] = aot.ToString("#,###.00");
                    workSheet.Cells[row, "J"] = apd.ToString("#,###.00");
                    workSheet.Cells[row, "K"] = att.ToString("#,###.00");

                    workSheet.get_Range("A" + row, "D" + row).Merge();


                    workSheet.get_Range("A" + row, "N" + row).Interior.Color = XlRgbColor.rgbLightSkyBlue;
                    workSheet.get_Range("A" + row, "N" + row).Font.Bold = true;

                    row++;
                }
                workSheet.Cells[row, "B"] = "Grand Total";
                workSheet.Cells[row, "E"] = thst.ToString("#,###.00");
                workSheet.Cells[row, "F"] = thot.ToString("#,###.00");
                workSheet.Cells[row, "G"] = thtt.ToString("#,###.00");
                workSheet.Cells[row, "H"] = tast.ToString("#,###.00");
                workSheet.Cells[row, "I"] = taot.ToString("#,###.00");
                workSheet.Cells[row, "J"] = tapd.ToString("#,###.00");
                workSheet.Cells[row, "K"] = tatt.ToString("#,###.00");

                workSheet.get_Range("A" + row, "D" + row).Merge();
                workSheet.get_Range("A" + row, "N" + row).Interior.Color = XlRgbColor.rgbSkyBlue;
                workSheet.get_Range("A" + row, "N" + row).Font.Bold = true;
                // ------------------------------------------------
                // Populate sheet with some real data from "list_" list
                // ------------------------------------------------


                // Apply some predefined styles for data to look nicely :)

                workSheet.get_Range("A1", "A2").Merge();
                workSheet.get_Range("B1", "B2").Merge();
                workSheet.get_Range("C1", "C2").Merge();
                workSheet.get_Range("D1", "D2").Merge();
                workSheet.get_Range("L1", "L2").Merge();
                workSheet.get_Range("M1", "M2").Merge();
                workSheet.get_Range("N1", "N2").Merge();


                workSheet.get_Range("E1", "G1").Merge();
                workSheet.get_Range("H1", "K1").Merge();
                workSheet.get_Range("A1", "N2").Interior.Color = XlRgbColor.rgbSkyBlue;
                workSheet.get_Range("A1", "N" + row).Borders.Color = XlRgbColor.rgbBlack;


                // Save this data as a file
                workSheet.SaveAs(fileName);


                status = true;
                msg += string.Format("\nThe file '{0}' is saved successfully!", fileName);

            }
            catch (Exception exception)
            {
                status = false;
                msg += "\nThere was a PROBLEM saving Excel file! [" + fileName + "]  \n" + exception.Message;
            }
            finally
            {
                // Quit Excel application
                excel.Quit();

                // Release COM objects (very important!)
                if (excel != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

                if (workSheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);

                // Empty variables
                excel = null;
                workSheet = null;

                // Force garbage collector cleaning
                GC.Collect();
            }
            #endregion All

            #region NormalxOt


            // Define filename
            fileName = string.Format(@"{0}\Sum(Standard_OT)" + epMonth + ".xlsx", subroot);

            // Load Excel application
            Microsoft.Office.Interop.Excel.Application excel_NormalxOt = new Microsoft.Office.Interop.Excel.Application();

            // Create empty workbook
            excel_NormalxOt.Workbooks.Add();
            // Create Worksheet from active sheet
            Microsoft.Office.Interop.Excel._Worksheet workSheet_NormalxOt = excel_NormalxOt.ActiveSheet;
            workSheet_NormalxOt.Name = "Sum(Standard OT)";
            // I created Application and Worksheet objects before try/catch,
            // so that i can close them in finnaly block.
            // It's IMPORTANT to release these COM objects!!
            try
            {
                // ------------------------------------------------
                // Creation of header cells
                // ------------------------------------------------
                workSheet_NormalxOt.Cells[1, "A"] = "Cost Center";
                workSheet_NormalxOt.Cells[1, "B"] = "Trainee Code";
                workSheet_NormalxOt.Cells[1, "C"] = "Name";
                workSheet_NormalxOt.Cells[1, "D"] = "Client Name";
                workSheet_NormalxOt.Cells[1, "E"] = "Code";
                workSheet_NormalxOt.Cells[1, "F"] = "Client";
                workSheet_NormalxOt.Cells[1, "G"] = "Office";
                workSheet_NormalxOt.Cells[1, "H"] = "Nomal";
                workSheet_NormalxOt.Cells[1, "J"] = "OT";
                workSheet_NormalxOt.Cells[1, "L"] = "Grand Total";

                workSheet_NormalxOt.Cells[2, "H"] = "Hours";
                workSheet_NormalxOt.Cells[2, "I"] = "Amount";
                workSheet_NormalxOt.Cells[2, "J"] = "Hours";
                workSheet_NormalxOt.Cells[2, "K"] = "Amount";



                workSheet_NormalxOt.Range["A1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

                // ------------------------------------------------
                // Populate sheet with some real data from "list_" list
                // ------------------------------------------------
                int row = 3; // start row (in row 1 are header cells)

                decimal ar1 = 0;
                decimal ar2 = 0;
                decimal ar3 = 0;
                decimal ar4 = 0;
                decimal ar5 = 0;
                decimal ar6 = 0;
                decimal ar7 = 0;
                var groupcost = list_.lstData_Sum.GroupBy(g => g.cost_center).ToList();
                foreach (var cs in groupcost)
                {
                    decimal cr1 = 0;
                    decimal cr2 = 0;
                    decimal cr3 = 0;
                    decimal cr4 = 0;
                    decimal cr5 = 0;
                    decimal cr6 = 0;
                    decimal cr7 = 0;
                    var groupname = cs.GroupBy(g => g.name).ToList();

                    //var groupsum = list_.lstData_Sum.GroupBy(g => g.name).ToList();
                    foreach (var ex in groupname)
                    {
                        decimal r1 = 0;
                        decimal r2 = 0;
                        decimal r3 = 0;
                        decimal r4 = 0;
                        decimal r5 = 0;
                        decimal r6 = 0;
                        decimal r7 = 0;
                        foreach (ResultLine_Sum rs in ex)
                        {
                            workSheet_NormalxOt.Cells[row, "A"] = rs.cost_center;
                            workSheet_NormalxOt.Cells[row, "B"] = rs.trainee_code;
                            workSheet_NormalxOt.Cells[row, "C"] = rs.name;
                            workSheet_NormalxOt.Cells[row, "D"] = rs.client_name;
                            workSheet_NormalxOt.Cells[row, "E"] = " " + rs.code;
                            workSheet_NormalxOt.Cells[row, "F"] = SystemFunction.GetNumberNullToZero(rs.client).ToString("#,###.00");
                            workSheet_NormalxOt.Cells[row, "G"] = SystemFunction.GetNumberNullToZero(rs.office).ToString("#,###.00");
                            workSheet_NormalxOt.Cells[row, "H"] = SystemFunction.GetNumberNullToZero(rs.nomal_hours).ToString("#,###.00");
                            workSheet_NormalxOt.Cells[row, "I"] = SystemFunction.GetNumberNullToZero(rs.nomal_amount).ToString("#,###.00");
                            workSheet_NormalxOt.Cells[row, "J"] = SystemFunction.GetNumberNullToZero(rs.ot_hours).ToString("#,###.00");
                            workSheet_NormalxOt.Cells[row, "K"] = SystemFunction.GetNumberNullToZero(rs.ot_amount).ToString("#,###.00");
                            workSheet_NormalxOt.Cells[row, "L"] = SystemFunction.GetNumberNullToZero(rs.grand_total).ToString("#,###.00");

                            r1 += SystemFunction.GetNumberNullToZero(rs.client + "");
                            r2 += SystemFunction.GetNumberNullToZero(rs.office + "");
                            r3 += SystemFunction.GetNumberNullToZero(rs.nomal_hours + "");
                            r4 += SystemFunction.GetNumberNullToZero(rs.nomal_amount + "");
                            r5 += SystemFunction.GetNumberNullToZero(rs.ot_hours + "");
                            r6 += SystemFunction.GetNumberNullToZero(rs.ot_amount + "");
                            r7 += SystemFunction.GetNumberNullToZero(rs.grand_total + "");


                            row++;
                        }

                        cr1 += r1;
                        cr2 += r2;
                        cr3 += r3;
                        cr4 += r4;
                        cr5 += r5;
                        cr6 += r6;
                        cr7 += r7;

                        //edit column K
                        workSheet_NormalxOt.Cells[row, "C"] = ex.First().name + " Total";
                        workSheet_NormalxOt.Cells[row, "F"] = r1.ToString("#,###.00");
                        workSheet_NormalxOt.Cells[row, "G"] = r2.ToString("#,###.00");
                        workSheet_NormalxOt.Cells[row, "H"] = r3.ToString("#,###.00");
                        workSheet_NormalxOt.Cells[row, "I"] = r4.ToString("#,###.00");
                        workSheet_NormalxOt.Cells[row, "J"] = r5.ToString("#,###.00");
                        workSheet_NormalxOt.Cells[row, "K"] = r6.ToString("#,###.00");
                        workSheet_NormalxOt.Cells[row, "L"] = r7.ToString("#,###.00");

                        workSheet_NormalxOt.get_Range("A" + row, "L" + row).Interior.Color = XlRgbColor.rgbLightSkyBlue;
                        workSheet_NormalxOt.get_Range("A" + row, "L" + row).Font.Bold = true;
                        row++;
                    }
                    ar1 += cr1;
                    ar2 += cr2;
                    ar3 += cr3;
                    ar4 += cr4;
                    ar5 += cr5;
                    ar6 += cr6;
                    ar7 += cr7;

                    workSheet_NormalxOt.Cells[row, "A"] = cs.First().cost_center + " Total";
                    workSheet_NormalxOt.Cells[row, "F"] = cr1.ToString("#,###.00");
                    workSheet_NormalxOt.Cells[row, "G"] = cr2.ToString("#,###.00");
                    workSheet_NormalxOt.Cells[row, "H"] = cr3.ToString("#,###.00");
                    workSheet_NormalxOt.Cells[row, "I"] = cr4.ToString("#,###.00");
                    workSheet_NormalxOt.Cells[row, "J"] = cr5.ToString("#,###.00");
                    workSheet_NormalxOt.Cells[row, "K"] = cr6.ToString("#,###.00");
                    workSheet_NormalxOt.Cells[row, "L"] = cr7.ToString("#,###.00");

                    workSheet_NormalxOt.get_Range("A" + row, "L" + row).Interior.Color = XlRgbColor.rgbDarkGreen;
                    workSheet_NormalxOt.get_Range("A" + row, "L" + row).Font.Bold = true;
                    row++;
                }
                workSheet_NormalxOt.Cells[row, "C"] = "Grand Total";
                workSheet_NormalxOt.Cells[row, "F"] = ar1.ToString("#,###.00");
                workSheet_NormalxOt.Cells[row, "G"] = ar2.ToString("#,###.00");
                workSheet_NormalxOt.Cells[row, "H"] = ar3.ToString("#,###.00");
                workSheet_NormalxOt.Cells[row, "I"] = ar4.ToString("#,###.00");
                workSheet_NormalxOt.Cells[row, "J"] = ar5.ToString("#,###.00");
                workSheet_NormalxOt.Cells[row, "K"] = ar6.ToString("#,###.00");
                workSheet_NormalxOt.Cells[row, "L"] = ar7.ToString("#,###.00");

                workSheet_NormalxOt.get_Range("A" + row, "L" + row).Interior.Color = XlRgbColor.rgbLightSkyBlue;
                workSheet_NormalxOt.get_Range("A" + row, "L" + row).Font.Bold = true;

                workSheet_NormalxOt.get_Range("A1", "A2").Merge();
                workSheet_NormalxOt.get_Range("B1", "B2").Merge();
                workSheet_NormalxOt.get_Range("C1", "C2").Merge();
                workSheet_NormalxOt.get_Range("D1", "D2").Merge();
                workSheet_NormalxOt.get_Range("E1", "E2").Merge();
                workSheet_NormalxOt.get_Range("F1", "F2").Merge();
                workSheet_NormalxOt.get_Range("G1", "G2").Merge();
                workSheet_NormalxOt.get_Range("L1", "L2").Merge();


                workSheet_NormalxOt.get_Range("H1", "I1").Merge();
                workSheet_NormalxOt.get_Range("J1", "K1").Merge();
                //workSheet_NormalxOt.Range["E1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassicPivotTable);

                workSheet_NormalxOt.get_Range("A1", "L2").Interior.Color = XlRgbColor.rgbSkyBlue;
                workSheet_NormalxOt.get_Range("A1", "L" + row).Borders.Color = XlRgbColor.rgbBlack;

                // Save this data as a file
                workSheet_NormalxOt.SaveAs(fileName);

                status = true;
                msg += string.Format("\nThe file '{0}' is saved successfully!", fileName);

            }
            catch (Exception exception)
            {
                status = false;
                msg += "\nThere was a PROBLEM saving Excel file! [" + fileName + "]  \n" + exception.Message;
            }
            finally
            {
                // Quit Excel application
                excel_NormalxOt.Quit();

                // Release COM objects (very important!)
                if (excel_NormalxOt != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_NormalxOt);

                if (workSheet_NormalxOt != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet_NormalxOt);

                // Empty variables
                excel_NormalxOt = null;
                workSheet_NormalxOt = null;

                // Force garbage collector cleaning
                GC.Collect();
            }
            #endregion NormalxOt

            #region Perdiem


            // Define filename
            fileName = string.Format(@"{0}\Perdiem_" + epMonth + ".xlsx", subroot);


            // Load Excel application
            Microsoft.Office.Interop.Excel.Application excel_Perdiem = new Microsoft.Office.Interop.Excel.Application();

            // Create empty workbook
            excel_Perdiem.Workbooks.Add();
            // Create Worksheet from active sheet
            Microsoft.Office.Interop.Excel._Worksheet workSheet_Perdiem = excel_Perdiem.ActiveSheet;
            workSheet_Perdiem.Name = "Perdiem";
            // I created Application and Worksheet objects before try/catch,
            // so that i can close them in finnaly block.
            // It's IMPORTANT to release these COM objects!!
            try
            {
                // ------------------------------------------------
                // Creation of header cells
                // ------------------------------------------------
                workSheet_Perdiem.Cells[1, "A"] = "Cost Center";
                workSheet_Perdiem.Cells[1, "B"] = "Withdrawal";
                workSheet_Perdiem.Cells[1, "C"] = "Trainee Code";
                workSheet_Perdiem.Cells[1, "D"] = "Name";
                workSheet_Perdiem.Cells[1, "E"] = "Client Name";
                workSheet_Perdiem.Cells[1, "F"] = "Code";
                workSheet_Perdiem.Cells[1, "G"] = "Amount";

                workSheet_Perdiem.Range["A1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

                // ------------------------------------------------
                // Populate sheet with some real data from "list_" list
                // ------------------------------------------------
                int row = 2; // start row (in row 1 are header cells)
                var groupperdiemtransport = list_.lstData_PerdiemTransport.GroupBy(g => g.Cost_Center).ToList();
                decimal Aamount = 0;
                foreach (var ex in groupperdiemtransport)
                {
                    decimal amount = 0;
                    foreach (ResultLine_Sum_Perdiem rs in ex)
                    {
                        amount += SystemFunction.GetNumberNullToZero(rs.Amount + "");

                        workSheet_Perdiem.Cells[row, "A"] = rs.Cost_Center;
                        workSheet_Perdiem.Cells[row, "B"] = rs.Type_of_withdrawal == "P" ? "Perdiem" : "Transport";
                        workSheet_Perdiem.Cells[row, "C"] = rs.Trainee_Code;
                        workSheet_Perdiem.Cells[row, "D"] = rs.Name;
                        workSheet_Perdiem.Cells[row, "E"] = rs.Engagement_Name;
                        workSheet_Perdiem.Cells[row, "F"] = rs.Engagement_Code;
                        workSheet_Perdiem.Cells[row, "G"] = rs.Amount.Value.ToString("#,###.00");

                        row++;
                    }

                    Aamount += amount;

                    workSheet_Perdiem.Cells[row, "A"] = ex.First().Cost_Center + " Total";
                    workSheet_Perdiem.Cells[row, "G"] = amount.ToString("#,###.00");

                    workSheet_Perdiem.get_Range("A" + row, "G" + row).Interior.Color = XlRgbColor.rgbLightSkyBlue;
                    workSheet_Perdiem.get_Range("A" + row, "G" + row).Font.Bold = true;

                    row++;
                }
                workSheet_Perdiem.Cells[row, "A"] = "Grand Total";
                workSheet_Perdiem.Cells[row, "G"] = Aamount.ToString("#,###.00");
                workSheet_Perdiem.get_Range("A" + row, "G" + row).Interior.Color = XlRgbColor.rgbLightSkyBlue;
                workSheet_Perdiem.get_Range("A" + row, "G" + row).Font.Bold = true;

                workSheet_Perdiem.get_Range("A1", "G1").Interior.Color = XlRgbColor.rgbSkyBlue;
                workSheet_Perdiem.get_Range("A1", "G" + row).Borders.Color = XlRgbColor.rgbBlack;

                // Save this data as a file
                workSheet_Perdiem.SaveAs(fileName);

                status = true;
                msg += string.Format("\nThe file '{0}' is saved successfully!", fileName);

            }
            catch (Exception exception)
            {
                status = false;
                msg += "\nThere was a PROBLEM saving Excel file! [" + fileName + "]  \n" + exception.Message;
            }
            finally
            {
                // Quit Excel application
                excel_Perdiem.Quit();

                // Release COM objects (very important!)
                if (excel_Perdiem != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_Perdiem);

                if (workSheet_Perdiem != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet_Perdiem);

                // Empty variables
                excel_Perdiem = null;
                workSheet_Perdiem = null;

                // Force garbage collector cleaning
                GC.Collect();
            }
            #endregion Perdiem

            #region OtSub


            // Define filename
            fileName = string.Format(@"{0}\OT_Detail_" + epMonth + ".xlsx", subroot);

            // Load Excel application
            Microsoft.Office.Interop.Excel.Application excel_OtSub = new Microsoft.Office.Interop.Excel.Application();

            // Create empty workbook
            excel_OtSub.Workbooks.Add();
            // Create Worksheet from active sheet
            Microsoft.Office.Interop.Excel._Worksheet workSheet_OtSub = excel_OtSub.ActiveSheet;
            workSheet_OtSub.Name = "OT Detail";
            // I created Application and Worksheet objects before try/catch,
            // so that i can close them in finnaly block.
            // It's IMPORTANT to release these COM objects!!
            try
            {
                // ------------------------------------------------
                // Creation of header cells
                // ------------------------------------------------
                workSheet_OtSub.Cells[1, "A"] = "Trainee Name";
                workSheet_OtSub.Cells[1, "B"] = "Month";
                workSheet_OtSub.Cells[1, "C"] = "Daily Wage";
                workSheet_OtSub.Cells[1, "D"] = "Rate 1 (hrs)";
                workSheet_OtSub.Cells[1, "E"] = "Rate 2 (hrs)";
                workSheet_OtSub.Cells[1, "F"] = "Rate 3 (hrs)";
                workSheet_OtSub.Cells[1, "G"] = "Total (hrs)";
                workSheet_OtSub.Cells[1, "H"] = "Total (Baht)";

                workSheet_OtSub.Range["A1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

                // ------------------------------------------------
                // Populate sheet with some real data from "list_" list
                // ------------------------------------------------
                int row = 2; // start row (in row 1 are header cells)
                var group = list_.lstData_OT.GroupBy(g => g.name).ToList();
                decimal ar1 = 0;
                decimal ar2 = 0;
                decimal ar3 = 0;
                decimal atth = 0;
                decimal atta = 0;
                foreach (var ex in group)
                {
                    decimal r1 = 0;
                    decimal r2 = 0;
                    decimal r3 = 0;
                    decimal tth = 0;
                    decimal tta = 0;
                    foreach (ResultLine_OT rs in ex)
                    {
                        r1 += SystemFunction.GetNumberNullToZero(rs.rate1 + "");
                        r2 += SystemFunction.GetNumberNullToZero(rs.rate2 + "");
                        r3 += SystemFunction.GetNumberNullToZero(rs.rate3 + "");
                        tth += SystemFunction.GetNumberNullToZero(rs.totalhrs + "");
                        tta += SystemFunction.GetNumberNullToZero(rs.total + "");

                        workSheet_OtSub.Cells[row, "A"] = rs.name;
                        workSheet_OtSub.Cells[row, "B"] = rs.month;
                        workSheet_OtSub.Cells[row, "C"] = rs.daily_wage;
                        workSheet_OtSub.Cells[row, "D"] = SystemFunction.GetNumberNullToZero(rs.rate1).ToString("#,###.00");
                        workSheet_OtSub.Cells[row, "E"] = SystemFunction.GetNumberNullToZero(rs.rate2).ToString("#,###.00");
                        workSheet_OtSub.Cells[row, "F"] = SystemFunction.GetNumberNullToZero(rs.rate3).ToString("#,###.00");
                        workSheet_OtSub.Cells[row, "G"] = SystemFunction.GetNumberNullToZero(rs.totalhrs).ToString("#,###.00");
                        workSheet_OtSub.Cells[row, "H"] = SystemFunction.GetNumberNullToZero(rs.total).ToString("#,###.00");


                        row++;
                    }
                    ar1 += r1;
                    ar2 += r2;
                    ar3 += r3;
                    atth += tth;
                    atta += tta;
                    workSheet_OtSub.Cells[row, "A"] = ex.First().name + " Total";
                    workSheet_OtSub.Cells[row, "D"] = r1.ToString("#,###.00");
                    workSheet_OtSub.Cells[row, "E"] = r2.ToString("#,###.00");
                    workSheet_OtSub.Cells[row, "F"] = r3.ToString("#,###.00");
                    workSheet_OtSub.Cells[row, "G"] = tth.ToString("#,###.00");
                    workSheet_OtSub.Cells[row, "H"] = tta.ToString("#,###.00");

                    workSheet_OtSub.get_Range("A" + row, "H" + row).Interior.Color = XlRgbColor.rgbLightSkyBlue;
                    workSheet_OtSub.get_Range("A" + row, "H" + row).Font.Bold = true;

                    row++;
                }
                workSheet_OtSub.Cells[row, "A"] = "Grand Total";
                workSheet_OtSub.Cells[row, "D"] = ar1.ToString("#,###.00");
                workSheet_OtSub.Cells[row, "E"] = ar2.ToString("#,###.00");
                workSheet_OtSub.Cells[row, "F"] = ar3.ToString("#,###.00");
                workSheet_OtSub.Cells[row, "G"] = atth.ToString("#,###.00");
                workSheet_OtSub.Cells[row, "H"] = atta.ToString("#,###.00");

                workSheet_OtSub.get_Range("A" + row, "H" + row).Interior.Color = XlRgbColor.rgbLightSkyBlue;
                workSheet_OtSub.get_Range("A" + row, "H" + row).Font.Bold = true;


                workSheet_OtSub.get_Range("A1", "H1").Interior.Color = XlRgbColor.rgbSkyBlue;
                workSheet_OtSub.get_Range("A1", "H" + row).Borders.Color = XlRgbColor.rgbBlack;

                // Save this data as a file
                workSheet_OtSub.SaveAs(fileName);
                status = true;
                msg += string.Format("\nThe file '{0}' is saved successfully!", fileName);

            }
            catch (Exception exception)
            {
                status = false;
                msg += "\nThere was a PROBLEM saving Excel file! [" + fileName + "]  \n" + exception.Message;
            }
            finally
            {
                // Quit Excel application
                excel_OtSub.Quit();

                // Release COM objects (very important!)
                if (excel_OtSub != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_OtSub);

                if (workSheet_OtSub != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet_OtSub);

                // Empty variables
                excel_OtSub = null;
                workSheet_OtSub = null;

                // Force garbage collector cleaning
                GC.Collect();
            }
            #endregion OtSub

            #region Autopay

            // Define filename
            fileName = string.Format(@"{0}\AutoPay_" + epMonth + ".xlsx", subroot);


            // Load Excel application
            Microsoft.Office.Interop.Excel.Application excel_Autopay = new Microsoft.Office.Interop.Excel.Application();

            // Create empty workbook
            excel_Autopay.Workbooks.Add();
            // Create Worksheet from active sheet
            Microsoft.Office.Interop.Excel._Worksheet workSheet_Autopay = excel_Autopay.ActiveSheet;
            workSheet_Autopay.Name = "Autopay";
            // I created Application and Worksheet objects before try/catch,
            // so that i can close them in finnaly block.
            // It's IMPORTANT to release these COM objects!!
            try
            {
                // ------------------------------------------------
                // Creation of header cells
                // ------------------------------------------------
                workSheet_Autopay.Cells[1, "A"] = "Vendor ID";
                workSheet_Autopay.Cells[1, "B"] = "Bank Code";
                workSheet_Autopay.Cells[1, "C"] = "Account Number";
                workSheet_Autopay.Cells[1, "D"] = "Vendor Name";
                workSheet_Autopay.Cells[1, "E"] = "Amount";
                workSheet_Autopay.Cells[1, "F"] = "Bene Ref";
                workSheet_Autopay.Cells[1, "G"] = "WHT";
                workSheet_Autopay.Cells[1, "H"] = "Advice";
                workSheet_Autopay.Cells[1, "I"] = "SMS";
                workSheet_Autopay.Cells[1, "J"] = "Payment";
                workSheet_Autopay.Cells[1, "K"] = "#Invoice";
                workSheet_Autopay.Cells[1, "L"] = "Branch Code";

                workSheet_Autopay.Range["A1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

                // ------------------------------------------------
                // Populate sheet with some real data from "list_" list
                // ------------------------------------------------
                int row = 2; // start row (in row 1 are header cells)
                decimal amounts = 0;
                foreach (ResultLine_AutoPay rs in list_.lstData_AutoPay)
                {
                    amounts += SystemFunction.GetNumberNullToZero(rs.Amount + "");

                    workSheet_Autopay.Cells[row, "A"] = rs.Vendor_ID;
                    workSheet_Autopay.Cells[row, "B"] = rs.Bank_Code;
                    workSheet_Autopay.Cells[row, "C"] = rs.Account_Number;
                    workSheet_Autopay.Cells[row, "D"] = rs.Vendor_Name;
                    workSheet_Autopay.Cells[row, "E"] = SystemFunction.GetNumberNullToZero(rs.Amount).ToString("#,###.00");
                    workSheet_Autopay.Cells[row, "F"] = rs.Bene_Ref;
                    workSheet_Autopay.Cells[row, "G"] = rs.WHT;
                    workSheet_Autopay.Cells[row, "H"] = rs.Advice;
                    workSheet_Autopay.Cells[row, "I"] = rs.SMS;
                    workSheet_Autopay.Cells[row, "J"] = rs.Payment_Detail;
                    workSheet_Autopay.Cells[row, "K"] = rs.Invoice;
                    workSheet_Autopay.Cells[row, "L"] = rs.Branch_Code;


                    row++;
                }
                workSheet_Autopay.Cells[row, "A"] = "Grand Total";
                workSheet_Autopay.Cells[row, "E"] = amounts.ToString("#,###.00");
                workSheet_Autopay.get_Range("A" + (row - 1), "L" + row).Interior.Color = XlRgbColor.rgbSkyBlue;
                workSheet_Autopay.get_Range("A" + row, "L" + row).Interior.Color = XlRgbColor.rgbLightSkyBlue;
                workSheet_Autopay.get_Range("A" + row, "L" + row).Font.Bold = true;


                workSheet_Autopay.get_Range("A1", "L1").Interior.Color = XlRgbColor.rgbSkyBlue;
                workSheet_Autopay.get_Range("A1", "L" + row).Borders.Color = XlRgbColor.rgbBlack;


                // Save this data as a file
                workSheet_Autopay.SaveAs(fileName);

                status = true;
                msg += string.Format("\nThe file '{0}' is saved successfully!", fileName);

            }
            catch (Exception exception)
            {
                status = false;
                msg += "\nThere was a PROBLEM saving Excel file! [" + fileName + "]  \n" + exception.Message;
            }
            finally
            {
                // Quit Excel application
                excel_Autopay.Quit();

                // Release COM objects (very important!)
                if (excel_Autopay != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel_Autopay);

                if (workSheet_Autopay != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet_Autopay);

                // Empty variables
                excel_Autopay = null;
                workSheet_Autopay = null;

                // Force garbage collector cleaning
                GC.Collect();
            }
            #endregion Autopay
            return msg;



        }

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
        [HttpPost]
        public JsonResult GetEngagement(string university_id)
        {
            Viztopia.ViztopiaSoapClient wsViztp = new Viztopia.ViztopiaSoapClient();

            string ChargeCode = "";
            string CompName = "";

            var data = wsViztp.GetAllEngagementFromSSA("", "", "2,3", "", "", "", "", "", "", "", "", "");

            lst_Engagement = new List<Engagement_PR>();
            lst_Engagement = data.AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("EngagementNo"), text = dataRow.Field<string>("EngagementName"), value = dataRow.Field<string>("EngagementManagerNo"), comp = dataRow.Field<string>("ClientName") }).ToList();


            var indata = wsViztp.Get_Internal_Chargecode("", "");
            var a = new List<Engagement_PR>();
            a = indata.Tables[0].AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("Code"), text = dataRow.Field<string>("Description"), comp = dataRow.Field<string>("Description") }).ToList();


            lst_Engagement.AddRange(a);


            return Json(lst_Engagement, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}