
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Data;

namespace Peer.PublicCsharpModule.zip
{
    public class GzipLib
    {
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static DataSet GetDatasetByString(string Value)
        {
            DataSet ds = new DataSet();
            string CC = GZipDecompressString(Value);
            System.IO.StringReader Sr = new StringReader(CC);
            ds.ReadXml(Sr);
            return ds;
        }
        /// <summary>
        /// 根据DATASET压缩字符串
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string GetStringByDataset(string ds)
        {
            return GZipCompressString(ds);
        }
        /// <summary>
        /// 将传入字符串以GZip算法压缩后，返回Base64编码字符
        /// </summary>
        /// <param name="rawString">需要压缩的字符串</param>
        /// <returns>压缩后的Base64编码的字符串</returns>
        public static string GZipCompressString(string rawString)
        {
            if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(rawString.ToString());
                byte[] zippedData = CompressBytes(rawData);
                return (string)(Convert.ToBase64String(zippedData));
            }
        }
        /// <summary>
        /// 将传入的二进制字符串资料以GZip算法解压缩
        /// </summary>
        /// <param name="zippedString">经GZip压缩后的二进制字符串</param>
        /// <returns>原始未压缩字符串</returns>
        public static string GZipDecompressString(string zippedString)
        {
            if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] zippedData = Convert.FromBase64String(zippedString.ToString());
                return (string)(System.Text.Encoding.UTF8.GetString(DecompressBytes(zippedData)));
            }
        }



        //压缩字节
        //1.创建压缩的数据流 
        //2.设定compressStream为存放被压缩的文件流,并设定为压缩模式
        //3.将需要压缩的字节写到被压缩的文件流
        public static byte[] CompressBytes(byte[] rawData)
        {
            using (MemoryStream compressStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(compressStream, CompressionMode.Compress,true))
                {
                    zipStream.Write(rawData, 0, rawData.Length);
                }
                return compressStream.ToArray();
            }
        }
        //解压缩字节
        //1.创建被压缩的数据流
        //2.创建zipStream对象，并传入解压的文件流
        //3.创建目标流
        //4.zipStream拷贝到目标流
        //5.返回目标流输出字节
        public static byte[] DecompressBytes(byte[] bytes)
        {
            using (var compressStream = new MemoryStream(bytes))
            {
                using (var zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        return resultStream.ToArray();
                    }
                }
            }
        }
        
        
        /// <summary>
        /// GZIP解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] zippedData)
        {
            MemoryStream ms = new MemoryStream(zippedData);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }
    }
}