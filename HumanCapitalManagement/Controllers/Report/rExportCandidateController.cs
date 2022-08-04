using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Report.DataSet;
using HumanCapitalManagement.Report.DevReport.Candidate;
using HumanCapitalManagement.Service.Common;
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
    public class rExportCandidateController : BaseController
    {
        private DivisionService _DivisionService;
        //private TM_GenderService _TM_GenderService;

        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        private New_HRISEntities dbHr = new New_HRISEntities();
        public rExportCandidateController(DivisionService DivisionService)
        {
            _DivisionService = DivisionService;
        }
        // GET: rHeadcount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewExportCandidate()
        {

            StoreDb db = new StoreDb();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            vExportCandidate_Result result = new vExportCandidate_Result();
            List<vExportCandidateData> lstData = new List<vExportCandidateData>();


            lstData = (from lst in db.TM_Candidates
                       select new vExportCandidateData
                       {
                           //Prefix = lst.candidate_Prefix,
                           First_Name = lst.first_name_en,
                           Last_Name = lst.last_name_en,
                           Nickname = lst.candidate_NickName,
                           Alternative_Name_TH = lst.candidate_AlternativeNameTH,
                           Gender = lst.Gender.GenderNameEN.Substring(0, 1),
                           Date_of_Birth = lst.candidate_DateOfBirth.ToString(),
                           Birthplace = lst.candidate_BirthPlace,
                           Country_of_Birth = lst.candidate_CountryOfBirth,
                           Marital_Status = lst.MaritalStatusName.MaritalStatusNameEN,                        
                           //Nationality = lst.candidate_Nationality,
                           Identity_Number = lst.id_card,
                           PA_Address = lst.candidate_PAHouseNo,
                           PA_Moo_And_Soi = lst.candidate_PAMooAndSoi,
                           PA_Street_And_Tambol = lst.candidate_PAStreet + " " + lst.PA_TM_SubDistrict.subdistrict_name_th,
                           PA_City = lst.PA_TM_SubDistrict.TM_District.TM_City.city_name_th,
                           PA_District = lst.PA_TM_SubDistrict.TM_District.district_name_th,
                           PA_Country = lst.PA_TM_SubDistrict.TM_District.TM_City.TM_Country.country_name_th,
                           PA_Mobile_Number = lst.candidate_PAMobileNumber,
                           PA_Postal_Code = lst.candidate_PAPostalCode,
                           PA_House_No = lst.candidate_PAHouseNo,
                           PA_Telephone_Number = lst.candidate_PATelephoneNumber,
                           CA_Address = lst.candidate_CAHouseNo,
                           CA_House_No = lst.candidate_CAHouseNo,
                           CA_Moo_Soi = lst.candidate_CAMooAndSoi,
                           CA_Street_And_Tambol = lst.candidate_CAStreet + " " + lst.CA_TM_SubDistrict.subdistrict_name_th,
                           CA_District = lst.CA_TM_SubDistrict.TM_District.district_name_th,
                           CA_City = lst.CA_TM_SubDistrict.TM_District.TM_City.city_name_th,
                           CA_Postal_Code = lst.candidate_CAPostalCode,
                           CA_Country = lst.CA_TM_SubDistrict.TM_District.TM_City.TM_Country.country_name_th,
                           CA_Telephone_Number = lst.candidate_CATelephoneNumber,
                           CA_Mobile_Number = lst.candidate_CAMobileNumber,
                           Bank_Name = lst.candidate_BankAccountName,
                           Account_Number = lst.candidate_BankAccountNumber,
                           Social_Security_TH = lst.candidate_SocialSecurityTH,
                           Provident_Fund_TH = lst.candidate_ProvidentFundTH,
                           Death_Contribution =lst.candidate_DeathContribution,
                           Institute_location_of_training = lst.candidate_EduInstituteOrLocationOfTraining,
                           Country = lst.candidate_EduCountry,
                           Final_GPA = lst.candidate_EduCurrentGPATranscript,
                           //Final_GPA = lst.TM_
                           Certificate = "--Hold--",
                           Major_study = "--Hold--",
                           Status = "--Hold--",
                           EngTestName = "--Hold--",
                           Oxford_Score = "--Hold--",
                           Oxford_Test_Date = "--Hold--",
                           StrengthName1 = "--Hold--",
                           StrengthName2 = "--Hold--",
                           StrengthName3 = "--Hold--",
                           StrengthName4 = "--Hold--",
                           StrengthName5 = "--Hold--",
                           NMG = lst.candidate_NMGTestScore + "" ,
                           NMG_VMG_Test_Date = "--Hold--",
                            
                           VMG = "--Hold--",
                           Sourcing = lst.TM_SourcingChannel.sourcingchannel_name_en,
                           Trainee_code = lst.candidate_TraineeNumber,
                           Rank = lst.TM_PR_Candidate_Mapping.Select(x => x.TM_Candidate_Rank.crank_name_en).FirstOrDefault() == null ? "" : lst.TM_PR_Candidate_Mapping.Select(x => x.TM_Candidate_Rank.crank_name_en).FirstOrDefault().ToString(),
                  
                       }).ToList();



            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpViewExportCandidate" + unixTimestamps;
            result.session = sSession;
            Session[sSession] = new List<vExportCandidateData>();
            if (lstData.Any())
            {
                result.lstData = lstData.ToList();
                Session[sSession] = lstData.ToList();
            }


            return View(result);

        }


        public ActionResult ExpViewExportCandidateByID(int candidateID, string sMode)
        {

            StoreDb db = new StoreDb();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }


            DateTime dNow = DateTime.Now;
            List<vExportCandidateData> lstData_result = new List<vExportCandidateData>();


            List<vExportCandidateData> lstData = new List<vExportCandidateData>();

            lstData = (from lst in db.TM_Candidates
                       where lst.Id == candidateID
                       select new vExportCandidateData
                       {
                           //Prefix = lst.candidate_Prefix,
                           First_Name = lst.first_name_en,
                           Last_Name = lst.last_name_en,
                           Nickname = lst.candidate_NickName,
                           Alternative_Name_TH = lst.candidate_AlternativeNameTH,
                           Gender = lst.candidate_Gender,
                           Date_of_Birth = lst.candidate_DateOfBirth.ToString(),
                           Birthplace = lst.candidate_BirthPlace,
                           Country_of_Birth = lst.candidate_CountryOfBirth,
                           Marital_Status = lst.candidate_MaritalStatus,
                           //Nationality = lst.candidate_Nationality,

                       }).ToList();


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
                            dsExportCandidate ds = new dsExportCandidate();
                            ds.tbExportCandidate.Merge(dtR);
                            ReportExportCandidateXls report = new ReportExportCandidateXls();
                            XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                            xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;
                            report.DataSource = ds;
                            report.CreateDocument();
                            PdfExportOptions options = new PdfExportOptions();
                            options.ShowPrintDialogOnOpen = true;

                            //Add page
                            xrTemp.Pages.AddRange(report.Pages);

                            xrTemp.ExportToPdf(stream);
                            return File(stream.GetBuffer(), "application/pdf", "ExportCandidate" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                        }
                        else
                        {

                            xrTemp.CreateDocument();

                            //DataTable dtR = SystemFunction.LinqToDataTable(lstData);
                            //dsExportCandidate ds = new dsExportCandidate();
                            //ds.tbExportCandidate.Merge(dtR);
                            ReportExportCandidateXls report = new ReportExportCandidateXls();
                            //report.DataSource = ds;
                            report.CreateDocument();
                            //Add page
                            xrTemp.Pages.AddRange(report.Pages);

                            XlsxExportOptions xlsxOptions = xrTemp.ExportOptions.Xlsx;
                            xlsxOptions.ShowGridLines = true;
                            xlsxOptions.SheetName = "sheet_";
                            xlsxOptions.TextExportMode = TextExportMode.Text;
                            xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;

                            xrTemp.ExportToXlsx(stream, xlsxOptions);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] breport = stream.ToArray();
                            return File(breport, "application/excel", "ExportCandidate" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");


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

        public ActionResult ExpViewExportCandidate(string qryStr, string sMode)
        {
            var sCheck = acCheckLoginAndPermission(true);
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            List<vExportCandidateData> lstData_result = new List<vExportCandidateData>();

            if (qryStr != "")
            {
                if (Session[qryStr] != null)
                {
                    List<vExportCandidateData> lstData = Session[qryStr] as List<vExportCandidateData>;
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
                                    //ViewExportCandidate
                                    DataTable dtR = SystemFunction.LinqToDataTable(lstData);
                                    dsExportCandidate ds = new dsExportCandidate();
                                    ds.tbExportCandidate.Merge(dtR);
                                    ReportExportCandidateXls report = new ReportExportCandidateXls();
                                    XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                                    xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;
                                    report.DataSource = ds;
                                    report.CreateDocument();
                                    PdfExportOptions options = new PdfExportOptions();
                                    options.ShowPrintDialogOnOpen = true;

                                    //Add page
                                    xrTemp.Pages.AddRange(report.Pages);

                                    xrTemp.ExportToPdf(stream);
                                    return File(stream.GetBuffer(), "application/pdf", "ExportCandidate" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                                }
                                else
                                {
                              
                                    DataTable dtR = SystemFunction.LinqToDataTable(lstData);
                                    dsExportCandidate ds = new dsExportCandidate();
                                    ds.tbExportCandidate.Merge(dtR);
                                    ReportExportCandidateXls report = new ReportExportCandidateXls();
                                    report.DataSource = ds;
                                    report.CreateDocument();
                                
                                    XlsxExportOptions xlsxOptions = report.ExportOptions.Xlsx;
                                    xlsxOptions.ShowGridLines = true;

                                    xlsxOptions.TextExportMode = TextExportMode.Text;
 
                                    report.ExportToXlsx(stream, xlsxOptions);
                                    stream.Seek(0, SeekOrigin.Begin);
                                    byte[] breport = stream.ToArray();
                                    return File(breport, "application/excel", "ExportCandidate" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");

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