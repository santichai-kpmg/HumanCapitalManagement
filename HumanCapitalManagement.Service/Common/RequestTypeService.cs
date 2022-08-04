using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class RequestTypeService : ServiceBase<TM_Request_Type>
    {
        public RequestTypeService(IRepository<TM_Request_Type> repo) : base(repo)
        {
            //
        }

        public TM_Request_Type Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_Request_Type> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Request_Type> GetRequestTypeForSave(int[] aID)//,bool isAdmin
        {
            var sQuery = Query();
            if (aID != null)
            {
                sQuery = sQuery.Where(w => aID.Contains(w.Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }
        public IEnumerable<TM_Request_Type> GetRequestType(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.request_type_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.request_type_name_en + "").Trim().ToLower())));
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
        public bool CanSave(TM_Request_Type TM_Request_Type)
        {
            bool sCan = false;

            if (TM_Request_Type.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.request_type_name_en + "").Trim() == (TM_Request_Type.request_type_name_en + "").Trim());
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w =>( w.request_type_name_en + "").Trim() == (TM_Request_Type.request_type_name_en + "").Trim() && w.Id != TM_Request_Type.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Request_Type s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Request_Type s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.request_type_name_en =( s.request_type_name_en + "").Trim();
                _getData.request_type_description = s.request_type_description;
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
