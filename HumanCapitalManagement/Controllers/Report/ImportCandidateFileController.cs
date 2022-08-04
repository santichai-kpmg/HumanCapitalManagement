using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Report.DataSet;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.EduService;
using HumanCapitalManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using static HumanCapitalManagement.ViewModel.vTempTransaction;
using System.Globalization;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using HumanCapitalManagement.Service.AddressService;
using static HumanCapitalManagement.App_Start.HCMClass;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Controllers.MainController;

namespace HumanCapitalManagement.Controllers.Report
{

    public class ImportCandidateFileController : BaseController
    {

        private TM_CandidatesService _TM_CandidatesService;
        private PersonnelRequestService _PersonnelRequestService;
        private TM_GenderService _TM_GenderService;
        private TM_MaritalStatusService _TM_MaritalStatusService;
        private TM_PrefixService _TM_PrefixService;
        private TM_SourcingChannelService _TM_SourcingChannelService;
        private TM_CountryService _TM_CountryService;
        private TM_NationalitiesService _TM_NationalitiesService;
        private TM_Education_DegreeServices _TM_Education_DegreeServices;
        private TM_SubDistrictService _TM_SubDistrictService;

        public CandidatesController candidatesctl;

        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        private New_HRISEntities dbHr = new New_HRISEntities();
        private TM_WorkExperienceService _TM_WorkExperienceService;

        public ImportCandidateFileController(TM_CandidatesService TM_CandidatesService,
            PersonnelRequestService PersonnelRequestService,
            TM_GenderService TM_GenderService,
            TM_MaritalStatusService TM_MaritalStatusService,
            TM_PrefixService TM_PrefixService,
            TM_SourcingChannelService TM_SourcingChannelService,
            TM_CountryService TM_CountryService,
            TM_NationalitiesService TM_NationalitiesService,
            TM_Education_DegreeServices TM_Education_DegreeServices,
            TM_SubDistrictService TM_SubDistrictService, TM_WorkExperienceService TM_WorkExperienceService)
        {
            _TM_CandidatesService = TM_CandidatesService;
            _PersonnelRequestService = PersonnelRequestService;
            _TM_GenderService = TM_GenderService;
            _TM_MaritalStatusService = TM_MaritalStatusService;
            _TM_PrefixService = TM_PrefixService;
            _TM_SourcingChannelService = TM_SourcingChannelService;
            _TM_CountryService = TM_CountryService;
            _TM_NationalitiesService = TM_NationalitiesService;
            _TM_Education_DegreeServices = TM_Education_DegreeServices;
            _TM_SubDistrictService = TM_SubDistrictService;
            _TM_WorkExperienceService = TM_WorkExperienceService;
        }
        public ActionResult ImportCandidateFile()
        {

            var sCheck = acCheckLoginAndPermission(true);
            if (sCheck != null)
            {
                return sCheck;
            }
            if (!CGlobal.UserIsAdmin())
            {
                return RedirectToAction("PRCandidateList", "PRCandidate");
            }
            vCan_file result = new vCan_file();
            DateTime dNow = DateTime.Now;
            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "ImportCandi" + unixTimestamps;
            result.Session = sSession;
            Session[sSession] = new File_Upload_Can();

            return View(result);


        }



        #region UploadFile


        [HttpPost]
        public ActionResult UploadFileBypass()
        {

            vCanReturn_UploadFile result = new vCanReturn_UploadFile();
            File_Upload_Can objFile = new File_Upload_Can();
            List<vCan_FileTemp> lstFile = new List<vCan_FileTemp>();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }

            string IdEncrypt = Request.Form["IdEncrypt"];

