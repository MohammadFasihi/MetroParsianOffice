using ParsianOffice.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BL;
using Arash;
using DA;
using System.Data;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winApointment.xaml
    /// </summary>
    public partial class winApointment : Window
    {
        public winApointment()
        {
            InitializeComponent();

            cmbTimePicker.Text = "00:00";

            int hour = int.Parse(AppSettingClass.setting.TimeInterval.Substring(0, 2));
            int minute = int.Parse(AppSettingClass.setting.TimeInterval.Substring(3, 2));

            int interval = (hour * 60) + minute;

            int hInterval = 0;
            int mInterval = 0;


            int h = 0;
            int m = 0;
            do
            {
                cmbTimePicker.Items.Add(h.ToString("00") + ":" + m.ToString("00"));

                m += interval;

                if (m >= 60)
                {
                    h += m / 60;
                    m = m % 60;
                }


            } while (h < 24);
        }

        public int id = 0;

        public bool doApointment = false,doEdit = false;
        public PersianDate date;

        public bool OnEdit = false;
        string section = "Apointment";

        DataTable dataTable = null;

        public MetroWindow main;
        BLApointment apointment = new BLApointment();

        BLUserClass user = new BLUserClass();

        public void LoadData()
        {
            BLCustomer customer = new BLCustomer();
            cmbCustomer.ItemsSource = customer.GetCustomerCobmo();
            cmbCustomer.DisplayMember = "cu_Name";
            cmbCustomer.ValueMember = "cu_ID";

            BLExpert expert = new BLExpert();
            cmbExpert.ItemsSource = expert.sp_GetExpert();
            cmbExpert.DisplayMember = "ep_Name";
            cmbExpert.ValueMember = "ID";

            BLKind kind = new BLKind();
            cmbKind.ItemsSource = kind.GetAllKind();
            cmbKind.DisplayMemberPath = "kind_Name";
            cmbKind.SelectedValuePath = "ID";

            BLSituation situation = new BLSituation();
            cmbSituation.ItemsSource = situation.GetAllSituation();
            cmbSituation.DisplayMemberPath = "st_Name";
            cmbSituation.SelectedValuePath = "ID";
        }
        public void Clear()
        {
            if (!doEdit)
            {
                txtID.Text = apointment.getLastID().ToString();
                cmbCustomer.SelectedIndex = -1;
                cmbExpert.SelectedIndex = -1;
                cmbKind.SelectedIndex = 0;
                cmbSituation.SelectedIndex = 0;

                if (doApointment)
                    dpDate.SelectedDate = date;

                txtAddress.Text = "";
                txtArea.Text = "";

                txtNumber.Text = "";
                txtCustomer.Text = "";

                cmbCustomer.Focus();

                btnSave.Visibility = (user.GetInsertAccessSection("Apointment", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
                btnDelete.Visibility = (user.GetDeleteAccessSection("Apointment", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;

                btnCancel.Content = "خروج";
                OnEdit = false;

                gcCancelation.ItemsSource = null;
            }
            else
                btnSearch_Click(null, null);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = double.Parse(ConfigClass.getSetting(section, "Width"));
                this.Height = double.Parse(ConfigClass.getSetting(section, "Height"));

                this.Left = double.Parse(ConfigClass.getSetting(section, "LocationX"));
                this.Top = double.Parse(ConfigClass.getSetting(section, "LocationY"));

                grdMain.ColumnDefinitions[0].Width = new GridLength(double.Parse(ConfigClass.getSetting(section, "Cancelation")));
            }
            catch (Exception)
            {

            }

            LoadData();
            Clear();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AppSettingClass.setting.SaveSizes)
            {
                ConfigClass.setSetting(section, "Width", this.ActualWidth);
                ConfigClass.setSetting(section, "Height", this.ActualHeight);

                ConfigClass.setSetting(section, "LocationX", this.Left);
                ConfigClass.setSetting(section, "LocationY", this.Top);

                ConfigClass.setSetting(section, "Cancelation",grdMain.ColumnDefinitions[0].Width);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!doEdit)
            {
                winDisplay display = new winDisplay();
                BLApointment.apointment = null;
                display.tableName = "Apointment";
                display.ShowDialog();
            }
            else
            {
                BLApointment ap = new BLApointment(this.id);
            }

            gcCancelation.ItemsSource = null;

            if (BLApointment.apointment != null)
            {
                txtID.Text = BLApointment.apointment.ID.ToString();
                cmbCustomer.EditValue = BLApointment.apointment.cu_ID;
                cmbExpert.EditValue = BLApointment.apointment.expert_ID;
                dpDate.SelectedDate = PersianDate.Parse(BLApointment.apointment.Date);
                cmbTimePicker.Text = BLApointment.apointment.Time;
                cmbKind.SelectedValue = BLApointment.apointment.kind_ID;
                cmbSituation.SelectedValue = BLApointment.apointment.st_ID;
                txtArea.Text = BLApointment.apointment.ap_Area;
                txtAddress.Text = BLApointment.apointment.Address;
                txtCustomer.Text = BLApointment.apointment.cu_Name;
                txtNumber.Text = BLApointment.apointment.cu_Phone;

                if(cmbSituation.Text == "لغو")
                {
                    SqlClass ado = new SqlClass();
                    ado.ConnectionString = ConnectionStringClass.GetADOConnectionString();
                    ado.OpenConnection();
                    ado.Command("sp_GetCancelationReason", System.Data.CommandType.StoredProcedure, BL.BLApointment.apointment.ID, "@apointmentID");
                    DataTable dt = ado.ExecuteQuery();
                    ado.CloseConnection();


                    for (int i = 0; i < dt.Rows.Count ; i++)
                    {
                        DataRow row = dataTable.NewRow();
                        dataTable.Rows.Add(row);
                        dataTable.Rows[i + (dt.Rows.Count)][0] = dt.Rows[i][0];
                        dataTable.Rows[i + (dt.Rows.Count)][1] = dt.Rows[i][1];
                    }

                    gcCancelation.ItemsSource = dataTable;
                }

                OnEdit = true;

                btnSave.Visibility = (user.GetUpdateAccessSection("Apointment", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BLCustomer cust = new BLCustomer();

            if (!cust.IsExist(txtCustomer.Text))
            {
                cust.Insert(txtCustomer.Text, null, null, txtAddress.Text, BLUserClass.VisitorID, null);
                int cu_id = cust.GetCustomer(txtCustomer.Text).cu_ID;

                if (!String.IsNullOrEmpty(txtNumber.Text) || !String.IsNullOrWhiteSpace(txtNumber.Text))
                    cust.InsertPhone(cu_id, "شماره موبایل", txtNumber.Text);

                BLCustomer customer = new BLCustomer();
                cmbCustomer.ItemsSource = customer.GetCustomerCobmo();
                cmbCustomer.DisplayMember = "cu_Name";
                cmbCustomer.ValueMember = "cu_ID";

                cmbCustomer.EditValue = cust.GetCustomer(txtCustomer.Text).cu_ID;
            }

            int id = int.Parse(txtID.Text);
            object customerID = cmbCustomer.EditValue;
            object expert = cmbExpert.EditValue;
            string cu_Name = txtCustomer.Text;
            string cu_Phone = txtNumber.Text;
            int kind = int.Parse(cmbKind.SelectedValue.ToString());
            int situation = int.Parse(cmbSituation.SelectedValue.ToString());
            string area = txtArea.Text;
            string address = txtAddress.Text;
            string time = cmbTimePicker.Text;
            PersianDate Date = dpDate.SelectedDate;
            string date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(Date.ToString()));

            string ep_Name = cmbExpert.Text;

            bool dialogRes = true;

            try
            {
                BLExpert expt = new BLExpert();

                if (AppSettingClass.setting.WarningRestInApointment)
                {
                    if (expt.IsExpertOnVacation(expert, Date.ToDateTime(), time))
                    {
                        MetroDialogSettings config = new MetroDialogSettings();

                        config.AffirmativeButtonText = "بله";
                        config.AnimateHide = true;
                        config.AnimateShow = true;
                        config.ColorScheme = MetroDialogColorScheme.Theme;
                        config.DefaultButtonFocus = MessageDialogResult.Affirmative;
                        config.DialogResultOnCancel = MessageDialogResult.Negative;
                        config.NegativeButtonText = "خیر";

                        var res = await main.ShowMessageAsync("", "برای کارشناس " + ep_Name + " در تاریخ " + date + " ساعت " + time + " مرخصی ثبت شده است" + "\n" + "آیا مایل به ثبت قرار ملاقات برای کارشناس انتخابی می باشید؟", MessageDialogStyle.AffirmativeAndNegative, config);

                        //MessageBoxResult res = MessageBox.Show("برای کارشناس " + ep_Name + " در تاریخ " + date + " ساعت " + time + " مرخصی ثبت شده است" + "\n" + "آیا مایل به ثبت قرار ملاقات برای کارشناس انتخابی می باشید؟", "هشدار", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageDialogResult.Affirmative)
                        {
                            if (!OnEdit)
                            {
                                if (apointment.getApointment(ep_Name, Date.ToString()).Count() != 0 && AppSettingClass.setting.ShowApointmentInInsert)
                                {
                                    winAlert alert = new winAlert();
                                    alert.ap_Date = Date.ToString();
                                    alert.expert = ep_Name;
                                    bool r = alert.ShowDialog().Value;

                                    if (!r)
                                        return;
                                }

                                apointment.Insert(customerID, cu_Name, cu_Phone, expert, Date, time, situation, area, kind, address, BLUserClass.UserID);
                                Clear();
                            }
                            else
                            {
                                if (cmbSituation.Text == "لغو" && !apointment.IsExist(id, situation))
                                {
                                    winCancelReson cancel = new winCancelReson();
                                    cancel.ap_ID = id;
                                    dialogRes = cancel.ShowDialog().Value;
                                }
                                if (dialogRes)
                                    apointment.Update(id, cu_Name, cu_Phone, customerID, expert, Date, time, situation, area, kind, address, BLUserClass.UserID);
                                Clear();
                            }
                        }
                    }
                    else
                    {
                        if (!OnEdit)
                        {
                            if (apointment.getApointment(ep_Name, Date.ToString()).Count() != 0 && AppSettingClass.setting.ShowApointmentInInsert)
                            {
                                winAlert alert = new winAlert();
                                alert.ap_Date = Date.ToString();
                                alert.expert = ep_Name;
                                bool r = alert.ShowDialog().Value;

                                if (!r)
                                    return;
                            }

                            apointment.Insert(customerID, cu_Name, cu_Phone, expert, Date, time, situation, area, kind, address, BLUserClass.UserID);
                            Clear();
                        }
                        else
                        {
                            if (cmbSituation.Text == "لغو" && !apointment.IsExist(id, situation))
                            {
                                winCancelReson cancel = new winCancelReson();
                                cancel.ap_ID = id;
                                dialogRes = cancel.ShowDialog().Value;
                            }
                            if (dialogRes)
                                apointment.Update(id, cu_Name, cu_Phone, customerID, expert, Date, time, situation, area, kind, address, BLUserClass.UserID);
                            Clear();
                        }
                    }
                }
                else
                {
                    if (!OnEdit)
                    {
                        if (apointment.getApointment(ep_Name, Date.ToString()).Count() != 0 && AppSettingClass.setting.ShowApointmentInInsert)
                        {
                            winAlert alert = new winAlert();
                            alert.ap_Date = Date.ToString();
                            alert.expert = ep_Name;
                            bool r = alert.ShowDialog().Value;

                            if (!r)
                                return;
                        }

                        apointment.Insert(customerID, cu_Name, cu_Phone, expert, Date, time, situation, area, kind, address, BLUserClass.UserID);
                        Clear();
                    }
                    else
                    {
                        if (cmbSituation.Text == "لغو" && !apointment.IsExist(id, situation))
                        {
                            winCancelReson cancel = new winCancelReson();
                            cancel.ap_ID = id;
                            dialogRes = cancel.ShowDialog().Value;
                        }
                        if (dialogRes)
                            apointment.Update(id, cu_Name, cu_Phone, customerID, expert, Date, time, situation, area, kind, address, BLUserClass.UserID);
                        Clear();
                    }
                }

                if (doApointment || doEdit)
                    Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(txtID.Text);
                apointment.Delete(id);

                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (OnEdit)
            {
                Clear();
                btnCancel.Content = "خروج";
            }
            else
                Close();
        }

        private void cmbCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.Insert)
            {
                winCustomer customer = new winCustomer();
                customer.ShowDialog();
                LoadData();
            }
        }

        private void cmbExpert_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.Insert)
            {
                winExpress expert = new winExpress();
                expert.ShowDialog();
                LoadData();
            }
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = user.GetDeleteAccessSection("Apintment", BLUserClass.UserID);
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadData();
            Clear();
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
                e.CanExecute = user.GetInsertAccessSection("Apintment", BLUserClass.UserID);
            else
                e.CanExecute = user.GetUpdateAccessSection("Apintment", BLUserClass.UserID);
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = -1;

                if (int.TryParse(cmbCustomer.EditValue.ToString(), out id))
                {
                    BLCustomer customer = new BLCustomer(id);
                    txtAddress.Text = BLCustomer.customer.cu_Address;
                    txtCustomer.Text = BLCustomer.customer.cu_Name;
                    var phone = BLCustomer.customer.tbl_CustomerPhone.ToList();
                    var lst = (from q in phone where q.cu_Field == "شماره موبایل" select q).ToList();
                    txtNumber.Text = lst[lst.Count - 1].cu_Data;

                    SqlClass ado = new SqlClass();
                    ado.ConnectionString = ConnectionStringClass.GetADOConnectionString();
                    ado.OpenConnection();
                    ado.Command("sp_GetLastCustomerAponintmentData", CommandType.StoredProcedure, id, "@cu_ID");
                    dataTable = ado.ExecuteQuery();
                    ado.CloseConnection();

                    gcCancelation.ItemsSource = dataTable;
                }

            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
