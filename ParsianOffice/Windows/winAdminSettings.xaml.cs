using BL;
using DA;
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
using System.Data.SqlClient;
using System.Data;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winAdminSettings.xaml
    /// </summary>
    public partial class winAdminSettings : Window
    {
        public winAdminSettings()
        {
            InitializeComponent();
        }

        AppSettingClass setting = new AppSettingClass(BLUserClass.UserID);

        string section = "AdminSetting";

        public void ApplyAppSetting()
        {
            try
            {
                string stpName = "";

                int id;

                if (radAllUser.IsChecked.Value)
                    stpName = "sp_UpdateAllUserSetting";
                else
                    stpName = "sp_UpdateOneUserSetting";

                if (radOneUser.IsChecked.Value)
                    id = int.Parse(cmbUser.SelectedValue.ToString());
                else
                    id = -1;

                AppSettingClass setting = new AppSettingClass();

                SqlConnection con = new SqlConnection(ConnectionStringClass.GetADOConnectionString());
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandText = stpName;
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@PagingQty", byte.Parse(txtPagingQty.Text));
                cmd.Parameters.AddWithValue("@CashPaymentText", txtCashPaymentText.Text);
                cmd.Parameters.AddWithValue("@MixPaymentText", txtMixPaymentText.Text);

                cmd.Parameters.AddWithValue("@AutoDefaultPaymentText", chkAutoDefualt.IsChecked.Value);
                cmd.Parameters.AddWithValue("@LoadVisitorInCustomerDefine", chkLoadVisitorInCustomerDefine.IsChecked.Value);
                cmd.Parameters.AddWithValue("@GridPaging", chkPaging.IsChecked.Value);
                cmd.Parameters.AddWithValue("@WarningRestInApointment", chkResyWarninigInApointmentInsert.IsChecked.Value);
                cmd.Parameters.AddWithValue("@SaveGridLayout", chkSaveGridLayout.IsChecked.Value);
                cmd.Parameters.AddWithValue("@SaveSizes", chkSaveSize.IsChecked.Value);
                cmd.Parameters.AddWithValue("@InsertContractAfterOrder", chkSetContractInOrderInsert.IsChecked.Value);
                cmd.Parameters.AddWithValue("@ShowApointmentInStart", chkShowApontmentInStart.IsChecked.Value);
                cmd.Parameters.AddWithValue("@ShowApointmentInInsert", chkShowAppointmentsInInsert.IsChecked.Value);
                cmd.Parameters.AddWithValue("@ShowFollowInStart", chkShowFollowInStart.IsChecked.Value);
                cmd.Parameters.AddWithValue("@ShowNextApointmentInExit", chkShowNextApontmentInExit.IsChecked.Value);
                cmd.Parameters.AddWithValue("@ShowNextFollowInExit", chkShowNextFollowInExit.IsChecked.Value);
                cmd.Parameters.AddWithValue("@ShowNextRestInExit", chkShowNextRestInExit.IsChecked.Value);
                cmd.Parameters.AddWithValue("@ShowRestInStart", chkShowRestInStart.IsChecked.Value);
                cmd.Parameters.AddWithValue("@TimeInterval", txtTimeInterval.Text);
                cmd.Parameters.AddWithValue("@userID", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "خطا در سیستم", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ApplyPrintSetting()
        {
            try
            {
                string stpName = "";

                int designID = int.Parse(cmbDesign.SelectedValue.ToString());

                int id;

                if (radAllUser.IsChecked.Value)
                    stpName = "sp_UpdateAllUserPrintingSetting";
                else
                    stpName = "sp_UpdateOneUserPrintingSetting";

                if (radOneUser.IsChecked.Value)
                    id = int.Parse(cmbUser.SelectedValue.ToString());
                else
                    id = -1;

                AppSettingClass setting = new AppSettingClass();

                SqlConnection con = new SqlConnection(ConnectionStringClass.GetADOConnectionString());
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandText = stpName;
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@PreviewBeforePrint", chkPreviewBeforePrint.IsChecked);
                cmd.Parameters.AddWithValue("@LoadDesignBeforePrint", chkLoadDesignPrint.IsChecked);
                cmd.Parameters.AddWithValue("@PrintQty", txtPrintQty.Text);
                cmd.Parameters.AddWithValue("@DefaultDesignID", designID);
                cmd.Parameters.AddWithValue("@userID", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "خطا در سیستم", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            ApplyAppSetting();
            ApplyPrintSetting();

            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = double.Parse(ConfigClass.getSetting(section, "Width"));
                this.Height = double.Parse(ConfigClass.getSetting(section, "Height"));

                this.Left = double.Parse(ConfigClass.getSetting(section, "LocationX"));
                this.Top = double.Parse(ConfigClass.getSetting(section, "LocationY"));

                DA.Entities db = new Entities();

                cmbUser.ItemsSource = db.tbl_Users.ToList();
                cmbUser.DisplayMemberPath = "username";
                cmbUser.SelectedValuePath = "ID";
                cmbUser.SelectedIndex = 0;

                cmbDesign.ItemsSource = db.tbl_PrintDesigns.ToList();
                cmbDesign.DisplayMemberPath = "design_Name";
                cmbDesign.SelectedValuePath = "ID";
                cmbDesign.SelectedValue = PrintSettingClass.setting.DefaultDesignID;
            }
            catch (Exception)
            {

            }
            //Load App Setting
            txtPagingQty.Text = AppSettingClass.setting.PagingQty.ToString();
            txtCashPaymentText.Text = AppSettingClass.setting.CashPaymentText;
            txtMixPaymentText.Text = AppSettingClass.setting.MixPaymentText;

            chkAutoDefualt.IsChecked = AppSettingClass.setting.AutoDefaultPaymentText;
            chkLoadVisitorInCustomerDefine.IsChecked = AppSettingClass.setting.LoadVisitorInCustomerDefine;
            chkPaging.IsChecked = AppSettingClass.setting.GridPaging;
            chkResyWarninigInApointmentInsert.IsChecked = AppSettingClass.setting.WarningRestInApointment;
            chkSaveGridLayout.IsChecked = AppSettingClass.setting.SaveGridLayout;
            chkSaveSize.IsChecked = AppSettingClass.setting.SaveSizes;
            chkSetContractInOrderInsert.IsChecked = AppSettingClass.setting.InsertContractAfterOrder;
            chkShowApontmentInStart.IsChecked = AppSettingClass.setting.ShowApointmentInStart;
            chkShowAppointmentsInInsert.IsChecked = AppSettingClass.setting.ShowApointmentInInsert;
            chkShowFollowInStart.IsChecked = AppSettingClass.setting.ShowFollowInStart;
            chkShowNextApontmentInExit.IsChecked = AppSettingClass.setting.ShowNextApointmentInExit;
            chkShowNextFollowInExit.IsChecked = AppSettingClass.setting.ShowNextFollowInExit;
            chkShowNextRestInExit.IsChecked = AppSettingClass.setting.ShowNextRestInExit;
            chkShowRestInStart.IsChecked = AppSettingClass.setting.ShowRestInStart;
            txtTimeInterval.Text = AppSettingClass.setting.TimeInterval;
            //Load Peint Settings
            chkLoadDesignPrint.IsChecked = PrintSettingClass.setting.LoadDesignBeforePrint;
            chkPreviewBeforePrint.IsChecked = PrintSettingClass.setting.PreviewBeforePrint;
            txtPrintQty.Text = PrintSettingClass.setting.PrintQty.ToString();
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
        }

        private void radOneUser_Checked(object sender, RoutedEventArgs e)
        {
            radAllUser.IsChecked = !radOneUser.IsChecked.Value;
        }

        private void radAllUser_Checked(object sender, RoutedEventArgs e)
        {
            radOneUser.IsChecked = !radAllUser.IsChecked.Value;
        }
    }
}