            if (Request != null)
            {
                string sSess = Request.Form["sSess"];
                objFile = Session[sSess] as File_Upload_Can;
                if (objFile == null)
                {
                    objFile = new File_Upload_Can();
                }

                if (Request.Files.Count > 0)
                {

                    foreach (string file in Request.Files)
                    {
                        var fileContent = Request.Files[file];
                        if (fileContent != null && fileContent.ContentLength > 0)
                        {
                            // get a stream
                            var stream = fileContent.InputStream;
                            // and optionally write the file to disk
                            var fileName = Path.GetFileName(file);
                            try
                            {
                                using (var package = new ExcelPackage(stream))
                                {
                                    var ws = package.Workbook.Worksheets.First();
                                    // objFile.sfile64 = package.GetAsByteArray();
                                    objFile.sfile_name = fileName;
                                    objFile.sfileType = Path.GetExtension(fileName).ToLower() + "";

                                    DataTable tbl = new DataTable();
                                    // List<string> lstHead = new List<string>();
                                    //var testget = ws.Cells[1, 1, 1, ws.Dimension.End.Column];
                                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                                    {
                                        tbl.Columns.Add(true ? (firstRowCell.Text + "").Trim().ToLower() : string.Format("Column {0}", firstRowCell.Start.Column));
                                        //lstHead.Add((firstRowCell.Text + "").Trim().ToLower());
                                    }
                                    var startRow = true ? 2 : 1;
                                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                    {
                                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                                        DataRow row = tbl.Rows.Add();
                                        foreach (var cell in wsRow)
                                        {
                                            row[cell.Start.Column - 1] = cell.Text;
                                        }
                                    }
                                    if (tbl != null)
                                    {
                                        if (tbl.Columns.Contains("(EN)First Name") && tbl.Columns.Contains("(EN)Last Name") && tbl.Columns.Contains("Identification Number"))
                                        {


                                            lstFile = (from row in tbl.AsEnumerable().Where(w => (w.Field<string>("(EN)First Name") + "") != ""
                                            && (w.Field<string>("(EN)Last Name") + "") != "")
                                                       group row by new
                                                       {

                                                           EN_Prefix = tbl.Columns.Contains("(EN)Prefix") ? row.Field<string>("(EN)Prefix") : "",
                                                           EN_fistname = tbl.Columns.Contains("(EN)First Name") ? row.Field<string>("(EN)First Name") : "",
                                                           EN_lastname = tbl.Columns.Contains("(EN)Last Name") ? row.Field<string>("(EN)Last Name") : "",
                                                           EN_nickname = tbl.Columns.Contains("(EN)Nickname") ? row.Field<string>("(EN)Nickname") : "",
                                                           IdentificationNumber = tbl.Columns.Contains("Identification Number") ? row.Field<string>("Identification Number") : "",


                                                           Mobile = tbl.Columns.Contains("Mobile") ? row.Field<string>("Mobile") : "",
                                                           Email = tbl.Columns.Contains("Email") ? row.Field<string>("Email") : "",

                                                           TH_Prefix = tbl.Columns.Contains("(TH)Prefix") ? row.Field<string>("(TH)Prefix") : "",
                                                           TH_FirstName = tbl.Columns.Contains("(TH)First Name") ? row.Field<string>("(TH)First Name") : "",
                                                           TH_LastName = tbl.Columns.Contains("(TH)Last Name") ? row.Field<string>("(TH)Last Name") : "",
                                                           TH_Name = tbl.Columns.Contains("(TH) Name") ? row.Field<string>("(TH) Name") : "",
                                                           Gender = tbl.Columns.Contains("Gender") ? row.Field<string>("Gender") : "",
                                                           DateOfBirth = tbl.Columns.Contains("Date of Birth") ? row.Field<string>("Date of Birth") : "",
                                                           //  Age = tbl.Columns.Contains("Age")  ? row.Field<string>("Age") : "",
                                                           Birthplace = tbl.Columns.Contains("Birthplace") ? row.Field<string>("Birthplace") : "",
                                                           CountryOfBirth = tbl.Columns.Contains("Country of Birth") ? row.Field<string>("Country of Birth") : "",
                                                           MaritualStatus = tbl.Columns.Contains("Marital Status") ? row.Field<string>("Marital Status") : "",
                                                           Nationality = tbl.Columns.Contains("Nationality") ? row.Field<string>("Nationality") : "",


                                                           CurrentOrLatestDegree = tbl.Columns.Contains("Current / Latest Degree") ? row.Field<string>("Current / Latest Degree") : "",
                                                           ProfessionalQualification = tbl.Columns.Contains("ProfessionalQualification") ? row.Field<string>("ProfessionalQualification") : "",
                                                           CPAPassedStatus = tbl.Columns.Contains("CPA Passed Status") ? row.Field<string>("CPA Passed Status") : "",
                                                           CPAPassedYear = tbl.Columns.Contains("CPA Passed Year") ? row.Field<string>("CPA Passed Year") : "",
                                                           CPALicenseNo = tbl.Columns.Contains("CPA License No.") ? row.Field<string>("CPA License No.") : "",
                                                           CostCenter = tbl.Columns.Contains("Cost Center") ? row.Field<string>("Cost Center") : "",

                                                           TypeOfCandidate = tbl.Columns.Contains("Type of Candidate (App Online)") ? row.Field<string>("Type of Candidate (App Online)") : "",

                                                           CurrentOrLatestBaseSalaryTHB = tbl.Columns.Contains("Current / Latest Base Salary (THB)") ? row.Field<string>("Current / Latest Base Salary (THB)") : "",
                                                           TransportationAllowanceTHB = tbl.Columns.Contains("Transportation Allowance (THB)") ? row.Field<string>("Transportation Allowance (THB)") : "",
                                                           MobileAllowanceTHB = tbl.Columns.Contains("Mobile Allowance (THB)") ? row.Field<string>("Mobile Allowance (THB)") : "",
                                                           PositionAllowanceTHB = tbl.Columns.Contains("Position Allowance (THB)") ? row.Field<string>("Position Allowance (THB)") : "",
                                                           OtherAllowancesTHB = tbl.Columns.Contains("Other Allowances (THB)") ? row.Field<string>("Other Allowances (THB)") : "",
                                                           AnnualLeaveDays = tbl.Columns.Contains("Annual Leave (Days)") ? row.Field<string>("Annual Leave (Days)") : "",
                                                           VariableBonusMonth = tbl.Columns.Contains("Variable Bonus (Month)") ? row.Field<string>("Variable Bonus (Month)") : "",
                                                           FixedBonusMonth = tbl.Columns.Contains("Fixed Bonus (Month)") ? row.Field<string>("Fixed Bonus (Month)") : "",
                                                           ExpectedSalaryTHB = tbl.Columns.Contains("Expected Salary (THB)") ? row.Field<string>("Expected Salary (THB)") : "",
                                                           YearOfPerformanceReview = tbl.Columns.Contains("Year of Performance Review") ? row.Field<string>("Year of Performance Review") : "",
                                                           TotalYearsOfWorkExpRelatedToThisPosition = tbl.Columns.Contains("Total years of work exp. related to this position") ? row.Field<string>("Total years of work exp. related to this position") : "",
                                                           TotalYearsOfWorkExpNotRelatedToThisPosition = tbl.Columns.Contains("Total years of work exp. not related to this position") ? row.Field<string>("Total years of work exp. not related to this position") : "",
                                                           AllTotalYearsOfWorkExp = tbl.Columns.Contains("(All) Total years of work exp.") ? row.Field<string>("(All) Total years of work exp.") : "",
                                                           CurrentOrLatestIndustry = tbl.Columns.Contains("Current / Latest Industry") ? row.Field<string>("Current / Latest Industry") : "",
                                                           CurrentOrLatestCompanyName = tbl.Columns.Contains("Current / Latest  Company Name") ? row.Field<string>("Current / Latest  Company Name") : "",
                                                           CurrentOrLatestPositionName = tbl.Columns.Contains("Current / Latest Position Name") ? row.Field<string>("Current / Latest Position Name") : "",
                                                           HRWrapUpComment = tbl.Columns.Contains("HR Wrap Up  Comment") ? row.Field<string>("HR Wrap Up  Comment") : "",
                                                           IndustryPreferences1 = tbl.Columns.Contains("Industry Preferences 1") ? row.Field<string>("Industry Preferences 1") : "",
                                                           IndustryPreferences2 = tbl.Columns.Contains("Industry Preferences 2") ? row.Field<string>("Industry Preferences 2") : "",
                                                           IndustryPreferences3 = tbl.Columns.Contains("Industry Preferences 3") ? row.Field<string>("Industry Preferences 3") : "",
                                                           IndustryPreferences4 = tbl.Columns.Contains("Industry Preferences 4") ? row.Field<string>("Industry Preferences 4") : "",
                                                           IndustryPreferences5 = tbl.Columns.Contains("Industry Preferences 5") ? row.Field<string>("Industry Preferences 5") : "",
                                                           CompleteInfoForOnBoard = tbl.Columns.Contains("Complete Info for on-board") ? row.Field<string>("Complete Info for on-board") : "",
                                                           BankAccountName = tbl.Columns.Contains("Bank Account Name") ? row.Field<string>("Bank Account Name") : "",
                                                           BankAccountNumber = tbl.Columns.Contains("Bank Account Number") ? row.Field<string>("Bank Account Number") : "",
                                                           BankAccountBranchName = tbl.Columns.Contains("Bank Account Branch Name") ? row.Field<string>("Bank Account Branch Name") : "",
                                                           BankAccountBranchNumber = tbl.Columns.Contains("Bank Account Branch Number") ? row.Field<string>("Bank Account Branch Number") : "",
                                                           StudentID = tbl.Columns.Contains("Student ID") ? row.Field<string>("Student ID") : "",
                                                           SocialSecurityTH = tbl.Columns.Contains("Social Security - TH") ? row.Field<string>("Social Security - TH") : "",
                                                           ProvidentFundTH = tbl.Columns.Contains("Provident Fund - TH") ? row.Field<string>("Provident Fund - TH") : "",
                                                           DeathContribution = tbl.Columns.Contains("Death Contribution") ? row.Field<string>("Death Contribution") : "",
                                                           MilitaryServicesDoc = tbl.Columns.Contains("Military services doc") ? row.Field<string>("Military services doc") : "",
                                                           IBMP = tbl.Columns.Contains("IBMP") ? row.Field<string>("IBMP") : "",
                                                           OfficialNoteForAnnouncement = tbl.Columns.Contains("Official Note for Announcement") ? row.Field<string>("Official Note for Announcement") : "",
                                                           InternalNoteForHRTeam = tbl.Columns.Contains("Internal Note for HR Team") ? row.Field<string>("Internal Note for HR Team") : "",
                                                           ApplyDate = tbl.Columns.Contains("Apply Date") ? row.Field<string>("Apply Date") : "",
                                                           LastUpdateDate = tbl.Columns.Contains("Last Update Date") ? row.Field<string>("Last Update Date") : "",



                                                       } into grp
                                                       select new vCan_FileTemp
                                                       {
                                                           nCandidate_ID = _TM_CandidatesService.FindForUploadFile(grp.Key.EN_fistname, grp.Key.EN_lastname),
                                                           EN_Prefix = grp.Key.EN_Prefix,
                                                           EN_fistname = grp.Key.EN_fistname,
                                                           EN_lastname = grp.Key.EN_lastname,
                                                           EN_nickname = grp.Key.EN_nickname,
                                                           IdentificationNumber = grp.Key.IdentificationNumber,
                                                           Mobile = grp.Key.Mobile,
                                                           Email = grp.Key.Email,

                                                           TH_Prefix = grp.Key.TH_Prefix,
                                                           TH_FirstName = grp.Key.TH_FirstName,
                                                           TH_LastName = grp.Key.TH_LastName,
                                                           TH_Name = grp.Key.TH_Name,
                                                           Gender = grp.Key.Gender,
                                                           DateOfBirth = grp.Key.DateOfBirth,
                                                           //Age = grp.Key.Age,
                                                           Birthplace = grp.Key.Birthplace,
                                                           CountryOfBirth = grp.Key.CountryOfBirth,
                                                           MaritualStatus = grp.Key.MaritualStatus,
                                                           Nationality = grp.Key.Nationality,
                                                           CurrentOrLatestDegree = grp.Key.CurrentOrLatestDegree,
                                                           ProfessionalQualification = grp.Key.ProfessionalQualification,
                                                           CPAPassedStatus = grp.Key.CPAPassedStatus,
                                                           CPAPassedYear = grp.Key.CPAPassedYear,
                                                           CPALicenseNo = grp.Key.CPALicenseNo,
                                                           CostCenter = grp.Key.CostCenter,
                                                           TypeOfCandidate = grp.Key.TypeOfCandidate,

                                                           CurrentOrLatestBaseSalaryTHB = grp.Key.CurrentOrLatestBaseSalaryTHB,

                                                           TransportationAllowanceTHB = grp.Key.TransportationAllowanceTHB,
                                                           MobileAllowanceTHB = grp.Key.MobileAllowanceTHB,
                                                           PositionAllowanceTHB = grp.Key.PositionAllowanceTHB,
                                                           OtherAllowancesTHB = grp.Key.OtherAllowancesTHB,
                                                           AnnualLeaveDays = grp.Key.AnnualLeaveDays,
                                                           VariableBonusMonth = grp.Key.VariableBonusMonth,
                                                           FixedBonusMonth = grp.Key.FixedBonusMonth,
                                                           ExpectedSalaryTHB = grp.Key.ExpectedSalaryTHB,
                                                           YearOfPerformanceReview = grp.Key.YearOfPerformanceReview,
                                                           TotalYearsOfWorkExpRelatedToThisPosition = grp.Key.TotalYearsOfWorkExpRelatedToThisPosition,
                                                           TotalYearsOfWorkExpNotRelatedToThisPosition = grp.Key.TotalYearsOfWorkExpNotRelatedToThisPosition,

                                                           AllTotalYearsOfWorkExp = grp.Key.AllTotalYearsOfWorkExp,
                                                           CurrentOrLatestIndustry = grp.Key.CurrentOrLatestIndustry,
                                                           CurrentOrLatestCompanyName = grp.Key.CurrentOrLatestCompanyName,
                                                           CurrentOrLatestPositionName = grp.Key.CurrentOrLatestPositionName,

                                                           HRWrapUpComment = grp.Key.HRWrapUpComment,
                                                           IndustryPreferences1 = grp.Key.IndustryPreferences1,
                                                           IndustryPreferences2 = grp.Key.IndustryPreferences2,
                                                           IndustryPreferences3 = grp.Key.IndustryPreferences3,
                                                           IndustryPreferences4 = grp.Key.IndustryPreferences4,
                                                           IndustryPreferences5 = grp.Key.IndustryPreferences5,
                                                           CompleteInfoForOnBoard = grp.Key.CompleteInfoForOnBoard,

                                                           BankAccountName = grp.Key.BankAccountName,
                                                           BankAccountNumber = grp.Key.BankAccountNumber,
                                                           BankAccountBranchName = grp.Key.BankAccountBranchName,
                                                           BankAccountBranchNumber = grp.Key.BankAccountBranchNumber,
                                                           StudentID = grp.Key.StudentID,
                                                           SocialSecurityTH = grp.Key.SocialSecurityTH,
                                                           ProvidentFundTH = grp.Key.ProvidentFundTH,
                                                           DeathContribution = grp.Key.DeathContribution,
                                                           MilitaryServicesDoc = grp.Key.MilitaryServicesDoc,
                                                           IBMP = grp.Key.IBMP,
                                                           OfficialNoteForAnnouncement = grp.Key.OfficialNoteForAnnouncement,
                                                           InternalNoteForHRTeam = grp.Key.InternalNoteForHRTeam,
                                                           ApplyDate = grp.Key.ApplyDate,
                                                           LastUpdateDate = grp.Key.LastUpdateDate,


                                                       }

                                            ).ToList();

                                            if (lstFile.Any())
                                            {
                                                result.lstNewData = (from lFile in lstFile
                                                                     select new vCan_obj
                                                                     {

                                                                         EN_Prefix = lFile.EN_Prefix,
                                                                         EN_fistname = lFile.EN_fistname,
                                                                         EN_lastname = lFile.EN_lastname,
                                                                         EN_nickname = lFile.EN_nickname,
                                                                         IdentificationNumber = lFile.IdentificationNumber,
                                                                         Mobile = lFile.Mobile,
                                                                         Email = lFile.Email,

                                                                         TH_Prefix = lFile.TH_Prefix,
                                                                         TH_FirstName = lFile.TH_FirstName,
                                                                         TH_LastName = lFile.TH_LastName,
                                                                         TH_Name = lFile.TH_Name,
                                                                         Gender = lFile.Gender,
                                                                         DateOfBirth = lFile.DateOfBirth,
                                                                         //Age = lFile.Age,
                                                                         Birthplace = lFile.Birthplace,
                                                                         CountryOfBirth = lFile.CountryOfBirth,
                                                                         MaritualStatus = lFile.MaritualStatus,
                                                                         Nationality = lFile.Nationality,
                                                                         CurrentOrLatestDegree = lFile.CurrentOrLatestDegree,
                                                                         ProfessionalQualification = lFile.ProfessionalQualification,
                                                                         CPAPassedStatus = lFile.CPAPassedStatus,
                                                                         CPAPassedYear = lFile.CPAPassedYear,
                                                                         CPALicenseNo = lFile.CPALicenseNo,
                                                                         CostCenter = lFile.CostCenter,
                                                                         CurrentOrLatestBaseSalaryTHB = lFile.CurrentOrLatestBaseSalaryTHB,
                                                                         TransportationAllowanceTHB = lFile.TransportationAllowanceTHB,
                                                                         MobileAllowanceTHB = lFile.MobileAllowanceTHB,
                                                                         PositionAllowanceTHB = lFile.PositionAllowanceTHB,
                                                                         OtherAllowancesTHB = lFile.OtherAllowancesTHB,
                                                                         AnnualLeaveDays = lFile.AnnualLeaveDays,
                                                                         VariableBonusMonth = lFile.VariableBonusMonth,
                                                                         FixedBonusMonth = lFile.FixedBonusMonth,
                                                                         ExpectedSalaryTHB = lFile.ExpectedSalaryTHB,
                                                                         YearOfPerformanceReview = lFile.YearOfPerformanceReview,
                                                                         TotalYearsOfWorkExpRelatedToThisPosition = lFile.TotalYearsOfWorkExpRelatedToThisPosition,
                                                                         TotalYearsOfWorkExpNotRelatedToThisPosition = lFile.TotalYearsOfWorkExpNotRelatedToThisPosition,


                                                                         AllTotalYearsOfWorkExp = lFile.AllTotalYearsOfWorkExp,
                                                                         CurrentOrLatestIndustry = lFile.CurrentOrLatestIndustry,
                                                                         CurrentOrLatestCompanyName = lFile.CurrentOrLatestCompanyName,
                                                                         CurrentOrLatestPositionName = lFile.CurrentOrLatestPositionName,

                                                                         HRWrapUpComment = lFile.HRWrapUpComment,
                                                                         IndustryPreferences1 = lFile.IndustryPreferences1,
                                                                         IndustryPreferences2 = lFile.IndustryPreferences2,
                                                                         IndustryPreferences3 = lFile.IndustryPreferences3,
                                                                         IndustryPreferences4 = lFile.IndustryPreferences4,
                                                                         IndustryPreferences5 = lFile.IndustryPreferences5,
                                                                         CompleteInfoForOnBoard = lFile.CompleteInfoForOnBoard,


                                                                         BankAccountName = lFile.BankAccountName,
                                                                         BankAccountNumber = lFile.BankAccountNumber,
                                                                         BankAccountBranchName = lFile.BankAccountBranchName,
                                                                         BankAccountBranchNumber = lFile.BankAccountBranchNumber,
                                                                         StudentID = lFile.StudentID,
                                                                         SocialSecurityTH = lFile.SocialSecurityTH,
                                                                         ProvidentFundTH = lFile.ProvidentFundTH,
                                                                         DeathContribution = lFile.DeathContribution,
                                                                         MilitaryServicesDoc = lFile.MilitaryServicesDoc,
                                                                         IBMP = lFile.IBMP,
                                                                         OfficialNoteForAnnouncement = lFile.OfficialNoteForAnnouncement,
                                                                         InternalNoteForHRTeam = lFile.InternalNoteForHRTeam,
                                                                         ApplyDate = lFile.ApplyDate,
                                                                         LastUpdateDate = lFile.LastUpdateDate,



                                                                     }).ToList();


                                                #region test 

                                                int row_N = 0;
                                                //List<object> lstFileWork_Exp = new List<object>();
                                                var lstFileWExp = (from row in tbl.AsEnumerable().Where(w => !String.IsNullOrEmpty((w.Field<string>("(EN)First Name") + ""))
                                                                   && !String.IsNullOrEmpty((w.Field<string>("(EN)Last Name") + ""))
                                                                   && !String.IsNullOrEmpty((w.Field<string>("Identification Number") + "")))
                                                                   group row by new
                                                                   {

                                                                       Candidates_code = tbl.Columns.Contains("(EN)First Name") && tbl.Columns.Contains("(EN)Last Name") ? row.Field<string>("(EN)First Name") + " " + row.Field<string>("(EN)Last Name") : "",

                                                                       CompanyName1 = tbl.Columns.Contains("CompanyName1") ? row.Field<string>("CompanyName1") : "",
                                                                       JobPosition1 = tbl.Columns.Contains("JobPosition1") ? row.Field<string>("JobPosition1") : "",
                                                                       StartDate1 = tbl.Columns.Contains("StartDate1") ? row.Field<string>("StartDate1") : "",
                                                                       EndDate1 = tbl.Columns.Contains("EndDate1") ? row.Field<string>("EndDate1") : "",
                                                                       TypeOfRelatedToJob1 = tbl.Columns.Contains("TypeOfRelatedToJob1") ? row.Field<string>("TypeOfRelatedToJob1") : "",
                                                                       base_salary1 = tbl.Columns.Contains("base_salary1") ? row.Field<string>("base_salary1") : "",
                                                                       transportation1 = tbl.Columns.Contains("transportation1") ? row.Field<string>("transportation1") : "",
                                                                       mobile_allowance1 = tbl.Columns.Contains("mobile_allowance1") ? row.Field<string>("mobile_allowance1") : "",
                                                                       position_allowance1 = tbl.Columns.Contains("position_allowance1") ? row.Field<string>("position_allowance1") : "",
                                                                       other_allowance1 = tbl.Columns.Contains("other_allowance1") ? row.Field<string>("other_allowance1") : "",
                                                                       annual_leave1 = tbl.Columns.Contains("annual_leave1") ? row.Field<string>("annual_leave1") : "",
                                                                       variable_bonus1 = tbl.Columns.Contains("variable_bonus1") ? row.Field<string>("variable_bonus1") : "",
                                                                       expected_salary1 = tbl.Columns.Contains("expected_salary1") ? row.Field<string>("expected_salary1") : "",


                                                                       CompanyName2 = tbl.Columns.Contains("CompanyName2") ? row.Field<string>("CompanyName2") : "",
                                                                       JobPosition2 = tbl.Columns.Contains("JobPosition2") ? row.Field<string>("JobPosition2") : "",
                                                                       StartDate2 = tbl.Columns.Contains("StartDate2") ? row.Field<string>("StartDate2") : "",
                                                                       EndDate2 = tbl.Columns.Contains("EndDate2") ? row.Field<string>("EndDate2") : "",
                                                                       TypeOfRelatedToJob2 = tbl.Columns.Contains("TypeOfRelatedToJob2") ? row.Field<string>("TypeOfRelatedToJob2") : "",
                                                                       base_salary2 = tbl.Columns.Contains("base_salary2") ? row.Field<string>("base_salary2") : "",
                                                                       transportation2 = tbl.Columns.Contains("transportation2") ? row.Field<string>("transportation2") : "",
                                                                       mobile_allowance2 = tbl.Columns.Contains("mobile_allowance2") ? row.Field<string>("mobile_allowance2") : "",
                                                                       position_allowance2 = tbl.Columns.Contains("position_allowance2") ? row.Field<string>("position_allowance2") : "",
                                                                       other_allowance2 = tbl.Columns.Contains("other_allowance2") ? row.Field<string>("other_allowance2") : "",
                                                                       annual_leave2 = tbl.Columns.Contains("annual_leave2") ? row.Field<string>("annual_leave2") : "",
                                                                       variable_bonus2 = tbl.Columns.Contains("variable_bonus2") ? row.Field<string>("variable_bonus2") : "",
                                                                       expected_salary2 = tbl.Columns.Contains("expected_salary2") ? row.Field<string>("expected_salary2") : "",

                                                                       CompanyName3 = tbl.Columns.Contains("CompanyName3") ? row.Field<string>("CompanyName3") : "",
                                                                       JobPosition3 = tbl.Columns.Contains("JobPosition3") ? row.Field<string>("JobPosition3") : "",
                                                                       StartDate3 = tbl.Columns.Contains("StartDate3") ? row.Field<string>("StartDate3") : "",
                                                                       EndDate3 = tbl.Columns.Contains("EndDate3") ? row.Field<string>("EndDate3") : "",
                                                                       TypeOfRelatedToJob3 = tbl.Columns.Contains("TypeOfRelatedToJob3") ? row.Field<string>("TypeOfRelatedToJob3") : "",
                                                                       base_salary3 = tbl.Columns.Contains("base_salary3") ? row.Field<string>("base_salary3") : "",
                                                                       transportation3 = tbl.Columns.Contains("transportation3") ? row.Field<string>("transportation3") : "",
                                                                       mobile_allowance3 = tbl.Columns.Contains("mobile_allowance3") ? row.Field<string>("mobile_allowance3") : "",
                                                                       position_allowance3 = tbl.Columns.Contains("position_allowance3") ? row.Field<string>("position_allowance3") : "",
                                                                       other_allowance3 = tbl.Columns.Contains("other_allowance3") ? row.Field<string>("other_allowance3") : "",
                                                                       annual_leave3 = tbl.Columns.Contains("annual_leave3") ? row.Field<string>("annual_leave3") : "",
                                                                       variable_bonus3 = tbl.Columns.Contains("variable_bonus3") ? row.Field<string>("variable_bonus3") : "",
                                                                       expected_salary3 = tbl.Columns.Contains("expected_salary3") ? row.Field<string>("expected_salary3") : "",

                                                                       CompanyName4 = tbl.Columns.Contains("CompanyName4") ? row.Field<string>("CompanyName4") : "",
                                                                       JobPosition4 = tbl.Columns.Contains("JobPosition4") ? row.Field<string>("JobPosition4") : "",
                                                                       StartDate4 = tbl.Columns.Contains("StartDate4") ? row.Field<string>("StartDate4") : "",
                                                                       EndDate4 = tbl.Columns.Contains("EndDate4") ? row.Field<string>("EndDate4") : "",
                                                                       TypeOfRelatedToJob4 = tbl.Columns.Contains("TypeOfRelatedToJob4") ? row.Field<string>("TypeOfRelatedToJob4") : "",
                                                                       base_salary4 = tbl.Columns.Contains("base_salary4") ? row.Field<string>("base_salary4") : "",
                                                                       transportation4 = tbl.Columns.Contains("transportation4") ? row.Field<string>("transportation4") : "",
                                                                       mobile_allowance4 = tbl.Columns.Contains("mobile_allowance4") ? row.Field<string>("mobile_allowance4") : "",
                                                                       position_allowance4 = tbl.Columns.Contains("position_allowance4") ? row.Field<string>("position_allowance4") : "",
                                                                       other_allowance4 = tbl.Columns.Contains("other_allowance4") ? row.Field<string>("other_allowance4") : "",
                                                                       annual_leave4 = tbl.Columns.Contains("annual_leave4") ? row.Field<string>("annual_leave4") : "",
                                                                       variable_bonus4 = tbl.Columns.Contains("variable_bonus4") ? row.Field<string>("variable_bonus4") : "",
                                                                       expected_salary4 = tbl.Columns.Contains("expected_salary4") ? row.Field<string>("expected_salary4") : "",

                                                                       CompanyName5 = tbl.Columns.Contains("CompanyName5") ? row.Field<string>("CompanyName5") : "",
                                                                       JobPosition5 = tbl.Columns.Contains("JobPosition5") ? row.Field<string>("JobPosition5") : "",
                                                                       StartDate5 = tbl.Columns.Contains("StartDate5") ? row.Field<string>("StartDate5") : "",
                                                                       EndDate5 = tbl.Columns.Contains("EndDate5") ? row.Field<string>("EndDate5") : "",
                                                                       TypeOfRelatedToJob5 = tbl.Columns.Contains("TypeOfRelatedToJob5") ? row.Field<string>("TypeOfRelatedToJob5") : "",
                                                                       base_salary5 = tbl.Columns.Contains("base_salary5") ? row.Field<string>("base_salary5") : "",
                                                                       transportation5 = tbl.Columns.Contains("transportation5") ? row.Field<string>("transportation5") : "",
                                                                       mobile_allowance5 = tbl.Columns.Contains("mobile_allowance5") ? row.Field<string>("mobile_allowance5") : "",
                                                                       position_allowance5 = tbl.Columns.Contains("position_allowance5") ? row.Field<string>("position_allowance5") : "",
                                                                       other_allowance5 = tbl.Columns.Contains("other_allowance5") ? row.Field<string>("other_allowance5") : "",
                                                                       annual_leave5 = tbl.Columns.Contains("annual_leave5") ? row.Field<string>("annual_leave5") : "",
                                                                       variable_bonus5 = tbl.Columns.Contains("variable_bonus5") ? row.Field<string>("variable_bonus5") : "",
                                                                       expected_salary5 = tbl.Columns.Contains("expected_salary5") ? row.Field<string>("expected_salary5") : "",

                                                                   }
                                                  ).ToList();

                                                List<vCandidate_WorkExp_onchange> lstFileWork_Exp = new List<vCandidate_WorkExp_onchange>();
                                                foreach (var v in lstFileWExp)
                                                {
                                                    vCandidate_WorkExp_onchange lst_data = new vCandidate_WorkExp_onchange();
                                                    lst_data.candidates_code = v.Key.Candidates_code;
                                                    lst_data.CompanyName = v.Key.CompanyName1;
                                                    lst_data.JobPosition = v.Key.JobPosition1;
                                                    lst_data.StartDate = v.Key.StartDate1;
                                                    lst_data.EndDate = v.Key.EndDate1;
                                                    lst_data.TypeOfRelatedToJob = v.Key.TypeOfRelatedToJob1;
                                                    lst_data.base_salary = v.Key.base_salary1;
                                                    lst_data.transportation = v.Key.transportation1;
                                                    lst_data.mobile_allowance = v.Key.mobile_allowance1;
                                                    lst_data.position_allowance = v.Key.position_allowance1;
                                                    lst_data.other_allowance = v.Key.other_allowance1;
                                                    lst_data.annual_leave = v.Key.annual_leave1;
                                                    lst_data.variable_bonus = v.Key.variable_bonus1;
                                                    lst_data.expected_salary = v.Key.expected_salary1;

                                                    if (!string.IsNullOrEmpty(lst_data.CompanyName))
                                                        lstFileWork_Exp.Add(lst_data);

                                                    lst_data = new vCandidate_WorkExp_onchange();
                                                    lst_data.candidates_code = v.Key.Candidates_code;
                                                    lst_data.CompanyName = v.Key.CompanyName2;
                                                    lst_data.JobPosition = v.Key.JobPosition2;
                                                    lst_data.StartDate = v.Key.StartDate2;
                                                    lst_data.EndDate = v.Key.EndDate2;
                                                    lst_data.TypeOfRelatedToJob = v.Key.TypeOfRelatedToJob2;
                                                    lst_data.base_salary = v.Key.base_salary2;
                                                    lst_data.transportation = v.Key.transportation2;
                                                    lst_data.mobile_allowance = v.Key.mobile_allowance2;
                                                    lst_data.position_allowance = v.Key.position_allowance2;
                                                    lst_data.other_allowance = v.Key.other_allowance2;
                                                    lst_data.annual_leave = v.Key.annual_leave2;
                                                    lst_data.variable_bonus = v.Key.variable_bonus2;
                                                    lst_data.expected_salary = v.Key.expected_salary2;

                                                    if (!string.IsNullOrEmpty(lst_data.CompanyName))
                                                        lstFileWork_Exp.Add(lst_data);

                                                    lst_data = new vCandidate_WorkExp_onchange();
                                                    lst_data.candidates_code = v.Key.Candidates_code;
                                                    lst_data.CompanyName = v.Key.CompanyName3;
                                                    lst_data.JobPosition = v.Key.JobPosition3;
                                                    lst_data.StartDate = v.Key.StartDate3;
                                                    lst_data.EndDate = v.Key.EndDate3;
                                                    lst_data.TypeOfRelatedToJob = v.Key.TypeOfRelatedToJob3;
                                                    lst_data.base_salary = v.Key.base_salary3;
                                                    lst_data.transportation = v.Key.transportation3;
                                                    lst_data.mobile_allowance = v.Key.mobile_allowance3;
                                                    lst_data.position_allowance = v.Key.position_allowance3;
                                                    lst_data.other_allowance = v.Key.other_allowance3;
                                                    lst_data.annual_leave = v.Key.annual_leave3;
                                                    lst_data.variable_bonus = v.Key.variable_bonus3;
                                                    lst_data.expected_salary = v.Key.expected_salary3;

                                                    if (!string.IsNullOrEmpty(lst_data.CompanyName))
                                                        lstFileWork_Exp.Add(lst_data);

                                                    lst_data = new vCandidate_WorkExp_onchange();
                                                    lst_data.candidates_code = v.Key.Candidates_code;
                                                    lst_data.CompanyName = v.Key.CompanyName4;
                                                    lst_data.JobPosition = v.Key.JobPosition4;
                                                    lst_data.StartDate = v.Key.StartDate4;
                                                    lst_data.EndDate = v.Key.EndDate4;
                                                    lst_data.TypeOfRelatedToJob = v.Key.TypeOfRelatedToJob4;
                                                    lst_data.base_salary = v.Key.base_salary4;
                                                    lst_data.transportation = v.Key.transportation4;
                                                    lst_data.mobile_allowance = v.Key.mobile_allowance4;
                                                    lst_data.position_allowance = v.Key.position_allowance4;
                                                    lst_data.other_allowance = v.Key.other_allowance4;
                                                    lst_data.annual_leave = v.Key.annual_leave4;
                                                    lst_data.variable_bonus = v.Key.variable_bonus4;
                                                    lst_data.expected_salary = v.Key.expected_salary4;

                                                    if (!string.IsNullOrEmpty(lst_data.CompanyName))
                                                        lstFileWork_Exp.Add(lst_data);

                                                    lst_data = new vCandidate_WorkExp_onchange();
                                                    lst_data.candidates_code = v.Key.Candidates_code;
                                                    lst_data.CompanyName = v.Key.CompanyName5;
                                                    lst_data.JobPosition = v.Key.JobPosition5;
                                                    lst_data.StartDate = v.Key.StartDate5;
                                                    lst_data.EndDate = v.Key.EndDate5;
                                                    lst_data.TypeOfRelatedToJob = v.Key.TypeOfRelatedToJob5;
                                                    lst_data.base_salary = v.Key.base_salary5;
                                                    lst_data.transportation = v.Key.transportation5;
                                                    lst_data.mobile_allowance = v.Key.mobile_allowance5;
                                                    lst_data.position_allowance = v.Key.position_allowance5;
                                                    lst_data.other_allowance = v.Key.other_allowance5;
                                                    lst_data.annual_leave = v.Key.annual_leave5;
                                                    lst_data.variable_bonus = v.Key.variable_bonus5;
                                                    lst_data.expected_salary = v.Key.expected_salary5;

                                                    if (!string.IsNullOrEmpty(lst_data.CompanyName))
                                                        lstFileWork_Exp.Add(lst_data);

                                                }
                                                #endregion test 


                                                objFile.lstTempCandidate = lstFile;
                                                objFile.lstWorkExpCandidate = lstFileWork_Exp;

                                            }
                                            result.Status = SystemFunction.process_Success;

                                        }
                                        else
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Incorrect template, please try again.";
                                        }
                                        //end of test import file

                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                result.Msg = e.Message + "";
                                result.Status = SystemFunction.process_Failed;
                            }
                        }
                    }

                    Session[sSess] = objFile;

                }
                else
                {
                    Session[sSess] = objFile;
                    result.Status = "Clear";
                }
            }
            return Json(new { result });
        }

