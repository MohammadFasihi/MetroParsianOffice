using Arash;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class DAContract
    {
        public List<tbl_Contract> getContract()
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Contract select q).ToList();
            return query;
        }

        public int getLastID()
        {
            Entities db = new Entities();

            return db.tbl_Contract.Select(p => p.ID).DefaultIfEmpty(0).Max() + 1;
        }

        protected tbl_Contract getContract(int ID)
        {
            Entities db = new Entities();
            return db.tbl_Contract.Where(p => p.ID == ID).Single();
        }
        public DataTable GetAllContract()
        {
            SqlAdoClass.Command("sp_DisplayContract", System.Data.CommandType.StoredProcedure);
            return SqlAdoClass.ExecuteQuery();
        }

        protected void Insert(int conID, int OrderID, string PaymentText, int UserID, PersianDate date, int? reportID)
        {
            Entities db = new Entities();
            tbl_Contract contract = new tbl_Contract();
            string Date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(date.ToString()));

            var query = (from q in db.tbl_Contract where q.sh_Date.Contains(date.Year.ToString()) select q).ToList();
            int count = query.Count() + 1;

            contract.ID = int.Parse(date.Year.ToString() + count.ToString());
            contract.order_ID = OrderID;
            contract.payment_Text = PaymentText;
            contract.user_ID = UserID;
            contract.date = date.ToDateTime();
            contract.sh_Date = Date;
            contract.report_ID = reportID;

            db.tbl_Contract.Add(contract);
            db.SaveChanges();
        }

        protected void Update(int conID, int OrderID, string PaymentText, int UserID, PersianDate date, int? reportID)
        {
            Entities db = new Entities();
            var contract = (from q in db.tbl_Contract where q.ID == conID select q).Single();
            string Date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(date.ToString()));

            contract.ID = conID;
            contract.order_ID = OrderID;
            contract.payment_Text = PaymentText;
            contract.user_ID = UserID;
            contract.date = date.ToDateTime();
            contract.sh_Date = Date;
            contract.report_ID = reportID;

            db.SaveChanges();
        }

        protected void Delete(int ID)
        {
            Entities db = new Entities();

            var contract = (from q in db.tbl_Contract where q.ID == ID select q).Single();
            db.tbl_Contract.Remove(contract);

            db.SaveChanges();
        }

        public bool IsContractOrderExist(int ID)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Contract where q.order_ID == ID select q);

            return query.Count() == 1;
        }

        public bool IsExist(int ID)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_Contract where q.ID == ID select q);

            return query.Count() == 1;
        }
    }
}
