using Autofac;
using Autofac.Integration.Mvc;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.AddressRepository;
using HumanCapitalManagement.Data.Common;
using HumanCapitalManagement.Data.EduRepository;
using HumanCapitalManagement.Data.eGreetingsRepository;
using HumanCapitalManagement.Data.LogRepository;
using HumanCapitalManagement.Data.MainRepository;
using HumanCapitalManagement.Data.PartnerEvaluation;
using HumanCapitalManagement.Data.PreInternAssessment;
using HumanCapitalManagement.Data.TM_StrengthFinderRepository;
using HumanCapitalManagement.Data.TB_StrengthFinderHistoryRepository;
using HumanCapitalManagement.Data.TIFForm;
using HumanCapitalManagement.Data.VisaExpiry;
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
using HumanCapitalManagement.Service;
using HumanCapitalManagement.Service.AddressService;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.ConsentService;
using HumanCapitalManagement.Service.EduService;
using HumanCapitalManagement.Service.eGreetingsService;
using HumanCapitalManagement.Service.LogService;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.PartnerEvaluation;
using HumanCapitalManagement.Service.PreInternAssessment;
using HumanCapitalManagement.Service.StrengthFinder;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.Service.TraineeSite;
using HumanCapitalManagement.Service.VisaExpiry;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HumanCapitalManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            SetupAutofac();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = ci;
        }
        private void SetupAutofac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Builder
            #region builder Menu
            builder.RegisterType<MPRepository>()
                   .AsSelf()
                   .As<IRepository<Models.Common.Menu>>();
            builder.RegisterType<MenuActionRepository>()
                   .AsSelf()
                   .As<IRepository<MenuAction>>();
            builder.RegisterType<MPService>()
                   .AsSelf()
                   .As<IService<Models.Common.Menu>>();
            builder.RegisterType<MenuActionService>()
                   .AsSelf()
                   .As<IService<MenuAction>>();

            #endregion
            #region builder Mail Content

            builder.RegisterType<MailContentRepository>()
                   .AsSelf()
                   .As<IRepository<MailContent>>();
            builder.RegisterType<MailContentService>()
                      .AsSelf()
                      .As<IService<MailContent>>();

            builder.RegisterType<TM_MailContent_CcRepository>()
                       .AsSelf()
                       .As<IRepository<TM_MailContent_Cc>>();
            builder.RegisterType<TM_MailContent_CcService>()
                      .AsSelf()
                      .As<IService<TM_MailContent_Cc>>();

            builder.RegisterType<TM_MailContent_Cc_bymailRepository>()
                  .AsSelf()
                  .As<IRepository<TM_MailContent_Cc_bymail>>();
            builder.RegisterType<TM_MailContent_Cc_bymailService>()
                      .AsSelf()
                      .As<IService<TM_MailContent_Cc_bymail>>();
            #endregion
            #region builder GroupPermission
            builder.RegisterType<GroupPermissionRepository>()
                   .AsSelf()
                   .As<IRepository<GroupPermission>>();
            builder.RegisterType<GroupListPermissionRepository>()
                    .AsSelf()
                    .As<IRepository<GroupListPermission>>();

            builder.RegisterType<GroupPermissionService>()
                   .AsSelf()
                   .As<IService<GroupPermission>>();
            builder.RegisterType<GroupListPermissionService>()
                   .AsSelf()
                   .As<IService<GroupListPermission>>();
            #endregion
            #region builder UserListPermission
            builder.RegisterType<UserPermissionRepository>()
                  .AsSelf()
                  .As<IRepository<UserPermission>>();
            builder.RegisterType<UserListPermissionRepository>()
                    .AsSelf()
                    .As<IRepository<UserListPermission>>();

            builder.RegisterType<UserUnitGroupRepository>()
                    .AsSelf()
                    .As<IRepository<UserUnitGroup>>();
            builder.RegisterType<UserPermissionService>()
                    .AsSelf()
                    .As<IService<UserPermission>>();
            builder.RegisterType<UserListPermissionService>()
                    .AsSelf()
                    .As<IService<UserListPermission>>();
            builder.RegisterType<UserUnitGroupService>()
                 .AsSelf()
                 .As<IService<UserUnitGroup>>();
            #endregion
            #region builder TM_Divisions
            builder.RegisterType<DivisionRepository>()
                   .AsSelf()
                   .As<IRepository<TM_Divisions>>();
            builder.RegisterType<TM_UnitGroup_Approve_PermitRepository>()
                   .AsSelf()
                   .As<IRepository<TM_UnitGroup_Approve_Permit>>();
            builder.RegisterType<DivisionService>()
                  .AsSelf()
                  .As<IService<TM_Divisions>>();
            builder.RegisterType<TM_UnitGroup_Approve_PermitService>()
                     .AsSelf()
                     .As<IService<TM_UnitGroup_Approve_Permit>>();
            //sup group
            builder.RegisterType<TM_SubGroupRepository>()
                     .AsSelf()
                     .As<IRepository<TM_SubGroup>>();
            builder.RegisterType<TM_SubGroupService>()
                     .AsSelf()
                     .As<IService<TM_SubGroup>>();
            #endregion
            #region builder TM_Pool
            builder.RegisterType<PoolRepository>()
                  .AsSelf()
                  .As<IRepository<TM_Pool>>();
            builder.RegisterType<PoolService>()
                      .AsSelf()
                      .As<IService<TM_Pool>>();
            builder.RegisterType<TM_Pool_Approve_PermitRepository>()
                        .AsSelf()
                        .As<IRepository<TM_Pool_Approve_Permit>>();
            builder.RegisterType<TM_Pool_Approve_PermitService>()
                      .AsSelf()
                      .As<IService<TM_Pool_Approve_Permit>>();

            builder.RegisterType<TM_Pool_RankRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Pool_Rank>>();
            builder.RegisterType<TM_Pool_RankService>()
                      .AsSelf()
                      .As<IService<TM_Pool_Rank>>();

            builder.RegisterType<TM_PositionRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Position>>();
            builder.RegisterType<TM_PositionService>()
                      .AsSelf()
                      .As<IService<TM_Position>>();
            #endregion
            #region builder Company
            builder.RegisterType<CompanyRepository>()
                 .AsSelf()
                 .As<IRepository<TM_Company>>();
            builder.RegisterType<CompanyService>()
                      .AsSelf()
                      .As<IService<TM_Company>>();
            builder.RegisterType<TM_Company_Approve_PermitRepository>()
        .AsSelf()
        .As<IRepository<TM_Company_Approve_Permit>>();
            builder.RegisterType<TM_Company_Approve_PermitService>()
                      .AsSelf()
                      .As<IService<TM_Company_Approve_Permit>>();

            #endregion
            #region builder PR Data

            builder.RegisterType<RequestTypeRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Request_Type>>();
            builder.RegisterType<RequestTypeService>()
                      .AsSelf()
                      .As<IService<TM_Request_Type>>();
            builder.RegisterType<EmploymentTypeRepository>()
                  .AsSelf()
                  .As<IRepository<TM_Employment_Type>>();
            builder.RegisterType<EmploymentTypeService>()
                      .AsSelf()
                      .As<IService<TM_Employment_Type>>();

            builder.RegisterType<TM_Employment_RequestRepository>()
                       .AsSelf()
                       .As<IRepository<TM_Employment_Request>>();
            builder.RegisterType<TM_Employment_RequestService>()
                      .AsSelf()
                      .As<IService<TM_Employment_Request>>();


            builder.RegisterType<RankRepository>()
                    .AsSelf()
                    .As<IRepository<TM_Rank>>();
            builder.RegisterType<RankService>()
                      .AsSelf()
                      .As<IService<TM_Rank>>();

            builder.RegisterType<TM_Step_ApproveRepository>()
                    .AsSelf()
                    .As<IRepository<TM_Step_Approve>>();
            builder.RegisterType<TM_Step_ApproveService>()
                      .AsSelf()
                      .As<IService<TM_Step_Approve>>();

            builder.RegisterType<TM_PR_StatusRepository>()
                 .AsSelf()
                 .As<IRepository<TM_PR_Status>>();
            builder.RegisterType<TM_PR_StatusService>()
                      .AsSelf()
                      .As<IService<TM_PR_Status>>();

            builder.RegisterType<PersonnelRequestRepository>()
                     .AsSelf()
                     .As<IRepository<PersonnelRequest>>();
            builder.RegisterType<PersonnelRequestService>()
                      .AsSelf()
                      .As<IService<PersonnelRequest>>();

            builder.RegisterType<E_Mail_HistoryRepository>()
                         .AsSelf()
                         .As<IRepository<E_Mail_History>>();
            builder.RegisterType<E_Mail_HistoryService>()
                      .AsSelf()
                      .As<IService<E_Mail_History>>();

            builder.RegisterType<TM_Recruitment_TeamRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Recruitment_Team>>();
            builder.RegisterType<TM_Recruitment_TeamService>()
                      .AsSelf()
                      .As<IService<TM_Recruitment_Team>>();


            builder.RegisterType<TM_TIF_FormRepository>()
                      .AsSelf()
                      .As<IRepository<TM_TIF_Form>>();
            builder.RegisterType<TM_TIF_FormService>()
                      .AsSelf()
                      .As<IService<TM_TIF_Form>>();

            builder.RegisterType<TM_Candidate_TIFRepository>()
                   .AsSelf()
                   .As<IRepository<TM_Candidate_TIF>>();
            builder.RegisterType<TM_Candidate_TIFService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_TIF>>();

            builder.RegisterType<TM_Candidate_TIF_AnswerRepository>()
                .AsSelf()
                .As<IRepository<TM_Candidate_TIF_Answer>>();
            builder.RegisterType<TM_Candidate_TIF_AnswerService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_TIF_Answer>>();

            builder.RegisterType<TM_Evaluation_FormRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Evaluation_Form>>();
            builder.RegisterType<TM_Evaluation_FormService>()
                      .AsSelf()
                      .As<IService<TM_Evaluation_Form>>();

            builder.RegisterType<TM_Evaluation_QuestionRepository>()
                   .AsSelf()
                   .As<IRepository<TM_Evaluation_Question>>();
            builder.RegisterType<TM_Evaluation_QuestionService>()
                      .AsSelf()
                      .As<IService<TM_Evaluation_Question>>();
            #endregion
            #region builder Mass TIF Form
            builder.RegisterType<TM_Mass_TIF_FormRepository>()
                   .AsSelf()
                   .As<IRepository<TM_Mass_TIF_Form>>();
            builder.RegisterType<TM_Mass_TIF_FormService>()
                      .AsSelf()
                      .As<IService<TM_Mass_TIF_Form>>();

            builder.RegisterType<TM_Mass_Question_TypeRepository>()
               .AsSelf()
               .As<IRepository<TM_Mass_Question_Type>>();
            builder.RegisterType<TM_Mass_Question_TypeService>()
                      .AsSelf()
                      .As<IService<TM_Mass_Question_Type>>();

            builder.RegisterType<TM_Mass_Auditing_QuestionRepository>()
                       .AsSelf()
                       .As<IRepository<TM_Mass_Auditing_Question>>();
            builder.RegisterType<TM_Mass_Auditing_QuestionService>()
                      .AsSelf()
                      .As<IService<TM_Mass_Auditing_Question>>();

            builder.RegisterType<TM_Mass_ScoringRepository>()
                           .AsSelf()
                           .As<IRepository<TM_Mass_Scoring>>();
            builder.RegisterType<TM_Mass_ScoringService>()
                      .AsSelf()
                      .As<IService<TM_Mass_Scoring>>();


            builder.RegisterType<TM_MassTIF_StatusRepository>()
                           .AsSelf()
                           .As<IRepository<TM_MassTIF_Status>>();
            builder.RegisterType<TM_MassTIF_StatusService>()
                      .AsSelf()
                      .As<IService<TM_MassTIF_Status>>();

            builder.RegisterType<TM_Candidate_MassTIF_Audit_QstRepository>()
                          .AsSelf()
                          .As<IRepository<TM_Candidate_MassTIF_Audit_Qst>>();
            builder.RegisterType<TM_Candidate_MassTIF_Audit_QstService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_MassTIF_Audit_Qst>>();

            builder.RegisterType<TM_Candidate_MassTIF_CoreRepository>()
                          .AsSelf()
                          .As<IRepository<TM_Candidate_MassTIF_Core>>();
            builder.RegisterType<TM_Candidate_MassTIF_CoreService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_MassTIF_Core>>();

            builder.RegisterType<TM_Candidate_MassTIFRepository>()
                          .AsSelf()
                          .As<IRepository<TM_Candidate_MassTIF>>();
            builder.RegisterType<TM_Candidate_MassTIFService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_MassTIF>>();

            builder.RegisterType<TM_Candidate_MassTIF_ApprovRepository>()
                        .AsSelf()
                        .As<IRepository<TM_Candidate_MassTIF_Approv>>();
            builder.RegisterType<TM_Candidate_MassTIF_ApprovService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_MassTIF_Approv>>();


            builder.RegisterType<TM_Additional_InformationRepository>()
            .AsSelf()
            .As<IRepository<TM_Additional_Information>>();
            builder.RegisterType<TM_Additional_InformationService>()
                      .AsSelf()
                      .As<IService<TM_Additional_Information>>();

            builder.RegisterType<TM_Additional_QuestionsRepository>()
           .AsSelf()
           .As<IRepository<TM_Additional_Questions>>();
            builder.RegisterType<TM_Additional_QuestionsService>()
                      .AsSelf()
                      .As<IService<TM_Additional_Questions>>();

            builder.RegisterType<TM_Additional_AnswersRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Additional_Answers>>();
            builder.RegisterType<TM_Additional_AnswersService>()
                      .AsSelf()
                      .As<IService<TM_Additional_Answers>>();


            builder.RegisterType<TM_Candidate_MassTIF_AdditionalRepository>()
               .AsSelf()
               .As<IRepository<TM_Candidate_MassTIF_Additional>>();
            builder.RegisterType<TM_Candidate_MassTIF_AdditionalService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_MassTIF_Additional>>();

            builder.RegisterType<TM_Candidate_MassTIF_Adnl_AnswerRepository>()
                     .AsSelf()
                     .As<IRepository<TM_Candidate_MassTIF_Adnl_Answer>>();
            builder.RegisterType<TM_Candidate_MassTIF_Adnl_AnswerService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_MassTIF_Adnl_Answer>>();


            #endregion
            #region Candidate
            builder.RegisterType<TM_Candidate_TypeRepository>()
                .AsSelf()
                .As<IRepository<TM_Candidate_Type>>();
            builder.RegisterType<TM_Candidate_TypeService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_Type>>();

            builder.RegisterType<TM_CandidatesRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Candidates>>();
            builder.RegisterType<TM_CandidatesService>()
                      .AsSelf()
                      .As<IService<TM_Candidates>>();

            builder.RegisterType<TM_Candidate_StatusRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Candidate_Status>>();
            builder.RegisterType<TM_Candidate_StatusService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_Status>>();

            builder.RegisterType<TM_Candidate_Status_CycleRepository>()
                       .AsSelf()
                       .As<IRepository<TM_Candidate_Status_Cycle>>();
            builder.RegisterType<TM_Candidate_Status_CycleService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_Status_Cycle>>();

            builder.RegisterType<TM_PR_Candidate_MappingRepository>()
                        .AsSelf()
                        .As<IRepository<TM_PR_Candidate_Mapping>>();
            builder.RegisterType<TM_PR_Candidate_MappingService>()
                      .AsSelf()
                      .As<IService<TM_PR_Candidate_Mapping>>();

            builder.RegisterType<TM_Candidate_RankRepository>()
             .AsSelf()
             .As<IRepository<TM_Candidate_Rank>>();
            builder.RegisterType<TM_Candidate_RankService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_Rank>>();

            builder.RegisterType<TM_Candidate_Status_NextRepository>()
                        .AsSelf()
                        .As<IRepository<TM_Candidate_Status_Next>>();
            builder.RegisterType<TM_Candidate_Status_NextService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_Status_Next>>();

            builder.RegisterType<TM_TIF_StatusRepository>()
                        .AsSelf()
                        .As<IRepository<TM_TIF_Status>>();
            builder.RegisterType<TM_TIF_StatusService>()
                      .AsSelf()
                      .As<IService<TM_TIF_Status>>();

            builder.RegisterType<TM_Candidate_TIF_ApprovRepository>()
                     .AsSelf()
                     .As<IRepository<TM_Candidate_TIF_Approv>>();
            builder.RegisterType<TM_Candidate_TIF_ApprovService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_TIF_Approv>>();

            builder.RegisterType<TM_SourcingChannelRepository>()
                .AsSelf()
                .As<IRepository<TM_SourcingChannel>>();
            builder.RegisterType<TM_SourcingChannelService>()
                      .AsSelf()
                      .As<IService<TM_SourcingChannel>>();

            builder.RegisterType<TM_GenderRepository>()
                .AsSelf()
                .As<IRepository<TM_Gender>>();
            builder.RegisterType<TM_GenderService>()
                      .AsSelf()
                      .As<IService<TM_Gender>>();


            builder.RegisterType<TM_MaritalStatusRepository>()
            .AsSelf()
            .As<IRepository<TM_MaritalStatus>>();
            builder.RegisterType<TM_MaritalStatusService>()
            .AsSelf()
            .As<IService<TM_MaritalStatus>>();

            builder.RegisterType<TM_TechnicalTestRepository>()
            .AsSelf()
            .As<IRepository<TM_TechnicalTest>>();
            builder.RegisterType<TM_TechnicalTestService>()
            .AsSelf()
            .As<IService<TM_TechnicalTest>>();


            builder.RegisterType<TM_TechnicalTestTransactionRepository>()
            .AsSelf()
            .As<IRepository<TM_TechnicalTestTransaction>>();
            builder.RegisterType<TM_TechnicalTestTransactionService>()
            .AsSelf()
            .As<IService<TM_TechnicalTestTransaction>>();
            //


            builder.RegisterType<TempTransactionRepository>()
            .AsSelf()
            .As<IRepository<TempTransaction>>();
            builder.RegisterType<TempTransactionService>()
            .AsSelf()
            .As<IService<TempTransaction>>();


            builder.RegisterType<TM_WorkExperienceRepository>()
            .AsSelf()
            .As<IRepository<TM_WorkExperience>>();
            builder.RegisterType<TM_WorkExperienceService>()
            .AsSelf()
            .As<IService<TM_WorkExperience>>();


            builder.RegisterType<TM_NationalitiesRepository>()
            .AsSelf()
            .As<IRepository<TM_Nationalities>>();
            builder.RegisterType<TM_NationalitiesService>()
            .AsSelf()
            .As<IService<TM_Nationalities>>();


            builder.RegisterType<TM_PrefixRepository>()
            .AsSelf()
            .As<IRepository<TM_Prefix>>();
            builder.RegisterType<TM_PrefixService>()
            .AsSelf()
            .As<IService<TM_Prefix>>();


            #endregion
            #region builder Education
            builder.RegisterType<TM_UniversitysRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Universitys>>();
            builder.RegisterType<TM_UniversitysService>()
                      .AsSelf()
                      .As<IService<TM_Universitys>>();

            builder.RegisterType<TM_MajorRepository>()
                    .AsSelf()
                    .As<IRepository<TM_Major>>();
            builder.RegisterType<TM_MajorService>()
                      .AsSelf()
                      .As<IService<TM_Major>>();
            builder.RegisterType<TM_FacultyRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Faculty>>();
            builder.RegisterType<TM_FacultyService>()
                      .AsSelf()
                      .As<IService<TM_Faculty>>();

            builder.RegisterType<TM_Universitys_FacultyRepository>()
               .AsSelf()
               .As<IRepository<TM_Universitys_Faculty>>();
            builder.RegisterType<TM_Universitys_FacultyService>()
                      .AsSelf()
                      .As<IService<TM_Universitys_Faculty>>();

            builder.RegisterType<TM_Universitys_MajorRepository>()
                           .AsSelf()
                           .As<IRepository<TM_Universitys_Major>>();
            builder.RegisterType<TM_Universitys_MajorService>()
                      .AsSelf()
                      .As<IService<TM_Universitys_Major>>();


            builder.RegisterType<TM_TIF_RatingRepository>()
               .AsSelf()
               .As<IRepository<TM_TIF_Rating>>();
            builder.RegisterType<TM_TIF_RatingService>()
                      .AsSelf()
                      .As<IService<TM_TIF_Rating>>();
            #endregion
            #region builder Address
            builder.RegisterType<TM_SubDistrictRepository>()
                      .AsSelf()
                      .As<IRepository<TM_SubDistrict>>();
            builder.RegisterType<TM_SubDistrictService>()
                      .AsSelf()
                      .As<IService<TM_SubDistrict>>();

            builder.RegisterType<TM_DistrictRepository>()
                    .AsSelf()
                    .As<IRepository<TM_District>>();
            builder.RegisterType<TM_DistrictService>()
                      .AsSelf()
                      .As<IService<TM_District>>();

            builder.RegisterType<TM_CountryRepository>()
                  .AsSelf()
                  .As<IRepository<TM_Country>>();
            builder.RegisterType<TM_CountryService>()
                      .AsSelf()
                      .As<IService<TM_Country>>();

            builder.RegisterType<TM_CityRepository>()
                 .AsSelf()
                 .As<IRepository<TM_City>>();
            builder.RegisterType<TM_CityService>()
                      .AsSelf()
                      .As<IService<TM_City>>();

            builder.RegisterType<TM_Education_HistoryRepository>()
                .AsSelf()
                .As<IRepository<TM_Education_History>>();
            builder.RegisterType<TM_Education_HistoryServices>()
                      .AsSelf()
                      .As<IService<TM_Education_History>>();



            builder.RegisterType<TM_Education_DegreeRepository>()
            .AsSelf()
            .As<IRepository<TM_Education_Degree>>();
            builder.RegisterType<TM_Education_DegreeServices>()
            .AsSelf()
            .As<IService<TM_Education_Degree>>();

            #endregion
            #region builder Evaluation Trainee
            builder.RegisterType<TM_TraineeEva_StatusRepository>()
            .AsSelf()
            .As<IRepository<TM_TraineeEva_Status>>();
            builder.RegisterType<TM_TraineeEva_StatusService>()
                      .AsSelf()
                      .As<IService<TM_TraineeEva_Status>>();
            builder.RegisterType<TM_Trainee_Eva_AnswerRepository>()
                        .AsSelf()
                        .As<IRepository<TM_Trainee_Eva_Answer>>();
            builder.RegisterType<TM_Trainee_Eva_AnswerService>()
                      .AsSelf()
                      .As<IService<TM_Trainee_Eva_Answer>>();


            builder.RegisterType<TM_Trainee_EvaRepository>()
                        .AsSelf()
                        .As<IRepository<TM_Trainee_Eva>>();
            builder.RegisterType<TM_Trainee_EvaService>()
                      .AsSelf()
                      .As<IService<TM_Trainee_Eva>>();

            builder.RegisterType<TM_Eva_RatingRepository>()
          .AsSelf()
          .As<IRepository<TM_Eva_Rating>>();
            builder.RegisterType<TM_Eva_RatingService>()
                      .AsSelf()
                      .As<IService<TM_Eva_Rating>>();


            builder.RegisterType<TM_TraineeEva_StatusRepository>()
                      .AsSelf()
                      .As<IRepository<TM_TraineeEva_Status>>();
            builder.RegisterType<TM_TraineeEva_StatusService>()
                      .AsSelf()
                      .As<IService<TM_TraineeEva_Status>>();


            builder.RegisterType<TM_Trainee_HiringRatingRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Trainee_HiringRating>>();
            builder.RegisterType<TM_Trainee_HiringRatingService>()
                      .AsSelf()
                      .As<IService<TM_Trainee_HiringRating>>();

            #endregion
            #region builder TimeSheet Trainee
            builder.RegisterType<TimeSheet_FormRepository>()
                       .AsSelf()
                       .As<IRepository<TimeSheet_Form>>();
            builder.RegisterType<TimeSheet_FormService>()
                      .AsSelf()
                      .As<IService<TimeSheet_Form>>();


            builder.RegisterType<TimeSheet_DetailRepository>()
              .AsSelf()
              .As<IRepository<TimeSheet_Detail>>();
            builder.RegisterType<TimeSheet_DetailService>()
                      .AsSelf()
                      .As<IService<TimeSheet_Detail>>();

            builder.RegisterType<TM_Time_TypeRepository>()
             .AsSelf()
             .As<IRepository<TM_Time_Type>>();
            builder.RegisterType<TM_Time_TypeService>()
                      .AsSelf()
                      .As<IService<TM_Time_Type>>();

            builder.RegisterType<Perdiem_TransportRepository>()
            .AsSelf()
            .As<IRepository<Perdiem_Transport>>();
            builder.RegisterType<Perdiem_TransportService>()
                      .AsSelf()
                      .As<IService<Perdiem_Transport>>();

            #endregion
            #region builder Partner Evaluation Form

            builder.RegisterType<TM_Partner_EvaluationRepository>()
                   .AsSelf()
                   .As<IRepository<TM_Partner_Evaluation>>();
            builder.RegisterType<TM_Partner_EvaluationService>()
                      .AsSelf()
                      .As<IService<TM_Partner_Evaluation>>();

            builder.RegisterType<PTR_Evaluation_YearRepository>()
               .AsSelf()
               .As<IRepository<PTR_Evaluation_Year>>();
            builder.RegisterType<PTR_Evaluation_YearService>()
                      .AsSelf()
                      .As<IService<PTR_Evaluation_Year>>();

            builder.RegisterType<PTR_EvaluationRepository>()
                       .AsSelf()
                       .As<IRepository<PTR_Evaluation>>();
            builder.RegisterType<PTR_EvaluationService>()
                      .AsSelf()
                      .As<IService<PTR_Evaluation>>();

            builder.RegisterType<TM_PTR_Eva_ApproveStepRepository>()
                      .AsSelf()
                      .As<IRepository<TM_PTR_Eva_ApproveStep>>();
            builder.RegisterType<TM_PTR_Eva_ApproveStepService>()
                      .AsSelf()
                      .As<IService<TM_PTR_Eva_ApproveStep>>();

            builder.RegisterType<TM_KPIs_BaseRepository>()
                   .AsSelf()
                   .As<IRepository<TM_KPIs_Base>>();
            builder.RegisterType<TM_KPIs_BaseService>()
                      .AsSelf()
                      .As<IService<TM_KPIs_Base>>();

            builder.RegisterType<PTR_Evaluation_KPIsRepository>()
                 .AsSelf()
                 .As<IRepository<PTR_Evaluation_KPIs>>();
            builder.RegisterType<PTR_Evaluation_KPIsService>()
                      .AsSelf()
                      .As<IService<PTR_Evaluation_KPIs>>();

            builder.RegisterType<PTR_Evaluation_FileRepository>()
                       .AsSelf()
                       .As<IRepository<PTR_Evaluation_File>>();
            builder.RegisterType<PTR_Evaluation_FileService>()
                      .AsSelf()
                      .As<IService<PTR_Evaluation_File>>();

            builder.RegisterType<TM_PTR_Eva_StatusRepository>()
                       .AsSelf()
                       .As<IRepository<TM_PTR_Eva_Status>>();
            builder.RegisterType<TM_PTR_Eva_StatusService>()
                      .AsSelf()
                      .As<IService<TM_PTR_Eva_Status>>();

            builder.RegisterType<TM_Annual_RatingRepository>()
                   .AsSelf()
                   .As<IRepository<TM_Annual_Rating>>();
            builder.RegisterType<TM_Annual_RatingService>()
                      .AsSelf()
                      .As<IService<TM_Annual_Rating>>();

            builder.RegisterType<PTR_Evaluation_ApproveRepository>()
             .AsSelf()
             .As<IRepository<PTR_Evaluation_Approve>>();
            builder.RegisterType<PTR_Evaluation_ApproveService>()
                      .AsSelf()
                      .As<IService<PTR_Evaluation_Approve>>();

            builder.RegisterType<PTR_Evaluation_AnswerRepository>()
                      .AsSelf()
                      .As<IRepository<PTR_Evaluation_Answer>>();
            builder.RegisterType<PTR_Evaluation_AnswerService>()
                      .AsSelf()
                      .As<IService<PTR_Evaluation_Answer>>();

            builder.RegisterType<TM_Feedback_RatingRepository>()
                     .AsSelf()
                     .As<IRepository<TM_Feedback_Rating>>();
            builder.RegisterType<TM_Feedback_RatingService>()
                      .AsSelf()
                      .As<IService<TM_Feedback_Rating>>();

            builder.RegisterType<PTR_Feedback_EmpRepository>()
                   .AsSelf()
                   .As<IRepository<PTR_Feedback_Emp>>();
            builder.RegisterType<PTR_Feedback_EmpService>()
                      .AsSelf()
                      .As<IService<PTR_Feedback_Emp>>();

            builder.RegisterType<PTR_Feedback_UnitGroupRepository>()
                .AsSelf()
                .As<IRepository<PTR_Feedback_UnitGroup>>();
            builder.RegisterType<PTR_Feedback_UnitGroupService>()
                      .AsSelf()
                      .As<IService<PTR_Feedback_UnitGroup>>();

            builder.RegisterType<TM_PTR_E_Mail_HistoryRepository>()
                          .AsSelf()
                          .As<IRepository<TM_PTR_E_Mail_History>>();
            builder.RegisterType<TM_PTR_E_Mail_HistoryService>()
                      .AsSelf()
                      .As<IService<TM_PTR_E_Mail_History>>();

            builder.RegisterType<PTR_Evaluation_AuthorizedRepository>()
                       .AsSelf()
                       .As<IRepository<PTR_Evaluation_Authorized>>();
            builder.RegisterType<PTR_Evaluation_AuthorizedService>()
                      .AsSelf()
                      .As<IService<PTR_Evaluation_Authorized>>();

            builder.RegisterType<PTR_Evaluation_AuthorizedEvaRepository>()
                   .AsSelf()
                   .As<IRepository<PTR_Evaluation_AuthorizedEva>>();
            builder.RegisterType<PTR_Evaluation_AuthorizedEvaService>()
                      .AsSelf()
                      .As<IService<PTR_Evaluation_AuthorizedEva>>();

            builder.RegisterType<PTR_Evaluation_IncidentsRepository>()
       .AsSelf()
       .As<IRepository<PTR_Evaluation_Incidents>>();
            builder.RegisterType<PTR_Evaluation_IncidentsService>()
                      .AsSelf()
                      .As<IService<PTR_Evaluation_Incidents>>();

            builder.RegisterType<PTR_Evaluation_Incidents_ScoreRepository>()
                        .AsSelf()
                        .As<IRepository<PTR_Evaluation_Incidents_Score>>();
            builder.RegisterType<PTR_Evaluation_Incidents_ScoreService>()
                      .AsSelf()
                      .As<IService<PTR_Evaluation_Incidents_Score>>();


            #endregion

            #region builder Nomination Form

            builder.RegisterType<PES_Nomination_YearRepository>()
                 .AsSelf()
                 .As<IRepository<PES_Nomination_Year>>();
            builder.RegisterType<PES_Nomination_YearService>()
                      .AsSelf()
                      .As<IService<PES_Nomination_Year>>();

            builder.RegisterType<TM_PES_NMN_StatusRepository>()
                      .AsSelf()
                      .As<IRepository<TM_PES_NMN_Status>>();
            builder.RegisterType<TM_PES_NMN_StatusService>()
                      .AsSelf()
                      .As<IService<TM_PES_NMN_Status>>();

            builder.RegisterType<TM_PES_NMN_SignatureStepRepository>()
                      .AsSelf()
                      .As<IRepository<TM_PES_NMN_SignatureStep>>();
            builder.RegisterType<TM_PES_NMN_SignatureStepService>()
                      .AsSelf()
                      .As<IService<TM_PES_NMN_SignatureStep>>();

            builder.RegisterType<PES_NominationRepository>()
                       .AsSelf()
                       .As<IRepository<PES_Nomination>>();
            builder.RegisterType<PES_NominationService>()
                      .AsSelf()
                      .As<IService<PES_Nomination>>();

            builder.RegisterType<PES_Nomination_SignaturesRepository>()
                   .AsSelf()
                   .As<IRepository<PES_Nomination_Signatures>>();
            builder.RegisterType<PES_Nomination_SignaturesService>()
                      .AsSelf()
                      .As<IService<PES_Nomination_Signatures>>();


            builder.RegisterType<TM_PES_NMN_CompetenciesRepository>()
                   .AsSelf()
                   .As<IRepository<TM_PES_NMN_Competencies>>();
            builder.RegisterType<TM_PES_NMN_CompetenciesService>()
                      .AsSelf()
                      .As<IService<TM_PES_NMN_Competencies>>();

            builder.RegisterType<PES_Nomination_CompetenciesRepository>()
                       .AsSelf()
                       .As<IRepository<PES_Nomination_Competencies>>();
            builder.RegisterType<PES_Nomination_CompetenciesService>()
                      .AsSelf()
                      .As<IService<PES_Nomination_Competencies>>();

            builder.RegisterType<PES_Final_RatingRepository>()
                    .AsSelf()
                    .As<IRepository<PES_Final_Rating>>();
            builder.RegisterType<PES_Final_RatingService>()
                      .AsSelf()
                      .As<IService<PES_Final_Rating>>();

            builder.RegisterType<PES_Final_Rating_YearRepository>()
                       .AsSelf()
                       .As<IRepository<PES_Final_Rating_Year>>();
            builder.RegisterType<PES_Final_Rating_YearService>()
                      .AsSelf()
                      .As<IService<PES_Final_Rating_Year>>();

            builder.RegisterType<PES_Nomination_KPIsRepository>()
                      .AsSelf()
                      .As<IRepository<PES_Nomination_KPIs>>();
            builder.RegisterType<PES_Nomination_KPIsService>()
                      .AsSelf()
                      .As<IService<PES_Nomination_KPIs>>();

            builder.RegisterType<PES_Nomination_AnswerRepository>()
                    .AsSelf()
                    .As<IRepository<PES_Nomination_Answer>>();
            builder.RegisterType<PES_Nomination_AnswerService>()
                      .AsSelf()
                      .As<IService<PES_Nomination_Answer>>();


            builder.RegisterType<PES_Nomination_Default_CommitteeRepository>()
                    .AsSelf()
                    .As<IRepository<PES_Nomination_Default_Committee>>();
            builder.RegisterType<PES_Nomination_Default_CommitteeService>()
                      .AsSelf()
                      .As<IService<PES_Nomination_Default_Committee>>();

            builder.RegisterType<PES_Nomination_FilesRepository>()
                      .AsSelf()
                      .As<IRepository<PES_Nomination_Files>>();
            builder.RegisterType<PES_Nomination_FilesService>()
                      .AsSelf()
                      .As<IService<PES_Nomination_Files>>();

            builder.RegisterType<TM_PES_NMN_StatusRepository>()
                          .AsSelf()
                          .As<IRepository<TM_PES_NMN_Status>>();
            builder.RegisterType<TM_PES_NMN_StatusService>()
                      .AsSelf()
                      .As<IService<TM_PES_NMN_Status>>();
            #endregion

            #region builder ErrorLog 
            builder.RegisterType<LogPersonnelRequestRepository>()
                 .AsSelf()
                 .As<IRepository<LogPersonnelRequest>>();
            builder.RegisterType<LogPersonnelRequestService>()
                      .AsSelf()
                      .As<IService<LogPersonnelRequest>>();

            #endregion
            // DbContext
            #region Probation

            builder.RegisterType<TM_Probation_QuestionRepository>()
            .AsSelf()
            .As<IRepository<TM_Probation_Question>>();
            builder.RegisterType<TM_Probation_QuestionService>()
            .AsSelf()
            .As<IService<TM_Probation_Question>>();

            builder.RegisterType<TM_Probation_Group_QuestionRepository>()
            .AsSelf()
            .As<IRepository<TM_Probation_Group_Question>>();
            builder.RegisterType<TM_Probation_Group_QuestionService>()
            .AsSelf()
            .As<IService<TM_Probation_Group_Question>>();


            builder.RegisterType<Probation_FormRepository>()
           .AsSelf()
           .As<IRepository<Probation_Form>>();
            builder.RegisterType<Probation_FormService>()
            .AsSelf()
            .As<IService<Probation_Form>>();

            builder.RegisterType<Probation_DetailRepository>()
           .AsSelf()
           .As<IRepository<Probation_Details>>();
            builder.RegisterType<Probation_DetailService>()
            .AsSelf()
            .As<IService<Probation_Details>>();

            builder.RegisterType<Action_PlansRepository>()
           .AsSelf()
           .As<IRepository<Action_Plans>>();
            builder.RegisterType<Action_PlansService>()
            .AsSelf()
            .As<IService<Action_Plans>>();

            builder.RegisterType<Probation_With_OutRepository>()
           .AsSelf()
           .As<IRepository<Probation_With_Out>>();
            builder.RegisterType<Probation_With_OutService>()
            .AsSelf()
            .As<IService<Probation_With_Out>>();

            #endregion

            #region Feedback

            builder.RegisterType<FeedbackRepository>()
           .AsSelf()
           .As<IRepository<Feedback>>();
            builder.RegisterType<FeedbackService>()
                      .AsSelf()
                      .As<IService<Feedback>>();

            #endregion

            #region MiniHeart

            builder.RegisterType<MiniHeart_MainRepository>()
           .AsSelf()
           .As<IRepository<MiniHeart_Main>>();
            builder.RegisterType<MiniHeart_MainService>()
                      .AsSelf()
                      .As<IService<MiniHeart_Main>>();

            builder.RegisterType<MiniHeart_DetailRepository>()
          .AsSelf()
          .As<IRepository<MiniHeart_Detail>>();
            builder.RegisterType<MiniHeart_DetailService>()
                      .AsSelf()
                      .As<IService<MiniHeart_Detail>>();

            builder.RegisterType<TM_MiniHeart_PeroidRepository>()
                     .AsSelf()
                     .As<IRepository<TM_MiniHeart_Peroid>>();
            builder.RegisterType<TM_MiniHeart_PeroidService>()
                      .AsSelf()
                      .As<IService<TM_MiniHeart_Peroid>>();

            builder.RegisterType<TM_MiniHeart_Group_QuestionRepository>()
         .AsSelf()
         .As<IRepository<TM_MiniHeart_Group_Question>>();
            builder.RegisterType<TM_MiniHeart_Group_QuestionService>()
                      .AsSelf()
                      .As<IService<TM_MiniHeart_Group_Question>>();

            builder.RegisterType<TM_MiniHeart_QuestionRepository>()
         .AsSelf()
         .As<IRepository<TM_MiniHeart_Question>>();
            builder.RegisterType<TM_MiniHeart_QuestionService>()
                      .AsSelf()
                      .As<IService<TM_MiniHeart_Question>>();

            #endregion

            #region MiniHeart2021

            builder.RegisterType<MiniHeart_Main2021Repository>()
           .AsSelf()
           .As<IRepository<MiniHeart_Main2021>>();
            builder.RegisterType<MiniHeart_Main2021Service>()
                      .AsSelf()
                      .As<IService<MiniHeart_Main2021>>();

            builder.RegisterType<MiniHeart_Detail2021Repository>()
          .AsSelf()
          .As<IRepository<MiniHeart_Detail2021>>();
            builder.RegisterType<MiniHeart_Detail2021Service>()
                      .AsSelf()
                      .As<IService<MiniHeart_Detail2021>>();

            builder.RegisterType<TM_MiniHeart_Peroid2021Repository>()
                     .AsSelf()
                     .As<IRepository<TM_MiniHeart_Peroid2021>>();
            builder.RegisterType<TM_MiniHeart_Peroid2021Service>()
                      .AsSelf()
                      .As<IService<TM_MiniHeart_Peroid2021>>();

            builder.RegisterType<TM_MiniHeart_Group_Question2021Repository>()
         .AsSelf()
         .As<IRepository<TM_MiniHeart_Group_Question2021>>();
            builder.RegisterType<TM_MiniHeart_Group_Question2021Service>()
                      .AsSelf()
                      .As<IService<TM_MiniHeart_Group_Question2021>>();

            builder.RegisterType<TM_MiniHeart_Question2021Repository>()
         .AsSelf()
         .As<IRepository<TM_MiniHeart_Question2021>>();
            builder.RegisterType<TM_MiniHeart_Question2021Service>()
                      .AsSelf()
                      .As<IService<TM_MiniHeart_Question2021>>();

            #endregion

            #region PreInternAssessment

            #region PIntern Form non Mass
            builder.RegisterType<TM_PIntern_FormRepository>()
         .AsSelf()
         .As<IRepository<TM_PIntern_Form>>();
            builder.RegisterType<TM_PIntern_FormService>()
                      .AsSelf()
                      .As<IService<TM_PIntern_Form>>();
            #endregion

            #region PIntern Form Mass
            builder.RegisterType<TM_PIntern_Mass_Form_QuestionRepository>()
         .AsSelf()
         .As<IRepository<TM_PIntern_Mass_Form_Question>>();
            builder.RegisterType<TM_PIntern_Mass_Form_QuestionService>()
                      .AsSelf()
                      .As<IService<TM_PIntern_Mass_Form_Question>>();
            #endregion



            #region PIntern Rating
            builder.RegisterType<TM_PIntern_RatingRepository>()
         .AsSelf()
         .As<IRepository<TM_PIntern_Rating>>();
            builder.RegisterType<TM_PIntern_RatingService>()
                      .AsSelf()
                      .As<IService<TM_PIntern_Rating>>();
            #endregion

            #region PIntern Status 
            builder.RegisterType<TM_PIntern_StatusRepository>()
         .AsSelf()
         .As<IRepository<TM_PIntern_Status>>();
            builder.RegisterType<TM_PIntern_StatusService>()
                      .AsSelf()
                      .As<IService<TM_PIntern_Status>>();
            #endregion

            #region PIntern Activities
            builder.RegisterType<TM_PreInternAssessment_ActivitiesRepository>()
         .AsSelf()
         .As<IRepository<TM_PInternAssessment_Activities>>();
            builder.RegisterType<TM_PInternAssessment_ActivitiesService>()
                      .AsSelf()
                      .As<IService<TM_PInternAssessment_Activities>>();
            #endregion

            #region PIntern Ratingform
            builder.RegisterType<TM_PIntern_RatingFormRepository>()
         .AsSelf()
         .As<IRepository<TM_PIntern_RatingForm>>();
            builder.RegisterType<TM_PIntern_RatingFormService>()
                      .AsSelf()
                      .As<IService<TM_PIntern_RatingForm>>();
            #endregion

            #region Candidate PIntern
            builder.RegisterType<TM_Candidate_PIntern_AnswerRepository>()
        .AsSelf()
        .As<IRepository<TM_Candidate_PIntern_Answer>>();
            builder.RegisterType<TM_Candidate_PIntern_AnswerService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_PIntern_Answer>>();

            builder.RegisterType<TM_Candidate_PIntern_ApprovRepository>()
      .AsSelf()
      .As<IRepository<TM_Candidate_PIntern_Approv>>();
            builder.RegisterType<TM_Candidate_PIntern_ApprovService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_PIntern_Approv>>();

            builder.RegisterType<TM_Candidate_PInternRepository>()
  .AsSelf()
  .As<IRepository<TM_Candidate_PIntern>>();
            builder.RegisterType<TM_Candidate_PInternService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_PIntern>>();

            #endregion

            #region Candidate PIntern Mass
            builder.RegisterType<TM_Candidate_PIntern_Mass_AnswerRepository>()
        .AsSelf()
        .As<IRepository<TM_Candidate_PIntern_Mass_Answer>>();
            builder.RegisterType<TM_Candidate_PIntern_Mass_AnswerService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_PIntern_Mass_Answer>>();

            builder.RegisterType<TM_Candidate_PIntern_Mass_ApprovRepository>()
      .AsSelf()
      .As<IRepository<TM_Candidate_PIntern_Mass_Approv>>();
            builder.RegisterType<TM_Candidate_PIntern_Mass_ApprovService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_PIntern_Mass_Approv>>();

            builder.RegisterType<TM_Candidate_PIntern_MassRepository>()
  .AsSelf()
  .As<IRepository<TM_Candidate_PIntern_Mass>>();
            builder.RegisterType<TM_Candidate_PIntern_MassService>()
                      .AsSelf()
                      .As<IService<TM_Candidate_PIntern_Mass>>();

            #endregion

            #endregion

            // Builder
            builder.RegisterType<TM_TIF_FormRepository>()
                      .AsSelf()
                      .As<IRepository<TM_TIF_Form>>();
            builder.RegisterType<TM_TIF_FormService>()
                      .AsSelf()
                      .As<IService<TM_TIF_Form>>();
            builder.RegisterType<TM_PR_Candidate_MappingRepository>()
                       .AsSelf()
                       .As<IRepository<TM_PR_Candidate_Mapping>>();
            builder.RegisterType<TM_PR_Candidate_MappingService>()
                      .AsSelf()
                      .As<IService<TM_PR_Candidate_Mapping>>();

            builder.RegisterType<TM_Evaluation_FormRepository>()
                           .AsSelf()
                           .As<IRepository<TM_Evaluation_Form>>();
            builder.RegisterType<TM_Evaluation_FormService>()
                      .AsSelf()
                      .As<IService<TM_Evaluation_Form>>();

            builder.RegisterType<TM_Eva_RatingRepository>()
                    .AsSelf()
                    .As<IRepository<TM_Eva_Rating>>();
            builder.RegisterType<TM_Eva_RatingService>()
                      .AsSelf()
                      .As<IService<TM_Eva_Rating>>();

            builder.RegisterType<TM_Trainee_Eva_AnswerRepository>()
                        .AsSelf()
                        .As<IRepository<TM_Trainee_Eva_Answer>>();
            builder.RegisterType<TM_Trainee_Eva_AnswerService>()
                      .AsSelf()
                      .As<IService<TM_Trainee_Eva_Answer>>();


            builder.RegisterType<TM_Trainee_EvaRepository>()
                        .AsSelf()
                        .As<IRepository<TM_Trainee_Eva>>();
            builder.RegisterType<TM_Trainee_EvaService>()
                      .AsSelf()
                      .As<IService<TM_Trainee_Eva>>();


            builder.RegisterType<TM_CandidatesRepository>()
                      .AsSelf()
                      .As<IRepository<TM_Candidates>>();
            builder.RegisterType<TM_CandidatesService>()
                      .AsSelf()
                      .As<IService<TM_Candidates>>();
            builder.RegisterType<TM_TraineeEva_StatusRepository>()
                  .AsSelf()
                  .As<IRepository<TM_TraineeEva_Status>>();
            builder.RegisterType<TM_TraineeEva_StatusService>()
                      .AsSelf()
                      .As<IService<TM_TraineeEva_Status>>();

            builder.RegisterType<MailContentRepository>()
                .AsSelf()
                .As<IRepository<MailContent>>();
            builder.RegisterType<MailContentService>()
                      .AsSelf()
                      .As<IService<MailContent>>();

            builder.RegisterType<TimeSheet_FormRepository>()
               .AsSelf()
               .As<IRepository<TimeSheet_Form>>();
            builder.RegisterType<TimeSheet_FormService>()
                      .AsSelf()
                      .As<IService<TimeSheet_Form>>();


            builder.RegisterType<TimeSheet_DetailRepository>()
              .AsSelf()
              .As<IRepository<TimeSheet_Detail>>();
            builder.RegisterType<TimeSheet_DetailService>()
                      .AsSelf()
                      .As<IService<TimeSheet_Detail>>();

            builder.RegisterType<TM_Time_TypeRepository>()
             .AsSelf()
             .As<IRepository<TM_Time_Type>>();
            builder.RegisterType<TM_Time_TypeService>()
                      .AsSelf()
                      .As<IService<TM_Time_Type>>();

            builder.RegisterType<Perdiem_TransportRepository>()
                        .AsSelf()
                        .As<IRepository<Perdiem_Transport>>();
            builder.RegisterType<Perdiem_TransportService>()
                      .AsSelf()
                      .As<IService<Perdiem_Transport>>();


            builder.RegisterType<TM_FY_PlanRepository>()
            .AsSelf()
            .As<IRepository<TM_FY_Plan>>();
            builder.RegisterType<TM_FY_PlanService>()
            .AsSelf()
            .As<IService<TM_FY_Plan>>();

            builder.RegisterType<TM_FY_DetailRepository>()
            .AsSelf()
            .As<IRepository<TM_FY_Detail>>();
            builder.RegisterType<TM_FY_DetailService>()
            .AsSelf()
            .As<IService<TM_FY_Detail>>();



            builder.RegisterType<TM_Company_TraineeRepository>()
                        .AsSelf()
                        .As<IRepository<TM_Company_Trainee>>();
            builder.RegisterType<TM_Company_TraineeService>()
                      .AsSelf()
                      .As<IService<TM_Company_Trainee>>();

            builder.RegisterType<StoreDb>()
                   .As<DbContext>().InstancePerLifetimeScope();

            #region Visa_Expiry
            builder.RegisterType<TM_EmployeeRepository>()
           .AsSelf()
           .As<IRepository<TM_Employee>>();
            builder.RegisterType<TM_EmployeeService>()
                   .AsSelf()
                   .As<IService<TM_Employee>>();

            builder.RegisterType<TM_Document_EmployeeRepository>()
          .AsSelf()
          .As<IRepository<TM_Document_Employee>>();
            builder.RegisterType<TM_Document_EmployeeService>()
                   .AsSelf()
                   .As<IService<TM_Document_Employee>>();

            builder.RegisterType<TM_Type_DocumentRepository>()
        .AsSelf()
        .As<IRepository<TM_Type_Document>>();
            builder.RegisterType<TM_Type_DocumentService>()
                   .AsSelf()
                   .As<IService<TM_Type_Document>>();

            builder.RegisterType<TM_Prefix_VisaRepository>()
           .AsSelf()
           .As<IRepository<TM_Prefix_Visa>>();
            builder.RegisterType<TM_Prefix_VisaService>()
            .AsSelf()
            .As<IService<TM_Prefix_Visa>>();

            builder.RegisterType<TM_Company_VisaRepository>()
                 .AsSelf()
                 .As<IRepository<TM_Company_Visa>>();
            builder.RegisterType<TM_Company_VisaService>()
            .AsSelf()
            .As<IService<TM_Company_Visa>>();




            builder.RegisterType<TM_EmployeeForeign_VisaRepository>()
          .AsSelf()
          .As<IRepository<TM_EmployeeForeign_Visa>>();
            builder.RegisterType<TM_EmployeeForeign_VisaService>()
            .AsSelf()
            .As<IService<TM_EmployeeForeign_Visa>>();

            builder.RegisterType<tblMstAutoMailsRepository>()
    .AsSelf()
    .As<IRepository<tblMstAutoMails>>();
            builder.RegisterType<tblMstAutoMailsService>()
            .AsSelf()
            .As<IService<tblMstAutoMails>>();

            builder.RegisterType<TM_Remark_VisaRepository>()
 .AsSelf()
 .As<IRepository<TM_Remark_Visa>>();
            builder.RegisterType<TM_Remark_VisaService>()
            .AsSelf()
            .As<IService<TM_Remark_Visa>>();
            #endregion

            #region eGreetings

            builder.RegisterType<eGreetings_MainRepository>()
           .AsSelf()
           .As<IRepository<eGreetings_Main>>();
            builder.RegisterType<eGreetings_MainService>()
                      .AsSelf()
                      .As<IService<eGreetings_Main>>();

            builder.RegisterType<eGreetings_DetailRepository>()
          .AsSelf()
          .As<IRepository<eGreetings_Detail>>();
            builder.RegisterType<eGreetings_DetailService>()
                      .AsSelf()
                      .As<IService<eGreetings_Detail>>();

            builder.RegisterType<TM_eGreetings_PeroidRepository>()
                     .AsSelf()
                     .As<IRepository<TM_eGreetings_Peroid>>();
            builder.RegisterType<TM_eGreetings_PeroidService>()
                      .AsSelf()
                      .As<IService<TM_eGreetings_Peroid>>();

            builder.RegisterType<TM_eGreetings_Group_QuestionRepository>()
         .AsSelf()
         .As<IRepository<TM_eGreetings_Group_Question>>();
            builder.RegisterType<TM_eGreetings_Group_QuestionService>()
                      .AsSelf()
                      .As<IService<TM_eGreetings_Group_Question>>();

            builder.RegisterType<TM_eGreetings_QuestionRepository>()
         .AsSelf()
         .As<IRepository<TM_eGreetings_Question>>();
            builder.RegisterType<TM_eGreetings_QuestionService>()
                      .AsSelf()
                      .As<IService<TM_eGreetings_Question>>();

            #endregion 
            
            #region Consent

            builder.RegisterType<Consent_AsnwerRepository>()
           .AsSelf()
           .As<IRepository<Consent_Asnwer>>();
            builder.RegisterType<Consent_AsnwerService>()
                      .AsSelf()
                      .As<IService<Consent_Asnwer>>();

            builder.RegisterType<Consent_Asnwer_LogRepository>()
          .AsSelf()
          .As<IRepository<Consent_Asnwer_Log>>();
            builder.RegisterType<Consent_Asnwer_LogService>()
                      .AsSelf()
                      .As<IService<Consent_Asnwer_Log>>();

            builder.RegisterType<Consent_Main_FormRepository>()
                     .AsSelf()
                     .As<IRepository<Consent_Main_Form>>();
            builder.RegisterType<Consent_Main_FormService>()
                      .AsSelf()
                      .As<IService<Consent_Main_Form>>();

            builder.RegisterType<Consent_SpacialRepository>()
         .AsSelf()
         .As<IRepository<Consent_Spacial>>();
            builder.RegisterType<Consent_SpacialService>()
                      .AsSelf()
                      .As<IService<Consent_Spacial>>();

            builder.RegisterType<TM_Consent_FormRepository>()
         .AsSelf()
         .As<IRepository<TM_Consent_Form>>();
            builder.RegisterType<TM_Consent_FormService>()
                      .AsSelf()
                      .As<IService<TM_Consent_Form>>();
            
            builder.RegisterType<TM_Consent_QuestionRepository>()
         .AsSelf()
         .As<IRepository<TM_Consent_Question>>();
            builder.RegisterType<TM_Consent_QuestionService>()
                      .AsSelf()
                      .As<IService<TM_Consent_Question>>();

            #endregion


            #region StrengthFinder
           
            builder.RegisterType<TM_StrengthFinderRepository>()
         .AsSelf()
         .As<IRepository<TM_StrenghtFinder>>();
            builder.RegisterType<StrengthFinderService>()
                     .AsSelf()
                     .As<IService<TM_StrenghtFinder>>();

            builder.RegisterType<TB_StrengthFinderHistoryRepository>()
     .AsSelf()
     .As<IRepository<TB_StrengthFinderHistory>>();
            #endregion

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}
