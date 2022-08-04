using HumanCapitalManagement.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace HumanCapitalManagement.ViewModel
{

    public class vExportCandidate
    {

    }

    public class vExportCandidate_Result : CResutlWebMethod
    {
        public string session { get; set; }
        public List<vExportCandidateData> lstData { get; set; }      
    }

    public class vExportCandidateData
    {

        /* P'Som's Candidate Report */
        public string No_ { get; set; }
        public string Start_Date { get; set; }
        public string Company { get; set; }
        public string OrgUnit { get; set; }
        public string Cost_Center { get; set; }
        public string Position { get; set; }
        public string Rank { get; set; }
        public string Employee_Group { get; set; }
        public string Prefix { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Nickname { get; set; }
        public string Alternative_Name_TH	 { get; set; }
        public string Gender { get; set; }
        public string Date_of_Birth { get; set; }
        public string Birthplace { get; set; }
        public string Country_of_Birth { get; set; }
        public string Marital_Status { get; set; }
        public string Nationality { get; set; }
        public string Identity_Number { get; set; }
        public string PA_Address { get; set; }
        public string PA_House_No { get; set; }
        public string PA_Moo_And_Soi { get; set; }
        public string PA_Street_And_Tambol { get; set; }
        public string PA_District { get; set; }
        public string PA_City { get; set; }
        public string PA_Postal_Code { get; set; }
        public string PA_Country { get; set; }
        public string PA_Telephone_Number { get; set; }
        public string PA_Mobile_Number { get; set; }
        public string CA_Address { get; set; }
        public string CA_House_No { get; set; }
        public string CA_Moo_Soi { get; set; }
        public string CA_Street_And_Tambol { get; set; }
        public string CA_District { get; set; }
        public string CA_City { get; set; }
        public string CA_Postal_Code { get; set; }
        public string CA_Country { get; set; }
        public string CA_Telephone_Number { get; set; }
        public string CA_Mobile_Number { get; set; }
        public string Bank_Name { get; set; }
        public string Account_Number { get; set; }
        public string Social_Security_TH { get; set; }
        public string Provident_Fund_TH { get; set; }
        public string Death_Contribution { get; set; }
        public string Education_Start_Date { get; set; }
        public string Education_End_Date { get; set; }
        public string Institute_location_of_training { get; set; }
        public string Country { get; set; }
        public string Final_GPA { get; set; }
        public string Certificate { get; set; }
        public string Major_study { get; set; }
        public string Status { get; set; }
        public string EngTestName { get; set; }
        public string Oxford_Score { get; set; }
        public string Oxford_Test_Date { get; set; }
        public string StrengthName1 { get; set; }
        public string StrengthName2 { get; set; }
        public string StrengthName3 { get; set; }
        public string StrengthName4 { get; set; }
        public string StrengthName5 { get; set; }
        public string NMG { get; set; }
        public string VMG { get; set; }
        public string NMG_VMG_Test_Date { get; set; }
        public string Sourcing { get; set; }
        public string Trainee_code { get; set; }

        public string test11 { get; set; }





        /*
        public string Identity_Number { get; set; }
        public string PA_PermanentAddress { get; set; }
        public string PA_HouseNo { get; set; }
        public string PA_MooAndSoi { get; set; }
        public string PA_StreetAndTambol { get; set; }
        public string PA_District { get; set; }
        public string PA_City { get; set; }
        public string PA_PostalCode { get; set; }
        public string PA_Country { get; set; }
        public string PA_TelephoneNumber { get; set; }
        public string PA_MobileNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string SocialSecurityTH { get; set; }
        public string ProvidentFundTH { get; set; }
        public string DeathContribution { get; set; }
        public string EducationStartDate { get; set; }
        public string EducationEndDate { get; set; }
        public string InstituteLocation { get; set; }
        public string InstituteCountry { get; set; }
        public string FinalGPA { get; set; }
        public string Certificate { get; set; }
        public string MajorStudy { get; set; }
        public string StatusExpCandidate { get; set; }
        public string EngTestName { get; set; }
        public string OxfordScore { get; set; }
        public string TestDate { get; set; }
        public string StrengthName1 { get; set; }
        public string StrengthName2 { get; set; }
        public string StrengthName3 { get; set; }
        public string StrengthName4 { get; set; }
        public string StrengthName5 { get; set; }
        public string NMG { get; set; }
        public string VMG { get; set; }
        public string Sourcing { get; set; }
        public string TraineeCode { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public string JoinDate { get; set; }
        public string EndDate { get; set; }
        public string Total { get; set; }
        */

    }

}