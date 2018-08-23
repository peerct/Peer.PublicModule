using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Data;
using System.IO.Compression;

namespace Peer.PublicCsharpModule.network
{
   public class HttpLib
    {
        /// <summary>
        /// post调用webservice接口
        /// </summary>
        /// <param name="WebServiceUrl">webservice的URL地址</param>
        /// <param name="CallMethod">webservice的方法名</param>
        /// <param name="parameters">参数集合(参数名必须和webservice方法名定义时的参数名称一致)</param>
        /// <param name="postStringEncoding">POST 数据的 CharSet 例如："UTF-8"</param>
        /// <param name="dataEncoding">页面的 CharSet 例如："UTF-8"</param>
        /// <returns></returns>
        private static string PostCallWebService(string WebServiceUrl, string CallMethod, IDictionary<string, string> parameters,string postStringEncoding, string dataEncoding)
        {
            string RetDataStr="";
            try
            {
                StringBuilder buffer = new StringBuilder();
                if (parameters != null && parameters.Count > 0)
                {
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                }
                string postData = buffer.ToString();
                buffer.Clear();
                buffer = null;
                byte[] dataArray = null;
                //设置编码规格
                if (postData.Length > 0)
                {
                    dataArray = Encoding.GetEncoding(postStringEncoding).GetBytes(postData);
                }
                //创建Web请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(WebServiceUrl + "/" + CallMethod);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                if (postData.Length > 0)
                {
                    request.ContentLength = dataArray.Length;
                    //写入post参数
                    Stream Writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
                    Writer.Write(dataArray, 0, dataArray.Length);//把参数数据写入请求数据流
                    Writer.Close();
                }
                else
                {
                    request.ContentLength = 0;
                }
                //传过来为XML用以下方法解析
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();//获得相应
                Stream streamIns = response.GetResponseStream();//获取响应流
                Encoding encoding = Encoding.GetEncoding(dataEncoding);
                StreamReader sr = new StreamReader(streamIns, encoding);
                RetDataStr = sr.ReadToEnd();
                sr.Close();
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            catch
            {
                RetDataStr="";
            }
            return RetDataStr;
        }

        /// <summary>
        /// 支持 Session 和 Cookie 的Post调用
        /// </summary>
        /// <param name="WebUrl">post的网址</param>
        /// <param name="parameters">参数集合(参数名必须和定义时的参数名称一致)</param>
        /// <param name="CookieStr">本次调用的Session 和 Cookie信息。例如：JSESSIONID=807948C02654A8113D5AF5B1CA4655A5;td_cookie=18446744070125319012</param>
        /// <param name="postStringEncoding">POST 数据的 CharSet 例如："UTF-8"</param>
        /// <param name="dataEncoding">页面的 CharSet 例如："UTF-8"</param>
        /// <returns></returns>
        private static string PostCallWithCookie(string WebUrl, IDictionary<string, string> parameters, string CookieStr, string postStringEncoding, string dataEncoding)
        {
            string RetDataStr = "";
            try
            {
                StringBuilder buffer = new StringBuilder();
                if (parameters != null && parameters.Count > 0)
                {
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                }
                string postData = buffer.ToString();
                buffer.Clear();
                buffer = null;
                CookieContainer cc = new CookieContainer();
                cc.SetCookies(new System.Uri(WebUrl), CookieStr);
                HttpProcessLib web = new HttpProcessLib(cc);
                web.PostData(WebUrl, postData, postStringEncoding, dataEncoding, out RetDataStr);
            }
            catch
            {
                RetDataStr = "";
            }
            return RetDataStr;
        }
        /// <summary>
        /// 上传本地文件
        /// </summary>
        /// <param name="WebUrl">上传Url</param>
        /// <param name="file">本地文件全路径</param>
        /// <returns></returns>
        public static string HttpUpload(string WebUrl, string file)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest myRequest = WebRequest.Create(WebUrl) as HttpWebRequest;
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
            string Datastr = sr.ReadToEnd().Trim();
            sr.Close();
            if (myResponse != null)
            {
                myResponse.Close();
                myRequest = null;
            }
            if (myRequest != null)
            {
                myRequest.Abort();
                myRequest = null;
            }
            return Datastr;
        }

        public static Stream HttpPost(string action, byte[] data)
        {
            HttpWebRequest myRequest;
            myRequest = WebRequest.Create(action) as HttpWebRequest;
            myRequest.Method = "POST";
            myRequest.Timeout = 20 * 1000;
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            using (Stream newStream = myRequest.GetRequestStream())
            {
                newStream.Write(data, 0, data.Length);
            }
            using (HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse)
            {
                return myResponse.GetResponseStream();
            }
        }
        public static string HttpPost2(string action, string data)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            using (var stream = HttpPost(action, buffer))
            {
                StreamReader sr = new StreamReader(stream);
                data = sr.ReadToEnd();
                return data;
            }
        }
        public static string HttpPost2(string action, byte[] data)
        {
            using (var stream = HttpPost(action, data))
            {
                StreamReader sr = new StreamReader(stream);
                return sr.ReadToEnd();
            }
        }

