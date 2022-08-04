using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.ConsentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Service.CommonClass.HCMServiceClass;

namespace HumanCapitalManagement.Service.ConsentService
{
    public class TM_Consent_FormService : ServiceBase<TM_Consent_Form>
    {
        public TM_Consent_FormService(IRepository<TM_Consent_Form> repo) : base(repo)
        {
            //
        }
        public TM_Consent_Form Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        
        public IEnumerable<TM_Consent_Form> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.Active_Status == "Y" || w.Active_Status =="N");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Consent_Form> GetDataForSelect_user()
        {
            var sQuery = Query().Where(w => w.Active_Status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Consent_Form> GetDataForSelect_Report()
        {
            /*var sQuery = Query().Where(w => w.Active_Status == "Y");*/
            var sQuery = Query();
            return sQuery.ToList();
        }

        #region Save Edit Delect 
        public int CreateNew(ref TM_Consent_Form s)
        {
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }

        public int CreateNewOrUpdate(TM_Consent_Form s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData == null)
            {
                Add(s);

                sResult = SaveChanges();
            }
            else
            {
               //Add Code for update

                sResult = SaveChanges();


            }
            //sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Consent_Form s)
        { 
            var sResult = 0;
            try 
            {
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            }
          catch(Exception e)
            {
                var pop = e.Message;
            }
            return sResult;
        }
        public int UpdateInactive(TM_Consent_Form s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id != s.Id && w.Active_Status == "Y").ToList();
            if (_getData.Any())
            {
                _getData.ForEach(ed =>
                {
                    ed.Active_Status = "N";
                    ed.Update_Date = DateTime.Now;
                    ed.Update_User = s.Update_User;
                });
                sResult = SaveChanges();
            }
            return sResult;
        }

        #endregion
    }
}
