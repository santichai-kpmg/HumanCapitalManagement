﻿using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Models.eGreetings;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.eGreetingsVM
{
    public class veGreetings_Group_Question_obj
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
        public virtual ICollection<TM_eGreetings_Question> TM_eGreetings_Question { get; set; }

    }
    public class veGreetings_Group_Question : CSearcheGreetings
    {
        public List<veGreetings_Group_Question_obj> lstData { get; set; }
    }

    public class veGreetings_Group_Question_Return : CResutlWebMethod
    {

        public veGreetings_Group_Question_obj maindata { get; set; }
        public List<veGreetings_Question_obj> lstData { get; set; }
    }

    public class veGreetings_All_Question_Save
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
        public virtual ICollection<TM_eGreetings_Question> TM_eGreetings_Question { get; set; }
        public List<veGreetings_Question_obj> lstData { get; set; }

    }
}