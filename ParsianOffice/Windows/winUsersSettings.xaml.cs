using BL;
using DA;
using ParsianOffice.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for winUsersSettings.xaml
    /// </summary>
    public partial class winUsersSettings : Window
    {
        public winUsersSettings()
        {
            InitializeComponent();
        }

        bool OnEdit = false;
        string section = "UserSetting";
        int userID;
        public winMain main;

        BLUserClass userClass = new BLUserClass();

        public string EncryptPassword(string str)
        {
            SHA1Cng hash = new SHA1Cng();
            byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(str));
            byte[] ResultFinal = hash.ComputeHash(result);
            string pass = Convert.ToBase64String(ResultFinal);

            return pass;
        }

        public void SelectAllAccess(bool value)
        {
             chkIssueAccess.IsChecked = value;
            gbIssue.IsEnabled = value;
            chkIssueInsert.IsChecked = value;
            chkIssueUpdate.IsChecked = value;
            chkIssueDelete.IsChecked = value;

            chkVisitorAccess.IsChecked = value;
            gbVisitor.IsEnabled = value;
            chkVisitorInsert.IsChecked = value;
            chkVisitorUpdate.IsChecked = value;
            chkVisitorDelete.IsChecked = value;

            chkExpertAccess.IsChecked = value;
            gbExpert.IsEnabled = value;
            chkExpertInsert.IsChecked = value;
            chkExpertUpdate.IsChecked = value;
            chkExpertDelete.IsChecked = value;

            chkCustomerAccess.IsChecked = value;
            gbCustomer.IsEnabled = value;
            chkCustomerInsert.IsChecked = value;
            chkCustomerUpdate.IsChecked = value;
            chkCustomerDelete.IsChecked = value;

            chkApointmentAccess.IsChecked = value;
            gbApointment.IsEnabled = value;
            chkApointmentInsert.IsChecked = value;
            chkApointmentUpdate.IsChecked = value;
            chkApointmentDelete.IsChecked = value;

            chkFollowUpAccess.IsChecked = value;
            gbFollowUp.IsEnabled = value;
            chkFollowUpInsert.IsChecked = value;
            chkFollowUpUpdate.IsChecked = value;
            chkFollowUpDelete.IsChecked = value;

            chkSaleAccess.IsChecked = value;
            gbSale.IsEnabled = value;
            chkSaleInsert.IsChecked = value;
            chkSaleUpdate.IsChecked = value;
            chkSaleDelete.IsChecked = value;

            chkReturnAccess.IsChecked = value;
            gbReturn.IsEnabled = value;
            chkReturnInsert.IsChecked = value;
            chkReturnUpdate.IsChecked = value;
            chkReturnDelete.IsChecked = value;

            chkRestAccess.IsChecked = value;
            gbRest.IsEnabled = value;
            chkRestInsert.IsChecked = value;
            chkRestUpdate.IsChecked = value;
            chkRestDelete.IsChecked = value;

            chkContractAccess.IsChecked = value;
            gbContract.IsEnabled = value;
            chkContractInsert.IsChecked = value;
            chkContractUpdate.IsChecked = value;
            chkContractDelete.IsChecked = value;


            chkExpertApointmentReport.IsChecked = value;
            chkTodayApointmentReport.IsChecked = value;
            chkNextDayApointmentReport.IsChecked = value;
            chkCustomerApointmentReport.IsChecked = value;
            chkFollowUpReport.IsChecked = value;
            chkTodayFollowUpReport.IsChecked = value;
            chkRestReport.IsChecked = value;
            chkVisitorSaleReport.IsChecked = value;
            chkSaleReport.IsChecked = value;

            chkBackup.IsChecked = value;
            chkRestor.IsChecked = value;
            chkUsersSettings.IsChecked = value;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            OnEdit = false;

            string kind = (radAdmin.IsChecked.Value) ? "Admin" : "User";

            if (radAdmin.IsChecked.Value)
                SelectAllAccess(true);

            bool issue = userClass.GetUserAccess(userID,"Issue");
            bool Visitor = userClass.GetUserAccess(userID, "Visitor");
            bool Expert = userClass.GetUserAccess(userID, "Expert");
            bool Customer = userClass.GetUserAccess(userID, "Customer");
            bool Apointment = userClass.GetUserAccess(userID, "Apointment");
            bool FollowUp = userClass.GetUserAccess(userID, "FollowUp");
            bool Order = userClass.GetUserAccess(userID, "Order");
            bool OrderReturn = userClass.GetUserAccess(userID, "OrderReturn");
            bool Rest = userClass.GetUserAccess(userID, "Rest");
            bool Contract = userClass.GetUserAccess(userID, "Contract");
            bool ApointmentReport = userClass.GetUserAccess(userID, "ApointmentReport");
            bool TodayApointmentReport = userClass.GetUserAccess(userID, "TodayApointmentReport");
            bool NextDayApointmentReport = userClass.GetUserAccess(userID, "NextDayApointmentReport");
            bool CustomerApointmentReport = userClass.GetUserAccess(userID, "CustomerApointmentReport");
            bool FollowUpReport = userClass.GetUserAccess(userID, "FollowUpReport");
            bool TodayFollowUpReport = userClass.GetUserAccess(userID, "TodayFollowUpReport");
            bool VacationReport = userClass.GetUserAccess(userID, "VacationReport");
            bool OrderReport = userClass.GetUserAccess(userID, "OrderReport");
            bool VisitorReport = userClass.GetUserAccess(userID, "VisitorReport");
            bool Backup = userClass.GetUserAccess(userID, "Backup");
            bool Restore = userClass.GetUserAccess(userID, "Restore");
            bool Users = userClass.GetUserAccess(userID, "Users");

            if (issue && chkIssueAccess.IsChecked.Value) // Edit Access
                userClass.UpdateUserAccess(userID, "Issue", chkIssueInsert.IsChecked.Value, chkIssueUpdate.IsChecked.Value, chkIssueDelete.IsChecked.Value, true);
            else if (issue && !chkIssueAccess.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "Issue");
            else if (!issue && chkIssueAccess.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "Issue", chkIssueInsert.IsChecked.Value, chkIssueUpdate.IsChecked.Value, chkIssueDelete.IsChecked.Value, true);

            if (Visitor && chkVisitorAccess.IsChecked.Value) // Edit Access
                userClass.UpdateUserAccess(userID, "Visitor", chkVisitorInsert.IsChecked.Value, chkVisitorUpdate.IsChecked.Value, chkVisitorDelete.IsChecked.Value, true);
            else if (Visitor && !chkVisitorAccess.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "Visitor");
            else if (!Visitor && chkVisitorAccess.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "Visitor", chkVisitorInsert.IsChecked.Value, chkVisitorUpdate.IsChecked.Value, chkVisitorDelete.IsChecked.Value, true);

            if (Expert && chkExpertAccess.IsChecked.Value) // Edit Access
                userClass.UpdateUserAccess(userID, "Expert", chkExpertInsert.IsChecked.Value, chkExpertUpdate.IsChecked.Value, chkExpertDelete.IsChecked.Value, true);
            else if (Expert && !chkExpertAccess.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "Expert");
            else if (!Expert && chkExpertAccess.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "Expert", chkExpertInsert.IsChecked.Value, chkExpertUpdate.IsChecked.Value, chkExpertDelete.IsChecked.Value, true);

            if (Customer && chkCustomerAccess.IsChecked.Value) // Edit Access
                userClass.UpdateUserAccess(userID, "Customer", chkCustomerInsert.IsChecked.Value, chkCustomerUpdate.IsChecked.Value, chkCustomerDelete.IsChecked.Value, true);
            else if (Customer && !chkCustomerAccess.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "Customer");
            else if (!Customer && chkCustomerAccess.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "Customer", chkCustomerInsert.IsChecked.Value, chkCustomerUpdate.IsChecked.Value, chkCustomerDelete.IsChecked.Value, true);

            if (Apointment && chkApointmentAccess.IsChecked.Value) // Edit Access
                userClass.UpdateUserAccess(userID, "Apointment", chkApointmentInsert.IsChecked.Value, chkApointmentUpdate.IsChecked.Value, chkApointmentDelete.IsChecked.Value, true);
            else if (Apointment && !chkApointmentAccess.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "Apointment");
            else if (!Apointment && chkApointmentAccess.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "Apointment", chkApointmentInsert.IsChecked.Value, chkApointmentUpdate.IsChecked.Value, chkApointmentDelete.IsChecked.Value, true);

            if (FollowUp && chkFollowUpAccess.IsChecked.Value) // Edit Access
                userClass.UpdateUserAccess(userID, "FollowUp", chkFollowUpInsert.IsChecked.Value, chkFollowUpUpdate.IsChecked.Value, chkFollowUpDelete.IsChecked.Value, true);
            else if (FollowUp && !chkFollowUpAccess.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "FollowUp");
            else if (!FollowUp && chkFollowUpAccess.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "FollowUp", chkFollowUpInsert.IsChecked.Value, chkFollowUpUpdate.IsChecked.Value, chkFollowUpDelete.IsChecked.Value, true);

            if (Order && chkSaleAccess.IsChecked.Value) // Edit Access
                userClass.UpdateUserAccess(userID, "Order", chkSaleInsert.IsChecked.Value, chkSaleUpdate.IsChecked.Value, chkSaleDelete.IsChecked.Value, true);
            else if (Order && !chkSaleAccess.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "Order");
            else if (!Order && chkSaleAccess.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "Order", chkSaleInsert.IsChecked.Value, chkSaleUpdate.IsChecked.Value, chkSaleDelete.IsChecked.Value, true);

            if (OrderReturn && chkReturnAccess.IsChecked.Value) // Edit Access
                userClass.UpdateUserAccess(userID, "OrderReturn", chkReturnInsert.IsChecked.Value, chkReturnUpdate.IsChecked.Value, chkReturnDelete.IsChecked.Value, true);
            else if (OrderReturn && !chkReturnAccess.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "OrderReturn");
            else if (!OrderReturn && chkReturnAccess.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "OrderReturn", chkReturnInsert.IsChecked.Value, chkReturnUpdate.IsChecked.Value, chkReturnDelete.IsChecked.Value, true);

            if (Rest && chkRestAccess.IsChecked.Value) // Edit Access
                userClass.UpdateUserAccess(userID, "Rest", chkRestInsert.IsChecked.Value, chkRestUpdate.IsChecked.Value, chkRestDelete.IsChecked.Value, true);
            else if (Rest && !chkRestAccess.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "Rest");
            else if (!Rest && chkRestAccess.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "Rest", chkRestInsert.IsChecked.Value, chkRestUpdate.IsChecked.Value, chkRestDelete.IsChecked.Value, true);

            if (Contract && chkContractAccess.IsChecked.Value) // Edit Access
                userClass.UpdateUserAccess(userID, "Contract", chkContractInsert.IsChecked.Value, chkContractUpdate.IsChecked.Value, chkContractDelete.IsChecked.Value, true);
            else if (Contract && !chkContractAccess.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "Contract");
            else if (!Contract && chkContractAccess.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "Contract", chkContractInsert.IsChecked.Value, chkContractUpdate.IsChecked.Value, chkContractDelete.IsChecked.Value, true);

            if (ApointmentReport && !chkExpertApointmentReport.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "ApointmentReport");
            else if (!ApointmentReport && chkExpertApointmentReport.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "ApointmentReport", chkExpertApointmentReport.IsChecked.Value, chkExpertApointmentReport.IsChecked.Value, chkExpertApointmentReport.IsChecked.Value, true);

            if (TodayApointmentReport && !chkTodayApointmentReport.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "TodayApointmentReport");
            else if (!TodayApointmentReport && chkTodayApointmentReport.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "TodayApointmentReport", chkTodayApointmentReport.IsChecked.Value, chkTodayApointmentReport.IsChecked.Value, chkTodayApointmentReport.IsChecked.Value, true);

            if (NextDayApointmentReport && !chkNextDayApointmentReport.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "NextDayApointmentReport");
            else if (!NextDayApointmentReport && chkNextDayApointmentReport.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "NextDayApointmentReport", chkNextDayApointmentReport.IsChecked.Value, chkNextDayApointmentReport.IsChecked.Value, chkNextDayApointmentReport.IsChecked.Value, true);

            if (CustomerApointmentReport && !chkCustomerApointmentReport.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "CustomerApointmentReport");
            else if (!CustomerApointmentReport && chkCustomerApointmentReport.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "CustomerApointmentReport", chkCustomerApointmentReport.IsChecked.Value, chkCustomerApointmentReport.IsChecked.Value, chkCustomerApointmentReport.IsChecked.Value, true);

            if (FollowUpReport && !chkFollowUpReport.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "FollowUpReport");
            else if (!FollowUpReport && chkFollowUpReport.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "FollowUpReport", chkFollowUpReport.IsChecked.Value, chkFollowUpReport.IsChecked.Value, chkFollowUpReport.IsChecked.Value, true);

            if (TodayFollowUpReport && !chkTodayFollowUpReport.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "TodayFollowUpReport");
            else if (!TodayFollowUpReport && chkTodayFollowUpReport.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "TodayFollowUpReport", chkTodayFollowUpReport.IsChecked.Value, chkTodayFollowUpReport.IsChecked.Value, chkTodayFollowUpReport.IsChecked.Value, true);

            if (VacationReport && !chkRestReport.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "VacationReport");
            else if (!VacationReport && chkRestReport.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "VacationReport", chkRestReport.IsChecked.Value, chkRestReport.IsChecked.Value, chkRestReport.IsChecked.Value, true);

            if (OrderReport && !chkVisitorSaleReport.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "OrderReport");
            else if (!OrderReport && chkVisitorSaleReport.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "OrderReport", chkVisitorSaleReport.IsChecked.Value, chkVisitorSaleReport.IsChecked.Value, chkVisitorSaleReport.IsChecked.Value, true);

            if (VisitorReport && !chkSaleReport.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "VisitorReport");
            else if (!VisitorReport && chkSaleReport.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "VisitorReport", chkSaleReport.IsChecked.Value, chkSaleReport.IsChecked.Value, chkSaleReport.IsChecked.Value, true);

            if (Backup && !chkBackup.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "Backup");
            else if (!Backup && chkBackup.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "Backup", chkBackup.IsChecked.Value, chkBackup.IsChecked.Value, chkBackup.IsChecked.Value, true);

            if (Restore && !chkRestor.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "Restore");
            else if (!Restore && chkRestor.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "Restore", chkRestor.IsChecked.Value, chkRestor.IsChecked.Value, chkRestor.IsChecked.Value, true);

            if (Users && !chkUsersSettings.IsChecked.Value) //Delete Access
                userClass.DeleteUserAccess(userID, "Users");
            else if (!Users && chkUsersSettings.IsChecked.Value) //Insert Access
                userClass.InsertUserAccess(userID, "Users", chkUsersSettings.IsChecked.Value, chkUsersSettings.IsChecked.Value, chkUsersSettings.IsChecked.Value, true);


            string pass = EncryptPassword(txtPassword.Password);

            userClass.UpdateUser(userID, txtPassword.Password, pass, chkActive.IsChecked.Value, kind);

            SelectAllAccess(false);
            txtUsername.Text = "";
            txtPassword.Password = "";
            radAdmin.IsChecked = false;
            radUser.IsChecked = false;
            chkActive.IsChecked = false;

            btnSave.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
                userClass.Delete(userID);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            AddUserWizardForm addNewUser = new AddUserWizardForm();
            addNewUser.ShowDialog();
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

            BLUserClass user = new BLUserClass();

            lst.ItemsSource = user.GetAllUsers();
            lst.DisplayMemberPath = "username";
            lst.SelectedValuePath = "ID";

            btnSave.IsEnabled = false;
            btnDelete.IsEnabled = false;
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

        private void chkIssueAccess_Checked(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
            {
                gbIssue.IsEnabled = chkIssueAccess.IsChecked.Value;
                chkIssueInsert.IsChecked = userClass.GetInsertAccessSection("Issue", userID);
                chkIssueUpdate.IsChecked = userClass.GetUpdateAccessSection("Issue", userID);
                chkIssueDelete.IsChecked = userClass.GetDeleteAccessSection("Issue", userID);
            }
            else
            {
                gbIssue.IsEnabled = true;
                chkIssueInsert.IsChecked = true;
                chkIssueUpdate.IsChecked = true;
                chkIssueDelete.IsChecked = true;
            }
        }

        private void chkVisitorAccess_Checked(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
            {
                gbVisitor.IsEnabled = chkVisitorAccess.IsChecked.Value;
                chkVisitorInsert.IsChecked = userClass.GetInsertAccessSection("Visitor", userID);
                chkVisitorUpdate.IsChecked = userClass.GetUpdateAccessSection("Visitor", userID);
                chkVisitorDelete.IsChecked = userClass.GetDeleteAccessSection("Visitor", userID);
            }
            else
            {
                gbVisitor.IsEnabled = true;
                chkVisitorInsert.IsChecked = true;
                chkVisitorUpdate.IsChecked = true;
                chkVisitorDelete.IsChecked = true;
            }
        }

        private void chkExpertAccess_Checked(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
            {
                gbExpert.IsEnabled = chkExpertAccess.IsChecked.Value;
                chkExpertInsert.IsChecked = userClass.GetInsertAccessSection("Expert", userID);
                chkExpertUpdate.IsChecked = userClass.GetUpdateAccessSection("Expert", userID);
                chkExpertDelete.IsChecked = userClass.GetDeleteAccessSection("Expert", userID);
            }
            else
            {
                gbExpert.IsEnabled = true;
                chkExpertInsert.IsChecked = true;
                chkExpertUpdate.IsChecked = true;
                chkExpertDelete.IsChecked = true;
            }
        }

        private void chkCustomerAccess_Checked(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
            {
                gbCustomer.IsEnabled = chkCustomerAccess.IsChecked.Value;
                chkCustomerInsert.IsChecked = userClass.GetInsertAccessSection("Customer", userID);
                chkCustomerUpdate.IsChecked = userClass.GetUpdateAccessSection("Customer", userID);
                chkCustomerDelete.IsChecked = userClass.GetDeleteAccessSection("Customer", userID);
            }
            else
            {
                gbCustomer.IsEnabled = true;
                chkCustomerInsert.IsChecked = true;
                chkCustomerUpdate.IsChecked = true;
                chkCustomerDelete.IsChecked = true;
            }
        }

        private void chkApointmentAccess_Checked(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
            {
                gbApointment.IsEnabled = chkApointmentAccess.IsChecked.Value;
                chkApointmentInsert.IsChecked = userClass.GetInsertAccessSection("Apointment", userID);
                chkApointmentUpdate.IsChecked = userClass.GetUpdateAccessSection("Apointment", userID);
                chkApointmentDelete.IsChecked = userClass.GetDeleteAccessSection("Apointment", userID);
            }
            else
            {
                gbApointment.IsEnabled = true;
                chkApointmentInsert.IsChecked = true;
                chkApointmentUpdate.IsChecked = true;
                chkApointmentDelete.IsChecked = true;
            }
        }

        private void chkFollowUpAccess_Checked(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
            {
                gbFollowUp.IsEnabled = chkFollowUpAccess.IsChecked.Value;
                chkFollowUpInsert.IsChecked = userClass.GetInsertAccessSection("FollowUp", userID);
                chkFollowUpUpdate.IsChecked = userClass.GetUpdateAccessSection("FollowUp", userID);
                chkFollowUpDelete.IsChecked = userClass.GetDeleteAccessSection("FollowUp", userID);
            }
            else
            {
                gbFollowUp.IsEnabled = true;
                chkFollowUpInsert.IsChecked = true;
                chkFollowUpUpdate.IsChecked = true;
                chkFollowUpDelete.IsChecked = true;
            }
        }

        private void chkSaleAccess_Checked(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
            {
                gbSale.IsEnabled = chkSaleAccess.IsChecked.Value;
                chkSaleInsert.IsChecked = userClass.GetInsertAccessSection("Order", userID);
                chkSaleUpdate.IsChecked = userClass.GetUpdateAccessSection("Order", userID);
                chkSaleDelete.IsChecked = userClass.GetDeleteAccessSection("Order", userID);
            }
            else
            {
                gbSale.IsEnabled = true;
                chkSaleInsert.IsChecked = true;
                chkSaleUpdate.IsChecked = true;
                chkSaleDelete.IsChecked = true;
            }
        }

        private void chkReturnAccess_Checked(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
            {
                gbReturn.IsEnabled = chkReturnAccess.IsChecked.Value;
                chkReturnInsert.IsChecked = userClass.GetInsertAccessSection("OrderReturn", userID);
                chkReturnUpdate.IsChecked = userClass.GetUpdateAccessSection("OrderReturn", userID);
                chkReturnDelete.IsChecked = userClass.GetDeleteAccessSection("OrderReturn", userID);
            }
            else
            {
                gbReturn.IsEnabled = true;
                chkReturnInsert.IsChecked = true;
                chkReturnUpdate.IsChecked = true;
                chkReturnDelete.IsChecked = true;
            }
        }
        private void chkRestAccess_Checked(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
            {
                gbRest.IsEnabled = chkRestAccess.IsChecked.Value;
                chkRestInsert.IsChecked = userClass.GetInsertAccessSection("Rest", userID);
                chkRestUpdate.IsChecked = userClass.GetUpdateAccessSection("Rest", userID);
                chkRestDelete.IsChecked = userClass.GetDeleteAccessSection("Rest", userID);
            }
            else
            {
                gbRest.IsEnabled = true;
                chkRestInsert.IsChecked = true;
                chkRestUpdate.IsChecked = true;
                chkRestDelete.IsChecked = true;
            }
        }

        private void chkContractAccess_Checked(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
            {
                gbContract.IsEnabled = chkContractAccess.IsChecked.Value;
                chkContractInsert.IsChecked = userClass.GetInsertAccessSection("Contract", userID);
                chkContractUpdate.IsChecked = userClass.GetUpdateAccessSection("Contract", userID);
                chkContractDelete.IsChecked = userClass.GetDeleteAccessSection("Contract", userID);
            }
            else
            {
                gbContract.IsEnabled = true;
                chkContractInsert.IsChecked = true;
                chkContractUpdate.IsChecked = true;
                chkContractDelete.IsChecked = true;
            }
        }

        private void lst_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (int.TryParse(lst.SelectedValue.ToString(), out userID))
                {
                    SelectAllAccess(false);

                    OnEdit = true;

                    BLUserClass user = new BLUserClass();
                    var userInfo = user.SearchUser(userID).Single();

                    radAdmin.IsChecked = userInfo.kind == "Admin";
                    radUser.IsChecked = userInfo.kind == "User";

                    chkActive.IsChecked = userInfo.IsActive;

                    txtUsername.Text = userInfo.username;
                    txtPassword.Password = userInfo.password;

                    chkIssueAccess.IsChecked = userClass.GetUserAccess(userID, "Issue");
                    chkVisitorAccess.IsChecked = userClass.GetUserAccess(userID, "Visitor");
                    chkExpertAccess.IsChecked = userClass.GetUserAccess(userID, "Expert");
                    chkCustomerAccess.IsChecked = userClass.GetUserAccess(userID, "Customer");

                    chkApointmentAccess.IsChecked = userClass.GetUserAccess(userID, "Apointment");
                    chkFollowUpAccess.IsChecked = userClass.GetUserAccess(userID, "FollowUp");
                    chkSaleAccess.IsChecked = userClass.GetUserAccess(userID, "Order");
                    chkReturnAccess.IsChecked = userClass.GetUserAccess(userID, "OrderReturn");
                    chkRestAccess.IsChecked = userClass.GetUserAccess(userID, "Rest");
                    chkContractAccess.IsChecked = userClass.GetUserAccess(userID, "Contract");

                    chkExpertApointmentReport.IsChecked = userClass.GetUserAccess(userID, "ApointmentReport");
                    chkTodayApointmentReport.IsChecked = userClass.GetUserAccess(userID, "TodayApointmentReport");
                    chkNextDayApointmentReport.IsChecked = userClass.GetUserAccess(userID, "NextDayApointmentReport");
                    chkCustomerApointmentReport.IsChecked = userClass.GetUserAccess(userID, "CustomerApointmentReport");
                    chkFollowUpReport.IsChecked = userClass.GetUserAccess(userID, "FollowUpReport");
                    chkTodayFollowUpReport.IsChecked = userClass.GetUserAccess(userID, "TodayFollowUpReport");
                    chkRestReport.IsChecked = userClass.GetUserAccess(userID, "VacationReport");
                    chkVisitorSaleReport.IsChecked = userClass.GetUserAccess(userID, "OrderReport");
                    chkSaleReport.IsChecked = userClass.GetUserAccess(userID, "VisitorReport");

                    chkBackup.IsChecked = userClass.GetUserAccess(userID, "Backup");
                    chkRestor.IsChecked = userClass.GetUserAccess(userID, "Restore");
                    chkUsersSettings.IsChecked = userClass.GetUserAccess(userID, "Users");

                    btnSave.IsEnabled = true;
                    btnDelete.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            SelectAllAccess(chkSelectAll.IsChecked.Value);
        }

        private void chkIssueAccess_Unchecked(object sender, RoutedEventArgs e)
        {
            gbIssue.IsEnabled = false;
            chkIssueInsert.IsChecked = false;
            chkIssueUpdate.IsChecked = false;
            chkIssueDelete.IsChecked = false;
        }

        private void chkVisitorAccess_Unchecked(object sender, RoutedEventArgs e)
        {
            gbVisitor.IsEnabled = false;
            chkVisitorInsert.IsChecked = false;
            chkVisitorUpdate.IsChecked = false;
            chkVisitorDelete.IsChecked = false;
        }

        private void chkExpertAccess_Unchecked(object sender, RoutedEventArgs e)
        {
            gbExpert.IsEnabled = false;
            chkExpertInsert.IsChecked = false;
            chkExpertUpdate.IsChecked = false;
            chkExpertDelete.IsChecked = false;
        }

        private void chkCustomerAccess_Unchecked(object sender, RoutedEventArgs e)
        {
            gbCustomer.IsEnabled = false;
            chkCustomerInsert.IsChecked = false;
            chkCustomerUpdate.IsChecked = false;
            chkCustomerDelete.IsChecked = false;
        }

        private void chkApointmentAccess_Unchecked(object sender, RoutedEventArgs e)
        {
            gbApointment.IsEnabled = false;
            chkApointmentInsert.IsChecked = false;
            chkApointmentUpdate.IsChecked = false;
            chkApointmentDelete.IsChecked = false;
        }

        private void chkFollowUpAccess_Unchecked(object sender, RoutedEventArgs e)
        {
            gbFollowUp.IsEnabled = false;
            chkFollowUpInsert.IsChecked = false;
            chkFollowUpUpdate.IsChecked = false;
            chkFollowUpDelete.IsChecked = false;
        }

        private void chkSaleAccess_Unchecked(object sender, RoutedEventArgs e)
        {
            gbSale.IsEnabled = false;
            chkSaleInsert.IsChecked = false;
            chkSaleUpdate.IsChecked = false;
            chkSaleDelete.IsChecked = false;
        }

        private void chkReturnAccess_Unchecked(object sender, RoutedEventArgs e)
        {
            gbReturn.IsEnabled = false;
            chkReturnInsert.IsChecked = false;
            chkReturnUpdate.IsChecked = false;
            chkReturnDelete.IsChecked = false;
        }

        private void chkRestAccess_Unchecked(object sender, RoutedEventArgs e)
        {
            gbRest.IsEnabled = false;
            chkRestInsert.IsChecked = false;
            chkRestUpdate.IsChecked = false;
            chkRestDelete.IsChecked = false;
        }

        private void chkContractAccess_Unchecked(object sender, RoutedEventArgs e)
        {
            gbContract.IsEnabled = false;
            chkContractInsert.IsChecked = false;
            chkContractUpdate.IsChecked = false;
            chkContractDelete.IsChecked = false;
        }
    }
}
