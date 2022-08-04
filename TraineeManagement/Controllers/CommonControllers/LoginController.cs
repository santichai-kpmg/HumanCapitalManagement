using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using TraineeManagement.App_Start;
using TraineeManagement.ViewModels.CommonVM;

namespace TraineeManagement.Controllers.CommonControllers
{
    public class LoginController : BaseController
    {

        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        public LoginController(TM_CandidatesService TM_CandidatesService, MailContentService MailContentService)
        {
            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;
        }

        // GET: Login
        public ActionResult Login()
        {

            vLogin_obj result = new vLogin_obj();
            string winloginname = this.User.Identity.Name;
            string EmpUserID = winloginname.Substring(winloginname.IndexOf("\\") + 1);
            if (!string.IsNullOrEmpty(EmpUserID))
            {
                //try
                //{
                //    DataTable staff = wsHRis.getActiveStaffByUserID(EmpUserID);
                //    // DataTable staff = wsHRis.getActiveStaffByUserID("zjaffer");
                //    if (staff != null)
                //    {
                //        if (staff.Rows.Count > 0)
                //        {
                //            if (staff.Rows.Count == 1)
                //            {
                //                var Login = HCMFunc.funcLogin(staff.Rows[0].Field<string>("EmpNo") + "");
                //                if (!Login.sSucc)
                //                {
                //                    ViewBag.UserName = CGlobal.UserInfo.FullName;
                //                    ViewBag.IsAdmin = CGlobal.UserIsAdmin();
                //                    return RedirectToAction("Index", "Home");
                //                }
                //                else
                //                {
                //                    return RedirectToAction("ErrorNopermission", "MasterPage");
                //                }
                //            }
                //            else
                //            {
                //                List<vSelect_Lg> lstEmp = staff.AsEnumerable().Select(s => new vSelect_Lg
                //                {
                //                    id = staff.Columns.Contains("EmpNo") ? HCMFunc.Encrypt(s.Field<string>("EmpNo")) : "",
                //                    name = s.Field<string>("EmpNo") + " : " + s.Field<string>("EmpName") + "(" + s.Field<string>("UnitGroup") + ")",
                //                }).ToList();
                //                result.lstEmp.AddRange(lstEmp);
                //            }
                //        }
                //        else
                //        {
                //            return RedirectToAction("ErrorNopermission", "MasterPage");
                //        }
                //    }
                //}
                //catch (Exception e)
                //{
                //    return RedirectToAction("Error500", "MasterPage");

                //}

            }
            return View(result);
        }
        public ActionResult Error404()
        {

            return View();
        }

