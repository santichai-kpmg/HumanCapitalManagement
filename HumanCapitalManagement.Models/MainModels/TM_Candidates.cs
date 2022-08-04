using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.EducationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public class TM_Candidates
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(20)]
        public string id_card { get; set; }
        [StringLength(250)]
        public string first_name_en { get; set; }
        [StringLength(250)]
        public string last_name_en { get; set; }

        public string candidate_NickName { get; set; }

        //public string candidate_prefix_TH { get; set; }

        public string candidate_FNameTH { get; set; }

        public string candiate_LNameTH { get; set; }

        [StringLength(50)]
        public string candidate_phone { get; set; }

        public string candidate_Email { get; set; }

        public string candidate_ProfessionalQualification { get; set; }



        public int? prefixEN_Id { get; set; }

        [ForeignKey("prefixEN_Id")]
        public virtual TM_Prefix prefixEN { get; set; }



        public int? prefixTH_Id { get; set; }

        [ForeignKey("prefixTH_Id")]
        public virtual TM_Prefix prefixTH { get; set; }

        [StringLength(250)]
        public string first_name_th { get; set; }
        [StringLength(250)]
        public string last_name_th { get; set; }

        [StringLength(400)]
        public string name_th { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        [StringLength(10)]
        public string save_success { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual ICollection<TM_PR_Candidate_Mapping> TM_PR_Candidate_Mapping { get; set; }
        public virtual ICollection<TM_Candidate_EmpNO> TM_Candidate_EmpNO { get; set; }
        public virtual TM_Candidate_Type TM_Candidate_Type { get; set; }
        public int? TM_SourcingChannel_Id { get; set; }

        [ForeignKey("TM_SourcingChannel_Id")]
        public virtual TM_SourcingChannel TM_SourcingChannel { get; set; }
        public virtual ICollection<TM_Education_History> TM_Education_History { get; set; }
        public virtual ICollection<TM_WorkExperience> TM_WorkExperience { get; set; }
        public virtual TM_SubDistrict TM_SubDistrict { get; set; }
        public int? PA_TM_SubDistrict_Id { get; set; }

        [ForeignKey("PA_TM_SubDistrict_Id")]
        public virtual TM_SubDistrict PA_TM_SubDistrict { get; set; }
        public int? CA_TM_SubDistrict_Id { get; set; }
        [ForeignKey("CA_TM_SubDistrict_Id")]
        public virtual TM_SubDistrict CA_TM_SubDistrict { get; set; }
        public int? MaritalStatusName_Id { get; set; }

        [ForeignKey("MaritalStatusName_Id")]
        public virtual TM_MaritalStatus MaritalStatusName { get; set; }
        public int? Gender_Id { get; set; }

        [ForeignKey("Gender_Id")]
        public virtual TM_Gender Gender { get; set; }

        public virtual ICollection<TM_TechnicalTestTransaction> TM_TechnicalTestTransaction { get; set; }


        public Decimal? candidate_oxfordscore { get; set; }

        public Decimal? candidate_TotalYearsOfWorkRelatedToThisPosition { get; set; }

        public Decimal? candidate_TotalYearsOfWorkNotRelatedToThisPosition { get; set; }

        public Decimal? AllTotalYearsOfWorkExp { get; set; }

        public string CurrentOrLatestIndustry { get; set; }

        public string CurrentOrLatestCompanyName { get; set; }

        public string CurrentOrLatestPositionName { get; set; }

        //[DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal? candidate_BaseSalary { get; set; }

        public Decimal? candidate_NMGTestScore { get; set; }

        //[DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? candidate_NMGTestDate { get; set; }

        public string candidate_CurrentIndustry { get; set; }

        public string candidate_CurrentPositionName { get; set; }

        public Decimal? candidate_MobileAllowance { get; set; }

        public Decimal? candidate_TransportationAllowance { get; set; }
        public Decimal? candidate_PositionAllowanceTHB { get; set; }

        public Decimal? candidate_OtherAllowance { get; set; }

        public Decimal? candidate_AnnualLeave { get; set; }

        public Decimal? candidate_VariableBonus { get; set; }

        public Decimal? candidate_FixedBonus { get; set; }

        public Decimal? candidate_ExpectedSalary { get; set; }

        [StringLength(50)]
        public string candidate_user_id { get; set; }
        [MaxLengthAttribute()]
        public string candidate_password { get; set; }

        //-------------For Full Report--------------//

        public string candidate_Company { get; set; }

        public string candidate_OrgUnit { get; set; }

        public string candidate_CostCenter { get; set; }

        public string candidate_Position { get; set; }

        //public string candidate_Rank { get; set; }

        public string candidate_TypeOfEmployment { get; set; }

        //public string candidate_Prefix { get; set; }

        //public string candidate_FirstName { get; set; }

        //public string candidate_LastName { get; set; }


        public string candidate_AlternativeNameTH { get; set; }

        public string candidate_Gender { get; set; }

        public DateTime? candidate_DateOfBirth { get; set; }

        public string candidate_BirthPlace { get; set; }

        public virtual TM_Education_Degree CurrentOrLatestDegree { get; set; }

        public string candidate_Age { get; set; }

        public string candidate_CountryOfBirth { get; set; }

        public string candidate_MaritalStatus { get; set; }

        public int? Nationalities_Id { get; set; }

        [ForeignKey("Nationalities_Id")]
        public virtual TM_Nationalities Nationalities { get; set; }

        public int? CountryOfBirth_Id { get; set; }

        [ForeignKey("CountryOfBirth_Id")]
        public virtual TM_Country CountryOfBirth { get; set; }

        // For PA  English
        public string candidate_PAHouseNo_EN { get; set; }
        public string candidate_PAVillageNoAndAlley_EN { get; set; }

        public string candidate_PAStreet_EN { get; set; }

        public string candidate_PAPostalCode_EN { get; set; }

        public string candidate_PATelephoneNumber_EN { get; set; }

        public string candidate_PAMobileNumber_EN { get; set; }



        public int? PA_EN_TM_SubDistrict_Id { get; set; }

        [ForeignKey("PA_EN_TM_SubDistrict_Id")]
        public virtual TM_SubDistrict PA_EN_TM_SubDistrict { get; set; }

        public string CountryAbroadID { get; set; }

        //End of PA English

        //For CA English
        public string candidate_CAHouseNo_EN { get; set; }

        public string candidate_CAVillageNoAndAlley_EN { get; set; }

        public string candidate_CAStreet_EN { get; set; }

        public string candidate_CAPostalCode_EN { get; set; }

        public string candidate_CATelephoneNumber_EN { get; set; }

        public string candidate_CAMobileNumber_EN { get; set; }


        public int? CA_EN_TM_SubDistrict_Id { get; set; }

        [ForeignKey("CA_EN_TM_SubDistrict_Id")]
        public virtual TM_SubDistrict CA_EN_TM_SubDistrict { get; set; }
        //End CA of English


        public int? CA_EN_CountryAbroad_Id { get; set; }


        [ForeignKey("CA_EN_CountryAbroad_Id")]
        public virtual TM_Country CA_EN_CountryAbroad { get; set; }


        public int? CA_TH_CountryAbroad_Id { get; set; }


        [ForeignKey("CA_TH_CountryAbroad_Id")]
        public virtual TM_Country CA_TH_CountryAbroad { get; set; }

        public int? PA_EN_CountryAbroad_Id { get; set; }


        [ForeignKey("PA_EN_CountryAbroad_Id")]
        public virtual TM_Country PA_EN_CountryAbroad { get; set; }


        public int? PA_TH_CountryAbroad_Id { get; set; }


        [ForeignKey("PA_TH_CountryAbroad_Id")]
        public virtual TM_Country PA_TH_CountryAbroad { get; set; }


        public string candidate_PAHouseNo { get; set; }

        public string candidate_PAMooAndSoi { get; set; }

        public string candidate_PAStreet { get; set; }

        public string candidate_PAPostalCode { get; set; }

        public string candidate_PATelephoneNumber { get; set; }

        public string candidate_PAMobileNumber { get; set; }

        public string candidate_CAHouseNo { get; set; }

        public string candidate_CAMooAndSoi { get; set; }

        public string candidate_CAStreet { get; set; }

        public string candidate_CAPostalCode { get; set; }

        public string candidate_CATelephoneNumber { get; set; }

        public string candidate_CAMobileNumber { get; set; }

        /*Information for On-Board*/
        public string candidate_CompleteInfoForOnBoard { get; set; }

        public string candidate_BankAccountName { get; set; }

        public string candidate_BankAccountNumber { get; set; }

        public string candidate_BankAccountBranchName { get; set; }

        public string candidate_BankAccountBranchNumber { get; set; }

        public string candidate_StudentID { get; set; }

        public string candidate_SocialSecurityTH { get; set; }// "Y/N"

        public string candidate_ProvidentFundTH { get; set; }// "Y/N"

        public string candidate_DeathContribution { get; set; }//  "Y/N"

        public string candidate_MilitaryServicesDoc { get; set; } // "Y/N"

        public string candidate_IBMP { get; set; }  // "Y/N"

        public string candidate_OfficialNote { get; set; }
        public string candidate_InternalNoteForHRTeam { get; set; }

        public string candidate_officialNoteAnnoucement { get; set; }
        //Application Status 

        //Application Status
        public string candidate_ApplicationStatus { get; set; }

        public DateTime? candidate_EducationStartDate { get; set; }

        public DateTime? candidate_EducationEndDate { get; set; }

        public string candidate_EduInstituteOrLocationOfTraining { get; set; }

        public string candidate_EduCountry { get; set; }

        public string candidate_EduCurrentGPATranscript { get; set; }

        public string candidate_EduCurrentOrLatestDegree { get; set; }

        public string candidate_EduMajorStudy { get; set; }

        public string candidate_EnglishTestName { get; set; }

        public Decimal? candidate_EnglishTestScoreOrOxford { get; set; }

        public string candidate_EnglishTestStatus { get; set; }

        public DateTime? candidate_EnglishTestDate { get; set; }

        public string candidate_TraineeNumber { get; set; }



        public Decimal? candidate_AuditingScore { get; set; }  // "Y/N"

        public DateTime? candidate_AuditingTestDate { get; set; }



        public string candidate_Program { get; set; }

        /*For Audit*/

        public string candidate_IndustryPrerences1 { get; set; }

        public string candidate_IndustryPrerences2 { get; set; }

        public string candidate_IndustryPrerences3 { get; set; }

        public string candidate_IndustryPrerences4 { get; set; }

        public string candidate_IndustryPrerences5 { get; set; }


        //[DisplayFormat(DataFormatString = "{0:n2}")]
        public Decimal? candidate_OtherAllowances { get; set; }

        public string AnnualLeaveDays { get; set; }

        public string CPAPassedStatus { get; set; }

        public string CPAPassedYear { get; set; }

        public string CPALicenseNo { get; set; }

        public string RankGradeForHiring { get; set; }

        public string TypeOfCandidate { get; set; }

        public DateTime? candidate_ApplyDate { get; set; }

        public DateTime? candidate_LastUpdateDate { get; set; }

        public string YearOfPerformanceReview { get; set; }

        public string HRWrapUpComment { get; set; }

        public string candidate_full_name()
        {
            return first_name_en + " " + last_name_en;
        }


        [StringLength(250)]
        public string trainee_email { get; set; }
        [StringLength(10)]
        public string is_verify { get; set; }
        [StringLength(10)]
        public string verify_code { get; set; }
    }
}
