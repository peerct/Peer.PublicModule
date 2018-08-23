
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
'功能: XML处理类 
'作者: peer
'日期: 
'修改:
'日期:
'备注:
'==============================================================================
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Peer.PublicCsharpModule
{
   public class StreamHelper
    {

       
        //二进制转换为图像
        public static Image BytesToImg(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            ms.Position = 0;
            Image img = Image.FromStream(ms);
            ms.Close();
            return img;
        }
        //Bitmap 转化为 Byte[]
        public static byte[] BitmapToBytes(Bitmap BitReturn)
        {
            byte[] bReturn = null;
            MemoryStream ms = new MemoryStream();
            BitReturn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            bReturn = ms.GetBuffer();
            return bReturn;
        }

        /// <summary>
        /// byte[]与string的转换代码
        /// </summary>
        /// <returns></returns>
        public static string BytesToString(byte[] inputBytes)
        {
            Encoding utf8 = Encoding.UTF8;
            string inputString = utf8.GetString(inputBytes);
            return inputString;
        }
        public static byte[] StringToBytes(string inputString)
        {
            Encoding utf8 = Encoding.UTF8;
            byte[] inputBytes = utf8.GetBytes(inputString);
            return inputBytes;
        }
        public static string BytesToString64(byte[] inputBytes)
        {
            string inputString = System.Convert.ToBase64String(inputBytes);
            return inputString;
        }
        public static byte[] String64ToBytes(string inputString)
        {
            byte[] inputBytes = System.Convert.FromBase64String(inputString);
            return inputBytes;
        }

        //=======================================================================================================
        //FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        //Byte[] streamByte = System.Convert.FromBase64String(pat64Str);
        //MemoryStream ms = new MemoryStream(streamByte);
        //pictureBox1.Image = System.Drawing.Image.FromStream(ms);
        //=======================================================================================================

        /// 将 Stream 转成 byte[]
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        /// 将 byte[] 转成 Stream

        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        //将 Stream 写入文件
        public void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[]
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            // 把 byte[] 写入文件
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }
        //从文件读取 Stream
        public Stream FileToStream(string fileName)
        {
            // 打开文件
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
