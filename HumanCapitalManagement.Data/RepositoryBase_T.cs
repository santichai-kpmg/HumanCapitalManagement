using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HumanCapitalManagement.Data
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        //private StoreDb db = new StoreDb();

        public RepositoryBase(DbContext context)
        {
            _context = context;
        }
        //old
        //public IQueryable<T> Query()
        //{
        //    return db.Set<T>().AsQueryable();
        //}

        //public int SaveChanges()
        //{
        //    return db.SaveChanges();
        //}
        //public T Add(T item )
        //{
        //    return db.Set<T>().Add(item);
        //}
        //public T Remove(T item)
        //{
        //    return db.Set<T>().Remove(item);
        //}
        //new
        public T Add(T item)
        {
            return _context.Set<T>().Add(item);
        }

        public T Find(params object[] keys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> Query(Func<T, bool> criteria)
        {
            return _context.Set<T>().Where(criteria).AsQueryable();
        }

        public T Remove(T item)
        {
            return _context.Set<T>().Remove(item);
        }


        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                //Create empty list to capture Validation error(s)
                //var outputLines = new List<string>();

                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    outputLines.Add(
                //        $"{DateTime.Now}: Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                //    outputLines.AddRange(eve.ValidationErrors.Select(ve =>
                //        $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\""));
                //}
                var sb = new StringBuilder();

                foreach (var failure in e.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                SaveErrorLog(sb.ToString());
                //Write to external file
                // File.AppendAllLines(@"c:\temp\dbErrors.txt", outputLines);
                throw;
            }

        }
        private void SaveErrorLog(string Error)
        {
            string sTextname = "e" + DateTime.Now.ToString("MMyyyy") + ".txt";
            var Path = HttpContext.Current.Server.MapPath("~/ErrorLog");
            if (!Directory.Exists(Path))//ก่อนจะย้าย ดูก่อนว่ามี directory ป่าว 
            {
                Directory.CreateDirectory(Path);//สร้าง directory 
            }
            if (!File.Exists(Path + "/" + sTextname))//ก่อนจะย้าย ดูก่อนว่ามี directory ป่าว 
            {
                File.CreateText(Path + "/" + sTextname);//สร้าง directory 
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(Path + @"\\" + sTextname, true))
                {
                    writer.WriteLine("Message :" + Error  +
                       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
            catch (Exception e)
            {


            }
        }


    }

}
