using DA;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace ParsianOffice
{
    /// <summary>
    /// Interaction logic for winPreview.xaml
    /// </summary>
    public partial class winPreview : Window
    {
        public winPreview()
        {
            InitializeComponent();
        }
        public DevReport report = new DevReport();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            report.CreateDocument();
            previewDocument.DocumentSource = report;
        }
    }
}
