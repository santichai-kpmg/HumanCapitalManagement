using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TraineeManagement.App_Start;

namespace TraineeManagement.ViewModels.CommonVM
{
    public class vLogin
    {
        public List<vLogin_obj> lstData { get; set; }
    }

    public class vLogin_Return : CResutlWebMethod
    {
        public List<vLogin_obj> lstData { get; set; }
    }
    public class vLogin_obj
    {
        public string user_name { get; set; }
        public string user_pass { get; set; }

        public string message { get; set; }

    }
    public class vLogin_password
    {
        public string old_pass { get; set; }
        public string new_1 { get; set; }
        public string new_2 { get; set; }

    }
    public class vSelect_Lg
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class vVerify_Trainee_Email
    {
        public string email { get; set; }
        public string verify_code { get; set; }

    }
    public class vTrainee_Profile
    {
        public string full_name { get; set; }
        public string trainee_no { get; set; }
        public string email { get; set; }
        public string id_crad { get; set; }
        public string internship_date { get; set; }
        public string bank_account_no { get; set; }

    }
}