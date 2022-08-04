using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.MainVM
{

    public class vCandidates : CSearchCandidates
    {
        public List<vCandidates_obj> lstData { get; set; }
    }

    public class VCandidates_Return : CResutlWebMethod
    {
        public List<vCandidates_obj> lstData { get; set; }
        public List<vAcKnowledge_lst> lstError { get; set; }
    }


    public class VCandidatesEduHistory_Return : CResutlWebMethod
    {
        public List<vEduHistory_obj> lstEduHistory { get; set; }
    }
    public class VCandidatesWorkExpHistory_Return : CResutlWebMethod
    {
        public List<vWorkExpHistory_obj> lstWorkExpHistory { get; set; }
    }

    public class VCandidatesTechnicalTestTransaction_Return : CResutlWebMethod
    {
        public List<vcandidatesTechnicalTestTransaction_obj_Save> lstTechnicalTestTransaction { get; set; }
    }


    public class vPAaddress_obj
    {

    }

    public class vCAaddress_obj
    {

    }


    public class vcandidatesTechnicalTestTransaction_obj
    {
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string IdEncrypt { get; set; }
        public string Id { get; set; }
        public string technicaltest_id { get; set; }
        public string technicaltest_score { get; set; }
        public string technicaltest_date { get; set; }
        public string Test_name_en { get; set; }
        public string Test_name_th { get; set; }
        public string Test_description { get; set; }
        public string Test_Status { get; set; }
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        public string update_user { get; set; }

    }


    public class vcandidatesTechnicalTestTransaction_obj_Save
    {
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string IdEncrypt { get; set; }
        public string Id { get; set; }
        public string technicaltest_id { get; set; }
        public string technicaltest_score { get; set; }
        public string technicaltest_date { get; set; }
        public string Test_name_en { get; set; }
        public string Test_name_th { get; set; }
        public string Test_description { get; set; }
        public string Test_Status { get; set; }
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        public string update_user { get; set; }

    }

    public class vEduHistory_obj
    {
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string IdEncrypt { get; set; }
        public string Id { get; set; }
        public string university_name { get; set; }
        public string faculty_name { get; set; }
        public string major_name { get; set; }
        public string Ref_Cert_ID { get; set; }
        public string degree { get; set; }
        public decimal? grade { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string active_status { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string education_history_description { get; set; }

    }

    public class vWorkExpHistory_obj
    {
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string IdEncrypt { get; set; }

        public string Id { get; set; }
        public string seq { get; set; }
        public string CompanyName { get; set; }
        public string JobPosition { get; set; }
        public string StartDate { get; set; }

        public string EndDate { get; set; }
        public string TypeOfRelatedToJob { get; set; }
        public string active_status { get; set; }
        public string base_salary { get; set; }
        public string transportation { get; set; }
        public string mobile_allowance { get; set; }
        public string position_allowance { get; set; }
        public string other_allowance { get; set; }
        public string annual_leave { get; set; }
        public string variable_bonus { get; set; }
        public string expected_salary { get; set; }

    }

    public class vCandidates_ExportList
    {
        public string candidate_Prefix { get; set; }
        public string candidate_name { get; set; }
        public string candidate_NickName { get; set; }
        public string candidate_lastname { get; set; }
    }

    public class vCandidates_obj
    {
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string division { get; set; }
        public string sgroup { get; set; }
        public string request_type { get; set; }
        public string position { get; set; }
        public string rank { get; set; }
        public string hc { get; set; }
        public string pr_status { get; set; }
        public string bu_interview { get; set; }
        public string select { get; set; }
        public string hiring { get; set; }
        public string strat_date { get; set; }
        public string request_by { get; set; }
        public string request_date { get; set; }

        public string name_en { get; set; }
        public string user_name { get; set; }
        public string id_card { get; set; }
        public string spassword { get; set; }
        public string ref_no { get; set; }
    }
   
    public class vCandidates_obj_Add
    {
        public string IdEncrypt { get; set; }
        public vCandidates_obj_Save objData { get; set; }
    }
    public class vCandidates_obj_Save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string request_status { get; set; }
        public string candidate_Prefix { get; set; }
        public string candidate_name { get; set; }
        public string candidate_NickName { get; set; }
        public string candidate_lastname { get; set; }

        public string candidate_FNameTH { get; set; }

        public string candidate_LNameTH { get; set; }
        public string activities_Id { get; set; }
        public string candidate_prefix_TH { get; set; }

        public string prefixTH_Id { get; set; }

        public string prefixEN_Id { get; set; }


        public string CPAPassedStatus { get; set; }

        public string CPAPassedYear { get; set; }

        public string CPALicenseNo { get; set; }

        public string candidate_phone { get; set; }
        public string candidate_id_type { get; set; }
        public string candidate_id { get; set; }
        public string recruitment_id { get; set; }
        public string candidate_OxfordScore { get; set; }
        public string candidate_BaseSalary { get; set; }
        public string candidate_TotalYearsOfWorkRelatedToThisPosition { get; set; }
        public string candidate_TotalYearsOfWorkNotRelatedToThisPosition { get; set; }
        public string candidate_Test_Id { get; set; }

        public string candidate_NMGTestScore { get; set; }

        public string candidate_NMGTestDate { get; set; }

        public string CurrentOrLatestIndustry { get; set; }

        public string CurrentOrLatestCompanyName { get; set; }

        public string CurrentOrLatestPositionName { get; set; }

        public string MyProperty { get; set; }

        public string candidate_CurrentIndustry { get; set; }

        public string candidate_CurrentPositionName { get; set; }

        public string candidate_EnglishTestName { get; set; }

        public string candidate_EnglishTestScoreOrOxford { get; set; }

        public string candidate_EnglishTestStatus { get; set; }

        public string candidate_EnglishTestDate { get; set; }

        public string candidate_MobileAllowance { get; set; }

        public string candidate_TransportationAllowance { get; set; }

        public string candidate_OtherAllowance { get; set; }

        public string candidate_AnnualLeave { get; set; }

        public string candidate_VariableBonus { get; set; }

        public string candidate_FixedBonus { get; set; }

        public string candidate_ExpectedSalary { get; set; }

        public string candidate_user_id { get; set; }

        public string candidate_SourcingChannel_id { get; set; }

        public string candidate_Company { get; set; }

        public string candidate_OrgUnit { get; set; }

        public string candidate_CostCenter { get; set; }

        public string candidate_Position { get; set; }

        public string candidate_TypeOfEmployment { get; set; }

        public string candidate_AlternativeNameTH { get; set; }

        public string candidate_Gender { get; set; }

        public string candidate_BirthPlace { get; set; }

        public string candidate_CountryOfBirth { get; set; }

        public string candidate_MaritalStatus { get; set; }

        public string candidate_Nationality { get; set; }

        public string candidate_PAHouseNo { get; set; }

        public string candidate_PAMooAndSoi { get; set; }

        public string candidate_PAStreet { get; set; }

        public string candidate_PAPostalCode { get; set; }


        public string candidate_PATelephoneNumber { get; set; }

        public string candidate_PAMobileNumber { get; set; }

        public string candidate_CAHouseNo { get; set; }

        public string candidate_CAMooAndSoi { get; set; }

        public string candidate_CAStreetAndTambol { get; set; }

        public string candidate_CAStreet { get; set; }

        public string candidate_CAPostalCode { get; set; }

        public string candidate_CATelephoneNumber { get; set; }

        public string candidate_CAMobileNumber { get; set; }

        public string candidate_CompleteInfoForOnBoard { get; set; }

        public string candidate_BankAccountName { get; set; }

        public string candidate_BankAccountNumber { get; set; }

        public string candidate_SocialSecurityTH { get; set; }// "Y/N"

        public string candidate_ProvidentFundTH { get; set; }// "Y/N"

        public string candidate_DeathContribution { get; set; }//  "Y/N"

        public string candidate_EduInstituteOrLocationOfTraining { get; set; }

        public string candidate_EduCountry { get; set; }

        public string candidate_EduCurrentGPATranscript { get; set; }

        public string candidate_EduCurrentOrLatestDegree { get; set; }

        public string candidate_EduMajorStudy { get; set; }

        public string candidate_TraineeNumber { get; set; }

        public string candidate_Email { get; set; }

        public string candidate_Program { get; set; }

        public string candidate_IndustryPrerences1 { get; set; }

        public string candidate_IndustryPrerences2 { get; set; }

        public string candidate_IndustryPrerences3 { get; set; }

        public string candidate_IndustryPrerences4 { get; set; }

        public string candidate_IndustryPrerences5 { get; set; }

        public string candidate_OfficialNote { get; set; }

        public string candidate_InternalNoteForHRTeam { get; set; }

        public string candidate_AuditingTestDate { get; set; }

        public string candidate_DateOfBirth { get; set; }

        public string candidate_AuditingScore { get; set; }

        public string candidate_EducationStartDate { get; set; }

        public string candidate_EducationEndDate { get; set; }

        public string candidate_MilitaryServicesDoc { get; set; }

        public string candidate_IBMP { get; set; }




        //For PA Eng
        public string candidate_PAHouseNo_EN { get; set; }
        public string candidate_PAVillageNoAndAlley_EN { get; set; }

        public string candidate_PAStreet_EN { get; set; }

        public string candidate_PAPostalCode_EN { get; set; }

        public string candidate_PATelephoneNumber_EN { get; set; }

        public string candidate_PAMobileNumber_EN { get; set; }

        public string PA_EN_city_id { get; set; }
        public string PA_EN_district_id { get; set; }
        public string PA_EN_subdistrict_id { get; set; }
        public string PA_EN_country_id { get; set; }

        public List<vSelect_PR> lstPA_EN_City { get; set; }
        public List<vSelect_PR> lstPA_EN_District { get; set; }

        public List<vSelect_PR> lstPA_EN_SubDistrict { get; set; }

        //End of PA Eng


        //For CA Eng
        public string candidate_CAHouseNo_EN { get; set; }

        public string candidate_CAVillageNoAndAlley_EN { get; set; }

        public string candidate_CAStreet_EN { get; set; }

        public string candidate_CAPostalCode_EN { get; set; }

        public string candidate_CATelephoneNumber_EN { get; set; }

        public string candidate_CAMobileNumber_EN { get; set; }

        public List<vSelect_PR> lstCA_EN_City { get; set; }
        public List<vSelect_PR> lstCA_EN_District { get; set; }

        public List<vSelect_PR> lstCA_EN_SubDistrict { get; set; }


        public string CA_EN_city_id { get; set; }
        public string CA_EN_district_id { get; set; }
        public string CA_EN_subdistrict_id { get; set; }
        public string CA_EN_country_id { get; set; }
        //End of CA Eng


        public List<vSelect_PR> lstCity { get; set; }
        public List<vSelect_PR> lstDistrict { get; set; }

        public List<vSelect_PR> lstSubDistrict { get; set; }

        public List<vSelect_PR> lstPACity { get; set; }
        public List<vSelect_PR> lstPADistrict { get; set; }

        public List<vSelect_PR> lstPASubDistrict { get; set; }

        public List<vSelect_PR> lstCACity { get; set; }
        public List<vSelect_PR> lstCADistrict { get; set; }

        public List<vSelect_PR> lstCASubDistrict { get; set; }

        public string city_id { get; set; }
        public string district_id { get; set; }
        public string subdistrict_id { get; set; }
        public string country_id { get; set; }

        public string PAcity_id { get; set; }
        public string PAdistrict_id { get; set; }
        public string PAsubdistrict_id { get; set; }
        public string PAcountry_id { get; set; }

        public string CAcity_id { get; set; }
        public string CAdistrict_id { get; set; }
        public string CAsubdistrict_id { get; set; }
        public string CAcountry_id { get; set; }
        public string university_id { get; set; }
        public string faculty_id { get; set; }
        public string major_id { get; set; }
        public string candidate_ProfessionalQualification { get; set; }

        public string name_th { get; set; }

        public string candidate_BankAccountBranchName { get; set; }

        public string candidate_BankAccountBranchNumber { get; set; }

        public string candidate_StudentID { get; set; }

        public string candidate_ApplyDate { get; set; }


        public List<vSelect_PR> lstUniversity { get; set; }
        public List<vSelect_PR> lstFaculty { get; set; }
        public List<vSelect_PR> lstMajor { get; set; }
        public List<vEduHistory_obj> lstEduHistory { get; set; }
        public List<vcandidatesTechnicalTestTransaction_obj_Save> lstTechnicalTestTransaction { get; set; }
        public List<vWorkExpHistory_obj> lstWorkExpHistory { get; set; }

        public string Update_UserName { get; set; }

    }
    public class objCandidates_Return : CResutlWebMethod
    {
        public List<vPRCandidates_lstData> lstCandidates { get; set; }
    }
    public class vCandidates_Approve_Permit
    {
        public string Edit { get; set; }
        public string emp_code { get; set; }
        public string emp_id { get; set; }
        public string emp_name { get; set; }
        public string emp_group { get; set; }
        public string emp_position { get; set; }

        public string emp_dec { get; set; }
    }
    public class vCandidates_lst_approve
    {
        public int nStep { get; set; }
        public string IdEncrypt { get; set; }
        public string step_name { get; set; }
        public string emp_no { get; set; }
        public string app_code { get; set; }
        public string app_name { get; set; }
        public string description { get; set; }
        public string approve_date { get; set; }
    }
    public class vEducation_Save
    {
        public string IdEncrypt { get; set; }
        public vCandidate_university_onchange ObjEdu { get; set; }
    }
    public class vWorkExp_Save
    {
        public string IdEncrypt { get; set; }
        public vCandidate_WorkExp_onchange ObjWorkExp { get; set; }
    }
    public class vTechnicalTestTransaction_Save
    {
        public string IdEncrypt { get; set; }
        public vCandidate_TechnicalTestTransaction_onchange ObjTechnicalTest { get; set; }

    }

    public class vCandidatePASubDistrict
    {
        public string PAsubdistrict_id { get; set; }

        public string PAsubdistrict_nameTH { get; set; }
    }

    public class vCandidate_university_onchange
    {

        public string IdEncrypt { get; set; }
        public string Id { get; set; }
        public string university_id { get; set; }
        public string faculty_id { get; set; }
        public string major_id { get; set; }
        public string Ref_Cert_ID { get; set; }
        public string degree { get; set; }
        public string grade { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string education_history_description { get; set; }
        public List<vSelect_PR> lstFaculty { get; set; }
        public List<vSelect_PR> lstMajor { get; set; }
    }

    public class vCandidate_TechnicalTestTransaction_onchange
    {
        public string IdEncrypt { get; set; }

        public List<vSelect_PR> lstTechnicalTest { get; set; }
        public string active_status { get; set; }

        public DateTime? create_date { get; set; }

        public string create_user { get; set; }

        public DateTime? update_date { get; set; }

        public string update_user { get; set; }

        public string technicaltest_id { get; set; }

        public string technicaltest_score { get; set; }

        public string technicaltest_date { get; set; }

        public string Test_name_en { get; set; }

        public string Test_name_th { get; set; }

        public string Test_description { get; set; }
        /*
      

        public string Test_Status { get; set; }

        public string active_status { get; set; }

        public DateTime? create_date { get; set; }

        public string create_user { get; set; }

        public DateTime? update_date { get; set; }

        public string update_user { get; set; }
         
         */

    }

    public class vCandidate_WorkExp_onchange
    {

        public string IdEncrypt { get; set; }
        public string Id { get; set; }
        public int seq { get; set; }
        public string CompanyName { get; set; }
        public string JobPosition { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TypeOfRelatedToJob { get; set; }
        public string active_status { get; set; }
        public string base_salary { get; set; }
        public string transportation { get; set; }
        public string mobile_allowance { get; set; }
        public string position_allowance { get; set; }
        public string other_allowance { get; set; }
        public string annual_leave { get; set; }
        public string variable_bonus { get; set; }
        public string expected_salary { get; set; }
        public string candidates_code { get; set; }

    }

    public class vCandidate_country_onchage
    {

        public List<vSelect_PR> lstCity { get; set; }
        public List<vSelect_PR> lstDistrict { get; set; }
        public List<vSelect_PR> lstSubDistrict { get; set; }
        public string city_id { get; set; }
        public string district_id { get; set; }
        public string subdistrict_id { get; set; }
        public string country_id { get; set; }

        public List<vSelect_PR> lstPACity { get; set; }
        public List<vSelect_PR> lstPADistrict { get; set; }
        public List<vSelect_PR> lstPASubDistrict { get; set; }
        public string PAcity_id { get; set; }
        public string PAdistrict_id { get; set; }
        public string PAsubdistrict_id { get; set; }
        public string PAcountry_id { get; set; }

        public List<vSelect_PR> lstPA_EN_City { get; set; }
        public List<vSelect_PR> lstPA_EN_District { get; set; }
        public List<vSelect_PR> lstPA_EN_SubDistrict { get; set; }
        public string PA_EN_city_id { get; set; }
        public string PA_EN_district_id { get; set; }
        public string PA_EN_subdistrict_id { get; set; }
        public string PA_EN_country_id { get; set; }


        public List<vSelect_PR> lstCACity { get; set; }
        public List<vSelect_PR> lstCADistrict { get; set; }
        public List<vSelect_PR> lstCASubDistrict { get; set; }
        public string CAcity_id { get; set; }
        public string CAdistrict_id { get; set; }
        public string CAsubdistrict_id { get; set; }
        public string CAcountry_id { get; set; }

        public List<vSelect_PR> lstCA_EN_City { get; set; }
        public List<vSelect_PR> lstCA_EN_District { get; set; }
        public List<vSelect_PR> lstCA_EN_SubDistrict { get; set; }
        public string CA_EN_city_id { get; set; }
        public string CA_EN_district_id { get; set; }
        public string CA_EN_subdistrict_id { get; set; }
        public string CA_EN_country_id { get; set; }
    }

    #region for update status candidate

    public class vCandidates_obj_update
    {
        public string IdEncrypt { get; set; }
        public string status_id { get; set; }
        public string group_id { get; set; }
        public string sub_group_id { get; set; }
        public string rank_id { get; set; }
        public string position_id { get; set; }
        public string candidate_name { get; set; }
        public string candidate_rank_id { get; set; }
        public string action_date { get; set; }
        public string action_remark { get; set; }
        public string candidate_remark { get; set; }
        public string recruitment_id { get; set; }
        public string active_status { get; set; }
        public string current_status_id { get; set; }
        public string status_addnew { get; set; }
        public string activities_Id { get; set; }
        public List<vSelect_PR> lstStatus { get; set; }
        public List<vCandidates_status_his> lstHisStatus { get; set; }
    }
    public class vCandidates_status_his
    {
        public string status_name { get; set; }
        public string status_date { get; set; }
    }
    public class vCandidates_status_save
    {
        public string IdEncrypt { get; set; }
        public string status_id { get; set; }
        public string description { get; set; }
    }

    public class VCandidates_EduHistory
    {
        public string IdEncrypt { get; set; }
    }

    public class VCandidate_TechnicalTestTransaction
    {
        public string IdEncrypt { get; set; }
    }

    public class VCandidates_WorkExperienceHistory
    {
        public string IdEncrypt { get; set; }
    }

    #endregion


    #region Upload File
    public class vCanReturn_UploadFile : CResutlWebMethod
    {
        public List<vCan_obj> lstNewData { get; set; }
        public string IdEncrypt { get; set; }
    }
    public class vCan_obj
    {
        public string Edit { get; set; }
        public string sID { get; set; }
        public string sName { get; set; }
        public string sLName { get; set; }
        public string sDuplicate { get; set; }
        public string sIsOld { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string institutename { get; set; }
        public string PresentAddressEng { get; set; }

        //Import for master
        //(EN) Nickname
        public string EN_Prefix { get; set; }
        public string EN_firstName { get; set; }
        public string EN_LastName { get; set; }
        public string EN_NickName { get; set; }
        public string IdentificationNumber { get; set; }
        public string Mobile { get; set; }
        public string TH_Prefix { get; set; }
        public string TH_FirstName { get; set; }
        public string TH_LastName { get; set; }
        public string TH_Name { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Age { get; set; }
        public string Birthplace { get; set; }
        public string CountryOfBirth { get; set; }
        public string MaritualStatus { get; set; }
        public string Nationality { get; set; }
        public string CurrentOrLatestDegree { get; set; }
        public string ProfessionalQualification { get; set; }
        public string CPAPassedStatus { get; set; }
        public string CPAPassedYear { get; set; }
        public string CPALicenseNo { get; set; }


        /////////////////////////////////////////////////////////////////////////

        public string IdEncrypt { get; set; }
        public int nCandidate_ID { get; set; }
        public string EN_fistname { get; set; }
        public string EN_lastname { get; set; }
        public string EN_nickname { get; set; }
        public string Email { get; set; }
        public string SourcingChannel { get; set; }
        public string CostCenter { get; set; }
        public string CurrentOrLatestBaseSalaryTHB { get; set; }
        public string TransportationAllowanceTHB { get; set; }
        public string MobileAllowanceTHB { get; set; }
        public string PositionAllowanceTHB { get; set; }
        public string OtherAllowancesTHB { get; set; }
        public string AnnualLeaveDays { get; set; }
        public string VariableBonusMonth { get; set; }
        public string FixedBonusMonth { get; set; }
        public string ExpectedSalaryTHB { get; set; }
        public string YearOfPerformanceReview { get; set; }
        public string TotalYearsOfWorkExpRelatedToThisPosition { get; set; }
        public string TotalYearsOfWorkExpNotRelatedToThisPosition { get; set; }
        public string AllTotalYearsOfWorkExp { get; set; }
        public string CurrentOrLatestIndustry { get; set; }
        public string CurrentOrLatestCompanyName { get; set; }
        public string CurrentOrLatestPositionName { get; set; }
        public string P_ENSubDistrict { get; set; }
        public string P_THSubDistrict { get; set; }
        public string C_ENSubDistrict { get; set; }
        public string C_THSubDistrict { get; set; }
        public string HRWrapUpComment { get; set; }
        public string IndustryPreferences1 { get; set; }
        public string IndustryPreferences2 { get; set; }
        public string IndustryPreferences3 { get; set; }
        public string IndustryPreferences4 { get; set; }
        public string IndustryPreferences5 { get; set; }
        public string CompleteInfoForOnBoard { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountBranchName { get; set; }
        public string BankAccountBranchNumber { get; set; }
        public string StudentID { get; set; }
        public string SocialSecurityTH { get; set; }
        public string ProvidentFundTH { get; set; }
        public string DeathContribution { get; set; }
        public string MilitaryServicesDoc { get; set; }
        public string IBMP { get; set; }
        public string OfficialNoteForAnnouncement { get; set; }
        public string InternalNoteForHRTeam { get; set; }
        public string ApplyDate { get; set; }
        public string LastUpdateDate { get; set; }
        public string Activity { get; set; }


    }
    public class vCan_FileTemp
    {
        public string IdEncrypt { get; set; }
        public int nCandidate_ID { get; set; }
        public string EN_Prefix { get; set; }
        public string EN_fistname { get; set; }
        public string EN_lastname { get; set; }
        public string EN_nickname { get; set; }
        public string IdentificationNumber { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string SourcingChannel { get; set; }
        public string TH_Prefix { get; set; }
        public string TH_FirstName { get; set; }
        public string TH_LastName { get; set; }
        public string TH_Name { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Age { get; set; }
        public string Birthplace { get; set; }
        public string CountryOfBirth { get; set; }
        public string MaritualStatus { get; set; }
        public string Nationality { get; set; }
        public string CurrentOrLatestDegree { get; set; }
        public string ProfessionalQualification { get; set; }
        public string CPAPassedStatus { get; set; }
        public string CPAPassedYear { get; set; }
        public string CPALicenseNo { get; set; }
        public string CostCenter { get; set; }
        public string TypeOfCandidate { get; set; }
        public string CurrentOrLatestBaseSalaryTHB { get; set; }
        public string TransportationAllowanceTHB { get; set; }
        public string MobileAllowanceTHB { get; set; }
        public string PositionAllowanceTHB { get; set; }
        public string OtherAllowancesTHB { get; set; }
        public string AnnualLeaveDays { get; set; }
        public string VariableBonusMonth { get; set; }
        public string FixedBonusMonth { get; set; }
        public string ExpectedSalaryTHB { get; set; }
        public string YearOfPerformanceReview { get; set; }
        public string TotalYearsOfWorkExpRelatedToThisPosition { get; set; }
        public string TotalYearsOfWorkExpNotRelatedToThisPosition { get; set; }
        public string AllTotalYearsOfWorkExp { get; set; }
        public string CurrentOrLatestIndustry { get; set; }
        public string CurrentOrLatestCompanyName { get; set; }
        public string CurrentOrLatestPositionName { get; set; }
        public string P_ENSubDistrict { get; set; }
        public string P_THSubDistrict { get; set; }
        public string C_ENSubDistrict { get; set; }
        public string C_THSubDistrict { get; set; }
        public string HRWrapUpComment { get; set; }
        public string IndustryPreferences1 { get; set; }
        public string IndustryPreferences2 { get; set; }
        public string IndustryPreferences3 { get; set; }
        public string IndustryPreferences4 { get; set; }
        public string IndustryPreferences5 { get; set; }
        public string CompleteInfoForOnBoard { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountBranchName { get; set; }
        public string BankAccountBranchNumber { get; set; }
        public string StudentID { get; set; }
        public string SocialSecurityTH { get; set; }
        public string ProvidentFundTH { get; set; }
        public string DeathContribution { get; set; }
        public string MilitaryServicesDoc { get; set; }
        public string IBMP { get; set; }
        public string OfficialNoteForAnnouncement { get; set; }
        public string InternalNoteForHRTeam { get; set; }
        public string ApplyDate { get; set; }
        public string LastUpdateDate { get; set; }

        public string Activity { get; set; }


        /*
        public string sName { get; set; }
        public string sLName { get; set; }
        public int nCandidate_ID { get; set; }
        public string id_card { get; set; }
        public string institutename { get; set; }
        public string PresentAddressEng { get; set; }

        //Import for master
        public string EN_Prefix { get; set; }
        public string EN_firstName { get; set; }
        public string EN_LastName { get; set; }
        public string EN_NickName { get; set; }
        public string IdentificationNumber { get; set; }
        public string Mobile { get; set; }
        public string email { get; set; }
        public string TH_Prefix { get; set; }
        public string TH_FirstName { get; set; }
        public string TH_LastName { get; set; }
        public string TH_Name { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Age { get; set; }
        public string Birthplace { get; set; }
        public string CountryOfBirth { get; set; }
        public string MaritualStatus { get; set; }
        public string Nationality { get; set; }
        public string CurrentOrLatestDegree { get; set; }
        public string ProfessionalQualification { get; set; }
        public string CPAPassedStatus { get; set; }
        public string CPAPassedYear { get; set; }
        public string CPALicenseNo { get; set; }

        public string EnglishTestName { get; set; }
        public string EnglishTestScores { get; set; }
        public string EnglishTestDate { get; set; }
        public string TechnicalTestName { get; set; }
        public string TechnicalTestScore { get; set; }
        public string TechnicalTestDate { get; set; }
        public string ReTestingTestName { get; set; }
        public string ReTestingScores { get; set; }
        public string ReTestingDate { get; set; }

        public string CostCenter { get; set; }
        public string TypeOfCandidate { get; set; }
        public string RankGradeForHiring { get; set; }
        public string CurrentOrLatestBaseSalaryTHB { get; set; }
        public string TransportationAllowanceTHB { get; set; }
        public string MobileAllowanceTHB { get; set; }
        public string PositionAllowanceTHB { get; set; }
        public string OtherAllowancesTHB { get; set; }
        public string AnnualLeaveDays { get; set; }
        public string VariableBonusMonth { get; set; }
        public string FixedBonusMonth { get; set; }
        public string ExpectedSalaryTHB { get; set; }
        public string YearOfPerformanceReview { get; set; }

        public string TotalYearsOfWorkExpRelatedToThisPosition { get; set; }
        public string TotalYearsOfWorkExpNotRelatedToThisPosition { get; set; }
        public string AllTotalYearsOfWorkExp { get; set; }
        public string CurrentOrLatestIndustry { get; set; }
        public string CurrentLatestCompanyName { get; set; }
        public string CurrentLatestPositionName { get; set; }


        public string HRWrapUpComment { get; set; }
        public string IndustryPreferences1 { get; set; }
        public string IndustryPreferences2 { get; set; }
        public string IndustryPreferences3 { get; set; }
        public string IndustryPreferences4 { get; set; }
        public string IndustryPreferences5 { get; set; }

        public string CompleteInfoForOnboard { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountBranchName { get; set; }
        public string BankAccountBranchNumber { get; set; }
        public string StudentID { get; set; }
        public string SocialSecurityTH { get; set; }

        public string ProvidentFundTH { get; set; }

        public string DeathContribution { get; set; }

        public string MilitaryServicesDoc { get; set; }

        public string IBMP { get; set; }

        public string OfficialNoteForAnnoucement { get; set; }

        public string InternalNoteForHRTeam { get; set; }

        public string ApplyDate { get; set; }

        public string LastUpdateDate { get; set; }
        */

    }

    public class File_Upload_Can
    {
        public byte[] sfile64 { get; set; }
        public string sfileType { get; set; }
        public string sfile_name { get; set; }
        public List<vCan_FileTemp> lstTempCandidate { get; set; }
        public List<vCandidate_WorkExp_onchange> lstWorkExpCandidate { get; set; }
    }

    public class vCan_file
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string empNo { get; set; }
        public string Session { get; set; }
    }
    #endregion


}