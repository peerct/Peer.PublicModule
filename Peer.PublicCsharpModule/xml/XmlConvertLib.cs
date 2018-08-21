
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace Peer.PublicCsharpModule.CSharpXml
{
    public class XmlConvertLib
    {
       /// <summary>
       /// 转换DataReader为DataTable
       /// </summary>
       /// <param name="reader"></param>
       /// <returns></returns>
       public static DataTable ConverDataReaderToDataTable(IDataReader reader)
        {
            // 检查外部传入的参数
            if (null == reader)
                return null;

            // 实例化一个DataTable
            DataTable dataTable = new DataTable();
            dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;

            int fieldCount = reader.FieldCount;

            // 在表中创建字段
            for (int counter = 0; counter < fieldCount; counter++)
            {
                dataTable.Columns.Add(reader.GetName(counter), reader.GetFieldType(counter));
            }

            dataTable.BeginLoadData();

            object[] values = new object[fieldCount];
            while (reader.Read())
            {
                // 添加行
                reader.GetValues(values);
                dataTable.LoadDataRow(values, true);
            }

            // 完成转换并返回
            dataTable.EndLoadData();
            return dataTable;
        }

       /// <summary>
       /// 转换datatable为xml串
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="rootNode">xml根节点</param>
       /// <param name="childNode">xml二级子节点</param>
       /// <returns></returns>
       public static string ConvertDataTableToXML(DataTable dt, string rootNode, string childNode)
       {
           StringBuilder strXml = new StringBuilder();
           strXml.Append("<?xml version='1.0' encoding='UTF-8'?>");
           //添加根节点名称
           strXml.AppendFormat("<{0}>", rootNode);
           if (dt != null)
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   if (childNode != "")//子节点为空时不再添加
                       strXml.AppendFormat("<{0}>", childNode);
                   for (int j = 0; j < dt.Columns.Count; j++)//将列名添加为节点名，并填充数据
                   {
                       strXml.AppendFormat("<{0}><![CDATA[{1}]]></{0}>", dt.Columns[j].ColumnName, dt.Rows[i][j].ToString().Trim());
                   }
                   if (childNode != "")
                       strXml.AppendFormat("</{0}>", childNode);
               }
           }
           //添加根节点名称
           strXml.AppendFormat("</{0}>", rootNode);
           string RetStr = strXml.ToString();
           strXml.Clear();
           strXml = null;
           return RetStr;//返回Xml字符串
       }
       /// <summary>
       /// 转换xml串为Dataset
       /// </summary>
       /// <param name="xmldata">带有<?xml version='1.0' encoding='UTF-8'?>格式的xml串</param>
       /// <returns></returns>
       public static DataSet ConvertXmlToDataset(string xmldata)
       {
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(xmldata));
            return ds;
       }
       

    }
}
