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
using ParsianOffice.Classes;
using DA;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winExpress.xaml
    /// </summary>
    public partial class winExpress : Window
    {
        public winExpress()
        {
            InitializeComponent();
        }

        BLUserClass user = new BLUserClass();

        bool OnEdit = false;
        string section = "Expert";
        BLExpert expert = new BLExpert();
        List<Contact> lst = new List<Contact>();

        public winMain main;

        byte value = 5;

        class Contact
        {
            public string Caption { get; set; }
            public string Value { get; set; }
        }

        void Clear()
        {
            txtData.Text = "";
            txtName.Text = "";
            txtNationalCode.Text = "";
            txtAddress.Text = "";
            txtID.Text = expert.getLastID().ToString();

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

            btnSave.Visibility = (user.GetInsertAccessSection("Expert", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            btnDelete.Visibility = (user.GetDeleteAccessSection("Expert", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
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

        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl)) && e.Key == Key.Space)
            {
                btnSearch_Click(null, null);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            BLExpert.expert = null;

            winDisplay display = new winDisplay();
            display.tableName = "Expert";
            display.ShowDialog();

            lst.Clear();
            grdMain.Items.Clear();

            if (BLExpert.expert != null)
            {
                txtID.Text = BLExpert.expert.ID.ToString();
                txtName.Text = BLExpert.expert.ep_Name;
                txtNationalCode.Text = BLExpert.expert.ep_NationalCode;
                txtAddress.Text = BLExpert.expert.ep_Address;

                radActive.IsChecked = BLExpert.expert.ep_IsActive;
                radDeActive.IsChecked = !BLExpert.expert.ep_IsActive;

                foreach (var item in BLExpert.expert.tbl_ExpertPhone.ToList())
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

                btnSave.Visibility = (user.GetUpdateAccessSection("Expert", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
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
            string address = txtAddress.Text;
            bool isActive = radActive.IsChecked.Value;

            try
            {
                if (!OnEdit)
                {
                    expert.Insert(name, nationalCode, address,isActive,value);
                    int id = expert.getLastID() - 1;
                    if (grdMain.Items.Count != 0)
                    {
                        foreach (var item in lst)
                        {
                            expert.InsertPhone(id, item.Caption, item.Value);
                        }
                    }
                }
                else
                {
                    expert.Update(BLExpert.expert.ID, name, nationalCode, address,isActive,value);
                    expert.DeletePhone(BLExpert.expert.ID);

                    foreach (var item in lst)
                    {
                        expert.InsertPhone(BLVisitor.visitor.ID, item.Caption, item.Value);
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
                expert.Delete(BLExpert.expert.ID);

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

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (user.GetDeleteAccessSection("Expert", BLUserClass.UserID));
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnDelete_Click(null, null);
        }

        private void InsertCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!OnEdit)
                e.CanExecute = (user.GetInsertAccessSection("Expert", BLUserClass.UserID));
            else
                e.CanExecute = (user.GetUpdateAccessSection("Expert", BLUserClass.UserID));
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

        private void grdMain_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Delete)
                    grdMain.Items.Remove(grdMain.SelectedItem);
            }
            catch (Exception)
            {

            }
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
                value = (byte)slidNumber.Value;
                lblDailyTimeWork.Text = string.Format(str, value);
            }
            catch (Exception)
            {

            }
        }
    }
}
