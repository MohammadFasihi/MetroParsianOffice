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
using System.Data;

namespace ParsianOffice.Windows
{
    /// <summary>
    /// Interaction logic for winDisplayApointment.xaml
    /// </summary>
    public partial class winDisplayApointment : Window
    {
        private DataTable dataTable;

        public winDisplayApointment()
        {
            InitializeComponent();
        }


        public int ID { get; set; }
        public int cu_ID { get; set; }
        public string Date { get; set; }
        public int ep_ID { get; set; }
        public int ap_Kind { get; set; }
        public int ap_Situation { get; set; }
        public string ap_Area { get; set; }
        public string ap_Time { get; set; }
        public string ap_Address { get; set; }

        void GetData()
        {
            BLApointment apointment = new BLApointment(ID);
            cu_ID = BLApointment.apointment.cu_ID.Value;
            Date = BLApointment.apointment.Date;
            ep_ID = BLApointment.apointment.expert_ID.Value;
            ap_Kind = BLApointment.apointment.kind_ID.Value;
            ap_Situation = BLApointment.apointment.st_ID.Value;
            ap_Area = BLApointment.apointment.ap_Area;
            ap_Time = BLApointment.apointment.Time;
            ap_Address = BLApointment.apointment.Address;
        }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            GetData();

            txtID.Text = ID.ToString();
            cmbCustomer.EditValue = cu_ID;
            cmbExpert.EditValue = ep_ID;
            cmbTimePicker.Text = ap_Time;
            dpDate.Text = Date;
            cmbKind.SelectedValue = ap_Kind;
            cmbSituation.SelectedValue = ap_Situation;
            txtAddress.Text = ap_Address;
            txtArea.Text = ap_Area;
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
                }

            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
