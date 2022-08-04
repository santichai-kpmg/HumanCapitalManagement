using HumanCapitalManagement.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.CommonVM
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
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string emp_no { get; set; }
        public List<vSelect_Lg> lstEmp { get; set; }

        public string message { get; set; }

    }

    public class vSelect_Lg
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}