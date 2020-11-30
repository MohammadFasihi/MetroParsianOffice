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
using Arash;
using System.Data;
using System.Data.SqlClient;
using BL;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winVacation.xaml
    /// </summary>
    public partial class winVacation : Window
    {
        public winVacation()
        {
            InitializeComponent();

            timStart.Text = "00:00";

            int hour = int.Parse(AppSettingClass.setting.TimeInterval.Substring(0, 2));
            int minute = int.Parse(AppSettingClass.setting.TimeInterval.Substring(3, 2));

            int interval = (hour * 60) + minute;

            int hInterval = 0;
            int mInterval = 0;


            int h = 0;
            int m = 0;
            do
            {
                timStart.Items.Add(h.ToString("00") + ":" + m.ToString("00"));

                m += interval;

                if (m >= 60)
                {
                    h += m / 60;
                    m = m % 60;
                }


            } while (h < 24);
        }
        byte dailyTimeWork = 0;
        bool OnEdit = false;
        public winMain main;
        string section = "Vacation";
        public static int id;

        int rest = 0;

        BLUserClass user = new BLUserClass();
        void Clear()
        {
            Entities db = new Entities();

            txtID.Text = (db.tbl_Vacations.Select(q => q.ID).DefaultIfEmpty(0).Max() + 1).ToString();
            cmbExpert.SelectedItem = null;
            startDate.SelectedDate = PersianDate.Today;
            timStart.Text = "00:00";

            sliderTime.Value = 0;
            sliderTime.IsEnabled = false;
            lblBaseUnit.Text = "";
            lblBaseValue.Text = "";
            lblUnit.Text = "";
            lblValue.Text = "";

            btnCancel.Content = "خروج";
            OnEdit = false;

            btnSave.Visibility = (user.GetInsertAccessSection("Rest", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
            btnDelete.Visibility = (user.GetDeleteAccessSection("Rest", BLUserClass.UserID)) ? Visibility.Visible : Visibility.Hidden;
        }

        void LoadExpert()
        {
            BLExpert expert = new BLExpert();
            cmbExpert.ItemsSource = expert.sp_GetAllExpert();
            cmbExpert.DisplayMember = "ep_Name";
            cmbExpert.ValueMember = "ID";
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

            LoadExpert();
            Clear();
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Entities db = new Entities();
                tbl_Vacations vac = new tbl_Vacations();
                int id = int.Parse(txtID.Text);

                if (!OnEdit)
                {
                    if (cmbExpert.SelectedItem != null)
                    {
                        vac.ep_ID = int.Parse(cmbExpert.EditValue.ToString());
                        string startTime = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(startDate.SelectedDate.ToString()));
                        vac.sh_StartDate = startTime;
                        vac.startTime = timStart.Text;
                        vac.ep_Name = cmbExpert.Text;
                        vac.vc_StartDate = startDate.SelectedDate.ToDateTime();

                        if (radDaily.IsChecked.Value)
                        {
                            vac.endTime = timStart.Text;
                            int day = (int)sliderTime.Value;
                            string date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(startDate.SelectedDate.AddDays(day).ToString()));
                            vac.sh_EndDate = date;
                            vac.vc_EndDate = startDate.SelectedDate.AddDays(day).ToDateTime();

                            vac.ep_TimeRest = int.Parse(lblBaseValue.Text);
                        }
                        else
                        {
                            int day = (int)sliderTime.Value / int.Parse(dailyTimeWork.ToString());
                            int hour = (int)sliderTime.Value % int.Parse(dailyTimeWork.ToString());

                            string endTime = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(startDate.SelectedDate.AddDays(day).ToString()));
                            vac.sh_EndDate = endTime;
                            vac.vc_EndDate = startDate.SelectedDate.AddDays(day).ToDateTime();

                            int startHour = int.Parse(timStart.Text.Substring(0, 2));
                            string min = timStart.Text.Substring(3, 2);
                            vac.endTime = (startHour + hour).ToString() +":"+ min;

                            vac.ep_TimeRest = int.Parse(lblValue.Text);
                        }

                        db.tbl_Vacations.Add(vac);

                        db.SaveChanges();
                    }
                }
                else
                {
                    vac = (from q in db.tbl_Vacations where q.ID == id select q).Single();

                    vac.ep_ID = int.Parse(cmbExpert.EditValue.ToString());
                    string startTime = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(startDate.SelectedDate.ToString()));
                    vac.sh_StartDate = startTime;
                    vac.startTime = timStart.Text;
                    vac.ep_Name = cmbExpert.Text;
                    vac.vc_StartDate = startDate.SelectedDate.ToDateTime();

                    if (radDaily.IsChecked.Value)
                    {
                        vac.endTime = timStart.Text;
                        int day = (int)sliderTime.Value;
                        string date = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(startDate.SelectedDate.AddDays(day).ToString()));
                        vac.sh_EndDate = date;
                        vac.vc_EndDate = startDate.SelectedDate.AddDays(day).ToDateTime();

                        vac.ep_TimeRest = int.Parse(lblBaseValue.Text);
                    }
                    else
                    {
                        int day = (int)sliderTime.Value / int.Parse(dailyTimeWork.ToString());
                        int hour = (int)sliderTime.Value % int.Parse(dailyTimeWork.ToString());

                        string endTime = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(startDate.SelectedDate.AddDays(day).ToString()));
                        vac.sh_EndDate = endTime;
                        vac.vc_EndDate = startDate.SelectedDate.AddDays(day).ToDateTime();

                        int startHour = int.Parse(timStart.Text.Substring(0, 2));
                        string min = timStart.Text.Substring(3, 2);
                        vac.endTime = (startHour + hour).ToString() + ":" + min;

                        vac.ep_TimeRest = int.Parse(lblValue.Text);
                    }

                    db.SaveChanges();
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
            Entities db = new Entities();
            int id = int.Parse(txtID.Text);

            var vac = (from q in db.tbl_Vacations where q.ID == id select q);

            if (vac.Count() != 0)
            {
                db.tbl_Vacations.Remove(vac.Single());
                db.SaveChangesAsync();
                Clear();
            }
            else
                MessageBox.Show("اطلاعات وارد شده معتبر نمی باشد", "خطا در برنامه", MessageBoxButton.OK, MessageBoxImage.Error);
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
            id = -1;

            Entities db = new Entities();
            winDisplay display = new winDisplay();
            display.tableName = "Vacation";
            display.ShowDialog();

            if (id != -1)
            {
                var vacation = (from q in db.tbl_Vacations where q.ID == id select q).Single();
                txtID.Text = vacation.ID.ToString();
                cmbExpert.EditValue = vacation.ep_ID;
                startDate.SelectedDate = PersianDate.Parse(vacation.sh_StartDate);
                timStart.Text = vacation.startTime;
                sliderTime.Value = (double)vacation.ep_TimeRest.Value;

                OnEdit = true;
                btnCancel.Content = "انصراف";
            }
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = user.GetDeleteAccessSection("Rest", BLUserClass.UserID);
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadExpert();
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
                e.CanExecute = user.GetInsertAccessSection("Rest", BLUserClass.UserID);
            else
                e.CanExecute = user.GetUpdateAccessSection("Rest", BLUserClass.UserID);
        }

        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (cmbExpert.EditValue != null)
            {
                if (radTime.IsChecked.Value)
                {
                    lblUnit.Text = radTime.Content.ToString();
                    lblValue.Text = ((int)sliderTime.Value).ToString();
                }
                else
                {
                    lblBaseUnit.Text = "ساعت";
                    rest = ((int)dailyTimeWork * (int)sliderTime.Value);
                    lblBaseValue.Text = rest.ToString();

                    lblValue.Text = ((int)sliderTime.Value).ToString();
                    lblUnit.Text = radDaily.Content.ToString();
                }
            }
        }

        private void cmbExpert_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                int id = int.Parse(cmbExpert.EditValue.ToString());
                BLExpert expert = new BLExpert(id);
                dailyTimeWork = BLExpert.expert.ep_DailyTimeWork.Value;

                sliderTime.IsEnabled = true;
            }
            catch (Exception ex)
            {
                
            }
        }

        private void radTime_Click(object sender, RoutedEventArgs e)
        {
            lblBaseUnit.Visibility = Visibility.Hidden;
            lblBaseValue.Visibility = Visibility.Hidden;
        }

        private void radDaily_Click(object sender, RoutedEventArgs e)
        {
            lblBaseUnit.Visibility = Visibility.Visible;
            lblBaseValue.Visibility = Visibility.Visible;
        }
    }
}
