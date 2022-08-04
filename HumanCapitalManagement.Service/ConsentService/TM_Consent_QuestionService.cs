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
    public class TM_Consent_QuestionService : ServiceBase<TM_Consent_Question>
    {
        public TM_Consent_QuestionService(IRepository<TM_Consent_Question> repo) : base(repo)
        {
            //
        }
        public TM_Consent_Question Find(int id)
        {
            /*return Query().SingleOrDefault(s => s.Id == id && s.Active_Status == "Y");*/
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_Consent_Question> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.Active_Status == "Y");
            return sQuery.ToList();
        }
       
        #region Save Edit Delect 
        public int CreateNew(ref TM_Consent_Question s)
        {
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }

        public int CreateNewOrUpdate(TM_Consent_Question s)
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
        public int Update(TM_Consent_Question s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }

        #endregion
    }
    
}
