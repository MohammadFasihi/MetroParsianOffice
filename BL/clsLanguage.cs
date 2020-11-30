using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ParsianOffice
{
    public class clsLanguage
    {
        static CultureInfo language { get; set; }
        static CultureInfo currCulture { get; set; }
        public static void Persion()
        {
            language = new CultureInfo("fa-ir");
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(language);
        }
        public static void English()
        {
            language = new CultureInfo("en-us");
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(language);
        }

        public static void SaveLanguage()
        {
            currCulture = CultureInfo.CurrentUICulture;
        }

        public static void RestoreLanguage()
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(currCulture);
        }
    }
}
