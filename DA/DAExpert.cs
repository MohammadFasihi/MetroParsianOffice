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
    public class DAExpert
    {
        public int getLastID()
        {
            Entities db = new Entities();

            return db.tbl_Experts.Select(p => p.ID).DefaultIfEmpty(0).Max() + 1;
        }

        public bool IsExpertOnVacation(object ep_ID,DateTime date,string Time)
        {
            bool result = false;

            Entities db = new Entities();

            int epID = 0;

            if (ep_ID != null)
            {
                epID = int.Parse(ep_ID.ToString());
                var query = (from q in db.tbl_Vacations where q.ep_ID == epID && 
                             (q.vc_StartDate.Value.Day <= date.Day && q.vc_StartDate.Value.Month == date.Month && q.vc_StartDate.Value.Year == date.Year)  &&
                             (q.vc_EndDate.Value.Day >= date.Day && q.vc_EndDate.Value.Month == date.Month && q.vc_EndDate.Value.Year == date.Year) && 
                             q.startTime.CompareTo(Time) == -1 && q.endTime.CompareTo(Time) == 1 select q);

                result = query.Count() != 0;
            }
            else
                throw new Exception("کارشناس انتخاب نشده است");

            return result;
        }

        public IQueryable<tbl_ExpertPhone> getContact(int ID)
        {
            Entities db = new Entities();
            return db.tbl_ExpertPhone.Where(p => p.ep_ID == ID);
        }

        protected tbl_Experts getExpert(int ID)
        {
            Entities db = new Entities();
            return db.tbl_Experts.Where(p => p.ID == ID).Single();
        }
        public DataTable GetAllExports()
        {
            SqlAdoClass.Command("sp_DisplayExpert", CommandType.StoredProcedure);
            return SqlAdoClass.ExecuteQuery();
        }

        public DataTable sp_GetAllExpert()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConnectionStringClass.GetADOConnectionString());
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "GetAllExpertCombo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public DataTable sp_GetExpert()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConnectionStringClass.GetADOConnectionString());
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "GetExpertCombo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        protected void Insert(string Name, string NationalCode, string Address,bool IsActive,byte DailyTimeWork)
        {
            Entities db = new Entities();

            tbl_Experts expert = new tbl_Experts();

            expert.ep_Name = Name;
            expert.ep_NationalCode = NationalCode;
            expert.ep_Address = Address;
            expert.ep_IsActive = IsActive;
            expert.ep_kind = 2;
            expert.ep_DailyTimeWork = DailyTimeWork;

            db.tbl_Experts.Add(expert);

            db.SaveChanges();
        }

        protected void Update(int ID, string Name, string NationalCode, string Address,bool IsActive, byte DailyTimeWork)
        {
            Entities db = new Entities();

            var query = (from q in db.tbl_Experts where q.ID == ID select q).Single();

            query.ep_Name = Name;
            query.ep_NationalCode = NationalCode;
            query.ep_Address = Address;
            query.ep_IsActive = IsActive;
            query.ep_DailyTimeWork = DailyTimeWork;

            db.SaveChanges();
        }

        protected void Delete(int ID)
        {
            Entities db = new Entities();

            var queryCustomer = (from q in db.tbl_Apointments where q.expert_ID == ID select q);
            if (queryCustomer.Count() == 0)
            {
                var query = (from q in db.tbl_Experts where q.ID == ID select q).Single();
                var queryPhone = (from q in db.tbl_ExpertPhone where q.ep_ID == ID select q);
                db.tbl_ExpertPhone.RemoveRange(queryPhone);
                db.tbl_Experts.Remove(query);

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