        public static Tuple<Stream, string, string> HttpGet(string action)
        {
            HttpWebRequest myRequest = WebRequest.Create(action) as HttpWebRequest;
            myRequest.Method = "GET";
            myRequest.Timeout = 20 * 1000;
            HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse;
            var stream = myResponse.GetResponseStream();
            var ct = myResponse.ContentType;
            if (ct.IndexOf("json") >= 0 || ct.IndexOf("text") >= 0)
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    var json = sr.ReadToEnd();
                    return new Tuple<Stream, string, string>(null, ct, json);
                }
            }
            else
            {
                Stream MyStream = new MemoryStream();
                byte[] buffer = new Byte[4096];
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    MyStream.Write(buffer, 0, bytesRead);
                MyStream.Position = 0;
                return new Tuple<Stream, string, string>(MyStream, ct, string.Empty);
            }
        }

        public static string HttpGet2(string action)
        {
            return HttpGet(action).Item3;
        }
       
        /// <summary>
        /// URLGet获取
        /// </summary>
        /// <param name="_GetURL"></param>
        /// <returns></returns>
        public static String URLGet(String _GetURL)
        {
            using (WebClient MyWebClient = new WebClient())
            {
                MyWebClient.Encoding = Encoding.UTF8;
                return MyWebClient.DownloadString(_GetURL);
            }
        }

        /// <summary>
        /// 文本数据Post
        /// </summary>
        /// <param name="_ServiceURL"></param>
        /// <param name="_PostDataString"></param>
        /// <param name="_PostEncoding"></param>
        /// <returns></returns>
        public static String TextMessagePost(String _ServiceURL, String _PostDataString, String _PostEncoding)
        {
            Stream MyResponseStream = default(Stream);
            //回复流
            byte[] PostBytes = Encoding.GetEncoding(_PostEncoding).GetBytes(_PostDataString);
            HttpWebRequest MyHttpWebRequest = (HttpWebRequest)WebRequest.Create(_ServiceURL);
            //填充HttpWebRequest的基本信息
            MyHttpWebRequest.ContentType = "text/plain";
            MyHttpWebRequest.Method = "POST";
            MyHttpWebRequest.Timeout = 7000; //7秒超时

            //填充要POST的内容
            MyHttpWebRequest.ContentLength = PostBytes.Length;
            Stream MyRequestStream = MyHttpWebRequest.GetRequestStream();
            MyRequestStream.Write(PostBytes, 0, PostBytes.Length);
            MyRequestStream.Close();
            //发送POST请求到服务器并读取服务器返回信息
            try
            {
                MyResponseStream = MyHttpWebRequest.GetResponse().GetResponseStream();
            }
            catch (Exception e)
            {
                throw e;
            }
            //读取服务器返回内容
            String TempResult = String.Empty;
            StreamReader MyResponseStreamReader = new StreamReader(MyResponseStream, Encoding.GetEncoding(_PostEncoding));
            TempResult = MyResponseStreamReader.ReadToEnd().ToString();
            MyResponseStream.Close();
            MyResponseStream.Dispose();
            return TempResult;
        }

        /// <summary>
        /// 文本数据Post
        /// </summary>
        /// <param name="_ServiceURL"></param>
        /// <param name="_PostDataString"></param>
        /// <param name="_PostEncoding"></param>
        /// <returns></returns>
        public static String TextMessagePost(String _ServiceURL, String _PostDataString)
        {
            Stream MyResponseStream = default(Stream);
            //回复流
            byte[] PostBytes = Encoding.GetEncoding("utf-8").GetBytes(_PostDataString);
            HttpWebRequest MyHttpWebRequest = (HttpWebRequest)WebRequest.Create(_ServiceURL);
            //填充HttpWebRequest的基本信息
            MyHttpWebRequest.ContentType = "text/plain";
            MyHttpWebRequest.Method = "POST";
            MyHttpWebRequest.Timeout = 7000; //7秒超时

            //填充要POST的内容
            MyHttpWebRequest.ContentLength = PostBytes.Length;
            Stream MyRequestStream = MyHttpWebRequest.GetRequestStream();
            MyRequestStream.Write(PostBytes, 0, PostBytes.Length);
            MyRequestStream.Close();
            //发送POST请求到服务器并读取服务器返回信息
            try
            {
                MyResponseStream = MyHttpWebRequest.GetResponse().GetResponseStream();
            }
            catch (Exception e)
            {
                throw e;
            }
            //读取服务器返回内容
            String TempResult = String.Empty;
            StreamReader MyResponseStreamReader = new StreamReader(MyResponseStream, Encoding.GetEncoding("utf-8"));
            TempResult = MyResponseStreamReader.ReadToEnd().ToString();
            MyResponseStream.Close();
            MyResponseStream.Dispose();
            return TempResult;
        }
        /// <summary>
        /// C#使用GZIP解压缩完整读取网页内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtmlWithUtf(string url)
        {
            if (!(url.Contains("http://") || url.Contains("https://")))
            {
                url = "http://" + url;
            }
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            req.Accept = "*/*";
            req.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            req.ContentType = "text/xml";

            string sHTML = "";
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            sHTML = reader.ReadToEnd();
                        }
                    }
                }
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            sHTML = reader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            sHTML = reader.ReadToEnd();
                        }
                    }
                }
            }
            return sHTML;
        }
    }

    public class Tuple<T1, T2, T3>
    {
       public T1 Item1 { get; set; }
       public T2 Item2 { get; set; }
       public T3 Item3 { get; set; }
       public Tuple(T1 t1, T2 t2, T3 t3)
       {
           this.Item1 = t1;
           this.Item2 = t2;
           this.Item3 = t3;
       }
    }
}
