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
    public class PoolRankController : BaseController
    {
        private TM_Pool_RankService _TM_Pool_RankService;
        private PoolService _PoolService;
        private RankService _RankService;
        public PoolRankController(TM_Pool_RankService TM_Pool_RankService, PoolService PoolService, RankService RankService)
        {
            _TM_Pool_RankService = TM_Pool_RankService;
            _PoolService = PoolService;
            _RankService = RankService;
        }
        // GET: PoolRank
        public ActionResult PoolRankList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPoolRank result = new vPoolRank();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPoolRank SearchItem = (CSearchPoolRank)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPoolRank)));
                int nPool_Id = SystemFunction.GetIntNullToZero(SearchItem.pool_id);
                var lstData = _TM_Pool_RankService.GetPool_Rank(
               SearchItem.name,
           SearchItem.active_status, nPool_Id);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vPoolRank_obj
                                      {
                                          name_en = lstAD.Pool_rank_name_en.StringRemark(500),
                                          pool_name = lstAD.TM_Pool.Pool_short_name_en,
                                          priority = lstAD.TM_Rank.piority + "",
                                          short_name_en = lstAD.Pool_rank_short_name_en + "",
                                          rank = lstAD.TM_Rank.rank_name_en + "",
                                          //  short_name_en = lstAD.Company_short_name_en,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          description = lstAD.Pool_rank_description.StringRemark(),
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

        public ActionResult PoolRankCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPoolRank_obj_save result = new vPoolRank_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            return View(result);
        }

        public ActionResult PoolRankEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vPoolRank_obj_save result = new vPoolRank_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Pool_RankService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.name_en = _getData.Pool_rank_name_en + "";
                        result.description = _getData.Pool_rank_description + "";
                        result.active_status = _getData.active_status;
                        result.short_name_en = _getData.Pool_rank_short_name_en;
                        result.pool_id = _getData.TM_Pool.Id + "";
                        result.rank_id = _getData.TM_Rank.Id + "";

                    }
                }
            }
            return View(result);

            #endregion

        }


        #region Ajax Function
        [HttpPost]
        public ActionResult LoadPoolRankList(CSearchPoolRank SearchItem)
        {
            vPoolRank_Return result = new vPoolRank_Return();
            List<vPoolRank_obj> lstData_resutl = new List<vPoolRank_obj>();
            int nPool_Id = SystemFunction.GetIntNullToZero(SearchItem.pool_id);
            var lstData = _TM_Pool_RankService.GetPool_Rank(
            SearchItem.name,
            SearchItem.active_status, nPool_Id);
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
                                  select new vPoolRank_obj
                                  {
                                      name_en = lstAD.Pool_rank_name_en.StringRemark(500),
                                      pool_name = lstAD.TM_Pool.Pool_short_name_en,
                                      priority = lstAD.TM_Rank.piority + "",
                                      short_name_en = lstAD.Pool_rank_short_name_en + "",
                                      rank = lstAD.TM_Rank.rank_name_en + "",
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      description = lstAD.Pool_rank_description.StringRemark(),
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
        public ActionResult CreatePoolRank(vPoolRank_obj_save ItemData)
        {
            vPoolRank_Return result = new vPoolRank_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.name_en))
                {
                    int nPoolID, nRankID;
                    nPoolID = SystemFunction.GetIntNullToZero(ItemData.pool_id + "");
                    nRankID = SystemFunction.GetIntNullToZero(ItemData.rank_id + "");
                    var GetPool = _PoolService.Find(nPoolID);
                    var GetRank = _RankService.Find(nRankID);
                    if (GetPool != null)
                    {
                        if (GetRank != null)
                        {
                            TM_Pool_Rank objSave = new TM_Pool_Rank()
                            {
                                Id = 0,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                active_status = "Y",
                                Pool_rank_name_en = (ItemData.name_en + "").Trim(),
                                Pool_rank_short_name_en = (ItemData.short_name_en + "").Trim(),
                                Pool_rank_description = ItemData.description,
                                TM_Pool = GetPool,
                                TM_Rank = GetRank,

                            };
                            if (_TM_Pool_RankService.CanSave(objSave))
                            {
                                var sComplect = _TM_Pool_RankService.CreateNew(objSave);
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
                                result.Msg = "Duplicate Rank name.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter Rank.";

                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, please enter Pool.";

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
        public ActionResult EditPoolRank(vPoolRank_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPoolRank_Return result = new vPoolRank_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Pool_RankService.Find(nId);
                    if (_getData != null)
                    {
                        int nPoolID, nRankID;
                        nPoolID = SystemFunction.GetIntNullToZero(ItemData.pool_id + "");
                        nRankID = SystemFunction.GetIntNullToZero(ItemData.rank_id + "");
                        var GetPool = _PoolService.Find(nPoolID);
                        var GetRank = _RankService.Find(nRankID);
                        if (!string.IsNullOrEmpty(ItemData.name_en))
                        {


                            if (GetPool != null)
                            {
                                if (GetRank != null)
                                {
                                    _getData.update_user = CGlobal.UserInfo.UserId;
                                    _getData.update_date = dNow;
                                    _getData.active_status = ItemData.active_status;
                                    _getData.Pool_rank_name_en = ItemData.name_en;
                                    _getData.Pool_rank_short_name_en = ItemData.short_name_en;
                                    _getData.Pool_rank_description = ItemData.description;
                                    //_getData.TM_Pool = GetPool;
                                    _getData.TM_Rank = GetRank;
                                    if (_TM_Pool_RankService.CanSave(_getData))
                                    {
                                        var sComplect = _TM_Pool_RankService.Update(_getData);
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
                                    result.Msg = "Error, please enter Rank.";

                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please enter Pool.";

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