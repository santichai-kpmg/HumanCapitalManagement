using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.VisaExpiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.VisaExpiry
{
    public class TM_EmployeeService : ServiceBase<TM_Employee>
    {
        public TM_EmployeeService(IRepository<TM_Employee> repo) : base(repo)
        {
            //
        }
        //public IEnumerable<TM_Employee> GetDataByCompany(int IdCompany)
        //{
        //    var sQuery = Query().Where(w => w.TM_CompanyDetail.Id == IdCompany && w.active_status == "Y" && w.family_group == null);
        //    return sQuery.ToList();
        //}
        public IEnumerable<TM_Employee> GetDataAllActive()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Employee> GetDataAllMemberActive()
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.family_group == null);
            return sQuery.ToList();
        }
        public IEnumerable<TM_Employee> GetDataMemberbyCode(string IdEmp)
        {
            var sQuery = Query().Where(w => w.family_group == IdEmp && w.active_status != "D");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Employee> GetDataMemberbyCodeActive(string IdEmp)
        {
            var sQuery = Query().Where(w => w.family_group == IdEmp && w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Employee> GetDataforSearch(string cus_code, string prefix_id, string active_status)
        {
            //Search all
            var sQuery = Query().Where(w => w.active_status != "" && w.active_status != "D");


            if (!string.IsNullOrEmpty(cus_code))
            {
                sQuery = sQuery.Where(w => w.em_code + "" == cus_code);
            }
            if (!string.IsNullOrEmpty(prefix_id))
            {
                sQuery = sQuery.Where(w => w.TM_Prefix_Id + "" == prefix_id);
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

            return sQuery.ToList();
        }
        public IEnumerable<TM_Employee> GetDataMemberforSearchactive(string active_status)
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
        public IEnumerable<TM_Employee> GetDataforSearchactive(string active_status)
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
        public TM_Employee Find(int id)
        {
            var Return = Query().FirstOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public TM_Employee FindByname(string name, string lastname)
        {
            var Return = Query().FirstOrDefault(s => s.em_name_eng == name && s.em_lastname_eng == lastname);
            return Return != null ? Return : null;
        }
        public TM_Employee FindCode(string code)
        {
            var Return = Query().FirstOrDefault(s => s.em_code == code);
            return Return != null ? Return : null;
        }
        public TM_Employee FindMembyCodeactive(string code)
        {
            var Return = Query().FirstOrDefault(s => s.family_group == code && s.active_status == "Y");
            return Return != null ? Return : null;
        }
        public TM_Employee FindLastRowId()
        {
            var Return = Query().OrderByDescending(o => o.Id).FirstOrDefault();
            return Return != null ? Return : null;
        }
        #region Save&Update
        public int CreateNewOrUpdate(ref TM_Employee s)
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
                _getDataObj.em_code = s.em_code;
                _getDataObj.active_status = s.active_status;
                s = _getDataObj;
            }
            sResult = SaveChanges();
            return sResult;
        }
        public int UpdateDelete(ref TM_Employee s)
        {
            var sResult = 0;
            var objSave = s;
            var _getDataObj = Query().Where(w => w.Id == objSave.Id).FirstOrDefault();
            if (_getDataObj != null)
            {
                _getDataObj.update_user = s.update_user;
                _getDataObj.update_date = s.update_date;
                _getDataObj.em_code = s.em_code;
                _getDataObj.active_status = s.active_status;
                _getDataObj.remark = s.remark;
                s = _getDataObj;
            }

            sResult = SaveChanges();
            return sResult;
        }
        #endregion
    }
}
