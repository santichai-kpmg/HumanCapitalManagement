using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.VisExpiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.VisaExpiry
{
    public class TM_EmployeeForeign_VisaService : ServiceBase<TM_EmployeeForeign_Visa>
    {
        public TM_EmployeeForeign_VisaService(IRepository<TM_EmployeeForeign_Visa> repo) : base(repo)
        {
            //
        }
        //public IEnumerable<TM_Employee> GetDataByCompany(int IdCompany)
        //{
        //    var sQuery = Query().Where(w => w.TM_CompanyDetail.Id == IdCompany && w.active_status == "Y" && w.family_group == null);
        //    return sQuery.ToList();
        //}
        public IEnumerable<TM_EmployeeForeign_Visa> GetDataAllActive()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_EmployeeForeign_Visa> GetDataAllMemberActive()
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.family_group == null);
            return sQuery.ToList();
        }
        public IEnumerable<TM_EmployeeForeign_Visa> GetDataMemberbyCode(string IdEmp)
        {
            var sQuery = Query().Where(w => w.family_group == IdEmp && w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_EmployeeForeign_Visa> GetDataMemberbyCodeActive(string IdEmp)
        {
            var sQuery = Query().Where(w => w.family_group == IdEmp && w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_EmployeeForeign_Visa> GetDataforSearch(string company_id, string cus_code, string prefix_id, string active_status, string emp_name, string emp_lastname, string staff_id)
        {
            //Search all
            var sQuery = Query().Where(w => w.active_status != "" && w.active_status != "D");
            //Company
            if (!string.IsNullOrEmpty(company_id))
            {
                sQuery = sQuery.Where(w => w.TM_Company_Id + "" == company_id);
            }
            //Register ID
            if (!string.IsNullOrEmpty(cus_code))
            {
                sQuery = sQuery.Where(w => w.EmployeeNo + "" == cus_code);
            }
            //Prefix Id
            if (!string.IsNullOrEmpty(prefix_id))
            {
                sQuery = sQuery.Where(w => w.TM_Prefix_Id + "" == prefix_id);
            }
            //Name
            if (!string.IsNullOrEmpty(emp_name))
            {
                sQuery = sQuery.Where(w => w.Employeename_ENG + "" == emp_name);
            }
            //lastname
            if (!string.IsNullOrEmpty(emp_lastname))
            {
                sQuery = sQuery.Where(w => w.Employeesurname_ENG + "" == emp_lastname);
            }
            //Check active status
            if (!string.IsNullOrEmpty(active_status) && active_status == "Y")
            {
                sQuery = sQuery.Where(w => w.active_status + "" == "Y");
            }
            if (!string.IsNullOrEmpty(active_status) && active_status == "N")
            {
                sQuery = sQuery.Where(w => w.active_status + "" == "N");
            }
            //staff ID
            if (!string.IsNullOrEmpty(staff_id))
            {
                sQuery = sQuery.Where(w => w.staff_No + "" == staff_id);
            }
            return sQuery.ToList();
        }
        public IEnumerable<TM_EmployeeForeign_Visa> GetDataMemberforSearchactive(string active_status)
        {
            //Search all
            var sQuery = Query().Where(w => w.active_status != "" && w.family_group == null);
            //Check active status
            if (!string.IsNullOrEmpty(active_status) && active_status == "Y")
            {
                sQuery = sQuery.Where(w => w.active_status + "" == "Y");
            }
            if (!string.IsNullOrEmpty(active_status) && active_status == "N")
            {
                sQuery = sQuery.Where(w => w.active_status + "" == "N");
            }
            //if (!string.IsNullOrEmpty(active_status) && active_status == "A")
            //{
            //    sQuery = sQuery.Where(w => w.active_status + "" == "Y" && w.active_status == "N");
            //}
            return sQuery.ToList();
        }
        public IEnumerable<TM_EmployeeForeign_Visa> GetDataforSearchactive(string active_status)
        {
            //Search all
            var sQuery = Query().Where(w => w.active_status != "");
            //Check active status
            if (!string.IsNullOrEmpty(active_status) && active_status == "Y")
            {
                sQuery = sQuery.Where(w => w.active_status + "" == "Y");
            }
            if (!string.IsNullOrEmpty(active_status) && active_status == "N")
            {
                sQuery = sQuery.Where(w => w.active_status + "" == "N");
            }
            //if (!string.IsNullOrEmpty(active_status) && active_status == "A")
            //{
            //    sQuery = sQuery.Where(w => w.active_status + "" == "Y" && w.active_status == "N");
            //}
            return sQuery.ToList();
        }
        public TM_EmployeeForeign_Visa Find(int id)
        {
            var Return = Query().FirstOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public TM_EmployeeForeign_Visa FindByname(string name, string lastname)
        {
            var Return = Query().FirstOrDefault(s => s.Employeename_ENG == name || s.Employeesurname_ENG == lastname);
            return Return != null ? Return : null;
        }
        public TM_EmployeeForeign_Visa FindCode(string code)
        {
            var Return = Query().FirstOrDefault(s => s.EmployeeNo == code);
            return Return != null ? Return : null;
        }
 
        public TM_EmployeeForeign_Visa FindMembyCodeactive(int code)
        {
            var Return = Query().FirstOrDefault(s => s.family_group == code+"" && s.active_status == "Y");
            return Return != null ? Return : null;
        }
        public TM_EmployeeForeign_Visa FindLastRowId()
        {
            var Return = Query().OrderByDescending(o => o.Id).FirstOrDefault();
            return Return != null ? Return : null;
        }
        #region Save&Update
        public int CreateNewOrUpdate(ref TM_EmployeeForeign_Visa s)
        {
            var sResult = 0;
            var objSave = s;
            var _getDataObj = Query().Where(w => w.Id == objSave.Id).FirstOrDefault();
            if (_getDataObj == null)
            {
                Add(s);
            }
            else
            {
                _getDataObj.update_user = s.update_user;
                _getDataObj.update_date = s.update_date;
                _getDataObj.EmployeeNo = s.EmployeeNo;
                _getDataObj.active_status = s.active_status;
                _getDataObj.TM_Company_Id = s.TM_Company_Id;
                _getDataObj.TM_Prefix_Id = s.TM_Prefix_Id;
                _getDataObj.Employeename_TH = s.Employeename_TH;
                _getDataObj.Employeesurname_TH = s.Employeesurname_TH;
                _getDataObj.Employeename_ENG = s.Employeename_ENG;
                _getDataObj.Employeesurname_ENG = s.Employeesurname_ENG;
                _getDataObj.Employee_Middle_ENG = s.Employee_Middle_ENG;
                _getDataObj.Employee_email = s.Employee_email;
                _getDataObj.Employee_telephone = s.Employee_telephone;
                _getDataObj.staff_No = s.staff_No;
                _getDataObj.TM_Remark_Id = s.TM_Remark_Id;
                s = _getDataObj;
            }
            sResult = SaveChanges();
            return sResult;
        }
        public int UpdateDelete(ref TM_EmployeeForeign_Visa s)
        {
            var sResult = 0;
            var objSave = s;
            var _getDataObj = Query().Where(w => w.Id == objSave.Id).FirstOrDefault();
            if (_getDataObj != null)
            {
                _getDataObj.update_user = s.update_user;
                _getDataObj.update_date = s.update_date;
                _getDataObj.EmployeeNo = s.EmployeeNo;
                _getDataObj.active_status = s.active_status;
                _getDataObj.TM_Remark_Id = s.TM_Remark_Id;
                s = _getDataObj;
            }

            sResult = SaveChanges();
            return sResult;
        }
        #endregion
    }
}
