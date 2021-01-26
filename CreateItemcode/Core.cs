using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Linq;
using System.Diagnostics;
using System.Globalization;

namespace CreateItemCode
{
    public class Core
    {
        private static Core _instance;
        private static DateTime _date;
        private static string _name;


        public static Core Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Core();
                return _instance;
            }
        }

        public static DateTime SectionDate
        {
            get { return _date; }
            set { _date = value; }
        }

        public static string SectionName
        {
            get { return _name; }
            set { _name = value; }
        }

        public string toMonkey(string text)
        {
            string[] unicode = {
                                   "а", "б", "в", "г", "д", "е", "ё", 
                                   "ж", "з", "и", "й", "к", "л", "м", 
                                   "н", "о", "ө", "п", "р", "с", "т", 
                                   "у", "ү", "ф", "х", "ц", "ч", "ш", 
                                   "щ", "ы", "ь", "ъ", "э", "ю", "я",
                                   "А", "Б", "В", "Г", "Д", "Е", "Ё", 
                                   "Ж", "З", "И", "Й", "К", "Л", "М", 
                                   "Н", "О", "Ө", "П", "Р", "С", "Т", 
                                   "У", "Ү", "Ф", "Х", "Ц", "Ч", "Ш", 
                                   "Щ", "Ы", "Ь", "Ъ", "Э", "Ю", "Я"
                               };

            string[] monkey = {
                                   "à", "á", "â", "ã", "ä", "å", "¸", 
                                   "æ", "ç", "è", "é", "ê", "ë", "ì", 
                                   "í", "î", "º", "ï", "ð", "ñ", "ò", 
                                   "ó", "¿", "ô", "õ", "ö", "÷", "ø", 
                                   "ù", "û", "ü", "ú", "ý", "þ", "ÿ",
                                   "À", "Á", "Â", "Ã", "Ä", "Å", "¨", 
                                   "Æ", "Ç", "È", "É", "Ê", "Ë", "Ì", 
                                   "Í", "Î", "ª", "Ï", "Ð", "Ñ", "Ò", 
                                   "Ó", "¯", "Ô", "Õ", "Ö", "×", "Ø", 
                                   "Ù", "Û", "Ü", "Ú", "Ý", "Þ", "ß"
                               };

            string replaced = text ?? "";

            for (int i = 0; i < unicode.Length; i++)
            {
                if (replaced.Contains(unicode[i]))
                    replaced = replaced.Replace(unicode[i], monkey[i]);
            }
            return replaced;
        }

        public string toUnicode(string text)
        {
            string[] unicode = {
                                   "а", "б", "в", "г", "д", "е", "ё", 
                                   "ж", "з", "и", "й", "к", "л", "м", 
                                   "н", "о", "ө", "п", "р", "с", "т", 
                                   "у", "ү", "ф", "х", "ц", "ч", "ш", 
                                   "щ", "ы", "ь", "ъ", "э", "ю", "я",
                                   "А", "Б", "В", "Г", "Д", "Е", "Ё", 
                                   "Ж", "З", "И", "Й", "К", "Л", "М", 
                                   "Н", "О", "Ө", "П", "Р", "С", "Т", 
                                   "У", "Ү", "Ф", "Х", "Ц", "Ч", "Ш", 
                                   "Щ", "Ы", "Ь", "Ъ", "Э", "Ю", "Я"
                               };

            string[] monkey = {
                                   "à", "á", "â", "ã", "ä", "å", "¸", 
                                   "æ", "ç", "è", "é", "ê", "ë", "ì", 
                                   "í", "î", "º", "ï", "ð", "ñ", "ò", 
                                   "ó", "¿", "ô", "õ", "ö", "÷", "ø", 
                                   "ù", "û", "ü", "ú", "ý", "þ", "ÿ",
                                   "À", "Á", "Â", "Ã", "Ä", "Å", "¨", 
                                   "Æ", "Ç", "È", "É", "Ê", "Ë", "Ì", 
                                   "Í", "Î", "ª", "Ï", "Ð", "Ñ", "Ò", 
                                   "Ó", "¯", "Ô", "Õ", "Ö", "×", "Ø", 
                                   "Ù", "Û", "Ü", "Ú", "Ý", "Þ", "ß"
                               };

            string replaced = text ?? "";

            for (int i = 0; i < monkey.Length; i++)
            {
                if (replaced.Contains(monkey[i]))
                    replaced = replaced.Replace(monkey[i], unicode[i]);
            }
            return replaced;
        }
        
        public static bool ToBool(object pObj)
        {
            if (pObj is bool)
                return (bool)pObj;
            string str = Convert.ToString(pObj);
            if (str == "1")
                return true;
            bool result;
            bool.TryParse(str, out result);
            return result;
        }
        public static char ToChar(object pobj)
        {
            if (Convert.IsDBNull(pobj) || pobj == null)
                return Convert.ToChar(" ");
            else
                return Convert.ToChar(pobj);
        }
        public static string ToStr(object pobj)
        {
            string str = "";
            if (pobj != null && !Convert.IsDBNull(pobj))
                str = Convert.ToString(pobj);
            return str;
        }
        public static byte ToByte(object pobj)
        {
            return (byte)ToDouble(pobj);
        }
        public static int ToInt(object pobj)
        {
            return (int)ToDouble(pobj);
        }
        public static short ToShort(object pobj)
        {
            if (pobj is short)
                return (short)pobj;
            short result;
            short.TryParse(Convert.ToString(pobj), NumberStyles.Number, (IFormatProvider)null, out result);
            return result;
        }
        public static float ToFloat(object pobj)
        {
            if (pobj is float)
                return (float)pobj;
            float result;
            float.TryParse(Convert.ToString(pobj), NumberStyles.Float, (IFormatProvider)null, out result);
            return result;
        }
        public static float ToSingle(object pobj)
        {
            if (pobj is float)
                return (float)pobj;
            float result;
            float.TryParse(Convert.ToString(pobj), NumberStyles.Float, (IFormatProvider)null, out result);
            return result;
        }
        public static double ToDouble(object pobj)
        {
            if (pobj is double)
                return (double)pobj;
            double result;
            if (pobj is bool)
                result = Convert.ToDouble(pobj);
            else
                double.TryParse(Convert.ToString(pobj), NumberStyles.Float, (IFormatProvider)null, out result);
            return result;
        }
        public static long ToLong(object pobj)
        {
            if (pobj is long)
                return (long)pobj;
            long result;
            if (pobj is bool)
                result = Convert.ToInt64(pobj);
            else
                long.TryParse(Convert.ToString(pobj), NumberStyles.Any, (IFormatProvider)null, out result);
            return result;
        }
        public static ulong ToULong(object pobj)
        {
            if (pobj is ulong)
                return (ulong)pobj;
            ulong result;
            if (pobj is bool)
                result = Convert.ToUInt64(pobj);
            else
                ulong.TryParse(Convert.ToString(pobj), NumberStyles.Any, (IFormatProvider)null, out result);
            return result;
        }
        public static Decimal ToDecimal(object pobj)
        {
            if (pobj is Decimal)
                return (Decimal)pobj;
            Decimal result;
            Decimal.TryParse(Convert.ToString(pobj), NumberStyles.Any, CultureInfo.CurrentCulture, out result);
            return result;
        }
        public static DateTime ToDate(object pobj)
        {
            if (pobj is DateTime)
                return (DateTime)pobj;
            DateTime result;
            DateTime.TryParseExact(Convert.ToString(pobj), new string[9]
            {
        "G",
        "yyyyMMdd HH:mm:ss",
        "yyyy/M/d HH:mm:ss",
        "yyyy-M-d HH:mm:ss",
        "yyyy.M.d HH:mm:ss",
        "yyyyMMdd",
        "yyyy/M/d",
        "yyyy-M-d",
        "yyyy.M.d"
            }, (IFormatProvider)null, DateTimeStyles.None, out result);
            return result.Date;
        }
        public static DateTime ToDateTime(object pobj)
        {
            if (pobj is DateTime)
                return (DateTime)pobj;
            DateTime result;
            DateTime.TryParseExact(Convert.ToString(pobj), new string[9]
            {
        "G",
        "yyyyMMdd HH:mm:ss",
        "yyyy/M/d HH:mm:ss",
        "yyyy-M-d HH:mm:ss",
        "yyyy.M.d HH:mm:ss",
        "yyyyMMdd",
        "yyyy/M/d",
        "yyyy-M-d",
        "yyyy.M.d"
            }, CultureInfo.CurrentCulture, DateTimeStyles.None, out result);
            return result;
        }
        public static string ToDateStr(object pobj)
        {
            return ToDate(pobj).ToString("yyyy.MM.dd");
        }
        public static string ToDateTimeStr(object pobj)
        {
            return ToDateTime(pobj).ToString("yyyy.MM.dd HH:mm:ss");
        }
    }
}
