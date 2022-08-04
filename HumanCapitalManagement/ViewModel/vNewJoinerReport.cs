using HumanCapitalManagement.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{
    public class vNewJoinerReport
    {

    }

    public class vNewJoinerReport_Result : CResutlWebMethod
    {
        public string session { get; set; }

        public List<vNewJoinerReportData> lstData { get; set; }
        public List<vNewJoinerReportData_Total> lstDataTotal { get; set; }
    }

    public class vNewJoinerReportData
    {

        public string No { get; set; }
        public string StartDate { get; set; }
        public string Company { get; set; }
        public string Division { get; set; }
        public string Group { get; set; }
        public string PositionTitle { get; set; }
        public string RankForHiring { get; set; }
        public string TypeOfEmployee { get; set; }
        public string EN_Prefix { get; set; }
        public string EN_FirstName { get; set; }
        public string EN_LastName { get; set; }
        public string EN_NickName { get; set; }
        public string TH_Name { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Birthplace { get; set; }
        public string CountryOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string IdentificationNumber { get; set; }
        public string PHouseNo { get; set; }
        public string PMooAndSoi { get; set; }
        public string PRoad { get; set; }
        public string PSubDistrict { get; set; }
        public string PProvince { get; set; }
        public string PPostalCode { get; set; }
        public string PCountry { get; set; }
        public string PTelephoneNumber { get; set; }
        public string PMobile { get; set; }
        public string CHouseNo { get; set; }
        public string CMooAndSoi { get; set; }
        public string CRoad { get; set; }
        public string CSubDistrict { get; set; }
        public string CDistrict { get; set; }
        public string CProvince { get; set; }
        public string CPostalCode { get; set; }
        public string CCountry { get; set; }
        public string CTelephoneNumber { get; set; }
        public string CMobile { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountBranchName { get; set; }
        public string StudentID { get; set; }
        public string SocialSecurity_TH { get; set; }
        public string ProvidentFund_TH { get; set; }
        public string DeathContribution { get; set; }
        public string Country { get; set; }
        public string BCurrentGPATranscript { get; set; }
        public string Certificate { get; set; }
        public string BMajorStudy { get; set; }
        public string RecruitmentStatus { get; set; }
        public string EnglishTestName { get; set; }
        public string EnglishTestScores { get; set; }
        public string EnglishTestDate { get; set; }
        public string SourcingChannel { get; set; }
        public string TraineeNumber { get; set; }
        public string MilitaryServicesDoc { get; set; }
        public string IBMP { get; set; }
        public string TechnicalTest1Score { get; set; }
        public string TechnicalTest1Date { get; set; }
        public string Email { get; set; }
        public string BProgram { get; set; }
        public string OfficialNoteForAnnouncement { get; set; }
        public string InternalNoteForHRTeam { get; set; }

    }

    public class vNewJoinerReportData_Total
    {
        
    }


}