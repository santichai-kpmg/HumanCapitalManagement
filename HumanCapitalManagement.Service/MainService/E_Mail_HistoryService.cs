using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class E_Mail_HistoryService : ServiceBase<E_Mail_History>
    {
        public E_Mail_HistoryService(IRepository<E_Mail_History> repo) : base(repo)
        {
            //
        }
        public E_Mail_History Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        #region Save Edit Delect 
        public int CreateNew(E_Mail_History s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(E_Mail_History s)
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
