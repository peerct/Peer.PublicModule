using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Reflection;
namespace Peer.PublicCsharpModule.CsharpCommon
{
    public class utils
    {

        /// <summary>
        /// Gets the client's IP address.
        /// This method takes into account the X-Forwarded-For header,
        /// in case the blog is hosted behind a load balancer or proxy.
        /// </summary>
        /// <returns>The client's IP address.</returns>
        public static string GetClientIP(System.Web.HttpContext current)
        {
            var context = current;
            if (context != null)
            {
                var request = context.Request;
                if (request != null)
                {
                    string xff = request.Headers["X-Forwarded-For"];
                    string clientIP = string.Empty;
                    if (!string.IsNullOrWhiteSpace(xff))
                    {
                        int idx = xff.IndexOf(',');
                        if (idx > 0)
                        {
                            // multiple IP addresses, pick the first one
                            clientIP = xff.Substring(0, idx);
                        }
                        else
                        {
                            clientIP = xff;
                        }
                    }

                    return string.IsNullOrWhiteSpace(clientIP) ? request.UserHostAddress : clientIP;
                }
            }

            return string.Empty;
        }




        /// <summary>
        /// Gets the current directory of executing assembly.
        /// </summary>
        /// <returns>Directory path</returns>
        public static string GetCurrentDirectory()
        {
            string directory;
            try
            {
                directory = (new FileInfo(Assembly.GetExecutingAssembly().Location)).Directory.FullName;
            }
            catch (Exception)
            {
                directory = Directory.GetCurrentDirectory();
            }
            if (!directory.EndsWith(@"\"))
            {
                directory += @"\";
            }
            return directory;
        }


        /// <summary>
        /// 文件上传方法
        /// </summary>
        /// <param name="action">UEL</param>
        /// <param name="file">要上传的文件路径</param>
        /// <returns></returns>
        public static string HttpUpload(string action, string file)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest myRequest = WebRequest.Create(action) as HttpWebRequest;
            myRequest.Method = "POST";
            myRequest.ContentType = "multipart/form-data;boundary=" + boundary;
            StringBuilder sb = new StringBuilder();
            sb.Append("--" + boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"media\"; filename=\"" + file + "\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: application/octet-stream");
            sb.Append("\r\n\r\n");
            string head = sb.ToString();
            long length = 0;
            byte[] form_data = Encoding.UTF8.GetBytes(head);
            byte[] foot_data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            length = form_data.Length + foot_data.Length;

            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                length += fileStream.Length;
                myRequest.ContentLength = length;
                Stream requestStream = myRequest.GetRequestStream();
                requestStream.Write(form_data, 0, form_data.Length);

                byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    requestStream.Write(buffer, 0, bytesRead);
                requestStream.Write(foot_data, 0, foot_data.Length);
            }
            HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string json = sr.ReadToEnd().Trim();
            sr.Close();
            if (myResponse != null)
            {
                myResponse.Close();
                myRequest = null;
            }
            if (myRequest != null)
            {
                myRequest = null;
            }
            return json;
        }

    }


    /// <summary>
    /// 生成随机内容
    /// </summary>
    public class Rand
    {
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
                result += random.Next(10).ToString();
            return result;
        }
        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="IntStr">生成长度</param>
        /// <returns></returns>
        public static string Str(int Length)
        {
            return Str(Length, false);
        }
        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Str(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTools.GetNow().Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="IntStr">生成长度</param>
        /// <returns></returns>
        public static string Str_char(int Length)
        {
            return Str_char(Length, false);
        }
        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Str_char(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTools.GetNow().Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
    }

    public class DateTools
    {
        private static int timezone = 8;
        public static DateTime GetNow()
        {
            return DateTime.UtcNow.AddHours(8);// 以UTC时间为准的时间戳
        }
        public static long GetValidityNum()
        {
            return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
        public static long GetValidityNum(DateTime now)
        {
            return Convert.ToInt64((now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
        public static string ConvertToWin(string timestampString)
        {
            DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long ticks = long.Parse(timestampString + "0000000");
            TimeSpan value = new TimeSpan(ticks);
            return dateTime.Add(value).ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static DateTime ConvertToDateTime(long timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).Add(new TimeSpan(timestamp * 10000000L));
        }
        public static DateTime ConvertToDateTime(string timestampString)
        {
            DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long ticks = long.Parse(timestampString + "0000000");
            TimeSpan value = new TimeSpan(ticks);
            return dateTime.Add(value);
        }
        public static string ConvertToUnix()
        {
            DateTime value = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            string text = DateTime.Now.Subtract(value).Ticks.ToString();
            return text.Substring(0, text.Length - 7);
        }
        public static long ConvertToUnixofLong()
        {
            DateTime value = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return DateTime.Now.Subtract(value).Ticks / 10000000L;
        }
        public static string ConvertToUnix(DateTime datetime)
        {
            DateTime value = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            string text = datetime.Subtract(value).Ticks.ToString();
            return text.Substring(0, text.Length - 7);
        }
        public static string GetDayOfWeek(DateTime now)
        {
            switch (Convert.ToInt32(now.DayOfWeek))
            {
                case 0:
                    return "周日";
                case 1:
                    return "周一";
                case 2:
                    return "周二";
                case 3:
                    return "周三";
                case 4:
                    return "周四";
                case 5:
                    return "周五";
                case 6:
                    return "周六";
                default:
                    return string.Empty;
            }
        }
    }
}
