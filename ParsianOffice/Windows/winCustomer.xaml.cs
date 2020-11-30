using ParsianOffice.Classes;
using System;
using System.Collections.Generic;
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
using BL;
using DA;
using System.Threading;
using MahApps.Metro.Controls;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winCustomer.xaml
    /// </summary>
    public partial class winCustomer : Window
    {
        public winCustomer()
        {
            InitializeComponent();
        }

        bool OnEdit = false;
        string section = "Customer";
        BLCustomer customer = new BLCustomer();
        List<Contact> lst = new List<Contact>();

        public MetroWindow main;

        class Contact
        {
            public string Caption { get; set; }
            public string Value { get; set; }
        }
        BLUserClass user = new BLUserClass();
        void Clear()
        {
            txtData.Text = "";
            txtName.Text = "";
            txtNationalCode.Text = "";
            txtAddress.Text = "";

            cmbVisitor.ItemsSource = new DAVisitor().GetVisitorCombo();
            cmbVisitor.DisplayMember = "ep_Name";
            cmbVisitor.ValueMember = "ID";

            

            if (AppSettingClass.setting.LoadVisitorInCustomerDefine)
            {
                if (BLUserClass.Kind == "Admin")
                    cmbVisitor.SelectedIndex = -1;
                else
                    cmbVisitor.EditValue = BLUserClass.VisitorID;

            }
            txtID.Text = customer.getLastID().ToString();

            OnEdit = false;

            btnCancel.Content = "خروج";
            grdMain.Items.Clear();

            lst.Clear();

            txtName.Focus();

            Entities db = new Entities();

            cmbFieldName.ItemsSource = db.tbl_ContactKind.ToList();
            cmbFieldName.DisplayMemberPath = "KindName";
            cmbFieldName.SelectedValuePath = "ID";
            cmbFieldName.SelectedIndex = 0;

            btnSave.Visibility = (user.GetInsertAccessSection("Customer", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            btnDelete.Visibility = (user.GetDeleteAccessSection("Customer", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
        }

        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl)) && e.Key == Key.Space)
            {
                btnSearch_Click(null, null);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            BLCustomer.customer = null;

            winDisplay display = new winDisplay();
            display.tableName = "Customer";
            display.ShowDialog();

            lst.Clear();
            grdMain.Items.Clear();

            if (BLCustomer.customer != null)
            {
                txtID.Text = BLCustomer.customer.cu_ID.ToString();
                txtName.Text = BLCustomer.customer.cu_Name;
                txtNationalCode.Text = BLCustomer.customer.cu_NationalCode;
                txtAddress.Text = BLCustomer.customer.cu_Address;
                cmbVisitor.EditValue = BLCustomer.customer.vt_ID;
                txtCustomName.Text = BLCustomer.customer.cu_CustomName;

                foreach (var item in BLCustomer.customer.tbl_CustomerPhone.ToList())
                {
                    Contact cnt = new Contact();

                    cnt.Caption = item.cu_Field;
                    cnt.Value = item.cu_Data;

                    grdMain.Items.Add(cnt);
                    lst.Add(cnt);
                }

                if (grdMain.Columns.Count != 0)
                {
                    grdMain.Columns[0].Header = "عنوان فیلد";
                    grdMain.Columns[1].Header = "مقدار فیلد";
                }

                txtName.Focus();

                OnEdit = true;

                btnCancel.Content = "انصراف";

                btnSave.Visibility = (user.GetUpdateAccessSection("Customer", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            }
            else
                Clear();
        }

        private void txtData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnAdd_Click(null, null);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtData.Text) || String.IsNullOrWhiteSpace(txtData.Text))
                return;
            Contact cnt = new Contact();

            cnt.Caption = cmbFieldName.Text;
            cnt.Value = txtData.Text;

            lst.Add(cnt);
            grdMain.Items.Add(cnt);

            grdMain.Columns[0].Header = "عنوان فیلد";
            grdMain.Columns[1].Header = "مقدار";

            txtData.Text = "";

            cmbFieldName.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string nationalCode = txtNationalCode.Text;
            string Email = txtEmail.Text;
            string address = txtAddress.Text;
            int? visitor = null;
            string customName = txtCustomName.Text;

            if (cmbVisitor.SelectedIndex != -1)
                visitor = int.Parse(cmbVisitor.EditValue.ToString());

            try
            {
                if (!OnEdit)
                {
                    customer.Insert(name, Email, nationalCode, address, visitor,customName);
                    Thread.Sleep(100);
                    int id = customer.getLastID() - 1;
                    if (grdMain.Items.Count != 0)
                    {
                        foreach (var item in lst)
                        {
                            customer.InsertPhone(id, item.Caption, item.Value);
                        }
                    }
                }
                else
                {
                    customer.Update(BLCustomer.customer.cu_ID, name, Email, nationalCode, address, visitor, customName);
                    customer.DeletePhone(BLCustomer.customer.cu_ID);

                    foreach (var item in lst)
                    {
                        customer.InsertPhone(BLCustomer.customer.cu_ID, item.Caption, item.Value);
                    }
                }

                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(txtID.Text);
                customer.DeletePhone(id);
                customer.Delete(id);

                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!OnEdit)
                Close();
            else
                Clear();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = double.Parse(ConfigClass.getSetting(section, "Width"));
                this.Height = double.Parse(ConfigClass.getSetting(section, "Height"));

                grd.ColumnDefinitions[2].Width = new GridLength(double.Parse(ConfigClass.getSetting(section, "Contact")));

                this.Left = double.Parse(ConfigClass.getSetting(section, "LocationX"));
                this.Top = double.Parse(ConfigClass.getSetting(section, "LocationY"));
            }
            catch (Exception)
            {

            }

            Clear();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AppSettingClass.setting.SaveSizes)
            {
                ConfigClass.setSetting(section, "Width", this.ActualWidth);
                ConfigClass.setSetting(section, "Height", this.ActualHeight);

                ConfigClass.setSetting(section, "Contact", grd.ColumnDefinitions[2].Width);

                ConfigClass.setSetting(section, "LocationX", this.Left);
                ConfigClass.setSetting(section, "LocationY", this.Top);
            }
        }

        private void cmbVisitor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                cmbVisitor.SelectedIndex = -1;
            }
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (user.GetDeleteAccessSection("Customer", BLUserClass.UserID));
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            cmbVisitor.ItemsSource = new DAVisitor().GetVisitorCombo();
            cmbVisitor.DisplayMember = "vt_Name";
            cmbVisitor.ValueMember = "ID";
            cmbVisitor.SelectedIndex = -1;
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
                e.CanExecute = (user.GetInsertAccessSection("Customer", BLUserClass.UserID));
            else
                e.CanExecute = (user.GetUpdateAccessSection("Customer", BLUserClass.UserID));
        }

        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void itmDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Contact cnt = (Contact)grdMain.SelectedItem;
                lst.Remove(cnt);
                grdMain.Items.Remove(grdMain.SelectedItem);
            }
            catch (Exception)
            {

            }
        }

        private void itmAddContactKind_Click(object sender, RoutedEventArgs e)
        {
            winAddContactKind kind = new winAddContactKind();
            kind.ShowDialog();

            cmbFieldName.ItemsSource = null;
            Entities db = new Entities();
            cmbFieldName.ItemsSource = db.tbl_ContactKind.ToList();
            cmbFieldName.DisplayMemberPath = "KindName";
            cmbFieldName.SelectedValuePath = "ID";
            cmbFieldName.SelectedIndex = 0;
        }

        private void itmDeleteContactKind_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
