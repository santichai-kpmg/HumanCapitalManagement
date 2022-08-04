using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.PreTraineeAssessment;
using HumanCapitalManagement.Service.PreTraineeAssessment;
using HumanCapitalManagement.ViewModel.PreTraineeAssessmentVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.PreTrainee
{
    public class PreTrainee_MasterDataController : BaseController
    {
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        private TM_PreTraineeAssessment_QuestionService _TM_PreTraineeAssessment_QuestionService;
        private TM_PreTraineeAssessment_ActivitiesService _TM_PreTraineeAssessment_ActivitiesService;
        private TM_PreTraineeAssessment_Group_QuestionService _TM_PreTraineeAssessment_Group_QuestionService;
        private TM_PreTraineeAssessment_GroupService _TM_PreTraineeAssessment_GroupService;

        New_HRISEntities dbHr = new New_HRISEntities();

        public PreTrainee_MasterDataController(
           TM_PreTraineeAssessment_QuestionService TM_PreTraineeAssessment_QuestionService,
           TM_PreTraineeAssessment_ActivitiesService TM_PreTraineeAssessment_ActivitiesService,
           TM_PreTraineeAssessment_Group_QuestionService TM_PreTraineeAssessment_Group_QuestionService,
          TM_PreTraineeAssessment_GroupService TM_PreTraineeAssessment_GroupService)
        {
            _TM_PreTraineeAssessment_QuestionService = TM_PreTraineeAssessment_QuestionService;
            _TM_PreTraineeAssessment_ActivitiesService = TM_PreTraineeAssessment_ActivitiesService;
            _TM_PreTraineeAssessment_Group_QuestionService = TM_PreTraineeAssessment_Group_QuestionService;
            _TM_PreTraineeAssessment_GroupService = TM_PreTraineeAssessment_GroupService;
        }

        #region Question
        public ActionResult QuestionMasterList()
        {

            vTM_PreTraineeAssessment_Group result = new vTM_PreTraineeAssessment_Group();
            return View(result);
        }
        [HttpPost]
        public JsonResult LoadQuestionMasterList()
        {
            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}

            vTM_PreTraineeAssessment_Group result = new vTM_PreTraineeAssessment_Group();
            var _getData = _TM_PreTraineeAssessment_GroupService.GetDataAllActive();
            string BackUrl = "";

            result.lstData = _getData.Select(s => new vTM_PreTraineeAssessment_Group_obj()
            {
                IdEncrypt = HCMFunc.Encrypt(s.Id + ""),
                Name = s.Name,
                View = @"<button id=""btnEdit""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(s.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                Active_Status = s.Active_Status == "Y" ? "Active" : "Inactive",
                Create_Date = s.Create_Date != null ? s.Create_Date.Value.ToString("dd MMM yyyy HH: mm") : DateTime.Now.ToString("dd MMM yyyy HH: mm"),
                Create_User = s.Create_User != null ? s.Create_User : s.Create_User,
                Update_Date = s.Update_Date.HasValue != null ? DateTime.Now.ToString("dd MMM yyyy HH: mm") : DateTime.Now.ToString("dd MMM yyyy HH: mm"),
                Update_User = s.Update_User != null ? s.Create_User : s.Update_User,
                //Seq = s.Seq,

            }).ToList();

             return Json(new { result });

        }
        #endregion


        #region Group_QuestionMaster
        public ActionResult Group_QuestionMasterList()
        {

            vTM_PreTraineeAssessment_Group result = new vTM_PreTraineeAssessment_Group();

            return View(result);
        }

        public ActionResult Group_QuestionMasterMange()
        {

            //vTM_PreTraineeAssessment_Group result = new vTM_PreTraineeAssessment_Group();

            return View();
        }
        #endregion

        #region Group Question
        [HttpPost]
        public JsonResult LoadGroup_QuestionMasterList()
        {
            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}

            vTM_PreTraineeAssessment_Question result = new vTM_PreTraineeAssessment_Question();
            var _getData = _TM_PreTraineeAssessment_Group_QuestionService.GetDataAllActive();
            string BackUrl = "";

            result.lstDataMaster = _getData.Select(s => new vTM_PreTraineeAssessment_Question_save()
            {
                Id = HCMFunc.Encrypt(s.Id + ""),
                Content = s.Name ,
                View = @"<button id=""btnEdit""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(s.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                Active_Status = s.Active_Status == "Y" ? "Active" : "Inactive",
                Create_Date = s.Create_Date != null ? s.Create_Date.Value.ToString("dd MMM yyyy HH: mm") : DateTime.Now.ToString("dd MMM yyyy HH: mm"),
                Create_User = s.Create_User != null ? CGlobal.UserInfo.UserId : s.Create_User,
                Update_Date = s.Update_Date.HasValue != null ? DateTime.Now.ToString("dd MMM yyyy HH: mm") : DateTime.Now.ToString("dd MMM yyyy HH: mm"),
                Update_User = s.Update_User != null ? s.Create_User : s.Update_User,
                Seq = s.Seq,

            }).OrderBy(s => Convert.ToInt32(s.Seq)).ToList();

            return Json(new { result });

        }

        

        #endregion


        #region ActivitesMaster
        #region ViewActivity
        public ActionResult Activites_MasterList()
        {

            vTM_PreTraineeAssessment_Activities result = new vTM_PreTraineeAssessment_Activities();
            result.lstsave = new vTM_Activities_save();

            return View(result);
        }
        #endregion

        #region LoadDataActivitesMaster
        [HttpPost]
        public ActionResult LoadActivites_MasterList()
        {
            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}

            vTM_PreTraineeAssessment_Activities result = new vTM_PreTraineeAssessment_Activities();

            var _getData = _TM_PreTraineeAssessment_ActivitiesService.GetDataAllActive();
            //string BackUrl = "";

            result.lstDataMaster = _getData.Select(s => new vTM_Activities_save()
            {
                IdEncrypt = HCMFunc.Encrypt(s.Id + ""),
                View = @"<button id=""btnEdit""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(s.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                active_status = s.active_status != null ? s.active_status == "Y" ? "Active" : "Inactive" : null,
                Activities_name_en = s.Activities_name_en == null ? "" : s.Activities_name_en,
                create_date = s.create_date.HasValue ? s.create_date.Value.ToString("dd MMM yyyy HH: mm") : DateTime.Now.ToString("dd MMM yyyy HH: mm"),
                create_user = s.create_user != null ? CGlobal.UserInfo.UserId : s.create_user,
                update_date = s.update_date.HasValue ? s.update_date.Value.ToString("dd MMM yyyy HH: mm") : DateTime.Now.ToString("dd MMM yyyy HH: mm"),
                update_user = s.update_user != null ? s.create_user : s.update_user,
                Seq = s.Seq,

            }).OrderBy(s=>Convert.ToInt32(s.Seq)).ToList();


            result.lstsave = new vTM_Activities_save();
            result.Status = SystemFunction.process_Success;

            return Json(new { result });

        }
        #endregion

        #region AddActivitesMaster
        [HttpPost]
        public ActionResult AddActivitesMasterList(vTM_Activities_save ItemSave)
        {
            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}

            vTM_PreTraineeAssessment_Activities result = new vTM_PreTraineeAssessment_Activities();
            var _getData = _TM_PreTraineeAssessment_ActivitiesService.GetDataAll();
            var _getDesc = _TM_PreTraineeAssessment_ActivitiesService.GetDataAll().OrderByDescending(s => s.Seq).Select(s=> Convert.ToInt32(s.Seq)).Max()+1;
           
            TM_PreTraineeAssessment_Activities objsave = new TM_PreTraineeAssessment_Activities();

            #region CheckInput
            if (objsave.Activities_name_en != null || objsave.Activities_name_en != String.Empty ||
                objsave.active_status != null || objsave.active_status != String.Empty)
            {
                #region SetData
                objsave.Activities_name_en = ItemSave.Activities_name_en;
                objsave.Activities_name_th = ItemSave.Activities_name_en;
                objsave.active_status = ItemSave.active_status;
                objsave.create_date = DateTime.Now;
                objsave.update_date = DateTime.Now;
                objsave.create_user = CGlobal.UserInfo.UserId;
                objsave.update_user = CGlobal.UserInfo.UserId;
                objsave.Seq = _getDesc.ToString();
                
                #endregion

                var _getstatus = _TM_PreTraineeAssessment_ActivitiesService.CreateNewOrUpdate(objsave);

                if (_getstatus != 0)
                {
                    result.Status = SystemFunction.process_Success;

                }
                else
                {
                    result.Status = SystemFunction.process_Failed;

                }

            }
            else
            {
                result.Status = SystemFunction.process_Failed;
            }
            #endregion

            return Json(new { result });
        }
        #endregion
        
        #region EditActivitesMaster
        [HttpPost]
        public JsonResult EditActivites_Master(string Id)
        {
            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}

            vTM_PreTraineeAssessment_Activities result = new vTM_PreTraineeAssessment_Activities();
            int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(Id));
            var _getData = _TM_PreTraineeAssessment_ActivitiesService.FindById(nId);
            result.lstsave = new vTM_Activities_save();

            if (_getData != null)
            {
                result.lstsave.IdEncrypt = HCMFunc.Encrypt(_getData.Id + "");
                result.lstsave.Activities_name_en = _getData.Activities_name_en;
                result.lstsave.active_status = _getData.active_status == "Y" ? "Active" : "Inactive";

            }
            else
            {
                result.Status = SystemFunction.process_Failed;

            }

            result.Status = SystemFunction.process_Success;

            return Json(new { result });

        }
        #endregion

        #region UpdateActivitesMaster

        [HttpPost]
        public ActionResult UpdateActivitesMasterList(vTM_Activities_save ItemSave){
            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}
            //string BackUrl = "";

            vTM_PreTraineeAssessment_Activities result = new vTM_PreTraineeAssessment_Activities();
            TM_PreTraineeAssessment_Activities objsave = new TM_PreTraineeAssessment_Activities();
            objsave.Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemSave.IdEncrypt));
            objsave.Activities_name_en = ItemSave.Activities_name_en;
            objsave.active_status = ItemSave.active_status;
            objsave.update_date = DateTime.Now;
            objsave.update_user = CGlobal.UserInfo.UserId;

            var _getstatus = _TM_PreTraineeAssessment_ActivitiesService.CreateNewOrUpdate(objsave);

            if (_getstatus != 0)
            {
                result.Status = SystemFunction.process_Success;

            }
            else
            {
                result.Status = SystemFunction.process_Failed;

            }


            return Json(new { result });

        }
        #endregion
        #endregion


    }
}