using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Threading;
using System.Globalization;

namespace ParsianOffice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Subscribe the Startup event 
            this.Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("fa-IR");
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
