using DA;
using ParsianOffice.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winDisplayOrderDetail.xaml
    /// </summary>
    public partial class winDisplayOrderDetail : Window
    {
        public winDisplayOrderDetail()
        {
            InitializeComponent();
        }
        string section = "DisplayOrderDetail";
        public int id = 0;
        private void Window_Loaded(object sender, RoutedEventArgs e)
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

            var query = (from q in db.tbl_OrderDetails where q.order_ID == id select q).ToList();

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

            grd.ItemsSource = dt;

            viwMain.AllowEditing = false;

            try
            {
                this.Width = double.Parse(ConfigClass.getSetting(section, "Width"));
                this.Height = double.Parse(ConfigClass.getSetting(section, "Height"));

                grd.RestoreLayoutFromXml(clsDateClass.appStartupPath + "\\Grid Layout\\orderGrid.xml");

                this.Left = double.Parse(ConfigClass.getSetting(section, "LocationX"));
                this.Top = double.Parse(ConfigClass.getSetting(section, "LocationY"));
            }
            catch (Exception)
            {

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AppSettingClass.setting.SaveSizes)
            {
                ConfigClass.setSetting(section, "Width", this.ActualWidth);
                ConfigClass.setSetting(section, "Height", this.ActualHeight);

                ConfigClass.setSetting(section, "LocationX", this.Left);
                ConfigClass.setSetting(section, "LocationY", this.Top);
            }
            if (AppSettingClass.setting.SaveGridLayout)
                grd.SaveLayoutToXml(clsDateClass.appStartupPath + "\\Grid Layout\\orderGrid.xml");
        }
    }
}
