using ParsianOffice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DA
{
    public class DAVisitor
    {
        public int getLastID()
        {
            Entities db = new Entities();

            return db.tbl_Experts.Select(p => p.ID).DefaultIfEmpty(0).Max() + 1;
        }

        public IQueryable<tbl_ExpertPhone> getContact(int ID)
        {
            Entities db = new Entities();
            return db.tbl_ExpertPhone.Where(p => p.ep_ID == ID);
        }

        protected tbl_Experts getVisitor(int ID)
        {
            Entities db = new Entities();
            return db.tbl_Experts.Where(p => p.ep_kind == 1 && p.ID == ID).Single();
        }
        public DataTable GetAllVisitors()
        {
            SqlAdoClass.Command("sp_DisplayVisitors", CommandType.StoredProcedure);
            return SqlAdoClass.ExecuteQuery();
        }

        public DataTable GetVisitorCombo()
        {
            SqlConnection con = new SqlConnection(ConnectionStringClass.GetADOConnectionString());
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetVisitorCombo";

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);

            return dt;
        }

        protected void Insert(string Name, string NationalCode, string Address, bool IsActive,byte time)
        {
            Entities db = new Entities();

            tbl_Experts visitor = new tbl_Experts();

            visitor.ep_Name = Name;
            visitor.ep_NationalCode = NationalCode;
            visitor.ep_Address = Address;
            visitor.ep_IsActive = IsActive;
            visitor.ep_kind = 1;
            visitor.ep_DailyTimeWork = time;

            db.tbl_Experts.Add(visitor);

            db.SaveChanges();
        }

        protected void Update(int ID, string Name, string NationalCode, string Address, bool IsActive, byte time)
        {
            Entities db = new Entities();

            var query = (from q in db.tbl_Experts where q.ID == ID select q).Single();

            query.ep_Name = Name;
            query.ep_NationalCode = NationalCode;
            query.ep_Address = Address;
            query.ep_IsActive = IsActive;
            query.ep_DailyTimeWork = time;

            db.SaveChanges();
        }

        protected void Delete(int ID)
        {
            Entities db = new Entities();

            var queryCustomer = (from q in db.tbl_Customer where q.vt_ID == ID select q);
            if (queryCustomer.Count() == 0)
            {
                var query = (from q in db.tbl_Experts where q.ID == ID select q).Single();

                var queryPhone = (from q in db.tbl_ExpertPhone where q.ep_ID == ID select q);
                db.tbl_Experts.Remove(query);
                db.tbl_ExpertPhone.RemoveRange(queryPhone);

                db.SaveChanges();
            }
            else
                throw new Exception("به دلیل داشتن مشتری بازاریاب مورد نظر غیر قابل حذف می باشد");
        }

        public void DeletePhone(int ID)
        {
            Entities db = new Entities();

            var queryPhone = (from q in db.tbl_ExpertPhone where q.ep_ID == ID select q);
            db.tbl_ExpertPhone.RemoveRange(queryPhone);

            db.SaveChanges();
        }

        public bool IsExist(int ID)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Experts where q.ID == ID select q);

            return query.Count() == 1;
        }

        public bool IsExist(string Name)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Experts where q.ep_Name == Name select q);

            return query.Count() == 1;
        }

        public void InsertPhone(int ID, string FieldName, string Value)
        {
            Entities db = new Entities();

            tbl_ExpertPhone phone = new tbl_ExpertPhone();

            phone.ep_ID = ID;
            phone.ep_Field = FieldName;
            phone.ep_Data = Value;

            db.tbl_ExpertPhone.Add(phone);

            db.SaveChanges();
        }
    }
}
