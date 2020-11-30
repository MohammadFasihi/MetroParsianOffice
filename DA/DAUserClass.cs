using ParsianOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class DAUserClass
    {
        public IQueryable<tbl_Users> SearchUser(int ID)
        {
            IQueryable<tbl_Users> find;

            Entities db = new Entities();

            var query = (from q in db.tbl_Users where q.ID == ID select q);

            find = query;

            return find;
        }

        public IQueryable<tbl_Users> SearchUser(string Username)
        {
            IQueryable<tbl_Users> find;

            Entities db = new Entities();

            var query = (from q in db.tbl_Users where q.username == Username select q);

            find = query;

            return find;
        }

        public bool GetUserAccess(int userID, string Section)
        {
            Entities db = new Entities();
            bool result = (from q in db.tbl_UserAccess where q.UserID == userID && q.Section == Section select q).Count() == 1;
            return result;
        }

        protected void DeleteUser(int userID)
        {
            Entities db = new Entities();

            var setting = (from q in db.AppSettings where q.userID == userID select q).Single();
            var printSetting = (from q in db.tbl_PrintSettings where q.UserID == userID select q).Single();
            var userAccess = (from q in db.tbl_UserAccess where q.UserID == userID select q);
            var user = (from q in db.tbl_Users where q.ID == userID select q).Single();

            db.AppSettings.Remove(setting);
            db.tbl_PrintSettings.Remove(printSetting);
            db.tbl_UserAccess.RemoveRange(userAccess);
            db.tbl_Users.Remove(user);

            db.SaveChanges();
        }

        protected bool IsAllowToDelete(int userID)
        {
            Entities db = new Entities();

            bool apointment = (from q in db.tbl_Apointments where q.user_ID == userID select q).Count() == 0;
            bool follow = (from q in db.tbl_FollowUP where q.user_ID == userID select q).Count() == 0;
            bool order = (from q in db.tbl_OrderHeader where q.user_ID == userID where q.order_kind == -1 select q).Count() == 0;
            bool orderReturn = (from q in db.tbl_OrderHeader where q.user_ID == userID select q).Count() == 0;
            bool contract = (from q in db.tbl_Contract where q.user_ID == userID select q).Count() == 0;

            return (apointment && follow && order && orderReturn && contract);

        }

        public List<tbl_Users> GetAllUsers()
        {
            Entities db = new Entities();
            return db.tbl_Users.ToList();
        }
        public bool IsExist(string Username)
        {
            bool find;

            Entities db = new Entities();

            var query = (from q in db.tbl_Users where q.username == Username select q);

            find = query.Count() == 1;

            return find;
        }

        protected void AddNewUser(string Username, string Password, bool IsActive, int? visitorID, string Kind)
        {
            Entities db = new Entities();
            tbl_Users users = new tbl_Users();

            users.IsActive = IsActive;
            users.kind = Kind;
            users.username = Username;
            users.vt_ID = visitorID;

            SHA1Cng hash = new SHA1Cng();
            byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(Password));
            byte[] ResultFinal = hash.ComputeHash(result);
            users.password = Convert.ToBase64String(ResultFinal);

            db.tbl_Users.Add(users);

            db.SaveChanges();

            tbl_PrintSettings printSetting = new tbl_PrintSettings();

            printSetting.LoadDesignBeforePrint = true;
            printSetting.PreviewBeforePrint = true;
            printSetting.PrintQty = 1;
            printSetting.UserID = SearchUser(Username).Single().ID;

            PrintSettingClass settingClass = new PrintSettingClass();
            settingClass.InsertSettings(printSetting);

            AppSettingClass appSettingClass = new AppSettingClass();
            appSettingClass.InserAppSetting(printSetting.UserID.Value);
        }

        public void InsertUserAccess(int UserID, string Section, bool insert, bool update, bool delete, bool print)
        {
            Entities db = new Entities();
            tbl_UserAccess userAccess = new tbl_UserAccess();

            userAccess.UserID = UserID;
            userAccess.Section = Section;
            userAccess.InsertAccess = insert;
            userAccess.UpdateAccess = update;
            userAccess.DeleteAccess = delete;
            userAccess.PrintAccess = print;

            db.tbl_UserAccess.Add(userAccess);
            db.SaveChanges();
        }

        public void UpdateUserAccess(int UserID, string Section, bool insert, bool update, bool delete, bool print)
        {
            Entities db = new Entities();
            var userAccess = (from q in db.tbl_UserAccess where q.UserID == UserID && q.Section == Section select q).Single();

            userAccess.UserID = UserID;
            userAccess.Section = Section;
            userAccess.InsertAccess = insert;
            userAccess.UpdateAccess = update;
            userAccess.DeleteAccess = delete;
            userAccess.PrintAccess = print;

            db.SaveChanges();
        }

        protected void ChangePassword(int userID,string Password)
        {
            Entities db = new Entities();

            var query = (from q in db.tbl_Users where q.ID == userID select q).Single();

            query.password = Password;

            db.SaveChanges();
        }

        public void UpdateUser(int UserID,string oldPassword,string newPassowrd,bool isActive,string kind)
        {
            Entities db = new Entities();

            var query = (from q in db.tbl_Users where q.ID == UserID select q).Single();

            if (oldPassword != query.password)
                query.password = newPassowrd;
            query.IsActive = isActive;
            query.kind = kind;

            db.SaveChanges();
        }

        public void DeleteUserAccess(int UserID, string Section)
        {
            Entities db = new Entities();
            var userAccess = (from q in db.tbl_UserAccess where q.UserID == UserID && q.Section == Section select q).Single();

            db.tbl_UserAccess.Remove(userAccess);
            db.SaveChanges();
        }
        public bool GetInsertAccessSection(string Section, int UserID)
        {
            Entities db = new Entities();
            bool access = false;
            var query = (from q in db.tbl_UserAccess where q.Section == Section && q.UserID == UserID select q.InsertAccess);

            if (query.Count() == 1)
                access = query.Single().Value;

            return access;
        }

        public bool GetUpdateAccessSection(string Section, int UserID)
        {
            Entities db = new Entities();
            bool access = false;
            var query = (from q in db.tbl_UserAccess where q.Section == Section && q.UserID == UserID select q.UpdateAccess);

            if (query.Count() == 1)
                access = query.Single().Value;

            return access;
        }
        public bool GetDeleteAccessSection(string Section, int UserID)
        {
            Entities db = new Entities();
            bool access = false;
            var query = (from q in db.tbl_UserAccess where q.Section == Section && q.UserID == UserID select q.DeleteAccess);

            if (query.Count() == 1)
                access = query.Single().Value;

            return access;
        }
        public bool GetPrintAccessSection(string Section, int UserID)
        {
            Entities db = new Entities();
            bool access = false;
            var query = (from q in db.tbl_UserAccess where q.Section == Section && q.UserID == UserID select q.PrintAccess);

            if (query.Count() == 1)
                access = query.Single().Value;

            return access;
        }

        public IQueryable<tbl_Users> SearchUser(string Username, string Password)
        {
            IQueryable<tbl_Users> find;

            Entities db = new Entities();

            var query = (from q in db.tbl_Users where q.username == Username select q);

            if (query.Count() != 0)
            {
                if (query.Single().password == Password)
                    find = query;
                else
                    throw new Exception("رمز عبور صحیح نمی باشد");
            }
            else
            {
                throw new Exception("نام کاربری یافت نشد");
            }

            find = query;

            return find;
        }
    }
}
