using Arash;
using BL;
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
using DA;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winFollowUp.xaml
    /// </summary>
    public partial class winFollowUp : Window
    {
        public winFollowUp()
        {
            InitializeComponent();
        }
        BLUserClass user = new BLUserClass();

        bool OnEdit = false;
        public winMain main;
        public static int ID;
        string section = "FollowUp";
        void Clear()
        {
            DA.Entities db = new DA.Entities();
            txtID.Text = (db.tbl_FollowUP.Select(p => p.ID).DefaultIfEmpty(0).Max() + 1).ToString();
            cmbCustomer.SelectedIndex = -1;
            dpDate.SelectedDate = PersianDate.Today;
            txtText.Text = "";

            btnCancel.Content = "خروج";
            OnEdit = false;

            btnSave.Visibility = (user.GetInsertAccessSection("FollowUp", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            btnDelete.Visibility = (user.GetDeleteAccessSection("FollowUp", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
        }

        void LoadCustomer()
        {
            BLCustomer customer = new BLCustomer();
            cmbCustomer.ItemsSource = customer.GetCustomerCobmo();
            cmbCustomer.DisplayMember = "cu_Name";
            cmbCustomer.ValueMember = "cu_ID";
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Entities db = new Entities();
            int id = int.Parse(txtID.Text);
            if (!OnEdit)
            {
                if (cmbCustomer.SelectedIndex != -1)
                {
                    int cuID = int.Parse(cmbCustomer.EditValue.ToString());
                    string Date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(dpDate.SelectedDate.ToString()));

                    tbl_FollowUP follow = new tbl_FollowUP();
                    follow.cu_ID = cuID;
                    follow.Date = Date;
                    follow.fl_Date = dpDate.SelectedDate.ToDateTime();
                    follow.fl_Text = txtText.Text;
                    follow.user_ID = BLUserClass.UserID;

                    db.tbl_FollowUP.Add(follow);

                    db.SaveChangesAsync();

                    Clear();
                }
                else
                {
                    MessageBox.Show("مشتری انتخاب نشده", "خطا در برنامه", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (cmbCustomer.SelectedIndex != -1)
                {
                    int cuID = int.Parse(cmbCustomer.EditValue.ToString());
                    var query = (from q in db.tbl_FollowUP where q.ID == id select q).Single();
                    string Date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(dpDate.SelectedDate.ToString()));

                    query.cu_ID = cuID;
                    query.Date = Date;
                    query.fl_Date = dpDate.SelectedDate.ToDateTime();
                    query.fl_Text = txtText.Text;
                    query.changedUserID = BLUserClass.UserID;

                    db.SaveChangesAsync();

                    Clear();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Entities db = new Entities();
            int id = int.Parse(txtID.Text);

            var query = db.tbl_FollowUP.Where(q => q.ID == id);

            if (query.Count() != 0)
            {
                db.tbl_FollowUP.Remove(query.Single());

                db.SaveChangesAsync();

                Clear();
            }
            else
            {
                MessageBox.Show("پیگیری مورد نظر یافت نشد", "خطا در برنامه", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
                Clear();
            else
                Close();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ID = 0;

            Entities db = new Entities();

            winDisplay display = new winDisplay();
            display.tableName = "FollowUp";
            display.ShowDialog();

            if (ID != 0)
            {
                var followUp = db.tbl_FollowUP.Where(q => q.ID == ID).Single();

                cmbCustomer.EditValue = followUp.cu_ID;
                txtID.Text = followUp.ID.ToString();
                dpDate.SelectedDate = PersianDate.Parse(followUp.Date);
                txtText.Text = followUp.fl_Text;

                btnCancel.Content = "انصراف";
                OnEdit = true;

                btnSave.Visibility = (user.GetUpdateAccessSection("FollowUp", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = double.Parse(ConfigClass.getSetting(section, "Width"));
                this.Height = double.Parse(ConfigClass.getSetting(section, "Height"));

                this.Left = double.Parse(ConfigClass.getSetting(section, "LocationX"));
                this.Top = double.Parse(ConfigClass.getSetting(section, "LocationY"));
            }
            catch (Exception)
            {

            }

            LoadCustomer();
            Clear();
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (user.GetDeleteAccessSection("FollowUp", BLUserClass.UserID));
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadCustomer();
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
            if(!OnEdit)
                e.CanExecute = (user.GetInsertAccessSection("FollowUp", BLUserClass.UserID));
            else
                e.CanExecute = (user.GetUpdateAccessSection("FollowUp", BLUserClass.UserID));
        }

        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (AppSettingClass.setting.SaveSizes)
            {
                ConfigClass.setSetting(section, "Width", this.ActualWidth);
                ConfigClass.setSetting(section, "Height", this.ActualHeight);

                ConfigClass.setSetting(section, "LocationX", this.Left);
                ConfigClass.setSetting(section, "LocationY", this.Top);
            }
        }
    }
}
