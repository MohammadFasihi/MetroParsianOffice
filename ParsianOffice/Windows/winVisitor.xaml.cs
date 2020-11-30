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
using System.Drawing;
using DA;
using MahApps.Metro.Controls;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winVisitor.xaml
    /// </summary>
    public partial class winVisitor : Window
    {
        public winVisitor()
        {
            InitializeComponent();
        }

        bool OnEdit = false;
        string section = "Visitor";
        BLVisitor visitor = new BLVisitor();
        List<Contact> lst = new List<Contact>();

        public MetroWindow main;

        int value = 5;
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
            txtID.Text = visitor.getLastID().ToString();

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

            btnSave.Visibility = (user.GetInsertAccessSection("Visitor", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            btnDelete.Visibility = (user.GetDeleteAccessSection("Visitor", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
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

            lblDailyTimeWork.Text = slidNumber.Value.ToString("{0} ساعت");
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
            string address = txtAddress.Text;
            bool isActive = radActive.IsChecked.Value;
            byte dailyTimeWork = byte.Parse(value.ToString());
            try
            {
                if (!OnEdit)
                {
                    visitor.Insert(name, nationalCode, address, isActive, dailyTimeWork);
                    int id = visitor.getLastID() - 1;
                    if (grdMain.Items.Count != 0)
                    {
                        foreach (var item in lst)
                        {
                            visitor.InsertPhone(id, item.Caption, item.Value);
                        }
                    }
                }
                else
                {
                    visitor.Update(BLVisitor.visitor.ID, name, nationalCode, address, isActive, dailyTimeWork);
                    visitor.DeletePhone(BLVisitor.visitor.ID);

                    foreach (var item in lst)
                    {
                        visitor.InsertPhone(BLVisitor.visitor.ID, item.Caption, item.Value);
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
                visitor.Delete(BLVisitor.visitor.ID);

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

        private void txtData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnAdd_Click(null, null);
        }

        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {
            if((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl)) && e.Key == Key.Space)
            {
                btnSearch_Click(null, null);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            BLVisitor.visitor = null;

            winDisplay display = new winDisplay();
            display.tableName = "Visitor";
            display.ShowDialog();

            lst.Clear();
            grdMain.Items.Clear();

            if (BLVisitor.visitor != null)
            {
                txtID.Text = BLVisitor.visitor.ID.ToString();
                txtName.Text = BLVisitor.visitor.ep_Name;
                txtNationalCode.Text = BLVisitor.visitor.ep_NationalCode;
                txtAddress.Text = BLVisitor.visitor.ep_Address;

                radActive.IsChecked = BLVisitor.visitor.ep_IsActive.Value;
                radDeActive.IsChecked = !BLVisitor.visitor.ep_IsActive.Value;

                foreach (var item in BLVisitor.visitor.tbl_ExpertPhone.ToList())
                {
                    Contact cnt = new Contact();

                    cnt.Caption = item.ep_Field;
                    cnt.Value = item.ep_Data;

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

                btnSave.Visibility = (user.GetUpdateAccessSection("Visitor", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            }
            else
                Clear();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = user.GetDeleteAccessSection("Visitor",BLUserClass.UserID);
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnDelete_Click(null, null);
        }

        private void InsertCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!OnEdit)
                e.CanExecute = user.GetInsertAccessSection("Visitor", BLUserClass.UserID);
            else
                e.CanExecute = user.GetUpdateAccessSection("Visitor", BLUserClass.UserID);
        }

        private void InsertCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSave_Click(null, null);
        }

        private void RefreshCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSearch_Click(null, null);
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

        private void slidNumber_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                string str = "{0}" + " ساعت ";
                value = (int)slidNumber.Value;
                lblDailyTimeWork.Text = string.Format(str, value);
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
    }
}