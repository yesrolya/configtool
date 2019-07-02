using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTool
{
    public static class Tools
    {
        public static bool Contains(string text, string value,
        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text != null && value != null && text.IndexOf(value, stringComparison) >= 0;
        }

        public static bool Contains(string text, string value, int number,
        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text != null && value != null && text.IndexOf(value, stringComparison) >= 0 && text.IndexOf(number.ToString(), stringComparison) >= 0;
        }

        public static int Number(string str)
        {
            int result;
            int.TryParse(System.Text.RegularExpressions.Regex.Match(str, @"\d+").Value, out result);
            return result;
        }
    }
}
