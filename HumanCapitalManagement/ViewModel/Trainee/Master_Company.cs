using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.Trainee
{
    public static class Master_Company
    {
   
        public class sMaster_Company
        {
            public string UnitGroupID { get; set; }
            public string UnitGroupName { get; set; }
            public string Company { get; set; }
            public string Company_Short { get; set; }
        }

        public static List<sMaster_Company> Mcompany()
        {
            List<sMaster_Company> companylist = new List<sMaster_Company>();

            companylist.Add(new sMaster_Company() { UnitGroupID = "14100025", UnitGroupName = "ADV-DA",             Company = "KPMG Phoomchai Bs.Adv Ltd", Company_Short = "KPBA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100018", UnitGroupName = "ADV-MC",             Company = "KPMG Phoomchai Bs.Adv Ltd", Company_Short = "KPBA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100022", UnitGroupName = "ADV-RC AAS/IARCS",   Company = "KPMG Phoomchai Bs.Adv Ltd", Company_Short = "KPBA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100070", UnitGroupName = "ADV-RC FRM",         Company = "KPMG Phoomchai Bs.Adv Ltd", Company_Short = "KPBA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100071", UnitGroupName = "ADV-RC FS",          Company = "KPMG Phoomchai Bs.Adv Ltd", Company_Short = "KPBA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100024", UnitGroupName = "ADV-Support",        Company = "KPMG Phoomchai Bs.Adv Ltd", Company_Short = "KPBA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100058", UnitGroupName = "Audit G1",           Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100059", UnitGroupName = "Audit G2",           Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100060", UnitGroupName = "Audit G3",           Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100061", UnitGroupName = "Audit G4",           Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100002", UnitGroupName = "Audit G5",           Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100063", UnitGroupName = "Audit ITAS",         Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100003", UnitGroupName = "Audit Oth",          Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100062", UnitGroupName = "Audit Pool",         Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100004", UnitGroupName = "Audit Support",      Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100033", UnitGroupName = "KPMG Institute",     Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100030", UnitGroupName = "SSVC-HR",            Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100027", UnitGroupName = "SSVC-ITS",           Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100036", UnitGroupName = "SSVC-Markets",       Company = "KPMG Phoomchai Tax Ltd.", Company_Short = "KPT" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100032", UnitGroupName = "SSVC-RM",            Company = "KPMG Phoomchai Audit Ltd.", Company_Short = "KPA" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100068", UnitGroupName = "T&L Advisory",       Company = "KPMG Phoomchai Tax Ltd.", Company_Short = "KPT" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100016", UnitGroupName = "T&L-Support",        Company = "KPMG Phoomchai Tax Ltd.", Company_Short = "KPT" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100066", UnitGroupName = "Tax Compliance",     Company = "KPMG Phoomchai Tax Ltd.", Company_Short = "KPT" });
            companylist.Add(new sMaster_Company() { UnitGroupID = "14100067", UnitGroupName = "TP & Trade",         Company = "KPMG Phoomchai Tax Ltd.", Company_Short = "KPT" });
         
           
            return companylist;
        }
    }
}