using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_TechnicalTestTransactionService : ServiceBase<TM_TechnicalTestTransaction>
    {
        public TM_TechnicalTestTransactionService(IRepository<TM_TechnicalTestTransaction> repo) : base(repo)
        {
            //
        }
        public TM_TechnicalTestTransaction Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_TechnicalTestTransaction> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }

        public bool CanSave(TM_TechnicalTestTransaction TM_TechnicalTestTransaction)
        {
            bool sCan = false;

            if (TM_TechnicalTestTransaction.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => w.TM_Candidates.Id == TM_TechnicalTestTransaction.TM_Candidates.Id
                && w.TM_TechnicalTest.Id == TM_TechnicalTestTransaction.TM_TechnicalTest.Id
                && DbFunctions.TruncateTime(w.Test_Date.Value) == DbFunctions.TruncateTime(TM_TechnicalTestTransaction.Test_Date.Value)
                && w.active_status =="Y"  );
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => w.Id != TM_TechnicalTestTransaction.Id
                && w.TM_Candidates.Id == TM_TechnicalTestTransaction.TM_Candidates.Id
                && w.TM_TechnicalTest.Id == TM_TechnicalTestTransaction.TM_TechnicalTest.Id
                && DbFunctions.TruncateTime(w.Test_Date.Value) == DbFunctions.TruncateTime(TM_TechnicalTestTransaction.Test_Date.Value)
                && w.active_status == "Y"
                  );

                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }

        #region Save Edit Delete 
        public int CreateNewAndEdit(TM_TechnicalTestTransaction s)
        {
            var sResult = 0;
            if (s.Id == 0)
            {
                Add(s);
            }
            else
            {
                var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
                if (_getData != null)
                {
                    _getData.active_status = s.active_status;
                    _getData.Test_Date = s.Test_Date;
                    _getData.Test_Score = s.Test_Score;
                    _getData.TM_Candidates = s.TM_Candidates;
                    _getData.TM_TechnicalTest = s.TM_TechnicalTest;
                    _getData.update_date = s.update_date;
                    _getData.update_user = s.update_user;


                }



            }


            sResult = SaveChanges();
            return sResult;
        }
        #endregion

        public int InActive(TM_TechnicalTestTransaction s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;

                sResult = SaveChanges();
            }
            return sResult;
        }


    }

}