        [HttpPost]
        public ActionResult UpdateExcelCandidate(vCan_file ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            List<vCandidate_WorkExp_onchange> list_for_savepass = new List<vCandidate_WorkExp_onchange>();

            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                string sSess = ItemData.Session;
                DateTime dNow = DateTime.Now;
                File_Upload_Can objFile = new File_Upload_Can();
                objFile = Session[sSess] as File_Upload_Can;
                if (objFile != null)
                {
                    if (objFile.lstTempCandidate != null && objFile.lstTempCandidate.Any())
                    {
                        foreach (var item in objFile.lstTempCandidate.Where(w => w.nCandidate_ID != 0))
                        {
                            var _getCandidate = _TM_CandidatesService.FindAddUploadFile(item.nCandidate_ID);
                            int nIDGender = (int)SystemFunction.GetNumberNullToZero(item.Gender);
                            int nMaritalID = (int)SystemFunction.GetNumberNullToZero(item.MaritualStatus);
                            int nENprefix = (int)SystemFunction.GetNumberNullToZero(item.EN_Prefix);
                            int nTHprefix = (int)SystemFunction.GetNumberNullToZero(item.TH_Prefix);
                            int nCountryOfBirth = (int)SystemFunction.GetNumberNullToZero(item.CountryOfBirth);
                            int nNationalities = (int)SystemFunction.GetNumberNullToZero(item.Nationality);


                            var _getMaritalID = _TM_MaritalStatusService.Find(nMaritalID);
                            var _getGender = _TM_GenderService.Find(nIDGender);
                            var _getENprefix = _TM_PrefixService.Find(nENprefix);
                            var _getTHprefix = _TM_PrefixService.Find(nTHprefix);
                            var _getCountryOfBirth = _TM_CountryService.Find(nCountryOfBirth);
                            var _getNationalities = _TM_NationalitiesService.Find(nNationalities);

                            var lst_save_new = new TM_Candidates();

                            //ถ้ามี cand อยู่แล้่วให้ทำการ อัพเดตใช้ cand ล่าสุด 
                            if (_getCandidate != null)
                            {

                                _getCandidate.prefixEN = _getENprefix != null ? _getENprefix : null;
                                _getCandidate.first_name_en = item.EN_fistname;
                                _getCandidate.last_name_en = item.EN_lastname;
                                _getCandidate.candidate_NickName = item.EN_nickname;
                                _getCandidate.id_card = item.IdentificationNumber;
                                _getCandidate.candidate_phone = item.Mobile;
                                _getCandidate.candidate_Email = item.Email;
                                _getCandidate.prefixTH = _getTHprefix;
                                

                                _getCandidate.candidate_FNameTH = item.TH_FirstName;
                                _getCandidate.candiate_LNameTH = item.TH_LastName;
                                _getCandidate.candidate_AlternativeNameTH = item.TH_Name;
                                _getCandidate.Gender = _getGender != null ? _getGender : null;
                                _getCandidate.candidate_DateOfBirth = SystemFunction.ConvertStringToDateTime(item.DateOfBirth, "", "yyyy-MM-dd");
                                _getCandidate.candidate_BirthPlace = item.Birthplace;
                                _getCandidate.CountryOfBirth = _getCountryOfBirth != null ? _getCountryOfBirth : null;
                                _getCandidate.MaritalStatusName = _getMaritalID != null ? _getMaritalID : null;
                                _getCandidate.Nationalities = _getNationalities != null ? _getNationalities : null;
                                _getCandidate.candidate_ProfessionalQualification = item.ProfessionalQualification;
                                _getCandidate.CPAPassedStatus = item.CPAPassedStatus;
                                _getCandidate.CPAPassedYear = item.CPAPassedYear;
                                _getCandidate.CPALicenseNo = item.CPALicenseNo;
                                _getCandidate.candidate_CostCenter = item.CostCenter;
                                _getCandidate.candidate_BaseSalary = SystemFunction.GetNumberNullToZero(item.CurrentOrLatestBaseSalaryTHB + ""); ;
                                _getCandidate.candidate_TransportationAllowance = SystemFunction.GetNumberNullToZero(item.TransportationAllowanceTHB + "");
                                _getCandidate.candidate_MobileAllowance = SystemFunction.GetNumberNullToZero(item.MobileAllowanceTHB + "");
                                _getCandidate.candidate_PositionAllowanceTHB = SystemFunction.GetNumberNullToZero(item.MobileAllowanceTHB + "");
                                _getCandidate.candidate_OtherAllowance = SystemFunction.GetNumberNullToZero(item.OtherAllowancesTHB + "");
                                _getCandidate.candidate_AnnualLeave = SystemFunction.GetNumberNullToZero(item.AnnualLeaveDays + "");
                                _getCandidate.candidate_VariableBonus = SystemFunction.GetNumberNullToZero(item.VariableBonusMonth + "");
                                _getCandidate.candidate_FixedBonus = SystemFunction.GetNumberNullToZero(item.FixedBonusMonth + "");
                                _getCandidate.candidate_ExpectedSalary = SystemFunction.GetNumberNullToZero(item.ExpectedSalaryTHB + "");
                                _getCandidate.YearOfPerformanceReview = item.YearOfPerformanceReview;
                                _getCandidate.candidate_TotalYearsOfWorkRelatedToThisPosition = SystemFunction.GetNumberNullToZero(item.TotalYearsOfWorkExpRelatedToThisPosition + "");
                                _getCandidate.candidate_TotalYearsOfWorkNotRelatedToThisPosition = SystemFunction.GetNumberNullToZero(item.TotalYearsOfWorkExpNotRelatedToThisPosition + "");
                                _getCandidate.AllTotalYearsOfWorkExp = SystemFunction.GetNumberNullToZero(item.AllTotalYearsOfWorkExp + "");

                                _getCandidate.CurrentOrLatestIndustry = item.CurrentOrLatestIndustry;
                                _getCandidate.CurrentOrLatestCompanyName = item.CurrentOrLatestCompanyName;
                                _getCandidate.CurrentOrLatestPositionName = item.CurrentOrLatestPositionName;

                                _getCandidate.HRWrapUpComment = item.HRWrapUpComment;
                                _getCandidate.candidate_IndustryPrerences1 = item.IndustryPreferences1;
                                _getCandidate.candidate_IndustryPrerences2 = item.IndustryPreferences2;
                                _getCandidate.candidate_IndustryPrerences3 = item.IndustryPreferences3;
                                _getCandidate.candidate_IndustryPrerences4 = item.IndustryPreferences4;
                                _getCandidate.candidate_IndustryPrerences5 = item.IndustryPreferences5;
                                _getCandidate.candidate_CompleteInfoForOnBoard = item.CompleteInfoForOnBoard;
                                _getCandidate.candidate_BankAccountName = item.BankAccountName;
                                _getCandidate.candidate_BankAccountNumber = item.BankAccountNumber;
                                _getCandidate.candidate_BankAccountBranchName = item.BankAccountBranchName;
                                _getCandidate.candidate_BankAccountBranchNumber = item.BankAccountBranchNumber;
                                _getCandidate.candidate_StudentID = item.StudentID;
                                _getCandidate.candidate_SocialSecurityTH = item.SocialSecurityTH;
                                _getCandidate.candidate_ProvidentFundTH = item.ProvidentFundTH;
                                _getCandidate.candidate_DeathContribution = item.DeathContribution;
                                _getCandidate.candidate_MilitaryServicesDoc = item.MilitaryServicesDoc;
                                _getCandidate.candidate_IBMP = item.IBMP;
                                _getCandidate.candidate_OfficialNote = item.OfficialNoteForAnnouncement;
                                _getCandidate.candidate_InternalNoteForHRTeam = item.InternalNoteForHRTeam;
                                _getCandidate.candidate_ApplyDate = SystemFunction.ConvertStringToDateTime(item.DateOfBirth, "");
                                _getCandidate.candidate_LastUpdateDate = SystemFunction.ConvertStringToDateTime(item.LastUpdateDate, "");
                                _getCandidate.update_date = dNow;
                                _getCandidate.update_user = CGlobal.UserInfo.UserId;


                                var sComplete = _TM_CandidatesService.Update(_getCandidate);
                                if (sComplete > 0)
                                {
                                    try
                                    {

                                        int run_seq = 0;
                                        List<vCandidate_WorkExp_onchange> list_for_save = objFile.lstWorkExpCandidate.Where(w => w.candidates_code == item.EN_fistname + " " + item.EN_lastname).ToList();
                                        List<vCandidate_WorkExp_onchange> setorder = list_for_save.OrderBy(o => o.StartDate).ToList();
                                        string endDatelast = setorder[0].EndDate;
                                        //endDatelast = Convert.ToDateTime(test);
                                        //int rundate = 0;

                                        for (int i = 0; i < setorder.Count(); i++)
                                        {
                                            bool check = false;
                                            if (i == 0 || i== setorder.Count())
                                            {
                                                check =true;
                                            }
                                            else if (Convert.ToDateTime(setorder[i].StartDate) >= Convert.ToDateTime(endDatelast))
                                            {
                                                check = true;
                                            }

                                            if (check)
                                            {
                                                if (Convert.ToDateTime(setorder[i].StartDate) <= Convert.ToDateTime(setorder[i].EndDate))
                                                {
                                                    list_for_savepass.Add(setorder[i]);
                                                }
                                            }
                                            endDatelast = setorder[i].EndDate;
                                        }

                                        //foreach (var so in setorder)
                                        //{

                                        //    if (rundate == 0)
                                        //    {
                                        //        endDatelast = so.EndDate;
                                        //    }

                                        //    if (Convert.ToDateTime(so.StartDate) >= Convert.ToDateTime(endDatelast))
                                        //    {

                                        //        if (Convert.ToDateTime(so.StartDate) <= Convert.ToDateTime(so.EndDate))
                                        //        {
                                        //            list_for_savepass.Add(so);
                                        //        }
                                        //        else
                                        //        {
                                        //            //ไม่ตรงเงื่อนไขวัน in row
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        // ไม่ตรงเงื่อนไขวัน ระหว่าง row to row
                                        //    }
                                        //    endDatelast = so.EndDate;
                                        //    rundate++;
                                        //}

                                        var _getData = _TM_CandidatesService.FindName(item.EN_fistname, item.EN_lastname);
                                        var testrun = _TM_WorkExperienceService.GetDataForSelect_AllStatus().ToList();
                                        var lstWorkExpinlist = testrun.Where(d => d.TM_Candidates.Id == _getData.Id);

                                        foreach (var c in lstWorkExpinlist)
                                        {
                                            c.update_user = CGlobal.UserInfo.UserId;
                                            c.update_date = DateTime.Now;
                                            c.active_status = "N";
                                            try
                                            {
                                                _TM_WorkExperienceService.InActive(c);
                                            }
                                            catch (Exception ex) { }
                                        }



                                        foreach (var w in list_for_savepass)
                                        {
                                            //if (Convert.ToDateTime(w.StartDate) <= Convert.ToDateTime(w.EndDate))
                                            //{
                                            //    list_for_savepass.Add(w);
                                            //}

                                            //var sublist = lstWorkExpinlist.Where(ws => ws.CompanyName == w.CompanyName && ws.JobPosition == w.JobPosition).FirstOrDefault();
                                            var id_sub_lst = 0;
                                            //if (sublist != null)
                                            //{
                                            //id_sub_lst = sublist.Id;
                                            //}
                                            //else
                                            //{
                                            run_seq = _TM_WorkExperienceService.GetDataForSelect_AllStatus().ToList().Where(d => d.TM_Candidates.Id == _getData.Id).Count();
                                            //}


                                            TM_WorkExperience objSave = new TM_WorkExperience()
                                            {
                                                Id = SystemFunction.GetIntNullToZero(id_sub_lst + ""),
                                                CompanyName = w.CompanyName,
                                                JobPosition = w.JobPosition,
                                                seq = run_seq + 1,
                                                TM_Candidates = _getData,
                                                active_status = "Y",
                                                StartDate = !string.IsNullOrEmpty(w.StartDate) ? Convert.ToDateTime(w.StartDate) : SystemFunction.ConvertStringToDateTime(w.StartDate, "", "yyyy-MMM-dd"),
                                                EndDate = !string.IsNullOrEmpty(w.EndDate) ? Convert.ToDateTime(w.EndDate) : SystemFunction.ConvertStringToDateTime(w.EndDate, "", "yyyy-MMM-dd"),
                                                TypeOfRelatedToJob = w.TypeOfRelatedToJob,
                                                base_salary = SystemFunction.GetNumberNullToZero(w.base_salary),
                                                transportation = SystemFunction.GetNumberNullToZero(w.transportation),
                                                mobile_allowance = SystemFunction.GetNumberNullToZero(w.mobile_allowance),
                                                position_allowance = SystemFunction.GetNumberNullToZero(w.position_allowance),
                                                other_allowance = SystemFunction.GetNumberNullToZero(w.other_allowance),
                                                annual_leave = SystemFunction.GetNumberNullToZero(w.annual_leave),
                                                variable_bonus = SystemFunction.GetNumberNullToZero(w.variable_bonus),
                                                expected_salary = SystemFunction.GetNumberNullToZero(w.expected_salary),
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = DateTime.Now
                                            };

                                            var updateorinsertworkexp = _TM_WorkExperienceService.CreateNewAndEdit(objSave);
                                            if (updateorinsertworkexp > 0)
                                            {
                                                result.Status = SystemFunction.process_Success;
                                            }
                                            else
                                            {
                                                result.Status = SystemFunction.process_Failed;
                                            }
                                        }
                                        result.Status = SystemFunction.process_Success;

                                    }
                                    catch (Exception ex)
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = ex.Message;
                                    }
                                    result.Status = SystemFunction.process_Success;
                                    result.Msg += "<br><span class='text-success'>Save successfully : " + item.EN_fistname + " " + item.EN_lastname + "</span>";
                                }
                            }

                        }

                        foreach (var outItem in objFile.lstTempCandidate.Where(w => w.nCandidate_ID == 0))
                        {
                            if (result.Status == "Success")
                                result.Msg += "<br><span class='text-danger'>Unmatched data : " + outItem.EN_fistname + " " + outItem.EN_lastname + "</span>";
                        }

                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, File Not Found.";
                    }


                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, File Not Found.";
                }

            }

            return Json(new
            {
                result
            });
        }

        #endregion

    }

}