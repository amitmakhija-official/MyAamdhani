using System;
using System.ComponentModel.DataAnnotations;
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