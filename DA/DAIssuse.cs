using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParsianOffice;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace DA
{
    public class DAIssuse
    {
        protected void InsertNewIssue(tbl_Issues newIssuse)
        {
            Entities db = new Entities();

            db.tbl_Issues.Add(newIssuse);

            db.SaveChanges();

        }

        public DataTable GetIssueCombo()
        {
            SqlConnection con = new SqlConnection(ConnectionStringClass.GetADOConnectionString());
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetIssueCombo";

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);

            return dt;
        }
        public int LastIssueID()
        {
            Entities db = new Entities();

            int lastID = (db.tbl_Issues.Select(p => p.ID).DefaultIfEmpty(0).Max()) + 1;

            return lastID;

        }

        protected IQueryable<tbl_Issues> Search(int ID)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Issues where q.ID == ID select q);
            return query;
        }

        protected void UpdateIssue(int ID, string Name, int Price)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Issues where q.ID == ID select q).Single();

            query.issue_Name = Name;
            query.issue_Price = Price;

            db.SaveChanges();
        }

        protected bool IsExist(string name)
        {
            Entities db = new Entities();

            var query = (from q in db.tbl_Issues where q.issue_Name == name select q).ToList();

            return query.Count != 0;
        }

        protected void DeleteIssue(int issue)
        {
            Entities db = new Entities();

            var queryDets = (from q in db.tbl_OrderDetails where q.issue_ID == issue select q).ToList();

            if(queryDets.Count == 0)
            {
                var query = (from q in db.tbl_Issues where q.ID == issue select q);
                if (query.Count() != 0)
                {
                    db.tbl_Issues.Remove(query.Single());

                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("کالای انتخاب شده وجود ندارد", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("به دلیل ثبت سفارش برای کالای انتخاب شده امکان حذف وجود ندارد","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);

        }

        public tbl_Issues getIssues(int ID)
        {
            try
            {
                Entities db = new Entities();
                var query = (from q in db.tbl_Issues where q.ID == ID select q).Single();
                return query;
            }
            catch (Exception)
            {

                throw new Exception("کالای کد : "+ID.ToString()+" یافت نشد");
            }
        }

        public tbl_Issues getIssues(string issueName)
        {
            try
            {
                Entities db = new Entities();
                var query = (from q in db.tbl_Issues where q.issue_Name == issueName select q).Single();
                return query;
            }
            catch (Exception)
            {

                throw new Exception("کالای  : " + issueName + " یافت نشد");
            }
        }
        public DataTable GetAllIssues()
        {
            SqlAdoClass.Command("sp_DisplayIssue", CommandType.StoredProcedure);
            return SqlAdoClass.ExecuteQuery();
        }

    }
}