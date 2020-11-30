using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsianOffice
{
    public class clsDateClass
    {

        public static string appStartupPath { get; set; } = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        public static string PersianDate(DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            string fullDate = pc.GetYear(dateTime).ToString("0000") + "/" + pc.GetMonth(dateTime).ToString("00") + "/" + pc.GetDayOfMonth(dateTime).ToString("00");
            return fullDate;
        }

        public static string AddDay(string date)
        {
            PersianCalendar Miladi = new PersianCalendar();
            int year = int.Parse(date.Substring(0, 4));
            int month = int.Parse(date.Substring(5, 2));
            int day = int.Parse(date.Substring(8, 2));
            DateTime currDate = Miladi.ToDateTime(year, month, day, 0, 0, 0, 0);
            return PersianDate(currDate.AddDays(1));
        }
        public static string MinuseDay(string date)
        {
            PersianCalendar Miladi = new PersianCalendar();
            int year = int.Parse(date.Substring(0, 4));
            int month = int.Parse(date.Substring(5, 2));
            int day = int.Parse(date.Substring(8, 2));
            DateTime currDate = Miladi.ToDateTime(year, month, day, 0, 0, 0, 0);
            return PersianDate(currDate.AddDays(-1));
        }
    }
}
