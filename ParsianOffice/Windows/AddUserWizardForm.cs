using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DA;
using ParsianOffice;
using BL;
using ParsianOffice.Classes;

namespace ParsianOffice.Windows
{
    public partial class AddUserWizardForm : DevExpress.XtraEditors.XtraForm
    {
        public AddUserWizardForm()
        {
            InitializeComponent();
        }

        string kind = "User";
        int? vt_id = 0;

        public void SelectAllAccess(bool value)
        {
            chkIssueAccess.Checked = value;
            chkVisitorAccess.Checked = value;
            chkExpretAccess.Checked = value;
            chkCustomerAccess.Checked = value;

            chkApointmentAccess.Checked = value;
            chkFollowUpAccess.Checked = value;
            chkSaleAccess.Checked = value;
            chkReturnAccess.Checked = value;
            chkRestAccess.Checked = value;
            chkContractAccess.Checked = value;

            chkAllAP.Checked = value;
            chkTodayAP.Checked = value;
            chkNextDayAP.Checked = value;
            chkCustomerAP.Checked = value;
            chkFollowUP.Checked = value;
            chkTodayFollowUp.Checked = value;
            chkRest.Checked = value;
            chkVisitorSale.Checked = value;
            chkSale.Checked = value;

            chkBackup.Checked = value;
            chkRestore.Checked = value;
            chkUserSettings.Checked = value;
        }

        private void welcomeWizardPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AddUserWizardForm_Load(object sender, EventArgs e)
        {
            SqlClass ado = new SqlClass();
            ado.ConnectionString = ConnectionStringClass.GetADOConnectionString(60);
            ado.OpenConnection();
            ado.Command("Select * from tbl_Experts where ep_kind = 1",CommandType.Text);
            cmbVisitor.DataSource =  ado.ExecuteQuery();
            cmbVisitor.DisplayMember = "ep_Name";
            cmbVisitor.ValueMember = "ID";
            ado.CloseConnection();

        }

        private void wlcomePage_FinishClick(object sender, CancelEventArgs e)
        {
            BLUserClass userClass = new BLUserClass();

            if (kind == "User")
            {
                if (cmbVisitor.SelectedValue == null)
                    vt_id = null;
                else
                    vt_id = int.Parse(cmbVisitor.SelectedValue.ToString());
            }
            else
                vt_id = null;

            userClass.InsertUser(txtUsername.Text, txtPassword.Text, kind, vt_id, true);

            int id = userClass.SearchUser(txtUsername.Text).Single().ID;

            if (chkIssueAccess.Checked)
                userClass.InsertUserAccess(id, "Issue", chkInsertIssue.Checked, chkUpdateIssue.Checked, chkDeleteIssue.Checked, false);

            if (chkVisitorAccess.Checked)
                userClass.InsertUserAccess(id, "Visitor", chkVisitorInsert.Checked, chkVisitorUpdate.Checked, chkVisitorDelete.Checked, false);

            if (chkExpretAccess.Checked)
                userClass.InsertUserAccess(id, "Expert", chkExpertUpdate.Checked, chkExpertInsert.Checked, chkExpertDelete.Checked, false);

            if (chkCustomerAccess.Checked)
                userClass.InsertUserAccess(id, "Customer", chkCustomerInsert.Checked, chkCustomerUpdate.Checked, chkCustomerDelete.Checked, false);

            if (chkApointmentAccess.Checked)
                userClass.InsertUserAccess(id, "Apointment", chkApInsert.Checked, chkApUpdate.Checked, chkApDelete.Checked, false);

            if (chkFollowUpAccess.Checked)
                userClass.InsertUserAccess(id, "FollowUp", chkFUInsert.Checked, chkFUUpdate.Checked, chkFUDelete.Checked, false);

            if (chkSaleAccess.Checked)
                userClass.InsertUserAccess(id, "Order", chkSaleInsert.Checked, chkSaleUpdate.Checked, chkSaleDelete.Checked, false);

            if (chkReturnAccess.Checked)
                userClass.InsertUserAccess(id, "OrderReturn", chkReturnInsert.Checked, chkReturnUpdate.Checked, chkReturnDelete.Checked, false);

            if (chkRestAccess.Checked)
                userClass.InsertUserAccess(id, "Rest", chkRestInsert.Checked, chkRestUpdate.Checked, chkRestDelete.Checked, false);

            if (chkContractAccess.Checked)
                userClass.InsertUserAccess(id, "Contract", chkContractInsert.Checked, chkContractUpdate.Checked, chkContractDelete.Checked, true);

            if (chkAllAP.Checked)
                userClass.InsertUserAccess(id, "ApointmentReport", true, true, true, true);

            if (chkTodayAP.Checked)
                userClass.InsertUserAccess(id, "TodayApointmentReport", true, true, true, true);

            if (chkNextDayAP.Checked)
                userClass.InsertUserAccess(id, "NextDayApointmentReport", true, true, true, true);

            if (chkCustomerAP.Checked)
                userClass.InsertUserAccess(id, "CustomerApointmentReport", true, true, true, true);

            if (chkFollowUP.Checked)
                userClass.InsertUserAccess(id, "FollowUpReport", true, true, true, true);

            if (chkTodayFollowUp.Checked)
                userClass.InsertUserAccess(id, "TodayFollowUpReport", true, true, true, true);

            if (chkRest.Checked)
                userClass.InsertUserAccess(id, "VacationReport", true, true, true, true);

            if (chkSale.Checked)
                userClass.InsertUserAccess(id, "OrderReport", true, true, true, true);

            if (chkVisitorSale.Checked)
                userClass.InsertUserAccess(id, "VisitorReport", true, true, true, true);

            if (chkBackup.Checked)
                userClass.InsertUserAccess(id, "Backup", true, true, true, true);

            if (chkRestore.Checked)
                userClass.InsertUserAccess(id, "Restore", true, true, true, true);

            if (chkUserSettings.Checked)
                userClass.InsertUserAccess(id, "Users", true, true, true, true);

        }

