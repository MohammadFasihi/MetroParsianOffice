using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParsianOffice;
using DA;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace BL
{
    public class BLUserClass : DAUserClass
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static int? VisitorID { get; set; }
        public static string Kind { get; set; }

        public static bool Issue { get; set; }
        public static bool Visitor { get; set; }
        public static bool Expert { get; set; }
        public static bool Customer { get; set; }
        public static bool Apointment { get; set; }
        public static bool FollowUp { get; set; }
        public static bool Order { get; set; }
        public static bool OrderReturn { get; set; }
        public static bool Rest { get; set; }
        public static bool Contract { get; set; }
        public static bool ApointmentReport { get; set; }
        public static bool TodayApointmentReport { get; set; }
        public static bool NextDayApointmentReport { get; set; }
        public static bool CustomerApointmentReport { get; set; }
        public static bool CustomerFollowUpReport { get; set; }
        public static bool TodayFollowUpReport { get; set; }
        public static bool VacationReport { get; set; }
        public static bool VisitorReport { get; set; }
        public static bool OrderReport { get; set; }
        public static bool Backup { get; set; }
        public static bool Restore { get; set; }
        public static bool Users { get; set; }

        public void SendSearchSignal(int ID)
        {

        }

        public void ChangePassword(string userName,string oldPassword,string newPassword)
        {
            SHA1Cng hashPass = new SHA1Cng();
            byte[] res = hashPass.ComputeHash(Encoding.UTF8.GetBytes(oldPassword));
            byte[] Result = hashPass.ComputeHash(res);
            string pass1 = Convert.ToBase64String(Result);

            var query = base.SearchUser(userName, pass1);
            if (query.Count() == 1)
            {
                SHA1Cng hash = new SHA1Cng();
                byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                byte[] ResultFinal = hash.ComputeHash(result);
                string pass = Convert.ToBase64String(ResultFinal);

                base.ChangePassword(query.Single().ID, pass);
            }
            else
                throw new Exception("رمز عبور صحیح نمی باشد");
        }

        public void Delete(int userID)
        {
            if (base.IsAllowToDelete(userID))
                base.DeleteUser(userID);
            else
                MessageBox.Show("کاربر انتخابی غیر قابل حذف می باشد");
        }

        public void InsertUser(string Username, string Password, string Kind, int? visitorID, bool Active)
        {
            if(base.IsExist(Username))
            {
                MessageBox.Show("نام کاربری وارد شده تکراری است", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                base.AddNewUser(Username, Password, Active, visitorID, Kind);
            }
        }

        public void GetUserAccess()
        {
            Issue = (base.GetInsertAccessSection("Issue", UserID) || base.GetUpdateAccessSection("Issue", UserID) || base.GetDeleteAccessSection("Issue", UserID) || base.GetPrintAccessSection("Issue", UserID));
            Visitor = (base.GetInsertAccessSection("Visitor", UserID) || base.GetUpdateAccessSection("Visitor", UserID) || base.GetDeleteAccessSection("Visitor", UserID) || base.GetPrintAccessSection("Visitor", UserID));
            Expert = (base.GetInsertAccessSection("Expert", UserID) || base.GetUpdateAccessSection("Expert", UserID) || base.GetDeleteAccessSection("Expert", UserID) || base.GetPrintAccessSection("Expert", UserID));
            Customer = (base.GetInsertAccessSection("Customer", UserID) || base.GetUpdateAccessSection("Customer", UserID) || base.GetDeleteAccessSection("Customer", UserID) || base.GetPrintAccessSection("Customer", UserID));
            Apointment = (base.GetInsertAccessSection("Apointment", UserID) || base.GetUpdateAccessSection("Apointment", UserID) || base.GetDeleteAccessSection("Apointment", UserID) || base.GetPrintAccessSection("Apointment", UserID));
            FollowUp = (base.GetInsertAccessSection("FollowUp", UserID) || base.GetUpdateAccessSection("FollowUp", UserID) || base.GetDeleteAccessSection("FollowUp", UserID) || base.GetPrintAccessSection("FollowUp", UserID));
            Order = (base.GetInsertAccessSection("Order", UserID) || base.GetUpdateAccessSection("Order", UserID) || base.GetDeleteAccessSection("Order", UserID) || base.GetPrintAccessSection("Order", UserID));
            OrderReturn = (base.GetInsertAccessSection("OrderReturn", UserID) || base.GetUpdateAccessSection("OrderReturn", UserID) || base.GetDeleteAccessSection("OrderReturn", UserID) || base.GetPrintAccessSection("OrderReturn", UserID));
            Rest = (base.GetInsertAccessSection("Rest", UserID) || base.GetUpdateAccessSection("Rest", UserID) || base.GetDeleteAccessSection("Rest", UserID) || base.GetPrintAccessSection("Rest", UserID));
            Contract = (base.GetInsertAccessSection("Contract", UserID) || base.GetUpdateAccessSection("Contract", UserID) || base.GetDeleteAccessSection("Contract", UserID) || base.GetPrintAccessSection("Contract", UserID));
            ApointmentReport = (base.GetInsertAccessSection("ApointmentReport", UserID) || base.GetUpdateAccessSection("ApointmentReport", UserID) || base.GetDeleteAccessSection("ApointmentReport", UserID) || base.GetPrintAccessSection("ApointmentReport", UserID));
            TodayApointmentReport = (base.GetInsertAccessSection("TodayApointmentReport", UserID) || base.GetUpdateAccessSection("TodayApointmentReport", UserID) || base.GetDeleteAccessSection("TodayApointmentReport", UserID) || base.GetPrintAccessSection("TodayApointmentReport", UserID));
            NextDayApointmentReport = (base.GetInsertAccessSection("NextDayApointmentReport", UserID) || base.GetUpdateAccessSection("NextDayApointmentReport", UserID) || base.GetDeleteAccessSection("NextDayApointmentReport", UserID) || base.GetPrintAccessSection("NextDayApointmentReport", UserID));
            CustomerApointmentReport = (base.GetInsertAccessSection("CustomerApointmentReport", UserID) || base.GetUpdateAccessSection("CustomerApointmentReport", UserID) || base.GetDeleteAccessSection("CustomerApointmentReport", UserID) || base.GetPrintAccessSection("CustomerApointmentReport", UserID));
            CustomerFollowUpReport = (base.GetInsertAccessSection("FollowUpReport", UserID) || base.GetUpdateAccessSection("FollowUpReport", UserID) || base.GetDeleteAccessSection("FollowUpReport", UserID) || base.GetPrintAccessSection("FollowUpReport", UserID));
            TodayFollowUpReport = (base.GetInsertAccessSection("TodayFollowUpReport", UserID) || base.GetUpdateAccessSection("TodayFollowUpReport", UserID) || base.GetDeleteAccessSection("TodayFollowUpReport", UserID) || base.GetPrintAccessSection("TodayFollowUpReport", UserID));
            VacationReport = (base.GetInsertAccessSection("VacationReport", UserID) || base.GetUpdateAccessSection("VacationReport", UserID) || base.GetDeleteAccessSection("VacationReport", UserID) || base.GetPrintAccessSection("VacationReport", UserID));
            VisitorReport = (base.GetInsertAccessSection("VisitorReport", UserID) || base.GetUpdateAccessSection("VisitorReport", UserID) || base.GetDeleteAccessSection("VisitorReport", UserID) || base.GetPrintAccessSection("VisitorReport", UserID));
            OrderReport = (base.GetInsertAccessSection("OrderReport", UserID) || base.GetUpdateAccessSection("OrderReport", UserID) || base.GetDeleteAccessSection("OrderReport", UserID) || base.GetPrintAccessSection("OrderReport", UserID));
            Backup = (base.GetInsertAccessSection("Backup", UserID) || base.GetUpdateAccessSection("Backup", UserID) || base.GetDeleteAccessSection("Backup", UserID) || base.GetPrintAccessSection("Backup", UserID));
            Restore = (base.GetInsertAccessSection("Restore", UserID) || base.GetUpdateAccessSection("Restore", UserID) || base.GetDeleteAccessSection("Restore", UserID) || base.GetPrintAccessSection("Restore", UserID));
            Users = (base.GetInsertAccessSection("Users", UserID) || base.GetUpdateAccessSection("Users", UserID) || base.GetDeleteAccessSection("Users", UserID) || base.GetPrintAccessSection("Users", UserID));
        }

        public static IQueryable<tbl_Users> user { get; set; }
        public IEnumerable<tbl_Users> SendSearchSignal(string Username, string Password)
        {
            IEnumerable<tbl_Users> find;

            try
            {
                find = base.SearchUser(Username, Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return find;
        }
    }
}