        #region ajax
        [HttpPost]
        public ActionResult ajLogin(vLogin_obj ItemData)
        {
            vLogin_Return result = new vLogin_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.user_name) && !string.IsNullOrEmpty(ItemData.user_pass))
                {
                    ItemData.user_name = (ItemData.user_name).ToLower();
                    string sPassword = ((ItemData.user_pass + "").Trim()).ToLower();//SHA.GenerateSHA256String(((ItemData.user_pass+"").Trim()).ToLower());
                    var _getCan = _TM_CandidatesService.LoginUser(ItemData.user_name);
                    if (_getCan != null)
                    {

                        if (SHA.IsPasswordValid(sPassword, _getCan.candidate_password))
                        {
                            User newUser = new User(_getCan.Id + "");
                            newUser.UserId = _getCan.Id + "";
                            newUser.FirstName = _getCan.first_name_en + "";
                            newUser.LastName = _getCan.last_name_en + "";
                            newUser.FullName = _getCan.first_name_en + " " + _getCan.last_name_en + "";
                            newUser.EMail = _getCan.trainee_email + "";
                            newUser.IsVerify = _getCan.is_verify + "";
                            newUser.nUserId = _getCan.Id;
                            Session["UserInfo"] = newUser;
                            result.Status = SystemFunction.process_Success;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Wrong password.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, User not found.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Please input Username & Password.";
                }
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult ChangePass(vLogin_password ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vLogin_Return result = new vLogin_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if ((ItemData.new_1 + "").Length < 8)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Please input New Password 1 more than 8 word.";
                return Json(new
                {
                    result
                });
            }
            if ((ItemData.new_2 + "").Length < 8)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Please input New Password 2 more than 8 word.";
                return Json(new
                {
                    result
                });
            }

            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId);
                if (nId != 0)
                {
                    var _getCandidate = _TM_CandidatesService.Find(nId);
                    if (_getCandidate != null)
                    {
                        if (SHA.IsPasswordValid((ItemData.old_pass + "").ToLower(), _getCandidate.candidate_password))
                        {
                            if (ItemData.new_1 == ItemData.new_2)
                            {
                                _getCandidate.candidate_password = SHA.GenerateSHA256String(ItemData.new_1.ToLower());
                                var sComplect = _TM_CandidatesService.Update(_getCandidate);
                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Please try again.";
                                    return Json(new
                                    {
                                        result
                                    });
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, New password don't match.";
                                return Json(new
                                {
                                    result
                                });
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Wrong password.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Trainee Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult VerifyTraineeEmail(vVerify_Trainee_Email ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vLogin_Return result = new vLogin_Return();

            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }


            if (string.IsNullOrEmpty(ItemData.email))
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Please input E-Mail.";
                return Json(new
                {
                    result
                });
            }
            else
            {
                if (IsValidEmail(ItemData.email))
                {
                    MailAddress address = new MailAddress(ItemData.email);
                    string host = address.Host;
                    if (host.ToLower() != "kpmg.co.th")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Please check your domain.";
                        return Json(new
                        {
                            result
                        });
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Please check your E-Mail.";
                    return Json(new
                    {
                        result
                    });
                }
            }

            //else if ((ItemData.verify_code + "").Length < 8)
            //{
            //    result.Status = SystemFunction.process_Failed;
            //    result.Msg = "Error, Please input Verify Code more than 8 word.";
            //    return Json(new
            //    {
            //        result
            //    });
            //}


            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId);
                if (nId != 0)
                {
                    var _getCandidate = _TM_CandidatesService.Find(nId);
                    if (_getCandidate != null)
                    {
                        _getCandidate.trainee_email = ItemData.email;
                        _getCandidate.is_verify = "N";

                        // gen random code
                        var verify_code_random = RandomString(8);
                        _getCandidate.verify_code = verify_code_random;
                        var sComplect = _TM_CandidatesService.Update(_getCandidate);
                        if (sComplect > 0)
                        {
                            string msg = "";

                            var Mail1 = _MailContentService.GetMailContent("Verify Trainee", "Y").FirstOrDefault();
                            string sContent = Mail1.content;
                            sContent = (sContent + "").Replace("$emailto", ItemData.email);
                            sContent = (sContent + "").Replace("$verifycode", verify_code_random);
                            sContent = (sContent + "").Replace("$verifyuser", CGlobal.UserInfo.FullName);

                            var objMail = new vObjectMail_Send();
                            objMail.mail_from = "hcmthailand@kpmg.co.th";
                            objMail.title_mail_from = "HCM System";
                            objMail.mail_to = ItemData.email;
                            objMail.mail_cc = "";
                            objMail.mail_subject = Mail1.mail_header;
                            objMail.mail_content = sContent;
                            var sSendMail = HCMFunc.SendMail(objMail, ref msg);

                            if (sSendMail)
                            {
                                result.Status = SystemFunction.process_Success;
                                result.Msg = "Success, Please check your email to verify the code. Fill in here.";
                                result.Content = "S"; //S คือส่งเมลไปแล้ว
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Can't contact your email.";
                                return Json(new
                                {
                                    result
                                });
                            }



                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Please try again.";
                            return Json(new
                            {
                                result
                            });
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Wrong password.";
                    }

                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }
            return Json(new
            {
                result
            });
        }


        [HttpPost]
        public ActionResult SaveVerifyTraineeEmail(vVerify_Trainee_Email ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vLogin_Return result = new vLogin_Return();

            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }


            if (string.IsNullOrEmpty(ItemData.email))
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Please input E-Mail.";
                return Json(new
                {
                    result
                });
            }
            else
            {

                MailAddress address = new MailAddress(ItemData.email);
                string host = address.Host;
                if (host.ToLower() != "kpmg.co.th")
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Please check your domain.";
                    return Json(new
                    {
                        result
                    });
                }
            }

            if (string.IsNullOrEmpty(ItemData.verify_code) && (ItemData.verify_code + "").Length < 8)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Please input Verify Code more than 8 word.";
                return Json(new
                {
                    result
                });
            }


            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId);
                if (nId != 0)
                {
                    var _getCandidate = _TM_CandidatesService.Find(nId);
                    if (_getCandidate != null)
                    {
                        if (ItemData.email == _getCandidate.trainee_email && ItemData.verify_code == _getCandidate.verify_code)
                        {
                            _getCandidate.is_verify = "Y";

                            var sComplect = _TM_CandidatesService.Update(_getCandidate);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                result.Msg = "Success verification, You can use it effectively.";
                                CGlobal.UserInfo.EMail = "Y";
                                CGlobal.UserInfo.IsVerify = "Y";

                                ViewBag.EMail = CGlobal.UserInfo.EMail;
                                ViewBag.IsVerify = CGlobal.UserInfo.IsVerify;

                                //return View();
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please try again.";
                                return Json(new
                                {
                                    result
                                });
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Incorrect verification code. Please try again.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Wrong password.";
                    }

                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult ViewTraineeProfile()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTrainee_Profile result = new vTrainee_Profile();

            DateTime dNow = DateTime.Now;
            int nId = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId);
            if (nId != 0)
            {
                var _getCandidate = _TM_CandidatesService.Find(nId);
                result.full_name = CGlobal.UserInfo.FullName;
                result.trainee_no = _getCandidate.candidate_TraineeNumber;
                result.email = _getCandidate.trainee_email;
                result.id_crad = _getCandidate.id_card;
                var getdate = _getCandidate.TM_PR_Candidate_Mapping.OrderByDescending(o => o.create_date).FirstOrDefault();
                result.internship_date = getdate.trainee_start.Value.ToString("dd MMM yyyy")+ " - "+ getdate.trainee_end.Value.ToString("dd MMM yyyy");
                result.bank_account_no = _getCandidate.candidate_BankAccountNumber;
            }

            return Json(new
            {
                result
            });
        }

        #endregion
        public class vObjectLogin_return
        {
            public bool sSucc { get; set; }
            public string msg { get; set; }

        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}