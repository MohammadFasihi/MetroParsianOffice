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
using BL;
using System.Data;
using ParsianOffice.Classes;
using DevExpress.XtraReports.UI;
using ParsianOffice.Windows;
using System.IO;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ParsianOffice
{
    /// <summary>
    /// Interaction logic for winPrintingSetting.xaml
    /// </summary>
    public partial class winPrintingSetting : Window
    {
        public winPrintingSetting()
        {
            InitializeComponent();
        }

        public MetroWindow  main = new MetroWindow();

        public static string designName;

        string fileName = "vbnsprhasuydfcghvaeriqtgiwtfgvasdg";

        DevReport report = new DevReport();

        int? userDefaultDesign;
        int? selectedDesign;

        PrintSettingClass printSetting = new PrintSettingClass(BLUserClass.UserID);

        string section = "PrintSetting";
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            tbl_PrintSettings print = new tbl_PrintSettings();
            byte printQty;

            print.UserID = BLUserClass.UserID;
            print.PreviewBeforePrint = chkPreviewBeforePrint.IsChecked.Value;
            print.LoadDesignBeforePrint = chkLoadDesignPrint.IsChecked.Value;

            if (String.IsNullOrEmpty(txtPrintQty.Text))
                printQty = 0;
            else
                printQty = byte.Parse(txtPrintQty.Text);

            print.PrintQty = printQty;

            selectedDesign = int.Parse(lueDesign.SelectedValue.ToString());

            if (selectedDesign != userDefaultDesign)
            {
                MetroDialogSettings config = new MetroDialogSettings();

                config.AffirmativeButtonText = "بله";
                config.AnimateHide = true;
                config.AnimateShow = true;
                config.ColorScheme = MetroDialogColorScheme.Theme;
                config.DefaultButtonFocus = MessageDialogResult.Affirmative;
                config.DialogResultOnCancel = MessageDialogResult.Negative;
                config.NegativeButtonText = "خیر";

                var res = await main.ShowMessageAsync("", "آیا طرح " + lueDesign.Text + " پیشفرض شود؟" + "\n" + "طرح پیشفرض طرحی می باشد که زمان چاپ با تنظیمات و قالب آن چاپ می شود", MessageDialogStyle.AffirmativeAndNegative, config);

                //MessageBoxResult res = MessageBox.Show("آیا طرح " + lueDesign.Text + " پیشفرض شود؟" + "\n" + "طرح پیشفرض طرحی می باشد که زمان چاپ با تنظیمات و قالب آن چاپ می شود", "طرح پیشفرض", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (res == MessageDialogResult.Negative)
                    selectedDesign = userDefaultDesign;
            }

            print.DefaultDesignID = selectedDesign;

            printSetting.UpdateSettings(BLUserClass.UserID, print);

            Close();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Entities db = new Entities();
            lueDesign.ItemsSource = db.tbl_PrintDesigns.ToList();
            lueDesign.DisplayMemberPath = "design_Name";
            lueDesign.SelectedValuePath = "ID";

            lueDesign.SelectedValue = selectedDesign = userDefaultDesign = printSetting.GetDefaultDesign(BLUserClass.UserID);

            chkLoadDesignPrint.IsChecked = PrintSettingClass.setting.LoadDesignBeforePrint;
            chkPreviewBeforePrint.IsChecked = PrintSettingClass.setting.PreviewBeforePrint;
            txtPrintQty.Text = PrintSettingClass.setting.PrintQty.Value.ToString();

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

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable detail;
            DataTable header;

            SqlAdoClass.ConnectionString = ConnectionStringClass.GetADOConnectionString(60);

            SqlAdoClass.Command("sp_OrderHeaderPrint", CommandType.StoredProcedure, 0, "@id");
            header = SqlAdoClass.ExecuteQuery("Header");

            SqlAdoClass.Command("sp_OrderDetailsPrint", CommandType.StoredProcedure, 0, "@id");
            detail = SqlAdoClass.ExecuteQuery("Detail");

            if (lueDesign.SelectedItem != null)
            {
                int? designID = int.Parse(lueDesign.SelectedValue.ToString());
                byte[] file = printSetting.GetDesign(designID);

                File.WriteAllBytes(clsDateClass.appStartupPath + "\\" + fileName, file);
                report.LoadLayoutFromXml(clsDateClass.appStartupPath + "\\" + fileName);
            }

            ds.Tables.Add(header);
            ds.Tables.Add(detail);

            report.DataSource = ds;
            report.RightToLeft = RightToLeft.Yes;

            ReportDesignTool designTool = new ReportDesignTool(report);
            designTool.ShowDesignerDialog();

            MetroDialogSettings config = new MetroDialogSettings();

            config.AffirmativeButtonText = "بله";
            config.AnimateHide = true;
            config.AnimateShow = true;
            config.ColorScheme = MetroDialogColorScheme.Theme;
            config.DefaultButtonFocus = MessageDialogResult.Affirmative;
            config.DialogResultOnCancel = MessageDialogResult.Canceled;
            config.NegativeButtonText = "خیر";
            config.FirstAuxiliaryButtonText = "انصراف";

            var res = await main.ShowMessageAsync("", "آیا مایل به ذخیره طرح می باشید؟",MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,config);

            //MessageBoxResult res = MessageBox.Show("آیا مایل به ذخیره طرح می باشید؟", "ذخیره طرح", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (res == MessageDialogResult.Affirmative)
            {
                report.SaveLayoutToXml(clsDateClass.appStartupPath + "\\" + fileName);
                byte[] reportFile = File.ReadAllBytes(clsDateClass.appStartupPath + "\\" + fileName);
                userDefaultDesign = int.Parse(lueDesign.SelectedValue.ToString());

                printSetting.SaveDesign(userDefaultDesign.Value, reportFile);
            }
            else if (res == MessageDialogResult.Negative)
            {
                report.SaveLayoutToXml(clsDateClass.appStartupPath + "\\" + fileName);
                byte[] reportFile = File.ReadAllBytes(clsDateClass.appStartupPath + "\\" + fileName);

                MetroDialogSettings setting = new MetroDialogSettings();

                setting.AffirmativeButtonText = "تایید";
                setting.DefaultButtonFocus = MessageDialogResult.Affirmative;
                setting.NegativeButtonText = "انصراف";
                setting.DialogResultOnCancel = MessageDialogResult.Negative;

                string name = await main.ShowInputAsync("", "نام طرح را وارد کنید", setting);

                if(!String.IsNullOrEmpty(name))
                    printSetting.SaveDesign(designName, reportFile);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(lueDesign.SelectedValue.ToString());
                printSetting.Delete(id);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "DevExpress Files (*.repx) | *.repx| All Files(*.*) |*.*";
                System.Windows.Forms.DialogResult result = ofd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    DataSet ds = new DataSet();
                    DataTable detail;
                    DataTable header;

                    SqlAdoClass.ConnectionString = ConnectionStringClass.GetADOConnectionString(60);

                    SqlAdoClass.Command("sp_OrderHeaderPrint", CommandType.StoredProcedure, 0, "@id");
                    header = SqlAdoClass.ExecuteQuery("Header");

                    SqlAdoClass.Command("sp_OrderDetailsPrint", CommandType.StoredProcedure, 0, "@id");
                    detail = SqlAdoClass.ExecuteQuery("Detail");

                    byte[] file = File.ReadAllBytes(ofd.FileName);
                    File.WriteAllBytes(clsDateClass.appStartupPath + "\\" + fileName, file);

                    report.LoadLayoutFromXml(clsDateClass.appStartupPath + "\\" + fileName);

                    ds.Tables.Add(detail);
                    ds.Tables.Add(header);

                    report.DataSource = ds;
                    report.RightToLeft = RightToLeft.Yes;

                    DevExpress.XtraReports.UI.ReportDesignTool designTool = new DevExpress.XtraReports.UI.ReportDesignTool(report);
                    designTool.ShowDesignerDialog();

                    MetroDialogSettings config = new MetroDialogSettings();

                    config.AffirmativeButtonText = "بله";
                    config.AnimateHide = true;
                    config.AnimateShow = true;
                    config.ColorScheme = MetroDialogColorScheme.Theme;
                    config.DefaultButtonFocus = MessageDialogResult.Affirmative;
                    config.DialogResultOnCancel = MessageDialogResult.Canceled;
                    config.NegativeButtonText = "خیر";
                    config.FirstAuxiliaryButtonText = "انصراف";

                    var res = await main.ShowMessageAsync("", "آیا مایل به ذخیره طرح می باشید؟", MessageDialogStyle.AffirmativeAndNegative, config);

                    //MessageBoxResult res = MessageBox.Show("آیا مایل به ذخیره طرح می باشید؟", "ذخیره طرح", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                    if (res == MessageDialogResult.Affirmative)
                    {
                        report.SaveLayoutToXml(clsDateClass.appStartupPath + "\\" + fileName);
                        byte[] reportFile = File.ReadAllBytes(clsDateClass.appStartupPath + "\\" + fileName);
                        userDefaultDesign = int.Parse(lueDesign.SelectedValue.ToString());

                        printSetting.SaveDesign(userDefaultDesign.Value, reportFile);
                    }
                    else if (res == MessageDialogResult.Negative)
                    {
                        report.SaveLayoutToXml(clsDateClass.appStartupPath + "\\" + fileName);
                        byte[] reportFile = File.ReadAllBytes(clsDateClass.appStartupPath + "\\" + fileName);

                        MetroDialogSettings setting = new MetroDialogSettings();

                        setting.AffirmativeButtonText = "تایید";
                        setting.DefaultButtonFocus = MessageDialogResult.Affirmative;
                        setting.NegativeButtonText = "انصراف";
                        setting.DialogResultOnCancel = MessageDialogResult.Negative;

                        string name = await main.ShowInputAsync("", "نام طرح را وارد کنید",setting);

                        //winGetDesignName getName = new winGetDesignName();
                        //getName.ShowDialog();

                        printSetting.SaveDesign(name, reportFile);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("محتویات فایل انتخابی نامعتبر می باشد" + "\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