        private void chkIssueAccess_CheckedChanged(object sender, EventArgs e)
        {
            gbIssue.Enabled = chkIssueAccess.Checked;
            chkInsertIssue.Checked = chkIssueAccess.Checked;
            chkUpdateIssue.Checked = chkIssueAccess.Checked;
            chkDeleteIssue.Checked = chkIssueAccess.Checked;
        }

        private void chkVisitorAccess_CheckedChanged(object sender, EventArgs e)
        {
            gbVisitor.Enabled = chkVisitorAccess.Checked;
            chkVisitorInsert.Checked = chkVisitorAccess.Checked;
            chkVisitorUpdate.Checked = chkVisitorAccess.Checked;
            chkVisitorDelete.Checked = chkVisitorAccess.Checked;
        }

        private void chkExpretAccess_CheckedChanged(object sender, EventArgs e)
        {
            gbExpert.Enabled = chkExpretAccess.Checked;
            chkExpertInsert.Checked = chkExpretAccess.Checked;
            chkExpertUpdate.Checked = chkExpretAccess.Checked;
            chkExpertDelete.Checked = chkExpretAccess.Checked;
        }

        private void chkCustomerAccess_CheckedChanged(object sender, EventArgs e)
        {
            gbCustomer.Enabled = chkCustomerAccess.Checked;
            chkCustomerInsert.Checked = chkCustomerAccess.Checked;
            chkCustomerUpdate.Checked = chkCustomerAccess.Checked;
            chkCustomerDelete.Checked = chkCustomerAccess.Checked;
        }

        private void chkApointmentAccess_CheckedChanged(object sender, EventArgs e)
        {
            gbApointment.Enabled = chkApointmentAccess.Checked;
            chkApInsert.Checked = chkApointmentAccess.Checked;
            chkApUpdate.Checked = chkApointmentAccess.Checked;
            chkApDelete.Checked = chkApointmentAccess.Checked;
        }

        private void chkFollowUpAccess_CheckedChanged(object sender, EventArgs e)
        {
            gbFollowUp.Enabled = chkFollowUpAccess.Checked;
            chkFUInsert.Checked = chkFollowUpAccess.Checked;
            chkFUDelete.Checked = chkFollowUpAccess.Checked;
            chkFUUpdate.Checked = chkFollowUpAccess.Checked;
        }

        private void chkSaleAccess_CheckedChanged(object sender, EventArgs e)
        {
            gbSale.Enabled = chkSaleAccess.Checked;
            chkSaleInsert.Checked = chkSaleAccess.Checked;
            chkSaleDelete.Checked = chkSaleAccess.Checked;
            chkSaleUpdate.Checked = chkSaleAccess.Checked;
        }

        private void chkReturnAccess_CheckedChanged(object sender, EventArgs e)
        {
            gbReturn.Enabled = chkReturnAccess.Checked;
            chkReturnInsert.Checked = chkReturnAccess.Checked;
            chkReturnDelete.Checked = chkReturnAccess.Checked;
            chkReturnUpdate.Checked = chkReturnAccess.Checked;
        }

        private void chkRestAccess_CheckedChanged(object sender, EventArgs e)
        {
            gbRest.Enabled = chkRestAccess.Checked;
            chkRestInsert.Checked = chkRestAccess.Checked;
            chkRestDelete.Checked = chkRestAccess.Checked;
            chkRestUpdate.Checked = chkRestAccess.Checked;
        }

        private void chkContractAccess_CheckedChanged(object sender, EventArgs e)
        {
            gbContract.Enabled = chkContractAccess.Checked;
            chkContractInsert.Checked = chkContractAccess.Checked;
            chkContractDelete.Checked = chkContractAccess.Checked;
            chkContractUpdate.Checked = chkContractAccess.Checked;
        }

        private void wlcomePage_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            try
            {
                if (e.Page.Name == "UserInfoPage")
                {
                    if (String.IsNullOrEmpty(txtUsername.Text))
                    {
                        MessageBox.Show("نام کاربری وارد نشده است", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (radAdmin.Checked)
                    {
                        SelectAllAccess(true);

                        UserAccessPage.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void radAdmin_CheckedChanged(object sender, EventArgs e)
        {
            if (radAdmin.Checked)
            {
                kind = "Admin";
                vt_id = null;
                cmbVisitor.DataSource = null;
            }
        }

        private void radUser_CheckedChanged(object sender, EventArgs e)
        {
            if (radUser.Checked)
            {
                kind = "User";
                AddUserWizardForm_Load(null, null);
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            SelectAllAccess(SelectAll.Checked);
        }
    }
}