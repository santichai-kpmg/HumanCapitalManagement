
using HumanCapitalManagement.Models._360Feedback;
using HumanCapitalManagement.Models._360Feedback.MiniHeart;
using HumanCapitalManagement.Models._360Feedback.MiniHeart2021;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.ConsentModel;
using HumanCapitalManagement.Models.EducationModels;
using HumanCapitalManagement.Models.eGreetings;
using HumanCapitalManagement.Models.LogModels;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.OldTable;
using HumanCapitalManagement.Models.PartnerEvaluation;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using HumanCapitalManagement.Models.PreInternAssessment;
using HumanCapitalManagement.Models.Probation;
using HumanCapitalManagement.Models.StrengthFinderModels;
using HumanCapitalManagement.Models.TIFForm;
using HumanCapitalManagement.Models.TraineeSite;
using HumanCapitalManagement.Models.VisaExpiry;
using HumanCapitalManagement.Models.VisExpiry;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data
{
    public class StoreDb : DbContext
    {
        #region Old Head Count
        public DbSet<Division> Divisions { get; set; }
        public DbSet<FYPlan> FYPlans { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<PreviousFY> PreviousFYs { get; set; }

        #endregion

        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuAction> MenuAction { get; set; }
        public DbSet<TraineeMenu> TraineeMenu { get; set; }
        public DbSet<TraineeMenuAction> TraineeMenuAction { get; set; }
        public DbSet<GroupPermission> GroupPermission { get; set; }
        public DbSet<GroupListPermission> GroupListPermission { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }
        public DbSet<UserListPermission> UserListPermission { get; set; }
        public DbSet<UserUnitGroup> UserUnitGroup { get; set; }
        public DbSet<TM_Employment_Type> TM_Employment_Type { get; set; }
        public DbSet<TM_Employment_Rank> TM_Employment_Rank { get; set; }
        public DbSet<TM_Rank> TM_Rank { get; set; }
        public DbSet<TM_Flow_Approve> TM_Flow_Approve { get; set; }
        public DbSet<TM_Employment_Request> TM_Employment_Request { get; set; }
        public DbSet<TM_Step_Approve> TM_Step_Approve { get; set; }
        public DbSet<TM_PR_Status> TM_PR_Status { get; set; }
        public DbSet<MailContent> MailContent { get; set; }
        public DbSet<TM_MailContent_Cc> TM_MailContent_Cc { get; set; }
        public DbSet<TM_MailContent_Cc_bymail> TM_MailContent_Cc_bymail { get; set; }
        public DbSet<TM_MailContent_Param> TM_MailContent_Param { get; set; }
        public DbSet<TM_TechnicalTest> TM_TechnicalTest { get; set; }
        public DbSet<TM_TechnicalTestTransaction> TM_TechnicalTestTransaction { get; set; }
        public DbSet<TempTransaction> TempTransaction { get; set; }

        public DbSet<TM_Nationalities> TM_Nationalities { get; set; }
        public DbSet<TM_WorkExperience> TM_WorkExperience { get; set; }
        public DbSet<TM_Prefix> TM_Prefix { get; set; }

        #region Master Data organization
        public DbSet<TM_Divisions> TM_Divisions { get; set; }
        public DbSet<TM_Pool> TM_Pool { get; set; }
        public DbSet<TM_Company> TM_Company { get; set; }
        public DbSet<TM_Company_Approve_Permit> TM_Company_Approve_Permit { get; set; }
        public DbSet<TM_UnitGroup_Approve_Permit> TM_UnitGroup_Approve_Permit { get; set; }
        public DbSet<TM_Pool_Approve_Permit> TM_Pool_Approve_Permit { get; set; }
        public DbSet<TM_SubGroup> TM_SubGroup { get; set; }
        public DbSet<TM_Pool_Rank> TM_Pool_Rank { get; set; }
        public DbSet<TM_Position> TM_Position { get; set; }
        public DbSet<TM_Recruitment_Team> TM_Recruitment_Team { get; set; }

        public DbSet<TM_MaritalStatus> TM_MaritalStatus { get; set; }

        public DbSet<TM_Gender> TM_Gender { get; set; }
        #endregion

        #region main Table
        public DbSet<PersonnelRequest> PersonnelRequest { get; set; }
        public DbSet<E_Mail_History> E_Mail_History { get; set; }
        public DbSet<TM_Candidates> TM_Candidates { get; set; }
        public DbSet<TM_Candidate_Status> TM_Candidate_Status { get; set; }
        public DbSet<TM_Candidate_Status_Cycle> TM_Candidate_Status_Cycle { get; set; }
        public DbSet<TM_Candidate_Type> TM_Candidate_Type { get; set; }
        public DbSet<TM_Candidate_Rank> TM_Candidate_Rank { get; set; }

        public DbSet<TM_PR_Candidate_Mapping> TM_PR_Candidate_Mapping { get; set; }

        public DbSet<TM_Evaluation_Form> TM_Evaluation_Form { get; set; }
        public DbSet<TM_Evaluation_Question> TM_Evaluation_Question { get; set; }
        public DbSet<TM_SourcingChannel> TM_SourcingChannel { get; set; }
        public DbSet<TM_Trainee_Eva> TM_Trainee_Eva { get; set; }
        public DbSet<TM_Trainee_Eva_Answer> TM_Trainee_Eva_Answer { get; set; }

        public DbSet<TM_FY_Plan> TM_FY_Plan { get; set; }
        public DbSet<TM_FY_Detail> TM_FY_Detail { get; set; }
        public DbSet<TM_Company_Trainee> TM_Company_Trainee { get; set; }

        #endregion
        #region education 
        public DbSet<TM_Education_History> TM_Education_History { get; set; }
        public DbSet<TM_Major> TM_Major { get; set; }
        public DbSet<TM_Universitys> TM_Universitys { get; set; }
        public DbSet<TM_Universitys_Major> TM_Universitys_Major { get; set; }
        public DbSet<TM_Faculty> TM_Faculty { get; set; }
        public DbSet<TM_Universitys_Faculty> TM_Universitys_Faculty { get; set; }

        public DbSet<TM_Country> TM_Country { get; set; }
        public DbSet<TM_City> TM_City { get; set; }
        public DbSet<TM_District> TM_District { get; set; }
        public DbSet<TM_SubDistrict> TM_SubDistrict { get; set; }
        public DbSet<TM_Education_Degree> TM_Education_Degree { get; set; }
        #endregion
        #region Mass TIF Form
        public DbSet<TM_Mass_TIF_Form> TM_Mass_TIF_Form { get; set; }
        public DbSet<TM_MassTIF_Form_Question> TM_MassTIF_Form_Question { get; set; }
        public DbSet<TM_Mass_Question_Type> TM_Mass_Question_Type { get; set; }
        public DbSet<TM_Mass_Auditing_Question> TM_Mass_Auditing_Question { get; set; }
        public DbSet<TM_Mass_Scoring> TM_Mass_Scoring { get; set; }
        public DbSet<TM_MassTIF_Status> TM_MassTIF_Status { get; set; }
        public DbSet<TM_TIF_Form> TM_TIF_Form { get; set; }
        public DbSet<TM_TIF_Form_Question> TM_TIF_Form_Question { get; set; }
        public DbSet<TM_TIF_Rating> TM_TIF_Rating { get; set; }
        public DbSet<TM_Eva_Rating> TM_Eva_Rating { get; set; }
        public DbSet<TM_TraineeEva_Status> TM_TraineeEva_Status { get; set; }
        public DbSet<TM_TIF_Status> TM_TIF_Status { get; set; }
        // mass 
        public DbSet<TM_Candidate_MassTIF> TM_Candidate_MassTIF { get; set; }
        public DbSet<TM_Candidate_MassTIF_Approv> TM_Candidate_MassTIF_Approv { get; set; }
        public DbSet<TM_Candidate_MassTIF_Audit_Qst> TM_Candidate_MassTIF_Audit_Qst { get; set; }
        public DbSet<TM_Candidate_MassTIF_Core> TM_Candidate_MassTIF_Core { get; set; }
        // non mass
        public DbSet<TM_Candidate_MassTIF_Additional> TM_Candidate_MassTIF_Additional { get; set; }
        public DbSet<TM_Candidate_TIF> TM_Candidate_TIF { get; set; }
        public DbSet<TM_Candidate_TIF_Answer> TM_Candidate_TIF_Answer { get; set; }
        public DbSet<TM_Additional_Information> TM_Additional_Information { get; set; }
        public DbSet<TM_Additional_Questions> TM_Additional_Questions { get; set; }
        public DbSet<TM_Additional_Answers> TM_Additional_Answers { get; set; }


        #endregion
        #region Pre Intern
        public DbSet<TM_PIntern_Form> TM_PIntern_Form { get; set; }
        public DbSet<TM_PIntern_Mass_Form_Question> TM_PIntern_Mass_Form_Question { get; set; }
        public DbSet<TM_PIntern_Mass_Question> TM_PIntern_Mass_Question { get; set; }
        public DbSet<TM_PIntern_RatingForm> TM_PIntern_RatingForm { get; set; }
        public DbSet<TM_PIntern_Form_Question> TM_PIntern_Form_Question { get; set; }
        public DbSet<TM_PIntern_Rating> TM_PIntern_Rating { get; set; }
        public DbSet<TM_PInternAssessment_Activities> TM_PInternAssessment_Activities { get; set; }
        public DbSet<TM_PIntern_Status> TM_PIntern_Status { get; set; }

        public DbSet<TM_Candidate_PIntern_Mass> TM_Candidate_PIntern_Mass { get; set; }
        public DbSet<TM_Candidate_PIntern_Mass_Answer> TM_Candidate_PIntern_Mass_Answer { get; set; }
        public DbSet<TM_Candidate_PIntern_Mass_Approv> TM_Candidate_PIntern_Mass_Approv { get; set; }

        public DbSet<TM_Candidate_PIntern> TM_Candidate_PIntern { get; set; }
        public DbSet<TM_Candidate_PIntern_Answer> TM_Candidate_PIntern_Answer { get; set; }
        public DbSet<TM_Candidate_PIntern_Approv> TM_Candidate_PIntern_Approv { get; set; }
        #endregion
        #region Partner Evaluation
        public DbSet<TM_Partner_Evaluation> TM_Partner_Evaluation { get; set; }
        public DbSet<TM_Annual_Rating> TM_Annual_Rating { get; set; }
        public DbSet<PTR_Evaluation> PTR_Evaluation { get; set; }
        public DbSet<PTR_Evaluation_Year> PTR_Evaluation_Year { get; set; }
        public DbSet<PTR_Evaluation_Answer> PTR_Evaluation_Answer { get; set; }
        public DbSet<PTR_Evaluation_KPIs> PTR_Evaluation_KPIs { get; set; }
        public DbSet<PTR_Evaluation_File> PTR_Evaluation_File { get; set; }
        public DbSet<PTR_Evaluation_Authorized> PTR_Evaluation_Authorized { get; set; }

        public DbSet<TM_PTR_Eva_Status> TM_PTR_Eva_Status { get; set; }
        public DbSet<TM_PTR_Eva_ApproveStep> TM_PTR_Eva_ApproveStep { get; set; }
        public DbSet<PTR_Evaluation_Approve> PTR_Evaluation_Approve { get; set; }
        public DbSet<TM_Feedback_Rating> TM_Feedback_Rating { get; set; }
        public DbSet<PTR_Feedback_Emp> PTR_Feedback_Emp { get; set; }
        public DbSet<PTR_Feedback_UnitGroup> PTR_Feedback_UnitGroup { get; set; }
        public DbSet<TM_PTR_E_Mail_History> TM_PTR_E_Mail_History { get; set; }

        public DbSet<TM_PTR_Eva_Incidents> TM_PTR_Eva_Incidents { get; set; }
        public DbSet<PTR_Evaluation_Incidents> PTR_Evaluation_Incidents { get; set; }
        public DbSet<TM_PTR_Eva_Incidents_Score> TM_PTR_Eva_Incidents_Score { get; set; }

        #endregion

        #region Nomination System

        public DbSet<PES_Nomination> PES_Nomination { get; set; }
        public DbSet<PES_Nomination_Answer> PES_Nomination_Answer { get; set; }
        public DbSet<PES_Nomination_Competencies> PES_Nomination_Competencies { get; set; }
        public DbSet<PES_Nomination_Files> PES_Nomination_Files { get; set; }
        public DbSet<PES_Nomination_KPIs> PES_Nomination_KPIs { get; set; }
        public DbSet<PES_Nomination_Signatures> PES_Nomination_Signatures { get; set; }
        public DbSet<PES_Nomination_Year> PES_Nomination_Year { get; set; }
        public DbSet<TM_PES_NMN_Competencies> TM_PES_NMN_Competencies { get; set; }
        public DbSet<TM_PES_NMN_Competencies_Rating> TM_PES_NMN_Competencies_Rating { get; set; }
        public DbSet<TM_PES_NMN_Questions> TM_PES_NMN_Questions { get; set; }
        public DbSet<TM_PES_NMN_SignatureStep> TM_PES_NMN_SignatureStep { get; set; }
        public DbSet<TM_PES_NMN_Status> TM_PES_NMN_Status { get; set; }
        public DbSet<TM_PES_NMN_Type> TM_PES_NMN_Type { get; set; }

        public DbSet<PES_Final_Rating_Year> PES_Final_Rating_Year { get; set; }
        public DbSet<PES_Final_Rating> PES_Final_Rating { get; set; }
        #endregion

        #region Visa Expiry
        public DbSet<TM_Employee> TM_Employee { get; set; }
        public DbSet<TM_Type_Document> TM_Type_Document { get; set; }
        public DbSet<TM_Document_Employee> TM_Document_Employee { get; set; }
        public DbSet<TM_Prefix_Visa> TM_Prefix_Visa { get; set; }
        public DbSet<TM_Company_Visa> TM_Company_Visa { get; set; }
        public DbSet<TM_Remark_Visa> TM_Remark_Visa { get; set; }
        public DbSet<TM_EmployeeForeign_Visa> TM_EmployeeForeign_Visa { get; set; }

        public DbSet<tblMstAutoMails> tblMstAutoMails { get; set; }

        #endregion

        #region Trainee

        public DbSet<TM_Time_Type> TM_Time_Type { get; set; }
        public DbSet<TM_TimeSheet_Status> TM_TimeSheet_Status { get; set; }
        public DbSet<TimeSheet_Form> TimeSheet_Form { get; set; }
        public DbSet<TimeSheet_Detail> TimeSheet_Detail { get; set; }
        public DbSet<Perdiem_Transport> Perdiem_Transport { get; set; }
        public DbSet<TM_Trainee_HiringRating> TM_Trainee_HiringRating { get; set; }
        #endregion

        #region Feedback
        public DbSet<Feedback> Feedback { get; set; }
        #endregion


        #region MiniHeart
        public DbSet<MiniHeart_Main> MiniHeart_Main { get; set; }
        public DbSet<MiniHeart_Detail> MiniHeart_Detail { get; set; }
        public DbSet<TM_MiniHeart_Peroid> TM_MiniHeart_Peroid { get; set; }
        public DbSet<TM_MiniHeart_Group_Question> TM_MiniHeart_Group_Question { get; set; }
        public DbSet<TM_MiniHeart_Question> TM_MiniHeart_Question { get; set; }
        #endregion

        #region MiniHeart2021
        public DbSet<MiniHeart_Main2021> MiniHeart_Main2021 { get; set; }
        public DbSet<MiniHeart_Detail2021> MiniHeart_Detail2021 { get; set; }
        public DbSet<TM_MiniHeart_Peroid2021> TM_MiniHeart_Peroid2021 { get; set; }
        public DbSet<TM_MiniHeart_Group_Question2021> TM_MiniHeart_Group_Question2021 { get; set; }
        public DbSet<TM_MiniHeart_Question2021> TM_MiniHeart_Question2021 { get; set; }
        #endregion

        #region Probation
        public DbSet<TM_Probation_Group_Question> TM_Probation_Group_Question { get; set; }
        public DbSet<TM_Probation_Question> TM_Probation_Question { get; set; }
        public DbSet<Probation_Form> Probation_Form { get; set; }
        public DbSet<Probation_Details> Probation_Detail { get; set; }
        public DbSet<Action_Plans> Action_Plans { get; set; }
        public DbSet<Probation_With_Out> Probation_With_Out { get; set; }
        #endregion

        #region eGreetings
        public DbSet<eGreetings_Main> eGreetings_Main { get; set; }
        public DbSet<eGreetings_Detail> eGreetings_Detail { get; set; }
        public DbSet<TM_eGreetings_Peroid> TM_eGreetings_Peroid { get; set; }
        public DbSet<TM_eGreetings_Group_Question> TM_eGreetings_Group_Question { get; set; }
        public DbSet<TM_eGreetings_Question> TM_eGreetings_Question { get; set; }
        #endregion

        #region Consent
        public DbSet<Consent_Asnwer> Consent_Asnwer { get; set; }
        public DbSet<Consent_Asnwer_Log> Consent_Asnwer_Log { get; set; }
        public DbSet<Consent_Main_Form> Consent_Main_Form { get; set; }
        public DbSet<Consent_Spacial> Consent_Spacial { get; set; }
        public DbSet<TM_Consent_Form> TM_Consent_Form { get; set; }
        public DbSet<TM_Consent_Question> TM_Consent_Question { get; set; }
        #endregion

        #region Log Data
        public DbSet<LogPersonnelRequest> LogPersonnelRequest { get; set; }
        public DbSet<LogUpdateEmpNO> LogUpdateEmpNO { get; set; }
        #endregion

        #region StrengthFinder 
        public DbSet<TM_StrenghtFinder> TM_StrenghtFinder { get; set; }
        public DbSet<TB_StrengthFinderHistory> TB_StrengthFinderHistory { get; set; }


        #endregion
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //  base.OnModelCreating(modelBuilder);
        }
    }
}
