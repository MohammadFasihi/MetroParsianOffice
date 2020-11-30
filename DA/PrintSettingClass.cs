using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DA
{
    public class PrintSettingClass
    {

        public static tbl_PrintSettings setting;

        public PrintSettingClass()
        {

        }

        public void Delete(int ID)
        {
            Entities db = new Entities();

            DialogResult res;

            var query = (from q in db.tbl_PrintDesigns where q.ID == ID select q);
            var defaultDes = (from q in db.tbl_PrintSettings where q.DefaultDesignID == ID select q);

            if (query.Count() == 0)
                throw new Exception("طرح انتخاب شده وجود ندارد");

            if (defaultDes.Count() != 0)
            {
                res = MessageBox.Show("طرح انتخابی پیش فرض می باشد آیا مایل به حذف آن می باشید؟", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
                {
                    defaultDes.Single().DefaultDesignID = null;
                    db.tbl_PrintDesigns.Remove(query.Single());
                }

                db.SaveChanges();
            }
            else
            {
                db.tbl_PrintDesigns.Remove(query.Single());
                db.SaveChanges();
            }

            
        }

        public PrintSettingClass(int UserID)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_PrintSettings where q.UserID == UserID select q).Single();
            setting = query;
        }

        public void InsertSettings(tbl_PrintSettings print)
        {
            Entities db = new Entities();
            db.tbl_PrintSettings.Add(print);
            db.SaveChanges();
        }

        public void UpdateSettings(int UserID, tbl_PrintSettings printSetting)
        {
            Entities db = new Entities();
            var query = (from q in db.tbl_PrintSettings where q.UserID == UserID select q).Single();

            query.PreviewBeforePrint = printSetting.PreviewBeforePrint;
            query.LoadDesignBeforePrint = printSetting.LoadDesignBeforePrint;
            query.PrintQty = printSetting.PrintQty;
            query.DefaultDesignID = printSetting.DefaultDesignID;

            db.SaveChanges();
        }

        public void SaveDesign(int designID ,byte[] designFile)
        {
            Entities db = new Entities();

            var query = (from q in db.tbl_PrintDesigns where q.ID == designID select q).Single();
            query.design_File = designFile;

            db.SaveChanges();
        }

        public void SaveDesign(string designName, byte[] designFile)
        {
            Entities db = new Entities();

            tbl_PrintDesigns design = new tbl_PrintDesigns();

            if(isExist(designName))
            {
                DialogResult res = MessageBox.Show("نام طرح وارد شده تکراری می باشد آیا مایل به ذخیره طرح با نام تکراری هستید؟", "نام تکراری", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.No)
                    return;
            }

            design.design_File = designFile;
            design.design_Name = designName;

            db.tbl_PrintDesigns.Add(design);
            db.SaveChanges();
        }

        public bool isExist(string fileName)
        {
            Entities db = new Entities();

            var query = (from q in db.tbl_PrintDesigns where q.design_Name == fileName select q);

            return query.Count() == 1;
        }

        public int? GetDefaultDesign(int UserID)
        {
            Entities db = new Entities();

            int? query = (from q in db.tbl_PrintSettings where q.UserID == UserID select q.DefaultDesignID).Single();

            return query;
        }

        public byte[] GetDesign(int? designID)
        {
            Entities db = new Entities();

            if (designID == null)
                return null;
            else
            {
                var query = (from q in db.tbl_PrintDesigns where q.ID == designID.Value select q).Single();

                return query.design_File;
            }
        }
    }
}
