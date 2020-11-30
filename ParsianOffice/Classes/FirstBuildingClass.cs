using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA;
using BL;

namespace ParsianOffice.Classes
{
    class FirstBuildingClass
    {
        public void Build()
        {
            BLUserClass userClass = new BLUserClass();

            int id = 1;

            userClass.InsertUser("مدیر", "", "Admin", null, true);

            userClass.InsertUserAccess(id, "Issue", true, true, true, true);

            userClass.InsertUserAccess(id, "Visitor", true, true, true, true);

            userClass.InsertUserAccess(id, "Expert", true, true, true, true);

            userClass.InsertUserAccess(id, "Customer", true, true, true, true);

            userClass.InsertUserAccess(id, "Apointment", true, true, true, true);

            userClass.InsertUserAccess(id, "FollowUp", true, true, true, true);

            userClass.InsertUserAccess(id, "Order", true, true, true, true);

            userClass.InsertUserAccess(id, "OrderReturn", true, true, true, true);

            userClass.InsertUserAccess(id, "Rest", true, true, true, true);

            userClass.InsertUserAccess(id, "Contract", true, true, true, true);

            userClass.InsertUserAccess(id, "ApointmentReport", true, true, true, true);

            userClass.InsertUserAccess(id, "TodayApointmentReport", true, true, true, true);

            userClass.InsertUserAccess(id, "NextDayApointmentReport", true, true, true, true);

            userClass.InsertUserAccess(id, "CustomerApointmentReport", true, true, true, true);

            userClass.InsertUserAccess(id, "FollowUpReport", true, true, true, true);

            userClass.InsertUserAccess(id, "TodayFollowUpReport", true, true, true, true);

            userClass.InsertUserAccess(id, "VacationReport", true, true, true, true);

            userClass.InsertUserAccess(id, "OrderReport", true, true, true, true);

            userClass.InsertUserAccess(id, "VisitorReport", true, true, true, true);

            userClass.InsertUserAccess(id, "Backup", true, true, true, true);

            userClass.InsertUserAccess(id, "Restore", true, true, true, true);

            userClass.InsertUserAccess(id, "Users", true, true, true, true);

            AddSituation("برقرار");
            AddSituation("لغو");
            AddSituation("تعلیق");
            AddSituation("انجام شد");

            AddExpertKind("ویزیتور");
            AddExpertKind("کارشناس");

            AddContactKind("شماره موبایل");
            AddContactKind("شماره تلفن");
            AddContactKind("کد پستی");
            AddContactKind("کد اقتصادی");

            AddApointmentKind("پرزنت");
            AddApointmentKind("نصب");
            AddApointmentKind("آموزش");
            AddApointmentKind("نصب و آموزش");
            AddApointmentKind("خدمات");
        }

        private void AddSituation(string st_Name)
        {
            Entities db = new Entities();
            tbl_Situation st_kind = new tbl_Situation();
            st_kind.st_Name = st_Name;
            db.tbl_Situation.Add(st_kind);
            db.SaveChanges();
        }

        private void AddExpertKind(string kind)
        {
            Entities db = new Entities();
            tbl_ExpertKind ep_kind = new tbl_ExpertKind();
            ep_kind.ExpertKind = kind;
            db.tbl_ExpertKind.Add(ep_kind);
            db.SaveChanges();
        }

        private void AddContactKind(string Kind)
        {
            Entities db = new Entities();

            tbl_ContactKind kind = new tbl_ContactKind();
            kind.KindName = Kind;
            db.tbl_ContactKind.Add(kind);
            db.SaveChanges();
        }

        private void AddApointmentKind(string ap_Kind)
        {
            Entities db = new Entities();

            tbl_ApKinds kind = new tbl_ApKinds();
            kind.kind_Name = ap_Kind;
            db.tbl_ApKinds.Add(kind);
            db.SaveChanges();
        }
    }
}
