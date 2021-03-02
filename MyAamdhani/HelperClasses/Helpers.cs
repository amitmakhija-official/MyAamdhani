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
        public static string AddCommaToDigits(this object str)
        {
            var _result = Convert.ToString(str);
            try
            {
                if (!string.IsNullOrEmpty(_result))
                    _result = string.Format("{0:#,0}", Convert.ToDecimal(_result));
            }
            catch (Exception e)
            {

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

            }
            return _result;
        }

    }
}