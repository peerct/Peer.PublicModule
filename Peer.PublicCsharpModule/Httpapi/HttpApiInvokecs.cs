
/*   
'                   _ooOoo_
'                  o8888888o
'                  88" . "88
'                  (| -_- |)
'                  O\  =  /O
'               ____/`---'\____
'             .'  \\|     |//  `.
'            /  \\|||  :  |||//  \
'           /  _||||| -:- |||||-  \
'           |   | \\\  -  /// |   |
'           | \_|  ''\---/''  |   |
'           \  .-\__  `-`  ___/-. /
'         ___`. .'  /--.--\  `. . __
'      ."" '<  `.___\_<|>_/___.'  >'"".
'     | | :  `- \`.;`\ _ /`;.`/ - ` : | |
'     \  \ `-.   \_ __\ /__ _/   .-` /  /
'======`-.____`-.___\_____/___.-`____.-'======
'                   `=---='
'^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'         佛祖保佑       永无BUG
'==============================================================================
'文件
'名称: 
'功能: 
'作者: peer
'日期: 
'修改:
'日期:
'备注:
'==============================================================================
*/
using System;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
//添加额外这些类库的引用
using System.Net;
using System.Net.Cache;
using System.Collections;
using System.Collections.Generic;
//必须为.net4.0完整版此类库才有
using System.Web;
using System.IO;
namespace Peer.PublicCsharpModule.Httpapi
{
    public class HttpApiInvokecs
    {
        public static object GetPostValue(string ReqUrl,Dictionary<string,string> Paras)
        {

            try
            {
                //webservice地址和方法名
                string url =ReqUrl;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                SetWebRequest(ref request);
                System.Collections.Hashtable parameters = new System.Collections.Hashtable();
                //--------参数的传递------------
                if (Paras != null)
                {
                    foreach (KeyValuePair<string, string> KeyValuePairins in Paras)
                    {
                        parameters.Add(KeyValuePairins.Key, KeyValuePairins.Value);
                    }
                }
                // -------------------------------------------------------
                Byte[] data = EncodePars(parameters);
                WriteRequestData(ref request, data);

                object retstr = ReadResponse((HttpWebResponse)request.GetResponse());
                parameters.Clear();
                parameters = null;
                return retstr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void WriteRequestData(ref HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }
        public static string ReadResponse(HttpWebResponse response)
        {

            Stream responseStream1 = response.GetResponseStream();
            StreamReader StreamReader = new StreamReader(responseStream1, Encoding.UTF8);
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string retstr = sr.ReadToEnd();
            sr.Close();
            return retstr;

        }
        public static byte[] EncodePars(Hashtable Pars)
        {
            return Encoding.UTF8.GetBytes(ParsToString(Pars));
        }
        public static string ParsToString(Hashtable Pars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in Pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(Pars[k].ToString()));
            }
            return sb.ToString();
        }

        private static void SetWebRequest(ref HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
        }
    }
}
