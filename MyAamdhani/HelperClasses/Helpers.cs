using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
namespace MyAamdhani.HelperClasses
{
    public static class Helpers
    {
        public static string GetEnumDisplay(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DisplayAttribute[] attributes = fi.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Name;
            }

            return value.ToString();
        }
        public static string DisplayName(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = ((DisplayAttribute)attrs[0]).Name;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetName();
            }

            return outString;
        }
        public static string CommaSeperated(this string number, bool showDecimal = true)
        {
            if (string.IsNullOrEmpty(number)) return string.Empty;

            double num = 0;
            if (double.TryParse(number, NumberStyles.Float, null, out num))
            {
                if (showDecimal)
                    return String.Format("{0:n}", num);
                else
                    return String.Format("{0:n0}", num);
            }
            else return number;
        }

        public static string GetAmountDifference(decimal amount1,decimal amount2)
        {
            var ded = amount2 - amount1;
            var add = amount2 + amount1;
            var Differ = (ded / (add / 2)) * 100;
            return Differ.ToString()+"%";
        }
        public static string AddCommaToDigits(this object str)
        {
            var _result = Convert.ToString(str);
            try
            {
                if (!string.IsNullOrEmpty(_result))
                    _result = string.Format("{0:#,0}", Convert.ToDecimal(_result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _result;
        }

        public static string RemoveCommaFromNumberString(this object str)
        {
            var _result = Convert.ToString(str);
            try
            {
                if (!string.IsNullOrEmpty(_result))
                {
                    _result = _result.Replace(",", "");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _result;
        }

    }
}