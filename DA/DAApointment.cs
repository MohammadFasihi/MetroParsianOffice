using Arash;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class DAApointment
    {

        public DataTable getApointment()
        {
            SqlAdoClass.Command("sp_DisplayApointment", CommandType.StoredProcedure);
            return SqlAdoClass.ExecuteQuery();
        }
        public void ChangeSituation(int ID, int st_ID)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Apointments where q.ID == ID select q).Single();
            query.st_ID = st_ID;
            db.SaveChanges();
        }

        public void ChangeKind(int ID, int Kind_ID)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Apointments where q.ID == ID select q).Single();
            query.kind_ID = Kind_ID;
            db.SaveChanges();
        }

        public List<viw_ApointmentReport> getApointment(string name, string Date)
        {
            Entities db = new Entities();
            string date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Date));
            var query = (from q in db.viw_ApointmentReport where q.Date == date && q.ep_Name == name select q).ToList();
            return query;
        }

        public int getLastID()
        {
            Entities db = new Entities();

            return db.tbl_Apointments.Select(p => p.ID).DefaultIfEmpty(0).Max() + 1;
        }

        protected tbl_Apointments getApointment(int ID)
        {
            Entities db = new Entities();
            return db.tbl_Apointments.Where(p => p.ID == ID).Single();
        }
        public IQueryable<tbl_Apointments> GetAllApointments()
        {
            Entities db = new Entities();
            return db.tbl_Apointments;
        }

        protected void Insert(int? cu_ID, string Name, string Phone, int ep_ID, PersianDate Date, string Time, int st_ID, string area, int ap_Kind, string Address, int UserID)
        {
            Entities db = new Entities();

            tbl_Apointments apointment = new tbl_Apointments();
            string date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Date.ToString()));

            apointment.ap_Area = area;
            apointment.Date = date;
            apointment.ap_Date = Date.ToDateTime();
            apointment.cu_ID = cu_ID;
            apointment.cu_Name = Name;
            apointment.cu_Phone = Phone;
            apointment.expert_ID = ep_ID;
            apointment.kind_ID = ap_Kind;
            apointment.st_ID = st_ID;
            apointment.user_ID = UserID;
            apointment.Time = Time;
            apointment.Address = Address;

            db.tbl_Apointments.Add(apointment);

            db.SaveChanges();
        }

        protected void Update(int ap_ID, int? cu_ID, string Name, string Phone, int ep_ID, PersianDate Date, string Time, int st_ID, string area, int ap_Kind, string Address, int UserID)
        {
            Entities db = new Entities();

            var apointment = (from q in db.tbl_Apointments where q.ID == ap_ID select q).Single();
            string date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Date.ToString()));

            apointment.ap_Area = area;
            apointment.Date = date;
            apointment.ap_Date = Date.ToDateTime();
            apointment.cu_Name = Name;
            apointment.cu_Phone = Phone;
            apointment.cu_ID = cu_ID;
            apointment.expert_ID = ep_ID;
            apointment.kind_ID = ap_Kind;
            apointment.st_ID = st_ID;
            apointment.changedUserID = UserID;
            apointment.Time = Time;
            apointment.Address = Address;

            db.SaveChanges();
        }

        protected void Delete(int ID)
        {
            Entities db = new Entities();

            var apointment = (from q in db.tbl_Apointments where q.ID == ID select q).Single();
            db.tbl_Apointments.Remove(apointment);

            db.SaveChanges();
        }

        public bool IsExist(int ID, int statusID)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Apointments where q.ID == ID && q.st_ID == statusID select q);

            return query.Count() == 1;
        }

        public bool IsExist(int ID)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Apointments where q.ID == ID select q);

            return query.Count() == 1;
        }
    }
}
