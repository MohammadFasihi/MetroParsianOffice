using Arash;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class DAOrder
    {
        public int getLastID()
        {
            Entities db = new Entities();

            return db.tbl_OrderHeader.Select(p => p.ID).DefaultIfEmpty(0).Max() + 1;
        }

        protected tbl_OrderHeader getOrder(int ID)
        {
            Entities db = new Entities();
            return db.tbl_OrderHeader.Where(p => p.ID == ID).Single();
        }

        public DataTable getOrderDetail(int ID)
        {
            Entities db = new Entities();

            DataTable dt = new DataTable("OrderDetail");

            dt.Columns.Add("recID", typeof(int));
            dt.Columns.Add("issue_ID", typeof(int));
            dt.Columns.Add("issue_Name");
            dt.Columns.Add("Qty", typeof(int));
            dt.Columns.Add("Comment");
            dt.Columns.Add("IsDone", typeof(bool));
            dt.Columns.Add("UsersQty", typeof(int));
            dt.Columns.Add("Discount");
            dt.Columns.Add("Discount_Per", typeof(byte));
            dt.Columns.Add("Price", typeof(int));
            dt.Columns.Add("RecPrice", typeof(int));
            dt.Columns.Add("detail_Price", typeof(int));
            dt.Columns.Add("detail_RecPrice", typeof(int));

            var query = (from q in db.tbl_OrderDetails where q.order_ID == ID select q ).ToList();

            int i = 0;
            foreach (var item in query)
            {
                dt.Rows.Add();

                dt.Rows[i]["recID"] = item.recID.Value;
                dt.Rows[i]["issue_ID"] = item.issue_ID.Value;
                dt.Rows[i]["issue_Name"] = item.issue_Name;
                dt.Rows[i]["Qty"] = item.detail_Qty.Value;
                dt.Rows[i]["Comment"] = item.detail_Comment;
                dt.Rows[i]["IsDone"] = item.detail_IsDone;
                dt.Rows[i]["UsersQty"] = item.detail_Users.Value;
                dt.Rows[i]["Discount"] = item.detail_Discount.Value;
                dt.Rows[i]["Discount_Per"] = item.detail_Discount_Per.Value;
                dt.Rows[i]["Price"] = item.Price.Value;
                dt.Rows[i]["RecPrice"] = item.RecPrice.Value;
                dt.Rows[i]["detail_Price"] = item.detail_RecPrice.Value;
                dt.Rows[i]["detail_RecPrice"] = item.detail_RecPrice.Value;

                i++;
            }

            return dt;
        }
        public DataTable GetAllOrders(int kind)
        {
            SqlAdoClass.Command("sp_DisplayOrderHeader", CommandType.StoredProcedure, kind,"@kind");
            return SqlAdoClass.ExecuteQuery();
        }

        protected void Insert(int OrderID,int CustomerID,PersianDate Date,int StatusID,int BasePrice,int Discount,int OrderPrice,int UserID,string Comment,int kind)
        {
            Entities db = new Entities();

            tbl_OrderHeader orderHeader = new tbl_OrderHeader();
            string date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Date.ToString()));

            orderHeader.ID = OrderID;
            orderHeader.order_BasePrice = (BasePrice * kind);
            orderHeader.order_Date = Date.ToDateTime();
            orderHeader.order_Dsicount = Discount;
            orderHeader.order_Price = (OrderPrice * kind);
            orderHeader.order_Side = CustomerID;
            orderHeader.sh_Date = date;
            orderHeader.user_ID = UserID;
            orderHeader.order_Comment = Comment;
            orderHeader.st_ID = StatusID;
            orderHeader.order_kind = kind;

            db.tbl_OrderHeader.Add(orderHeader);

            db.SaveChanges();
        }

        protected void Insert(int OrderID,DataTable TableDetail)
        {
            Entities db = new Entities();

            for (int i = 0; i < TableDetail.Rows.Count; i++)
            {
                tbl_OrderDetails orderDetails = new tbl_OrderDetails();

                orderDetails.order_ID = OrderID;

                orderDetails.recID = int.Parse(TableDetail.Rows[i]["recID"].ToString());
                orderDetails.issue_ID = int.Parse(TableDetail.Rows[i]["issue_ID"].ToString());
                orderDetails.issue_Name = TableDetail.Rows[i]["issue_Name"].ToString();
                orderDetails.detail_Qty = int.Parse(TableDetail.Rows[i]["Qty"].ToString());
                orderDetails.detail_Comment = TableDetail.Rows[i]["Comment"].ToString();
                orderDetails.detail_IsDone = bool.Parse(TableDetail.Rows[i]["IsDone"].ToString());
                orderDetails.detail_Users = int.Parse(TableDetail.Rows[i]["UsersQty"].ToString());
                orderDetails.detail_Discount = int.Parse(TableDetail.Rows[i]["Discount"].ToString());
                orderDetails.detail_Discount_Per = byte.Parse(TableDetail.Rows[i]["Discount_Per"].ToString());
                orderDetails.Price = int.Parse(TableDetail.Rows[i]["Price"].ToString());
                orderDetails.RecPrice = int.Parse(TableDetail.Rows[i]["RecPrice"].ToString());
                orderDetails.detail_Price = int.Parse(TableDetail.Rows[i]["detail_Price"].ToString());
                orderDetails.detail_RecPrice = int.Parse(TableDetail.Rows[i]["detail_RecPrice"].ToString());

                db.tbl_OrderDetails.Add(orderDetails);
            }

            db.SaveChanges();
        }

        protected void Update(int OrderID, int CustomerID, PersianDate Date, int StatusID, int BasePrice, int Discount, int OrderPrice, int UserID, string Comment,int kind)
        {
            Entities db = new Entities();

            var orderHeader = (from q in db.tbl_OrderHeader where q.ID == OrderID select q).Single();
            string date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Date.ToString()));

            orderHeader.ID = OrderID;
            orderHeader.order_BasePrice = (BasePrice * kind);
            orderHeader.order_Date = Date.ToDateTime();
            orderHeader.order_Dsicount = Discount;
            orderHeader.order_Price = (OrderPrice * kind);
            orderHeader.order_Side = CustomerID;
            orderHeader.sh_Date = date;
            orderHeader.changedUserID = UserID;
            orderHeader.order_Comment = Comment;
            orderHeader.st_ID = StatusID;
            orderHeader.order_kind = kind;

            db.SaveChanges();
        }

        protected void DeleteDetail(int ID)
        {
            Entities db = new Entities();

            var orderDets = (from q in db.tbl_OrderDetails where q.order_ID == ID select q);
            db.tbl_OrderDetails.RemoveRange(orderDets);

            db.SaveChanges();
        }
        protected void Delete(int ID)
        {
            Entities db = new Entities();

            DeleteDetail(ID);

            var order = (from q in db.tbl_OrderHeader where q.ID == ID select q).Single();
            db.tbl_OrderHeader.Remove(order);

            db.SaveChanges();
        }

        public bool IsExist(int ID,int status)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_OrderHeader where q.ID == ID && q.st_ID == status select q).ToList();

            return query.Count() == 1;
        }

        public bool IsExist(int ID)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_OrderHeader where q.ID == ID select q).ToList();

            return query.Count() == 1;
        }
    }
}
