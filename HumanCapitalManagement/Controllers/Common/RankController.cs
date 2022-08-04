using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Common
{
    public class RankController : BaseController
    {
        private RankService _RankService;
        public RankController(RankService RankService)
        {
            _RankService = RankService;
        }
        // GET: Rank
        public ActionResult RankList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vRank result = new vRank();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchRank SearchItem = (CSearchRank)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchRank)));
                var lstData = _RankService.GetRank(
               SearchItem.name,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vRank_obj
                                      {
                                          name_en = lstAD.rank_name_en.StringRemark(500),
                                          short_name_en = lstAD.rank_short_name_en,
                                          need_ceo = lstAD.ceo_approve + "" == "Y" ? "Yes" : "No",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          description = lstAD.rank_description.StringRemark(),
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();

                }
            }

            #endregion
            return View(result);
        }

        public ActionResult RankCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vRank_obj_save result = new vRank_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            result.ceo_approve = "N";
            return View(result);
        }

        public ActionResult RankEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vRank_obj_save result = new vRank_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _RankService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.name_en = _getData.rank_name_en + "";
                        result.description = _getData.rank_description + "";
                        result.active_status = _getData.active_status;
                        result.ceo_approve = _getData.ceo_approve;
                        result.short_name_en = _getData.rank_short_name_en + "";
                        result.piority = _getData.piority + "";
                    }
                }
            }
            return View(result);

            #endregion

        }


        #region Ajax Function
        [HttpPost]
        public ActionResult LoadRankList(CSearchPool SearchItem)
        {
            vRank_Return result = new vRank_Return();
            List<vRank_obj> lstData_resutl = new List<vRank_obj>();
            var lstData = _RankService.GetRank(
            SearchItem.name,
            SearchItem.active_status);
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);
            if (lstData.Any())
            {

                lstData_resutl = (from lstAD in lstData
                                  select new vRank_obj
                                  {
                                      name_en = lstAD.rank_name_en.StringRemark(500),
                                      short_name_en = lstAD.rank_short_name_en,
                                      need_ceo = lstAD.ceo_approve + "" == "Y" ? "Yes" : "No",
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      description = lstAD.rank_description.StringRemark(),
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult CreateRank(vRank_obj_save ItemData)
        {
            vRank_Return result = new vRank_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.name_en))
                {
                    TM_Rank objSave = new TM_Rank()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        rank_name_en = (ItemData.name_en + "").Trim(),
                        rank_short_name_en = (ItemData.short_name_en + "").Trim(),
                        rank_description = ItemData.description,
                        ceo_approve = ItemData.ceo_approve + "",
                        piority = Convert.ToInt32(SystemFunction.GetNumberNullToZero(ItemData.piority)),

                    };
                    if (_RankService.CanSave(objSave))
                    {
                        var sComplect = _RankService.CreateNew(objSave);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please try again.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Duplicate;
                        result.Msg = "Duplicate Type name.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please enter name";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditRank(vRank_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vRank_Return result = new vRank_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _RankService.Find(nId);
                    if (_getData != null)
                    {
                        if (!string.IsNullOrEmpty(ItemData.name_en))
                        {

                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.active_status = ItemData.active_status;
                            _getData.rank_name_en = ItemData.name_en;
                            _getData.rank_short_name_en = ItemData.short_name_en;
                            _getData.rank_description = ItemData.description;
                            _getData.ceo_approve = ItemData.ceo_approve + "";
                            _getData.piority = Convert.ToInt32(SystemFunction.GetNumberNullToZero(ItemData.piority));
                            if (_RankService.CanSave(_getData))
                            {
                                var sComplect = _RankService.Update(_getData);
                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Duplicate;
                                result.Msg = "Duplicate Type name.";
                            }

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter name";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Request Type Not Found.";
                    }
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