using Autofac;
using Autofac.Integration.Mvc;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Common;
using HumanCapitalManagement.Data.MainRepository;
using HumanCapitalManagement.Data.TIFForm;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.TIFForm;
using HumanCapitalManagement.Models.TraineeSite;
using HumanCapitalManagement.Service;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.Service.TraineeSite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TraineeManagement
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
        }
        private void SetupAutofac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

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

            // DbContext
            builder.RegisterType<StoreDb>()
                   .As<DbContext>().InstancePerLifetimeScope();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}
