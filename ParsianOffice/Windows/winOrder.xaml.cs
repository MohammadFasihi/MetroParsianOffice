using Arash;
using BL;
using DA;
using DevExpress.Xpf.Grid.LookUp;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ParsianOffice.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    /// Interaction logic for winOrder.xaml
    /// </summary>
    public partial class winOrder : Window
    {
        public winOrder()
        {
            InitializeComponent();
        }

        bool OnEdit = false;
        string section = "Order";
        BLOrder order = new BLOrder();
        public MetroWindow main;
        int row, basePrice, orderPrice, rowIndex, Discount = 0, quantity = 0;

        public int orderKindID;

        DataTable dt = new DataTable("OrderDetail");

        public object AppSetting { get; private set; }

        void GridSource()
        {
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

            mainGrid.ItemsSource = dt;
        }

        void LoadOrderInfo()
        {
            basePrice = 0;
            Discount = 0;
            quantity = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!String.IsNullOrEmpty(dt.Rows[i]["RecPrice"].ToString()))
                {
                    basePrice += int.Parse(dt.Rows[i]["RecPrice"].ToString());
                    int qty = int.Parse(dt.Rows[i]["Qty"].ToString());
                    quantity += qty;
                    int discount = int.Parse(dt.Rows[i]["Discount"].ToString());
                    Discount += discount * qty;
                }
            }

            orderPrice = (basePrice - Discount);

            txtBasePrice.Text = basePrice.ToString();
            txtDiscountSum.Text = Discount.ToString();
            txtPrice.Text = orderPrice.ToString();

            lblOrderPrice.Text = StringClass.GetNumberToCharachter(orderPrice);
        }

        private void SetData(int index, int Data)
        {
            dt.Rows[rowIndex]["detail_Price"] = Data;
            dt.Rows[rowIndex]["Price"] = Data;
        }

        void Calculate(int index)
        {
            int qty = int.Parse(dt.Rows[index]["Qty"].ToString());
            int discount = int.Parse(dt.Rows[index]["Discount"].ToString());
            int price = int.Parse(dt.Rows[index]["Price"].ToString());

            dt.Rows[rowIndex]["RecPrice"] = qty * price;

            int discount_per = (discount * 100) / price;

            dt.Rows[rowIndex]["Discount_Per"] = byte.Parse(discount_per.ToString());
            dt.Rows[rowIndex]["detail_Price"] = (price - discount);
            dt.Rows[rowIndex]["detail_RecPrice"] = qty * (price - discount);
        }

        void Calculate(int index, string DiscountPer)
        {
            DiscountPer = DiscountPer.Replace("%", "");
            dt.Rows[rowIndex]["Discount_Per"] = DiscountPer;
            int discount_per = int.Parse(dt.Rows[index]["Discount_Per"].ToString());
            int qty = int.Parse(dt.Rows[index]["Qty"].ToString());
            int price = int.Parse(dt.Rows[index]["Price"].ToString());

            int discount = (discount_per * price) / 100;

            dt.Rows[rowIndex]["Discount"] = discount;
            dt.Rows[rowIndex]["RecPrice"] = qty * price;

            dt.Rows[rowIndex]["detail_Price"] = (price - discount);
            dt.Rows[rowIndex]["detail_RecPrice"] = qty * (price - discount);
        }

        void LoadIssueData(string issueName)
        {
            BLIssuse issue = new BLIssuse();
            var selectedIssue = issue.getIssues(issueName);

            dt.NewRow();
            rowIndex++;

            dt.Rows[rowIndex]["issue_Name"] = issueName;
            dt.Rows[rowIndex]["issue_ID"] = selectedIssue.ID;
            dt.Rows[rowIndex]["Qty"] = 1;
            dt.Rows[rowIndex]["IsDone"] = false;
            dt.Rows[rowIndex]["Discount"] = 0;
            dt.Rows[rowIndex]["Discount_Per"] = 0;
            dt.Rows[rowIndex]["UsersQty"] = 1;
            dt.Rows[rowIndex]["Price"] = selectedIssue.issue_Price.Value;
            int recPrice = 1 * selectedIssue.issue_Price.Value;
            dt.Rows[rowIndex]["RecPrice"] = recPrice;

            dt.Rows[rowIndex]["detail_Price"] = selectedIssue.issue_Price.Value;
            dt.Rows[rowIndex]["detail_RecPrice"] = recPrice;
        }

        void LoadIssueData(int ID)
        {
            BLIssuse issue = new BLIssuse();
            var selectedIssue = issue.getIssues(ID);

            dt.NewRow();
            rowIndex++;

            dt.Rows[rowIndex]["issue_Name"] = selectedIssue.issue_Name;
            dt.Rows[rowIndex]["issue_ID"] = ID;
            dt.Rows[rowIndex]["Qty"] = 1;
            dt.Rows[rowIndex]["IsDone"] = false;
            dt.Rows[rowIndex]["Discount"] = 0;
            dt.Rows[rowIndex]["Discount_Per"] = 0;
            dt.Rows[rowIndex]["UsersQty"] = 1;
            dt.Rows[rowIndex]["Price"] = selectedIssue.issue_Price.Value;
            int recPrice = 1 * selectedIssue.issue_Price.Value;
            dt.Rows[rowIndex]["RecPrice"] = recPrice;

            dt.Rows[rowIndex]["detail_Price"] = selectedIssue.issue_Price.Value;
            dt.Rows[rowIndex]["detail_RecPrice"] = recPrice;
        }
        public void LoadData()
        {
            BLIssuse issue = new BLIssuse();
            cmbIssueName.ItemsSource = issue.GetIssueCombo();
            cmbIssueName.DisplayMember = "issue_Name";
            cmbIssueName.ValueMember = "issue_Name";
            

            BLCustomer customer = new BLCustomer();
            cmbCustomer.ItemsSource = customer.GetCustomerCobmo(BLUserClass.VisitorID);
            cmbCustomer.DisplayMember = "cu_Name";
            cmbCustomer.ValueMember = "cu_ID";

            cmbSituation.ItemsSource = new BLSituation().GetAllSituation();
            cmbSituation.DisplayMemberPath = "st_Name";
            cmbSituation.SelectedValuePath = "ID";
        }

        BLUserClass user = new BLUserClass();

        private void Clear()
        {
            mainGrid.ItemsSource = null;
            cmbCustomer.SelectedIndex = -1;
            txtID.Text = order.getLastID().ToString();
            cmbSituation.SelectedIndex = 0;

            pDate.SelectedDate = PersianDate.Today;

            row = 0;
            rowIndex = -1;
            basePrice = 0;
            Discount = 0;
            quantity = 0;

            txtComment.Text = "";
            txtBasePrice.Text = "";
            txtDiscountSum.Text = "";
            txtPrice.Text = "";

            dt.Rows.Clear();
            mainGrid.ItemsSource = dt;
            btnCancel.Content = "خروج";
            OnEdit = false;

            lblOrderPrice.Text = "";
            txtDiscount.Text = "";

            btnSave.Visibility = (user.GetInsertAccessSection("Order", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            btnDelete.Visibility = (user.GetDeleteAccessSection("Order", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = double.Parse(ConfigClass.getSetting(section, "Width"));
                this.Height = double.Parse(ConfigClass.getSetting(section, "Height"));

                grd.RowDefinitions[0].Height = new GridLength(double.Parse(ConfigClass.getSetting(section, "Header")));
                grd.RowDefinitions[5].Height = new GridLength(double.Parse(ConfigClass.getSetting(section, "Footer")));

                this.Left = double.Parse(ConfigClass.getSetting(section, "LocationX"));
                this.Top = double.Parse(ConfigClass.getSetting(section, "LocationY"));
            }
            catch (Exception)
            {

            }

            if (File.Exists(clsDateClass.appStartupPath + "\\Grid Layout\\orderGrid.xml"))
                mainGrid.RestoreLayoutFromXml(clsDateClass.appStartupPath + "\\Grid Layout\\orderGrid.xml");

            GridSource();
            LoadData();
            Clear();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AppSettingClass.setting.SaveSizes)
            {
                ConfigClass.setSetting(section, "Width", this.ActualWidth);
                ConfigClass.setSetting(section, "Height", this.ActualHeight);

                ConfigClass.setSetting(section, "Header", grd.RowDefinitions[0].Height);
                ConfigClass.setSetting(section, "Footer", grd.RowDefinitions[5].Height);

                ConfigClass.setSetting(section, "LocationX", this.Left);
                ConfigClass.setSetting(section, "LocationY", this.Top);
            }
            if (AppSettingClass.setting.SaveGridLayout)
                mainGrid.SaveLayoutToXml(clsDateClass.appStartupPath + "\\Grid Layout\\orderGrid.xml");
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int status = int.Parse(cmbSituation.SelectedValue.ToString());
                int id = int.Parse(txtID.Text);
                if (!OnEdit)
                {
                    order.Insert(txtID.Text, cmbCustomer, pDate.SelectedDate, cmbSituation.SelectedValue, basePrice, Discount, orderPrice, BLUserClass.UserID, txtComment.Text, dt, orderKindID);
                }
                else
                {
                    order.Update(txtID.Text, cmbCustomer, pDate.SelectedDate, cmbSituation.SelectedValue, basePrice, Discount, orderPrice, BLUserClass.UserID, txtComment.Text, dt, orderKindID);
                }

                Clear();

                if (AppSettingClass.setting.InsertContractAfterOrder)
                {

                    MetroDialogSettings config = new MetroDialogSettings();

                    config.AffirmativeButtonText = "بله";
                    config.AnimateHide = true;
                    config.AnimateShow = true;
                    config.ColorScheme = MetroDialogColorScheme.Theme;
                    config.DefaultButtonFocus = MessageDialogResult.Affirmative;
                    config.DialogResultOnCancel = MessageDialogResult.Negative;
                    config.NegativeButtonText = "خیر";

                    var res = await main.ShowMessageAsync("", "آیا مایل به ذخیره طرح می باشید؟", MessageDialogStyle.AffirmativeAndNegative, config);

                    //MessageBoxResult res = MessageBox.Show("ثبت فاکتور با موفقیت انجام شد" + "\n" + "آیا مایل به ثبت قرارداد برای فاکتور می باشید؟", "ثبت قرارداد", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (res == MessageDialogResult.Affirmative)
                    {
                        winContract contract = new winContract();
                        contract.orderID = id;
                        contract.isLoaded = true;
                        contract.ShowDialog();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطا در برنامه", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            BLOrder.order = null;

            winDisplay display = new winDisplay();
            display.tableName = "OrderHeader";
            display.kind = orderKindID;
            display.ShowDialog();

            if (BLOrder.order != null)
            {
                txtID.Text = BLOrder.order.ID.ToString();
                pDate.SelectedDate = PersianDate.Parse(BLOrder.order.sh_Date);
                cmbCustomer.EditValue = BLOrder.order.order_Side;
                cmbSituation.SelectedValue = BLOrder.order.st_ID;
                txtComment.Text = BLOrder.order.order_Comment;
                txtBasePrice.Text = (BLOrder.order.order_BasePrice.Value * BLOrder.order.order_kind).ToString();
                txtDiscountSum.Text = BLOrder.order.order_Dsicount.Value.ToString();
                txtPrice.Text = (BLOrder.order.order_Price.Value * BLOrder.order.order_kind).ToString();

                dt = order.getOrderDetail(BLOrder.order.ID);

                mainGrid.ItemsSource = dt;

                row = dt.Rows.Count;
                rowIndex = row - 1;

                OnEdit = true;

                btnCancel.Content = "انصراف";

                btnSave.Visibility = (user.GetUpdateAccessSection("Order", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.Space)
            {
                btnSearch_Click(this, null);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(txtID.Text);
            order.Delete(id);

            Clear();
        }

        private void txtDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (String.IsNullOrEmpty(txtDiscount.Text))
                        txtDiscount.Text = "0";
                    int discount = 0;
                    int disc = 0;

                    if (txtDiscount.Text.Contains("%"))
                    {
                        int per = int.Parse(txtDiscount.Text.Replace("%", ""));

                        discount = (per * basePrice) / 100;
                    }
                    else
                    {
                        discount = int.Parse(txtDiscount.Text);
                    }

                    disc = (discount / quantity);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Discount"] = disc;
                        Calculate(i);
                    }

                    txtDiscount.Text = disc.ToString();
                    LoadOrderInfo();
                    mainGrid.ItemsSource = dt;
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
                Clear();
            else
                Close();

        }

        private void viwMain_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "issue_Name")
                {
                    string issueName = e.Value.ToString();

                    mainGrid.ItemsSource = null;

                    if (!String.IsNullOrEmpty(issueName))
                    {
                        LoadIssueData(issueName);
                    }
                }

                if (e.Column.FieldName == "issue_ID")
                {
                    int issueID = int.Parse(e.Value.ToString());

                    mainGrid.ItemsSource = null;

                    LoadIssueData(issueID);
                }

                if (e.Column.FieldName == "Discount")
                {
                    string discount = e.Value.ToString();

                    mainGrid.ItemsSource = null;

                    if (!discount.Contains("%"))
                        Calculate(e.RowHandle);
                    else
                    {
                        Calculate(e.RowHandle, discount);
                    }
                }

                if (e.Column.FieldName == "Discount_Per")
                {
                    string discount = e.Value.ToString();

                    mainGrid.ItemsSource = null;

                    Calculate(e.RowHandle, discount);
                }

                if (e.Column.FieldName == "Qty")
                    Calculate(e.RowHandle);

                if (e.Column.FieldName == "Price" || e.Column.FieldName == "detail_Price")
                {
                    int value = int.Parse(e.Value.ToString());
                    SetData(e.RowHandle, value);
                    Calculate(e.RowHandle);
                }

                LoadOrderInfo();

                mainGrid.ItemsSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطا در برنامه", MessageBoxButton.OK, MessageBoxImage.Error);
                dt.Rows.RemoveAt(--row);
                mainGrid.ItemsSource = dt;
            }
        }


        private void viwGrid_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            mainGrid.SetCellValue(e.RowHandle, "recID", ++row);
            mainGrid.ItemsSource = dt;
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (user.GetDeleteAccessSection("Order", BLUserClass.UserID));
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadData();
        }

        private void RefreshCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void InsertCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSave_Click(sender, e);
        }

        private void InsertCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!OnEdit)
                e.CanExecute = (user.GetInsertAccessSection("Order", BLUserClass.UserID));
            else
                e.CanExecute = (user.GetUpdateAccessSection("Order", BLUserClass.UserID));
        }

    }
}
