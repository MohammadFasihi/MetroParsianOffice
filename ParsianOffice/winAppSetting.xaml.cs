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

namespace ParsianOffice
{
    /// <summary>
    /// Interaction logic for winAppSetting.xaml
    /// </summary>
    public partial class winAppSetting : Window
    {
        public winAppSetting()
        {
            InitializeComponent();
        }

        AppSettingClass setting = new AppSettingClass(BLUserClass.UserID);

        string section = "AppSetting";
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

            AppSettingClass setting = new AppSettingClass();

            setting.appSetting.PagingQty = byte.Parse(txtPagingQty.Text);
            setting.appSetting.CashPaymentText = txtCashPaymentText.Text;
            setting.appSetting.MixPaymentText = txtMixPaymentText.Text;

            setting.appSetting.AutoDefaultPaymentText = chkAutoDefualt.IsChecked.Value;
            setting.appSetting.LoadVisitorInCustomerDefine = chkLoadVisitorInCustomerDefine.IsChecked.Value;
            setting.appSetting.GridPaging = chkPaging.IsChecked.Value;
            setting.appSetting.WarningRestInApointment = chkResyWarninigInApointmentInsert.IsChecked.Value;
            setting.appSetting.SaveGridLayout = chkSaveGridLayout.IsChecked.Value ;
            setting.appSetting.SaveSizes = chkSaveSize.IsChecked.Value;
            setting.appSetting.InsertContractAfterOrder = chkSetContractInOrderInsert.IsChecked.Value;
            setting.appSetting.ShowApointmentInStart = chkShowApontmentInStart.IsChecked.Value;
            setting.appSetting.ShowApointmentInInsert = chkShowAppointmentsInInsert.IsChecked.Value ;
            setting.appSetting.ShowFollowInStart = chkShowFollowInStart.IsChecked.Value;
            setting.appSetting.ShowNextApointmentInExit = chkShowNextApontmentInExit.IsChecked.Value;
            setting.appSetting.ShowNextFollowInExit = chkShowNextFollowInExit.IsChecked.Value;
            setting.appSetting.ShowNextRestInExit = chkShowNextRestInExit.IsChecked.Value;
            setting.appSetting.ShowRestInStart = chkShowRestInStart.IsChecked.Value;
            setting.appSetting.TimeInterval = txtTimeInterval.Text;

            setting.UpdateAppSetting(BLUserClass.UserID);
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            winChangePassword changePass = new winChangePassword();
            changePass.ShowDialog();
        }
    }
}
