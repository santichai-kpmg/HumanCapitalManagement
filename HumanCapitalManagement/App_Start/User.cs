﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace HumanCapitalManagement.App_Start
{
    public class User : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public User(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public string UserId { get; set; }
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EMail { get; set; }
        public string OfficePhone { get; set; }
        public string Company { get; set; }
        public string PI { get; set; }
        public string Pool { get; set; }
        public string Division { get; set; }
        public List<lstDivision> lstDivision { get; set; }
        public string UnitGroup { get; set; }
        public string Rank { get; set; }
        public string RankID { get; set; }
        public bool IsAdmin { get; set; }
        [DefaultValue(false)]
        public bool IsLoginSuss { get; set; }
        public string[] roles { get; set; }
        public string Picture { get; set; }
        public byte[] aPicture { get; set; }
    }

    public class lstDivision
    {
        public string sID { get; set; }
        public string sName { get; set; }
        public string sCode { get; set; }
        public string from_role { get; set; }
        public string sCompany_code { get; set; }
    }
}