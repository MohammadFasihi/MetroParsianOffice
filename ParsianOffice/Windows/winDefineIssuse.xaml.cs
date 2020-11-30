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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winDefineIssuse.xaml
    /// </summary>
    public partial class winDefineIssuse : Window
    {
        public winDefineIssuse()
        {
            InitializeComponent();
        }
        bool IsEdit = false;
        public MetroWindow main;

        BLUserClass user = new BLUserClass();

        void Clear()
        {
            BLIssuse issue = new BLIssuse();
            txtID.Text = issue.LastIssueID().ToString();

            txtName.Text = "";
            txtPrice.Text = "0";

            txtName.Focus();

            IsEdit = false;

            btnSave.Visibility = (user.GetInsertAccessSection("Customer", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            btnDelete.Visibility = (user.GetDeleteAccessSection("Customer", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = double.Parse(ConfigClass.getSetting("Issue", "Width"));
                this.Height = double.Parse(ConfigClass.getSetting("Issue", "Height"));

                this.Left = double.Parse(ConfigClass.getSetting("Issue", "LocationX"));
                this.Top = double.Parse(ConfigClass.getSetting("Issue", "LocationY"));
            }
            catch (Exception)
            {

            }

            Clear();
        }

        private void txtPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtPrice.Text))
                {
                    int number = StringClass.RemoveNumberSeparator(txtPrice.Text);
                    string str = StringClass.NumberFormatString(number);
                    txtPrice.Text = str;
                    txtPrice.Select(str.Length, 0);
                    lblCharachter.Text = StringClass.GetNumberToCharachter(number);
                }
            }
            catch (Exception)
            {
                txtPrice.Text = txtPrice.Text.Substring(0, txtPrice.Text.Length - 1);
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BLIssuse issue = new BLIssuse();
            if (!String.IsNullOrWhiteSpace(txtName.Text) || !String.IsNullOrEmpty(txtName.Text))
            {
                if (IsEdit)
                {
                    issue.Update(txtID.Text, txtName.Text, txtPrice.Text);
                    Clear();
                }
                else
                {
                    issue.Insert(txtName.Text, txtPrice.Text);
                    Clear();
                }
            }
            else
            {

                MetroDialogSettings config = new MetroDialogSettings();

                config.AffirmativeButtonText = "بله";
                config.AnimateHide = true;
                config.AnimateShow = true;
                config.ColorScheme = MetroDialogColorScheme.Theme;
                config.DefaultButtonFocus = MessageDialogResult.Affirmative;
                config.DialogResultOnCancel = MessageDialogResult.Canceled;
                config.NegativeButtonText = "خیر";
                config.FirstAuxiliaryButtonText = "انصراف";

                var res = await main.ShowMessageAsync("", "نام را وارد کنید", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, config);

                //MessageBox.Show("نام را وارد کنید", "نام", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            BLIssuse issue = new BLIssuse();
            issue.Delete(txtID.Text);

            Clear();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Add)
                txtPrice.Text += "000";
            if (e.Key == Key.Subtract)
                txtPrice.Text += "00";
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            winDisplay display = new winDisplay();
            display.tableName = "Issue";
            display.ShowDialog();

            if(BLIssuse.issue != null)
            {
                txtID.Text = BLIssuse.issue.ID.ToString();
                txtName.Text = BLIssuse.issue.issue_Name;
                txtPrice.Text = BLIssuse.issue.issue_Price.Value.ToString();

                IsEdit = true;

                btnSave.Visibility = (user.GetUpdateAccessSection("Customer", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {
            if((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.Space)
            {
                btnSearch_Click(this, null);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (AppSettingClass.setting.SaveSizes)
            {
                ConfigClass.setSetting("Issue", "Width", this.ActualWidth);
                ConfigClass.setSetting("Issue", "Height", this.ActualHeight);

                ConfigClass.setSetting("Issue", "LocationX", this.Left);
                ConfigClass.setSetting("Issue", "LocationY", this.Top);
            }


        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = user.GetDeleteAccessSection("Issue",BLUserClass.UserID);
        }

        private void InsertCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSave_Click(sender, e);
        }

        private void InsertCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!IsEdit)
                e.CanExecute = user.GetInsertAccessSection("Issue", BLUserClass.UserID);
            else
                e.CanExecute = user.GetUpdateAccessSection("Issue", BLUserClass.UserID);
        }

        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

    }
}
