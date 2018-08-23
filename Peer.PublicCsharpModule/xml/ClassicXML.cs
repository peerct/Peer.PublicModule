
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
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Peer.PublicCsharpModule.xml
{
    public class ClassicXML
    {
        private XmlDocument xmlDoc = null;

        #region 构造析构函数
        public ClassicXML()
        {
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<xml/>");
            _XMLPath = "";
            _errorInfo = "";
        }
        ~ClassicXML()
        {
            _XMLPath = "";
            _errorInfo = "";
            xmlDoc = null;
        }
        #endregion

        #region 公有属性
        private string _XMLPath;
        public string XMLPath
        {
            get { return this._XMLPath; }
        }
        //最后的错误信息
        private string _errorInfo;
        public string LastError
        {
            get { return this._errorInfo; }
        }
        #endregion


        #region XmlDocument基本操作
        public void BaseOperateXmlFile(string filename, string SectionName)
        {
            //创建XmlDocument对象

            XmlDocument xmlDoc = new XmlDocument();

            //载入xml文件名

            xmlDoc.Load(filename);

            //读取根节点的所有子节点，放到xn0中

            XmlNodeList xn0 = xmlDoc.DocumentElement.ChildNodes;

            //查找二级节点的内容或属性

            foreach (XmlNode node in xn0)

            {

                if (node.Name.Equals(SectionName))

                {
                    string innertext = node.InnerText.Trim(); //匹配二级节点的内容
                    string attr = node.Attributes[0].ToString(); //属性
                }

            }

        }

        public void BaseOperateXmlString(string xmldata, string SectionName)
        {
            //创建XmlDocument对象

            XmlDocument xmlDoc = new XmlDocument();

            //如果是xml字符串，则用以下形式

            xmlDoc.LoadXml(xmldata);

            //读取根节点的所有子节点，放到xn0中

            XmlNodeList xn0 = xmlDoc.DocumentElement.ChildNodes;

            //查找二级节点的内容或属性

            foreach (XmlNode node in xn0)

            {

                if (node.Name.Equals(SectionName))

                {

                    string innertext = node.InnerText.Trim(); //匹配二级节点的内容

                    string attr = node.Attributes[0].ToString(); //属性

                }

            }

        }



        #endregion

        /// <summary>
        /// 加载XML文件
        /// </summary>
        /// <param name="xmlFilePath">XML文件路径</param>
        /// <returns></returns>
        public bool ImportXmlFile(string xmlFilePath)
        {
            Boolean RetFlag = false;
            xmlDoc = new XmlDocument();
            try
            {
                //加载XML文档
                xmlDoc.Load(@xmlFilePath);
                _XMLPath = xmlFilePath;
                _errorInfo = "";
                RetFlag = true;
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->ImportXmlFile", @xmlFilePath);
            }
            return RetFlag;
        }
        /// <summary>
        /// 加载XML字符串
        /// </summary>
        /// <param name="xmlString">XML字符串</param>
        /// <returns></returns>
        public bool ImportXmlStr(string xmlString)
        {
            Boolean RetFlag = false;
            xmlDoc = new XmlDocument();
            try
            {
                string str = xmlString.Substring(0, 2);
                if (str == "<?")
                {
                    xmlDoc.LoadXml(xmlString); //加载XML文档
                }
                else
                {
                    xmlDoc.LoadXml("<?xml version='1.0' encoding='utf-8'?>" + xmlString); //加载XML文档
                }
                _XMLPath = "";
                _errorInfo = "";
                RetFlag = true;
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->ImportXmlStr", xmlString);
            }
            return RetFlag;
        }
        /// <summary>
        /// 导出xml文件
        /// </summary>
        /// <param name="outXmlFilePath">XML文件路径</param>
        /// <returns></returns>
        public Boolean ExportXmlFile(string outXmlFilePath)
        {
            Boolean RetFlag = false;
            try
            {
                xmlDoc.Save(@outXmlFilePath);
                RetFlag = true;
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->ExportXmlFile", @outXmlFilePath);
            }
            return RetFlag;
        }
        /// <summary>
        /// 导出xml字符串
        /// </summary>
        /// <returns></returns>
        public string ExportXmlStr()
        {
            string RetStr = "";
            try
            {
                RetStr = xmlDoc.InnerXml;
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->ExportXmlStr");
            }
            return RetStr;
        }
        /// <summary>
        /// 跟INI一样,读取KEY值
        /// </summary>
        /// <param name="Section">第二层节点</param>
        /// <param name="Key">第三层节点</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public string ReadKey(string Section, string Key, string DefaultValue = "")
        {
            string RetStr = DefaultValue;
            try
            {
                //第一层根节点
                XmlElement xmlRootElement = xmlDoc.DocumentElement;
                //编程中经常遇到空对象引用的异常，有时为了代码简洁我们可以这么写.
                //如果xmlRootElement是null的话，直接调用HasChildNodes会异常，加上问号则为null时不再执行后面的方法.
                if (xmlRootElement?.HasChildNodes == true)
                {
                    //第二层Section
                    XmlNode sectionNode = xmlRootElement.SelectSingleNode(Section);
                    if (sectionNode?.HasChildNodes == true)
                    {
                        //第三层Key
                        XmlNode KeyNode = sectionNode.SelectSingleNode(Key);
                        if (KeyNode == null)
                        {
                            _errorInfo = "Key not found.";
                            RetStr = DefaultValue;
                        }
                        else
                        {
                            RetStr = KeyNode.InnerText;
                            _errorInfo = "";
                        }
                    }
                    else
                    {
                        _errorInfo = "Section not found.";
                        RetStr = DefaultValue;
                    }
                }
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->ReadKey");
            }
            return RetStr;
        }
        /// <summary>
        /// 跟INI一样,写入KEY值
        /// </summary>
        /// <param name="Section">第二层节点</param>
        /// <param name="Key">第三层节点</param>
        /// <param name="NodeValue">第三层节点值</param>
        /// <returns></returns>
        public Boolean WriteKey(string Section, string Key, string NodeValue)
        {
            Boolean RetFlag = false;
            try
            {
                //第一层根节点
                XmlElement xmlRootElement = xmlDoc.DocumentElement;
                //编程中经常遇到空对象引用的异常，有时为了代码简洁我们可以这么写.
                //如果xmlRootElement是null的话，直接调用HasChildNodes会异常，加上问号则为null时不再执行后面的方法.
                if (xmlRootElement?.HasChildNodes == true)
                {
                    //第二层Section
                    XmlNode sectionNode = xmlRootElement.SelectSingleNode(Section);
                    if (sectionNode?.HasChildNodes == true)
                    {
                        //第三层Key
                        XmlNode KeyNode = sectionNode.SelectSingleNode(Key);
                        if (KeyNode == null)
                        {
                            _errorInfo = "Key not found.";
                            RetFlag = false;
                        }
                        else
                        {
                            KeyNode.InnerText = NodeValue;
                            if (_XMLPath.Length > 0)
                            {
                                if (ExportXmlFile(@_XMLPath))
                                {
                                    _errorInfo = "";
                                    RetFlag = true;
                                }
                            }
                            else
                            {
                                _errorInfo = "File not found.";
                                RetFlag = false;
                            }
                        }
                    }
                    else
                    {
                        _errorInfo = "Section not found.";
                        RetFlag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->WriteKey");
            }
            return RetFlag;
        }
        /// <summary>
        /// xml字符串转换为数据集
        /// </summary>
        /// <param name="XMLString">XML字符串</param>
        /// <returns></returns>
        public DataSet XmlString2DataSet(string XMLString)
        {
            DataSet ds = null;
            try
            {
                string str = XMLString.Substring(0, 2);
                if (str != "<?")
                {
                    XMLString = "<?xml version='1.0' encoding='utf-8'?>" + XMLString; //加载XML文档
                }
                ds = new DataSet();
                ds.ReadXml(new StringReader(XMLString));
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                ds = null;
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->XMLString2DataSet");
            }
            return ds;
        }
        /// <summary>
        /// 数据集转换为xml字符串
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public string DataSet2XmlString(DataSet ds)
        {
            string RetStr = "";
            try
            {
                StringBuilder strXml = new StringBuilder();
                strXml.Append("<?xml version='1.0' encoding='utf-8'?>");
                //添加根节点名称
                strXml.AppendFormat("<XML>");
                if (ds != null)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strXml.AppendFormat("<{0}>", dt.TableName.ToUpper());
                            for (int j = 0; j < dt.Columns.Count; j++)//将列名添加为节点名，并填充数据
                            {
                                strXml.AppendFormat("<{0}><![CDATA[{1}]]></{0}>", dt.Columns[j].ColumnName.ToUpper(), dt.Rows[i][j].ToString().Trim());
                            }
                            strXml.AppendFormat("</{0}>", dt.TableName.ToUpper());
                        }
                    }
                }
                //添加根节点名称
                strXml.AppendFormat("</XML>");
                RetStr = strXml.ToString();
                strXml.Clear();
                strXml = null;
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->DataSet2XmlString");
            }
            return RetStr;
        }
        /// <summary>
        /// 将DataSet转换为xml文件
        /// </summary>
        /// <param name="xmlDS">dataset对象</param>
        /// <param name="xmlFile">xml文件路径</param>
        public void DataSet2XmlFile(DataSet xmlDS, string xmlFile)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            try
            {
                stream = new MemoryStream();
                //从stream装载到XmlTextReader
                writer = new XmlTextWriter(stream, Encoding.UTF8);
                //用WriteXml方法写入文件.
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);

                //返回utf-8编码的文本
                StreamWriter sw = new StreamWriter(xmlFile);
                sw.WriteLine("<?xml version='1.0' encoding='utf-8'?>");
                sw.WriteLine(Encoding.UTF8.GetString(arr).Trim());
                sw.Close();
                _errorInfo = "";
            }
            catch (System.Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->DataSet2XmlFile");
            }
            finally
            {
                if (stream != null) stream.Close();
                if (writer != null) writer.Close();
            }
        }
        /// <summary>
        /// 转换datatable为xml串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rootNode">xml根节点</param>
        /// <param name="childNode">xml二级子节点</param>
        /// <returns></returns>
        public string DataTableToXmlSring(DataTable dt, string rootNode, string childNode)
        {
            string RetStr = "";
            try
            {
                StringBuilder strXml = new StringBuilder();
                strXml.Append("<?xml version='1.0' encoding='utf-8'?>");
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
                RetStr = strXml.ToString();
                strXml.Clear();
                strXml = null;
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->DataTableToXmlSring");
            }
            return RetStr;//返回Xml字符串
        }

        /// <summary>
        /// xml文件转换为数据集
        /// </summary>
        /// <param name="XmlFilePath">xml文件路径</param>
        /// <returns></returns>
        public DataSet XmlFile2DataSet(string XmlFilePath)
        {
            DataSet dsXML = null;
            try
            {
                if (File.Exists(@XmlFilePath))
                {
                    FileStream fsReadXml = new FileStream(XmlFilePath, FileMode.Open);
                    XmlTextReader myXmlReader = new XmlTextReader(fsReadXml);
                    dsXML = new DataSet();
                    dsXML.ReadXml(myXmlReader);
                    myXmlReader.Close();
                    fsReadXml.Close();
                    fsReadXml.Dispose();
                    _errorInfo = "";
                }
                else
                {
                    _errorInfo = string.Format("路径为{0}的xml文件不存在！", XmlFilePath);
                }
            }
            catch (Exception ex)
            {
                dsXML = null;
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->XmlFile2DataSet");
            }
            return dsXML;
        }




        /// <summary>
        /// XML字符串变为数组
        /// </summary>
        /// <param name="XmlString">XML字符串</param>
        public string[] XMLString2StringArray(string XmlString)
        {
            string[] str = { };
            try
            {
                int i = 0;
                Boolean RetFlag = ImportXmlStr(XmlString);
                if (RetFlag)
                {
                    XmlElement xmlRootElement = xmlDoc.DocumentElement;
                    XmlNodeList nodelist = xmlRootElement.ChildNodes;  //得到该节点的子节点
                    if (nodelist.Count > 0)
                    {
                        str = new string[nodelist.Count];
                        foreach (XmlElement el in nodelist)//读元素值
                        {
                            str[i] = el.Value;
                            i++;
                        }
                    }
                    _errorInfo = "";
                }
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->XMLString2StringArray");
            }
            return str;
        }
        /// <summary>
        /// 从指定起始节点按照XPath取子节点信息
        /// </summary>
        /// <param name="Start_At_Node">起始节点</param>
        /// <param name="Node_XPath">例："/Node/Element"</param>
        /// <returns></returns>
        public string GetNodeValueFromNode(XmlNode Start_At_Node, string Node_XPath)
        {
            string RetStr = "";
            try
            {
                XmlNode xn = Start_At_Node.SelectSingleNode(Node_XPath);
                RetStr = xn.InnerText;
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->GetNodeValueFromNode");
            }
            return RetStr;
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="node">XPath节点串</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Read("/Node", "")
         * XmlHelper.Read("/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public string GetNodeValueFromXpath(string node, string attribute)
        {
            string value = "";
            try
            {
                XmlNode xn = xmlDoc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->GetNodeValueFromXpath");
            }
            return value;
        }
        /// <summary>
        /// 向指定的父节点添加子节点
        /// </summary>
        /// <param name="Parent">XML父节点</param>
        /// <param name="Node_Name">子节点名称</param>
        /// <param name="Node_Value">子节点值</param>
        public void CreateNode(XmlNode ParentNode, string Node_Name, string Node_Value)
        {
            try
            {
                XmlElement subElement = ParentNode.OwnerDocument.CreateElement(Node_Name);
                subElement.InnerText = Node_Value;
                ParentNode.AppendChild(subElement);
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->CreateNode");
            }
        }
        /// <summary>
        /// 数组变为XML字符串
        /// </summary>
        /// <param name="StringArray">字符串数组</param>
        public string StringArray2XMLString(string[] StringArray)
        {
            string RetStr = "";
            try
            {
                xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDeclaration);
                XmlNode rootNode = xmlDoc.CreateElement("XML");
                xmlDoc.AppendChild(rootNode);
                for (int i = 0; i < StringArray.Length; i++)
                {
                    CreateNode(rootNode, string.Format("Array{0}", i), StringArray[i]);
                }
                RetStr = xmlDoc.InnerXml;
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->StringArray2XMLString");
            }
            return RetStr;
        }

        /// <summary>
        /// 创建一个XML文档 CreateXmlDocument("XML", "utf-8", null);
        /// </summary>
        /// <param name="rootNodeName">XML文档根节点名称(须指定一个根节点名称)</param>
        /// <param name="encoding">XML文档编码方式</param>
        /// <param name="standalone">该值必须是"yes"或"no",如果为null,Save方法不在XML声明上写出独立属性</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool CreateXmlDocument(string rootNodeName, string encoding, string standalone)
        {
            bool isSuccess = false;
            try
            {
                xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", encoding, standalone);
                xmlDoc.AppendChild(xmlDeclaration);
                XmlNode root = xmlDoc.CreateElement(rootNodeName);
                xmlDoc.AppendChild(root);
                _errorInfo = "";
                isSuccess = true;
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->CreateXmlDocument");
            }
            return isSuccess;
        }
        /// <summary>
        /// 取section总数
        /// </summary>
        /// <returns></returns>
        public int SectionCount()
        {
            int RetInt = 0;
            try
            {
                XmlElement xmlRootElement = xmlDoc.DocumentElement;
                if (xmlRootElement?.HasChildNodes == true)
                {
                    XmlNodeList nodelist = xmlRootElement.ChildNodes;  //得到该节点的子节点
                    RetInt = nodelist.Count;
                    _errorInfo = "";
                }
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->SectionCount");
            }
            return RetInt;
        }
        /// <summary>
        /// 根据索引取Section名称
        /// </summary>
        /// <param name="Index">索引值</param>
        /// <returns></returns>
        public string GetSections(int Index)
        {
            string RetStr = "";
            try
            {
                XmlElement xmlRootElement = xmlDoc.DocumentElement;
                if (xmlRootElement?.HasChildNodes == true)
                {
                    if (Index < xmlRootElement.ChildNodes.Count)
                    {
                        XmlNode nodeIns = xmlRootElement.ChildNodes[Index];  //得到该节点的子节点
                        if (nodeIns != null)
                        {
                            RetStr = nodeIns.Name;
                            _errorInfo = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->GetSections");
            }
            return RetStr;
        }
        /// <summary>
        /// 此函数仅用于生成包含SQL语句参数值的XML串
        /// </summary>
        /// <param name="Para_Name">参数名</param>
        /// <param name="Para_Value">参数值</param>
        /// <param name="Para_Type">参数类型</param>
        /// <returns></returns>
        public Boolean AddParameter(string Para_Name, string Para_Value, string Para_Type)
        {
            Boolean RetFlag = false;
            try
            {
                XmlElement xmlRootElement = xmlDoc.DocumentElement;
                XmlNode nodeIns = xmlRootElement.SelectSingleNode(Para_Name);
                if (nodeIns == null)
                {
                    XmlElement theElem = xmlDoc.CreateElement(Para_Name);
                    theElem.InnerText = Para_Value;
                    theElem.SetAttribute("lx", Para_Type);
                    xmlRootElement.AppendChild(theElem);
                    _errorInfo = "";
                }
                else
                {
                    nodeIns.InnerText = Para_Type;
                    nodeIns.Attributes["lx"].Value = Para_Type;
                    _errorInfo = "参数已经存在！";
                }
                RetFlag = true;
            }
            catch (Exception ex)
            {
                string[] Paras = { Para_Name, Para_Value, Para_Type };
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->AddParameter", String.Join("->", Paras));
            }
            return RetFlag;
        }
        /// <summary>
        /// 读取指定名称的参数的信息
        /// </summary>
        /// <param name="Para_Name">参数名称</param>
        /// <returns></returns>
        public Tuple<string, string, string> ReadParameter(string Para_Name)
        {
            Tuple<string, string, string> tupleIns = null;
            try
            {
                XmlElement xmlRootElement = xmlDoc.DocumentElement;
                XmlNode nodeIns = xmlRootElement.SelectSingleNode(Para_Name);
                if (nodeIns == null)
                {
                    _errorInfo = "参数不存在！";
                }
                else
                {
                    string Para_Value = nodeIns.InnerText;
                    string Para_Type = nodeIns.Attributes["lx"].Value;
                    tupleIns = new Tuple<string, string, string>(Para_Name, Para_Value, Para_Type);
                    _errorInfo = "";
                }
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->ReadParameter", Para_Name);
            }
            return tupleIns;
        }
        /// <summary>
        /// 跟INI一样,添加section
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <returns></returns>
        public Boolean AddSection(string SectionName)
        {
            Boolean RetFlag = false;
            try
            {
                XmlElement xmlRootElement = xmlDoc.DocumentElement;
                XmlNode nodeIns = xmlRootElement.SelectSingleNode(SectionName);
                if (nodeIns == null)
                {
                    XmlElement theElem = xmlDoc.CreateElement(SectionName);
                    xmlRootElement.AppendChild(theElem);
                    _errorInfo = "";
                    RetFlag = true;
                }
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->AddSection", SectionName);
            }
            return RetFlag;
        }
        /// <summary>
        /// 删除Section
        /// </summary>
        /// <param name="SectionName">节点名称</param>
        /// <returns></returns>
        public Boolean DelSection(string SectionName)
        {
            Boolean RetFlag = false;
            try
            {
                XmlElement xmlRootElement = xmlDoc.DocumentElement;
                XmlNode nodeIns = xmlRootElement.SelectSingleNode(SectionName);
                if (nodeIns != null)
                {
                    XmlElement theElem = xmlDoc.CreateElement(SectionName);
                    xmlRootElement.RemoveChild(theElem);
                    _errorInfo = "";
                    RetFlag = true;
                }
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->DelSection", SectionName);
            }
            return RetFlag;
        }
        /// <summary>
        /// 跟INI一样,向指定Section节点添加KEY和值
        /// </summary>
        /// <param name="Section">二级节点名称</param>
        /// <param name="Key">三级节点名称</param>
        /// <param name="Key">三级节点值</param>
        /// <returns></returns>
        public Boolean AddKey(string Section, string Key, string value)
        {
            Boolean RetFlag = false;
            try
            {
                //第一层根节点
                XmlElement xmlRootElement = xmlDoc.DocumentElement;
                //编程中经常遇到空对象引用的异常，有时为了代码简洁我们可以这么写.
                //如果xmlRootElement是null的话，直接调用HasChildNodes会异常，加上问号则为null时不再执行后面的方法.
                if (xmlRootElement?.HasChildNodes == true)
                {
                    //第二层Section
                    XmlNode sectionNode = xmlRootElement.SelectSingleNode(Section);
                    if (sectionNode?.HasChildNodes == true)
                    {
                        //第三层Key
                        XmlNode KeyNode = sectionNode.SelectSingleNode(Key);
                        if (KeyNode == null)
                        {
                            XmlElement theElem = xmlDoc.CreateElement(Key);
                            theElem.InnerText = value;
                            sectionNode.AppendChild(theElem);
                        }
                        else
                        {
                            KeyNode.InnerText = value;
                        }
                        RetFlag = true;
                    }
                    else
                    {
                        _errorInfo = "Section not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->AddKey", string.Format("{0}->{1}->{2}", Section, Key, value));
            }
            return RetFlag;
        }

        /// <summary>
        /// 删除第三级节点
        /// </summary>
        /// <param name="Section">二级节点名称</param>
        /// <param name="Key">三级节点名称</param>
        /// <returns></returns>
        public Boolean DelKey(string Section, string Key)
        {
            Boolean RetFlag = false;
            try
            {
                //第一层根节点
                XmlElement xmlRootElement = xmlDoc.DocumentElement;
                //编程中经常遇到空对象引用的异常，有时为了代码简洁我们可以这么写.
                //如果xmlRootElement是null的话，直接调用HasChildNodes会异常，加上问号则为null时不再执行后面的方法.
                if (xmlRootElement?.HasChildNodes == true)
                {
                    //第二层Section
                    XmlNode sectionNode = xmlRootElement.SelectSingleNode(Section);
                    if (sectionNode?.HasChildNodes == true)
                    {
                        //第三层Key
                        XmlNode KeyNode = sectionNode.SelectSingleNode(Key);
                        if (KeyNode == null)
                        {
                            _errorInfo = "Key not found.";
                        }
                        else
                        {
                            sectionNode.RemoveChild(KeyNode);
                            _errorInfo = "";
                            RetFlag = true;
                        }
                    }
                    else
                    {
                        _errorInfo = "Section not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->DelKey", string.Format("{0}->{1}", Section, Key));
            }
            return RetFlag;
        }
        /// <summary>
        /// 编码或者解码方式格式化POST到IIS的字符串数据
        /// </summary>
        /// <param name="PostData">post的字符串内容</param>
        /// <param name="CodeType">FormatType类型</param>
        /// <returns></returns>
        public string FormatDeEnString(string PostData , FormatType CodeType)
        {
            string RetStr = "";
            try
            {
                switch (CodeType)
                {
                    case FormatType.DeCode:
                        //utf-8的编码代码页是65001
                        RetStr = HttpUtility.UrlDecode(PostData, System.Text.Encoding.GetEncoding(65001));
                        break;
                    case FormatType.EnCode:
                        //65001   utf-8   Unicode(UTF-8)
                        //两次编码解决原理：https://blog.csdn.net/xjaaaxl/article/details/60868692
                        RetStr = HttpUtility.UrlEncode(HttpUtility.UrlEncode(PostData, System.Text.Encoding.GetEncoding(65001)));
                        break;
                }
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->FormatString", string.Format("{0}->{1}", PostData, CodeType.ToString()));
            }
            return RetStr;
        }
        /// <summary>
        /// 将xml格式的string格式化，方便阅读。
        /// </summary>
        /// <param name="XMLString">xml串</param>
        /// <returns></returns>
        public string FormatXML(string XMLString)
        {
            string RetStr = "";
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(XMLString);
                MemoryStream memoryStream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(memoryStream, System.Text.Encoding.GetEncoding(65001))
                {
                    Formatting = Formatting.Indented//缩进
                };
                document.Save(writer);
                StreamReader streamReader = new StreamReader(memoryStream);
                memoryStream.Position = 0;
                RetStr = streamReader.ReadToEnd();
                streamReader.Close();
                memoryStream.Close();
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->FormatString", XMLString);
            }
            return RetStr;
        }

        /// <summary>
        /// 返回字符串加解密
        /// </summary>
        /// <param name="DataSetString">字符串内容</param>
        /// <param name="CodeType">FormatType类型</param>
        /// <returns></returns>
        public string FormatReturnString(string DataSetXMLString, FormatType CodeType)
        {
            string RetStr = "";
            try
            {
                switch (CodeType)
                {
                    case FormatType.DeCode:
                        //utf-8的编码代码页是65001
                        RetStr = HttpUtility.UrlDecode(DataSetXMLString, System.Text.Encoding.GetEncoding(65001));
                        break;
                    case FormatType.EnCode:
                        //65001   utf-8   Unicode(UTF-8)
                        RetStr = HttpUtility.UrlEncode(DataSetXMLString, System.Text.Encoding.GetEncoding(65001));
                        break;
                }
                _errorInfo = "";
            }
            catch (Exception ex)
            {
                _errorInfo = Log.LogLib.GetExceptionInfo(ex, "Peer.PublicCsharpModule.xml->ClassicXML->FormatReturnString", string.Format("{0}->{1}", DataSetXMLString, CodeType.ToString()));
            }
            return RetStr;
        }
    }

    public enum FormatType
    {
        EnCode = 0,
        DeCode = 1
    }
   
}
