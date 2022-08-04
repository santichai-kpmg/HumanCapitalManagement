using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Models._360Feedback.MiniHeart2021;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.Feedbacks.MiniHeart_Main2021
{
    public class vMiniHeart_Group_Question2021_obj
    {
        public string View { get; set; }
        public string Edit { get; set; }
        public string IdEncrypt { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public string Active_Status { get; set; }
        public string Action_date { get; set; }
        public string Description { get; set; }
        public string Create_Date { get; set; }
        public string Create_User { get; set; }
        public string Update_Date { get; set; }
        public string Update_User { get; set; }
        public virtual ICollection<TM_MiniHeart_Question2021> TM_MiniHeart_Question { get; set; }

    }
    public class vMiniHeart_Group_Question2021 : CSearchMiniHeart
    {
        public List<vMiniHeart_Group_Question2021_obj> lstData { get; set; }
    }

    public class vMiniHeart_Group_Question2021_Return : CResutlWebMethod
    {

        public vMiniHeart_Group_Question2021_obj maindata { get; set; }
        public List<vMiniHeart_Question2021_obj> lstData { get; set; }
    }

    public class vMiniHeart_All_Question2021_Save
    {
        public string View { get; set; }
        public string Edit { get; set; }
        public string IdEncrypt { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Active_Status { get; set; }
        public string Action_date { get; set; }
        public string Description { get; set; }
        public string Create_Date { get; set; }
        public string Create_User { get; set; }
        public string Update_Date { get; set; }
        public string Update_User { get; set; }
        public virtual ICollection<TM_MiniHeart_Question2021> TM_MiniHeart_Question { get; set; }
        public List<vMiniHeart_Question2021_obj> lstData { get; set; }

    }
}