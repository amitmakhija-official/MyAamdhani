using MyAamdhani.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyAamdhani.Helper
{
    public static class CommonFunction
    {
        private static readonly string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyAamdhani"].ToString();
        private static readonly SqlConnection Con = new SqlConnection(ConnectionString);

        public static string GetIp()
        {
            var visitorsIpAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                visitorsIpAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.UserHostAddress))
            {
                visitorsIpAddr = HttpContext.Current.Request.UserHostAddress;
            }
            return visitorsIpAddr;
        }
        public static string GetMobileDetail(string userAgent)
        {
            var deviceInfo = string.Empty;
            try
            {
                var os = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if (os.IsMatch(userAgent))
                    deviceInfo = os.Match(userAgent).Groups[0].Value;
                if (device.IsMatch(userAgent.Substring(0, 1)))
                    deviceInfo += device.Match(userAgent).Groups[0].Value;
            }
            catch (Exception)
            {
                //
            }
            return deviceInfo;
        }
        public static string GetDisplayName<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);
            return (attributes.Length > 0) ? attributes[0].GetName() : value.ToString();
        }

     



        public static List<SelectListItem> Title()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "", Value = "", Selected = true });
                list.Add(new SelectListItem { Text = "Mr.", Value = "Mr." });
                list.Add(new SelectListItem { Text = "Mrs.", Value = "Mrs." });
                list.Add(new SelectListItem { Text = "Miss", Value = "Miss" });
                list.Add(new SelectListItem { Text = "Ms.", Value = "Ms." });
                list.Add(new SelectListItem { Text = "Dr.", Value = "Dr." });
                list.Add(new SelectListItem { Text = "Sir", Value = "Sir" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> Suffix()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "", Value = "", Selected = true });
                list.Add(new SelectListItem { Text = "Jr.", Value = "Jr." });
                list.Add(new SelectListItem { Text = "Sr.", Value = "Sr." });
                list.Add(new SelectListItem { Text = "I", Value = "I" });
                list.Add(new SelectListItem { Text = "II", Value = "II" });
                list.Add(new SelectListItem { Text = "III", Value = "III" });
                list.Add(new SelectListItem { Text = "IV", Value = "IV" });
                list.Add(new SelectListItem { Text = "V", Value = "V" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> Gender()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "Select One", Value = "", Selected = true });
                list.Add(new SelectListItem { Text = "Male", Value = "Male" });
                list.Add(new SelectListItem { Text = "Female", Value = "Female" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> MaritalStatus()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "Select One", Value = "", Selected = true });
                list.Add(new SelectListItem { Text = "Single", Value = "Single" });
                list.Add(new SelectListItem { Text = "Married", Value = "Married" });
                list.Add(new SelectListItem { Text = "Divorced", Value = "Divorced" });
                list.Add(new SelectListItem { Text = "Fiancee", Value = "Fiancee" });
                list.Add(new SelectListItem { Text = "Separated", Value = "Separated" });
                list.Add(new SelectListItem { Text = "Widowed", Value = "Widowed" });
                list.Add(new SelectListItem { Text = "Domestic Partner (unmarried)", Value = "DomesticPartner" });
                list.Add(new SelectListItem { Text = "Civil Union/Registered Domestic Partner", Value = "CivilUnion" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> TypeOfBusiness()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "", Value = "", Selected = true });
                list.Add(new SelectListItem { Text = "C Corporation", Value = "Corporation" });
                list.Add(new SelectListItem { Text = "S Corporation", Value = "SubchapterSCorp" });
                list.Add(new SelectListItem { Text = "LLC", Value = "LLC" });
                list.Add(new SelectListItem { Text = "Individual", Value = "Individual" });
                list.Add(new SelectListItem { Text = "Two Individuals", Value = "TwoIndividuals" });
                list.Add(new SelectListItem { Text = "Three Individuals", Value = "ThreeIndividuals" });
                list.Add(new SelectListItem { Text = "Partnership", Value = "Partnership" });
                list.Add(new SelectListItem { Text = "Joint Venture", Value = "JointVenture" });
                list.Add(new SelectListItem { Text = "Not for profit", Value = "Notforprofit" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }

        public static List<SelectListItem> TypeOfBusinessForRolodex()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "", Value = "", Selected = true });
                list.Add(new SelectListItem { Text = "Corporation", Value = "Corporation" });
                list.Add(new SelectListItem { Text = "LLC", Value = "LLC" });
                list.Add(new SelectListItem { Text = "Individual", Value = "Individual" });
                list.Add(new SelectListItem { Text = "Two Individuals", Value = "TwoIndividuals" });
                list.Add(new SelectListItem { Text = "Three Individuals", Value = "ThreeIndividuals" });
                list.Add(new SelectListItem { Text = "Partnership", Value = "Partnership" });
                list.Add(new SelectListItem { Text = "Joint Venture", Value = "JointVenture" });
                list.Add(new SelectListItem { Text = "Not for profit", Value = "Notforprofit" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> AddressType(string name)
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "Home", Value = "Home" });
                list.Add(new SelectListItem { Text = "Office", Value = "Office" ,Selected = true });
                list.Add(new SelectListItem { Text = "Mailing", Value = "Mailing" });
                list.Add(new SelectListItem { Text = "Garage", Value = "Garage" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> PhoneType()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "", Value = "", Selected = true });
                list.Add(new SelectListItem { Text = "Home", Value = "Home" });
                list.Add(new SelectListItem { Text = "Work", Value = "Work" });
                list.Add(new SelectListItem { Text = "Cell", Value = "Cell" });
                list.Add(new SelectListItem { Text = "Fax", Value = "Fax" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> NoOfOwner()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "0", Value = "0", Selected = true });
                list.Add(new SelectListItem { Text = "1", Value = "1" });
                list.Add(new SelectListItem { Text = "2", Value = "2" });
                list.Add(new SelectListItem { Text = "3", Value = "3" });
                list.Add(new SelectListItem { Text = "4", Value = "4" });
                list.Add(new SelectListItem { Text = "5", Value = "5" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> PaymentMethod()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "Select One", Value = "", Selected = true });
                list.Add(new SelectListItem { Text = "Cash", Value = "3" });
                list.Add(new SelectListItem { Text = "Check", Value = "2" });
                list.Add(new SelectListItem { Text = "Credit Card", Value = "1" });
                list.Add(new SelectListItem { Text = "Certified Check", Value = "4" });
                list.Add(new SelectListItem { Text = "Money Order", Value = "5" });
                list.Add(new SelectListItem { Text = "ACH Deposit", Value = "6" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> FrequencyOfPayments()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "Daily", Value = "1" });
                list.Add(new SelectListItem { Text = "Weekly", Value = "7" });
                list.Add(new SelectListItem { Text = "Monthly", Value = "30", Selected = true });
                list.Add(new SelectListItem { Text = "Bi-Monthly", Value = "60" });
                list.Add(new SelectListItem { Text = "Quarterly", Value = "90" });
                list.Add(new SelectListItem { Text = "Semi-Annual", Value = "180" });
                list.Add(new SelectListItem { Text = "Yearly", Value = "365" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> PaymentDueDay()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "Daily", Value = "1" });
                list.Add(new SelectListItem { Text = "Month", Value = "30", Selected = true });
                list.Add(new SelectListItem { Text = "Bi-Monthly", Value = "60" });
                list.Add(new SelectListItem { Text = "Quarterly", Value = "90" });
                list.Add(new SelectListItem { Text = "Semi-Annual", Value = "180" });
                list.Add(new SelectListItem { Text = "Yearly", Value = "365" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> AccountType()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "Checking", Value = "checking" });
                list.Add(new SelectListItem { Text = "Saving", Value = "savings" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static List<SelectListItem> EntityType()
        {
            var list = new List<SelectListItem>();
            try
            {
                list.Add(new SelectListItem { Text = "Personal", Value = "personal" });
                list.Add(new SelectListItem { Text = "Business", Value = "business" });
            }
            catch (Exception)
            {
                //
            }
            return list;
        }
        public static int getInvoiceCommentCount(string agencyCode,int clientId)
        {
            var count = 0;
            try
            {
                var query = $@"select count(ic.Id)  from InvoiceComment ic
                            left join Invoice i on ic.InvoiceId = i.Id where 
                            i.AgencyCode = {agencyCode} and i.ClientId = {clientId}";
                Con.Open();
                count = Con.Query<int>(query, commandTimeout: 0).FirstOrDefault();
            }
            catch (Exception e)
            {
               //
            }
            finally
            {
                Con.Close();
            }
            return count;
        }
        public static int getNotesCount(string agencyCode, int clientId)
        {
            var count = 0;
            try
            {
                var query = $@"select count(*) from ClientNote where AddedByAgencyCode ={agencyCode} and ClientId = { clientId}";
                Con.Open();
                count = Con.Query<int>(query, commandTimeout: 0).FirstOrDefault();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                Con.Close();
            }
            return count;
        }


        public static DateTime GetUtcTimeFromCustomTimeZone(string timezone, DateTime date)
        {
            try
            {
                TimeZoneInfo timeZoneInfo;
                try
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                }
                catch
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                }
                var totalMin = timeZoneInfo.BaseUtcOffset.TotalMinutes;
                if (timeZoneInfo.IsDaylightSavingTime(date))
                    totalMin += 60;

                var convertedTime = date.AddMinutes(totalMin);
                return convertedTime;
            }
            catch(Exception e)
            {
                return date;
            }
       }

        public static string GetAmountDifference(decimal amount1, decimal amount2)
        {
            var ded = amount2 - amount1;
            var add = amount2 + amount1;
            var Differ = (ded / (add / 2)) * 100;
            return Differ.ToString() + "%";
        }
    }
}

