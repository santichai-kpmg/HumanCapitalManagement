using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class EmploymentTypeService : ServiceBase<TM_Employment_Type>
    {
        public EmploymentTypeService(IRepository<TM_Employment_Type> repo) : base(repo)
        {
            //
        }
        public TM_Employment_Type Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_Employment_Type> GetEmploymentType(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.employee_type_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.employee_type_name_en + "").Trim().ToLower())));
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public IEnumerable<TM_Employment_Type> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public bool CanSave(TM_Employment_Type TM_Employment_Type)
        {
            bool sCan = false;

            if (TM_Employment_Type.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.employee_type_name_en + "").Trim() == (TM_Employment_Type.employee_type_name_en + "").Trim());
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.employee_type_name_en + "").Trim() == (TM_Employment_Type.employee_type_name_en + "").Trim() && w.Id != TM_Employment_Type.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Employment_Type s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Employment_Type s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.employee_type_name_en = (s.employee_type_name_en + "").Trim();
                _getData.employee_type_description = s.employee_type_description;
                _getData.target_period_validate = s.target_period_validate;
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
