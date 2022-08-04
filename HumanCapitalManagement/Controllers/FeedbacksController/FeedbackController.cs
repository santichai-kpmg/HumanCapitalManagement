using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models._360Feedback;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel.Feedbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.FeedbacksController
{
    public class FeedbackController : BaseController
    {
        //private FeedbackService _FeedbackService;
        //public FeedbacksController(FeedbackService feedbackservice
        //       )
        //{ _FeedbackService = feedbackservice;
        //}

        // GET: Feedback
        public ActionResult Feedback()
        {
            vFeedback_obj_save result = new vFeedback_obj_save();

            return View(result);
        }

        public class image_obj
        {
            public string path { get; set; }
        }
        [HttpPost]
        public ActionResult getImageEmp(string empNo)
        {

            image_obj result = new image_obj();

            New_HRISEntities dbHr = new New_HRISEntities();
            var GetPic = dbHr.vw_StaffPhoto_FileName.Where(w => w.Employeeno == empNo).FirstOrDefault();
            try
            {
                //string sPicturePath = @"\\thbkkfsr05\data3$\PMPS\HR\Photo\Phoomchai_All\Phoomchai_Active\";
                string sPicturePath = System.Web.Configuration.WebConfigurationManager.AppSettings["PathPic"];// "/thbkkfsr05/data3$/PMPS/HR/Photo/Phoomchai_All/Phoomchai_Active/";
                var absolutePath = HttpContext.Server.MapPath("~/Image/noimgaaaa.png");
                if (GetPic != null)
                {
                    sPicturePath = (sPicturePath + GetPic.PhotoName + ".jpg").Replace("/", "\\");

                }

                result.path = sPicturePath;
            }
            catch (Exception ex)
            {
            }


            return Json(new { result });
        }

        [HttpPost]
        public ActionResult Save_Feedback(vFeedback_obj_save itemdata)
        {
            vFeedback_obj_save result = new vFeedback_obj_save();
            try
            {
                //var getall = _FeedbackService.GetDataForSelect();

                Feedback feedback = new Feedback();
                feedback.Positive = itemdata.Positive;


            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }
            if (true)
            {
                result.Status = SystemFunction.process_Success;
                result.Msg = "Success ";
            }


            return Json(new
            {
                result
            });
        }


    }
}