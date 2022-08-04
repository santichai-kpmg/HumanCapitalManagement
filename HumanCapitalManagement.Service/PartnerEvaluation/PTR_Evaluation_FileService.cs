using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PTR_Evaluation_FileService : ServiceBase<PTR_Evaluation_File>
    {
        public PTR_Evaluation_FileService(IRepository<PTR_Evaluation_File> repo) : base(repo)
        {
            //
        }
        public PTR_Evaluation_File Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public PTR_Evaluation_File FindForDelete(int id, int nFileid)
        {
            return Query().SingleOrDefault(s => s.Id == nFileid && s.PTR_Evaluation.Id == id);
        }
        public int CreateNew(PTR_Evaluation_File s)
        {
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(PTR_Evaluation_File s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int DeleteFile(PTR_Evaluation_File s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
    }
}
