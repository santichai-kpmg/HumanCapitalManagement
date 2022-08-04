using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Controllers.MainController;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Report.DataSet;
using HumanCapitalManagement.Report.DevReport.Candidate;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Report
{
    public class rNewJoinerController : BaseController
    {
        private DivisionService _DivisionService;
        private TM_CandidatesService _TM_CandidatesService;
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private CompanyService _CompanyService;
        private TM_Pool_RankService _TM_Pool_RankService;
        private PoolService _PoolService;
        private TM_SubGroupService _TM_SubGroupService;
        private RankService _RankService;
        private PersonnelRequestService _PersonnelRequestService;
        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        private New_HRISEntities dbHr = new New_HRISEntities();
        public rNewJoinerController(DivisionService DivisionService, 
            TM_CandidatesService TM_CandidatesService, 
            TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService, 
            CompanyService CompanyService, 
            TM_Pool_RankService TM_Pool_RankService,
            PoolService PoolService,
            TM_SubGroupService TM_SubGroupService,
            RankService RankService,
            PersonnelRequestService PersonnelRequestService)
        {
            _DivisionService = DivisionService;
            _TM_CandidatesService = TM_CandidatesService;
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _CompanyService = CompanyService;
            _TM_Pool_RankService = TM_Pool_RankService;
            _PoolService = PoolService;
            _TM_SubGroupService = TM_SubGroupService;
            _RankService = RankService;
            _PersonnelRequestService = PersonnelRequestService;

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NJOnboardingList()
        {

            StoreDb db = new StoreDb();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }


            vNewJoinerReport_Result result = new vNewJoinerReport_Result();
            List<vNewJoinerReportData> lstData = new List<vNewJoinerReportData>();
            //System.Globalization.CultureInfo _cultureEnInfo = new System.Globalization.CultureInfo("en-US");
            //private _TM_CandidateService 

            /*
            var id = 1;
            var query =
            from post in database.Posts
            join meta in database.Post_Metas on post.ID equals meta.Post_ID
            where post.ID == id
            select new { Post = post, Meta = meta };
            */


            lstData = (from lst in _TM_CandidatesService.GetDataForSelect()
                       join lst2 in _TM_PR_Candidate_MappingService.GetDataForSelect() on lst.Id equals lst2.TM_Candidates.Id
                       join lst3 in _PersonnelRequestService.GetDataForSelect() on lst2.PersonnelRequest.Id equals lst3.Id
                       join lst4 in _DivisionService.GetAll() on lst3.TM_Divisions.Id equals lst4.Id
                       join lst5 in _PoolService.GetDataForSelect() on lst4.TM_Pool.Id equals lst5.Id
                       join lst6 in _CompanyService.GetDataForSelect() on lst5.TM_Company.Id equals lst6.Id
                       /*var query = objEntities.Employee.Join(objEntities.Department, r => r.EmpId, p => p.EmpId, (r, p) =>
                        * new { r.FirstName, r.LastName, p.DepartmentName });
                       GridView1.DataSource = query;
                       GridView1.DataBind();
                       }
                       */
                       where lst.candidate_DateOfBirth != null && lst.candidate_DateOfBirth.ToString() != ""
                       select new vNewJoinerReportData
                       {
                           No = lst.Id + "",
                           // result.candidate_NMGTestDate = _getData.candidate_NMGTestDate.HasValue ? _getData.candidate_NMGTestDate.Value.DateTimebyCulture() : "-- / -- / ----";
                           
                           //StartDate = lst.candidate_ApplyDate ,
                           StartDate = lst.candidate_ApplyDate.HasValue? lst.candidate_ApplyDate.Value.DateTimebyCulture() : "",
                           //Division = 
                           Division = "Division",
                           Company = lst.candidate_Company??"",
                           //Division = "division",
                           //Division = lst.TM_PR_Candidate_Mapping.Join
                           //Group = lst
                           PositionTitle = lst.candidate_Position??"",
                           RankForHiring = lst.RankGradeForHiring ?? "",
                           TypeOfEmployee = lst.candidate_TypeOfEmployment ?? "",

                           EN_Prefix = lst.prefixEN == null? "": lst.prefixEN.PrefixNameEN,
                           EN_FirstName = lst.first_name_en ?? "",
                           EN_LastName = lst.last_name_en ?? "",
                           EN_NickName = lst.candidate_NickName ?? "",
                           TH_Name = lst.name_th ?? "",
                           Gender = lst.Gender == null? ""  : lst.Gender.GenderNameEN,

                           DateOfBirth = lst.candidate_DateOfBirth.HasValue? lst.candidate_DateOfBirth.Value.DateTimebyCulture() : "",
                           Birthplace = lst.candidate_BirthPlace,
                           CountryOfBirth = lst.CountryOfBirth == null? "" : lst.CountryOfBirth.country_name_en,
                           MaritalStatus = lst.MaritalStatusName == null? "" : lst.MaritalStatusName.MaritalStatusNameEN,
                           Nationality = lst.Nationalities == null? "" : lst.Nationalities.NationalitiesNameEN,
                           IdentificationNumber = lst.id_card + "",
                           PHouseNo = lst.candidate_PAHouseNo,
                           PMooAndSoi = lst.candidate_PAMooAndSoi,
                           PRoad = lst.candidate_PAStreet,
                           PSubDistrict = lst.PA_TM_SubDistrict == null? "" : lst.PA_TM_SubDistrict.subdistrict_name_en,
                           PProvince = lst.PA_TM_SubDistrict == null? "" : lst.PA_TM_SubDistrict.TM_District.TM_City.city_name_en,
                           PPostalCode = lst.candidate_PAPostalCode,
                           PCountry = lst.PA_TM_SubDistrict == null? "" : lst.PA_TM_SubDistrict.TM_District.TM_City.TM_Country.country_name_en,
                           PTelephoneNumber = lst.candidate_PATelephoneNumber,
                           PMobile = lst.candidate_PAMobileNumber,
                           CHouseNo = lst.candidate_CAHouseNo,
                           CMooAndSoi = lst.candidate_CAMooAndSoi,
                           CRoad = lst.candidate_CAStreet,
                           CSubDistrict = lst.CA_TM_SubDistrict == null? "" :  lst.CA_TM_SubDistrict.subdistrict_name_en,
                           CDistrict = lst.CA_TM_SubDistrict == null? "" : lst.CA_TM_SubDistrict.TM_District.district_name_en,
                           CProvince = lst.CA_TM_SubDistrict == null? "" : lst.CA_TM_SubDistrict.TM_District.TM_City.city_name_en,
                           CPostalCode = lst.candidate_CAPostalCode,
                           CCountry = lst.CA_TM_SubDistrict == null? "" : lst.CA_TM_SubDistrict.TM_District.TM_City.TM_Country.country_name_en,
                           CTelephoneNumber = lst.candidate_CATelephoneNumber,
                           CMobile = lst.candidate_CAMobileNumber,
                           BankAccountName =  lst.candidate_BankAccountName,
                           BankAccountNumber = lst.candidate_BankAccountNumber,
                           BankAccountBranchName = lst.candidate_BankAccountBranchName,
                           
                           StudentID = lst.candidate_StudentID+"",
                           SocialSecurity_TH = lst.candidate_SocialSecurityTH,
                           ProvidentFund_TH = lst.candidate_ProvidentFundTH,
                           DeathContribution = lst.candidate_DeathContribution,
                           Country = lst.candidate_EduCountry,
                           BCurrentGPATranscript = lst.TM_Education_History == null? "" : lst.TM_Education_History.Where(x => x.end_date == lst.TM_Education_History.Max(y => y.end_date).Value).Select(z => z.grade).FirstOrDefault() + "",
                           Certificate =  lst.CurrentOrLatestDegree.degree_name_en ,
                           //BMajorStudy = lst.TM_Education_History.Where(x => x.end_date == lst.TM_Education_History.Max(y => y.end_date).Value).Select(z => z.TM_Universitys_Major.TM_Major.major_name_en).FirstOrDefault() + "",
                           //RecruitmentStatus = "",//already had in master 214 fields
                          
                           EnglishTestName = lst.TM_TechnicalTestTransaction == null? "" : lst.TM_TechnicalTestTransaction.Where(x => x.TM_TechnicalTest.Test_name_en.Contains("Oxford") || x.TM_TechnicalTest.Test_name_en.Contains("TOEIC")).Select(x => x.TM_TechnicalTest.Test_name_en).FirstOrDefault(),
                           EnglishTestScores = lst.TM_TechnicalTestTransaction == null ? "" : lst.TM_TechnicalTestTransaction.Where(x => x.TM_TechnicalTest.Test_name_en.Contains("Oxford") || x.TM_TechnicalTest.Test_name_en.Contains("TOEIC")).Select(x => x.Test_Score).FirstOrDefault() + "",
                           EnglishTestDate = lst.TM_TechnicalTestTransaction == null ? "" : lst.TM_TechnicalTestTransaction.Where(x => x.TM_TechnicalTest.Test_name_en.Contains("Oxford") || x.TM_TechnicalTest.Test_name_en.Contains("TOEIC")).Select(x => x.Test_Date).FirstOrDefault() + "",
                           SourcingChannel = lst.TM_SourcingChannel == null? "" : lst.TM_SourcingChannel.sourcingchannel_name_en,
                           TraineeNumber = lst.candidate_TraineeNumber,
                           MilitaryServicesDoc = lst.candidate_MilitaryServicesDoc,
                           IBMP = lst.candidate_IBMP,
                           //TechnicalTest1Score = "TechnicalTest1Score",
                           //TechnicalTest1Date = "TechnicalTest1Date",
                           Email = lst.candidate_Email,
                           BProgram = lst.TM_Education_History == null? "" : lst.TM_Education_History.Where(x => x.end_date == lst.TM_Education_History.Max(y => y.end_date).Value).Select(z => z.TM_Universitys_Major.TM_Universitys_Faculty.universitys_faculty_name_en).FirstOrDefault() + "",
                           OfficialNoteForAnnouncement = lst.candidate_OfficialNote,
                           InternalNoteForHRTeam = lst.candidate_InternalNoteForHRTeam,

                       }).Take(10).ToList();

            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpViewExportNewJoiner" + unixTimestamps;
            result.session = sSession;
            Session[sSession] = new List<vNewJoinerReportData>();
            if (lstData.Any())
            {
                result.lstData = lstData.ToList();
                Session[sSession] = lstData.ToList();
            }


            return View(result);


            //return View();
        }

        public ActionResult ExpNJOnboardingList(string qryStr, string sMode)
        {

            var sCheck = acCheckLoginAndPermission(true);
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            List<vNewJoinerReportData> lstData_result = new List<vNewJoinerReportData>();

            if (qryStr != "")
            {
                if (Session[qryStr] != null)
                {
                    List<vNewJoinerReportData> lstData = Session[qryStr] as List<vNewJoinerReportData>;
                    if (lstData.Any())
                    {
                        try
                        {
                            if (lstData.Any())
                            {
                                var stream = new MemoryStream();
                                XrTemp xrTemp = new XrTemp();
                                if (!string.IsNullOrEmpty(sMode) && sMode == "pdf")
                                {

                                    xrTemp.CreateDocument();
                                    DataTable dtR = SystemFunction.LinqToDataTable(lstData);
                                    dsNJOnboardingList ds = new dsNJOnboardingList();
                                    ds.tbNJOnboardingList.Merge(dtR);

                                    ReportNJOnboardingList report = new global::ReportNJOnboardingList();
                                    XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                                    xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;
                                    report.DataSource = ds;
                                    report.CreateDocument();
                                    PdfExportOptions options = new PdfExportOptions();
                                    options.ShowPrintDialogOnOpen = true;

                                    //Add page
                                    xrTemp.Pages.AddRange(report.Pages);

                                    xrTemp.ExportToPdf(stream);
                                    return File(stream.GetBuffer(), "application/pdf", "ExportNewJoinerOnboardingList" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                                }
                                else
                                {

                                    DataTable dtR = SystemFunction.LinqToDataTable(lstData);
                                    dsNJOnboardingList ds = new dsNJOnboardingList();
                                    ds.tbNJOnboardingList.Merge(dtR);
                                    ReportNJOnboardingList report = new ReportNJOnboardingList();
                                    report.DataSource = ds;
                                    report.CreateDocument();

                                    XlsxExportOptions xlsxOptions = report.ExportOptions.Xlsx;
                                    xlsxOptions.ShowGridLines = true;

                                    xlsxOptions.TextExportMode = TextExportMode.Text;

                                    report.ExportToXlsx(stream, xlsxOptions);
                                    stream.Seek(0, SeekOrigin.Begin);
                                    byte[] breport = stream.ToArray();
                                    return File(breport, "application/excel", "ExportNewJoinerOnboardingList" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");

                                }
                            }
                            return View();
                        }
                        catch (Exception e)
                        {
                            return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                            throw;
                        }
                    }
                    else
                    {
                        return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error Data not found." });
                    }
                }
                else
                {
                    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                }
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }
        }


    }




}