using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA;
using ParsianOffice;
using System.Windows.Forms;

namespace BL
{
    public class BLIssuse : DAIssuse
    {

        public static tbl_Issues issue { get; set; }

        public BLIssuse(int IssueID)
        {
            issue = base.getIssues(IssueID);
        }

        public BLIssuse()
        {
            issue = null;
        }
        public void Insert(object Name, object Price)
        {
            try
            {
                int price = 0;

                if (!int.TryParse(Price.ToString(), out price))
                {
                    new Exception("مقدار فیلد قیمت صحیح وارد نشده است");
                }

                tbl_Issues issue = new tbl_Issues();

                issue.issue_Name = Name.ToString();
                issue.issue_Price = price;

                if (base.IsExist(Name.ToString()))
                {
                    DialogResult r = MessageBox.Show("نام کالا وارد شده موجود می باشد آیا مایل به تعریف مجدد دارید؟", "کالا موجود می باشد", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (r != DialogResult.No)
                        base.InsertNewIssue(issue);
                }
                else
                    base.InsertNewIssue(issue);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Update(object ID, object Name, object Price)
        {
            try
            {
                int price = 0;
                int id = 0;

                if (!int.TryParse(ID.ToString(), out id))
                {
                    new Exception("مقدار فیلد کد کالا صحیح وارد نشده است");
                }

                Price = StringClass.RemoveNumberSeparator(Price.ToString());

                if (!int.TryParse(Price.ToString(), out price))
                {
                    new Exception("مقدار فیلد قیمت صحیح وارد نشده است");
                }

                base.UpdateIssue(id, Name.ToString(), price);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Delete(object ID)
        {
            try
            {
                int id = 0;

                if (!int.TryParse(ID.ToString(), out id))
                {
                    new Exception("مقدار فیلد کد کالا صحیح وارد نشده است");
                }

                base.DeleteIssue(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
