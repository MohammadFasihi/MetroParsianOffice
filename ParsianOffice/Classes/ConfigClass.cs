using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.Profile;

namespace ParsianOffice.Classes
{
    class ConfigClass
    {
        public static void setSetting(string section, string entry, object value)
        {
            Ini winConfig = new Ini(clsDateClass.appStartupPath + "\\SystemConfigs.ini");

            winConfig.SetValue(section, entry, value);

        }

        public static string getSetting(string section, string entry)
        {
            Ini winConfig = new Ini(clsDateClass.appStartupPath + "\\SystemConfigs.ini");

            return winConfig.GetValue(section, entry).ToString();
        }
    }
}
