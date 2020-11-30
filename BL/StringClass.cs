using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JntNum2Text;

namespace BL
{
    public class StringClass
    {
        public static int RemoveNumberSeparator(string str)
        {
            string newStr = str.Replace(",","");
            int number = int.Parse(newStr);
            return number;
        }

        public static string NumberFormatString(int number)
        {
            string str = string.Format("{0:#,0}", number);
            return str;
        }

        public static string GetNumberToCharachter(long number)
        {
            return Num2Text.ToFarsi(number);
        }
    }
}
