using ParsianOffice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class DACustomer
    {
        public int getLastID()
        {
            Entities db = new Entities();

            return db.tbl_Customer.Select(p => p.cu_ID).DefaultIfEmpty(0).Max() + 1;
        }

        public tbl_Customer GetCustomer(int cu_ID)
        {
            Entities db = new Entities();
            tbl_Customer query = (from q in db.tbl_Customer where q.cu_ID == cu_ID select q).Single();
            return query;
        }

        public tbl_Customer GetCustomer(string CustomerName)
        {
            Entities db = new Entities();
            tbl_Customer query = (from q in db.tbl_Customer where q.cu_Name == CustomerName select q).Single();
            return query;
        }
        public IQueryable<tbl_CustomerPhone> getContact(int ID)
        {
            Entities db = new Entities();
            return db.tbl_CustomerPhone.Where(p => p.cu_ID == ID);
        }

        protected tbl_Customer getCustomer(int ID)
        {
            Entities db = new Entities();
            return db.tbl_Customer.Where(p => p.cu_ID == ID).Single();
        }
        public DataTable GetAllCustomer()
        {
            SqlAdoClass.Command("sp_DisplayCustomer", CommandType.StoredProcedure);
            return SqlAdoClass.ExecuteQuery();
        }

        public DataTable GetCustomerCobmo()
        {
            SqlConnection con = new SqlConnection(ConnectionStringClass.GetADOConnectionString());
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetCustomerCombo";

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);

            return dt;
        }

        public DataTable GetCustomerCobmo(int? VisitorID)
        {
            SqlConnection con = new SqlConnection(ConnectionStringClass.GetADOConnectionString());
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;

            if (VisitorID == null)
                cmd.CommandText = "GetCustomerCombo";
            else
            {
                cmd.CommandText = "GetOwnCustomerCombo";
                cmd.Parameters.AddWithValue("@vt_ID", VisitorID);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);

            return dt;
        }

        protected void Insert(string Name, string NationalCode, string Email, string Address, int? visitorID, string customName)
        {
            Entities db = new Entities();

            tbl_Customer customer = new tbl_Customer();

            customer.cu_Name = Name;
            customer.cu_NationalCode = NationalCode;
            customer.cu_Email = Email;
            customer.vt_ID = visitorID;
            customer.cu_CustomName = customName;

            if (visitorID != null)
                customer.vt_Name = (from q in db.tbl_Experts where q.ID == visitorID select q.ep_Name).Single();

            customer.cu_Address = Address;

            db.tbl_Customer.Add(customer);

            db.SaveChanges();
        }

        protected void Update(int ID, string Name, string NationalCode, string Email, string Address, int? visitorID, string customName)
        {
            Entities db = new Entities();

            var query = (from q in db.tbl_Customer where q.cu_ID == ID select q).Single();

            query.cu_Name = Name;
            query.cu_NationalCode = NationalCode;
            query.cu_Email = Email;
            query.vt_ID = visitorID;
            query.cu_CustomName = customName;

            if (visitorID != null)
                query.vt_Name = (from q in db.tbl_Experts where q.ID == visitorID select q.ep_Name).Single();
            else
                query.vt_Name = null;

            query.cu_Address = Address;

            db.SaveChanges();
        }

        protected void Delete(int ID)
        {
            Entities db = new Entities();

            var queryCustomer = (from q in db.tbl_Customer where q.cu_ID== ID  select q).Single();
            if (queryCustomer.tbl_Apointments.Count == 0 && queryCustomer.tbl_OrderHeader.Count == 0 && queryCustomer.tbl_FollowUP.Count == 0)
            {
                var query = (from q in db.tbl_Customer where q.cu_ID == ID select q).Single();
                var queryPhone = (from q in db.tbl_CustomerPhone where q.cu_ID == ID select q);
                db.tbl_CustomerPhone.RemoveRange(queryPhone);
                db.tbl_Customer.Remove(query);

                db.SaveChanges();
            }
            else
                throw new Exception("به دلیل داشتن ثبت سفارش، ثبت قرار ملاقات، ثبت پیگیری این مشتری غیر قابل حذف می باشد");
        }

        public void DeletePhone(int ID)
        {
            Entities db = new Entities();

            var queryPhone = (from q in db.tbl_CustomerPhone where q.cu_ID == ID select q);
            db.tbl_CustomerPhone.RemoveRange(queryPhone);

            db.SaveChanges();
        }

        public bool IsExist(int ID)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Customer where q.cu_ID == ID select q);

            return query.Count() == 1;
        }

        public bool IsExist(string Name)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Customer where q.cu_Name == Name select q);

            return query.Count() == 1;
        }

        public void InsertPhone(int ID, string FieldName, string Value)
        {
            Entities db = new Entities();

            tbl_CustomerPhone phone = new tbl_CustomerPhone();

            phone.cu_ID = ID;
            phone.cu_Field = FieldName;
            phone.cu_Data = Value;

            db.tbl_CustomerPhone.Add(phone);

            db.SaveChanges();
        }
    }
}
