using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.Feedbacks.MiniHeart_Main2021
{
    public class vMiniHeart_Peroid2021_obj
    {
        public int Id { get; set; }
        public string Start_Peroid { get; set; }
        public string End_Peroid { get; set; }
        
        public string Active_Status { get; set; }
        public string Create_Date { get; set; }
        public string Create_User { get; set; }
        public string Update_Date { get; set; }
        public string Update_User { get; set; }
        public string Remark { get; set; }
        public string reVal { get; set; }
    }
    public class vMiniHeart_Peroid2021 : CSearchMiniHeart
    {
        public List<vMiniHeart_Peroid2021> lstData { get; set; }
    }

    public class vMiniHeart_Peroid2021_Return : CResutlWebMethod
    {

        public vMiniHeart_Peroid2021_obj maindata { get; set; }
        public List<vMiniHeart_Peroid2021_obj> lstData { get; set; }
    }
}