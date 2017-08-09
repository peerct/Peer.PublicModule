using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Diagnostics;
namespace Peer.PublicCsharpModule.CSharpXml
{
    /// <summary>
    /// XMLHelper XML文档操作管理器
    /// </summary>
    public class XMLHelper
    {
        public  XmlDocument xmlDoc = null;
        public XMLHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public enum XmlType
        {
            File,
            String
        };
        #region 创建XML文档，保存Xml
        /// <summary>
        /// 读取XML
        /// </summary>
        /// <param name="xmlString"> xml路径或者xmlstring</param>
        /// <param name="type"></param>
        /// <returns></returns>
    
        public  bool LoadXmlDocument(string xmlString, XmlType type)
        {
            xmlDoc = new XmlDocument();
            try
            {
                if (type == XmlType.File)
                {
                    xmlDoc.Load(xmlString); //加载XML文档
                } 
                else
                {
                    string str = xmlString.Substring(0, 2);
                    if (str=="<?")
                    {
                        xmlDoc.LoadXml( xmlString); //加载XML文档
                    } 
                    else
                    {
                        xmlDoc.LoadXml("<?xml version='1.0' encoding='gb2312'?>" + xmlString ); //加载XML文档
                    }
                    
                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }
            
        }
        
        /// <summary>
        /// 导出xml
        /// </summary>
        /// <param name="outString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public  string Save(string outString, XmlType type)
        {
            try
            {
                if (type == XmlType.File)
                {
                    xmlDoc.Save(outString); 
                    return outString;
                }
                else
                {
                    return xmlDoc.InnerXml;
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region XML内容操作
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Read("/Node", "")
         * XmlHelper.Read("/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public  string Read(string node, string attribute)
        {
            string value = "";
            try
            {
                XmlNode xn = xmlDoc.SelectSingleNode(node);
                
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch { }
            return value;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert( "/Node", "Element", "", "Value")
         * XmlHelper.Insert( "/Node", "Element", "Attribute", "Value")
         * XmlHelper.Insert( "/Node", "", "Attribute", "Value")
         ************************************************/
        public  void Insert(string node, string element, string attribute, string value)
        {
            try
            {

                XmlNode xn = xmlDoc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = xmlDoc.CreateElement(element);
                    if (attribute.Equals(""))
                    {
                        xe.InnerText = value;
                        xn.AppendChild(xe);
                    } 
                    else
                    {
                        xe.SetAttribute(attribute, value);
                        xn.AppendChild(xe);
                    }
                }
                
            }
            catch { }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert( "/Node", "", "Value")
         * XmlHelper.Insert( "/Node", "Attribute", "Value")
         ************************************************/
        public  void Update(string node, string attribute, string value)
        {
            try
            {

                XmlNode xn = xmlDoc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                {
                    xe.InnerText = value;
                } 
                else
                {
                    xe.SetAttribute(attribute, value);
                }

            }
            catch { }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Delete("/Node", "")
         * XmlHelper.Delete( "/Node", "Attribute")
         ************************************************/
        public  void Delete(string node, string attribute)
        {
            try
            {

                XmlNode xn = xmlDoc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                {
                    xn.ParentNode.RemoveChild(xn);
                } 
                else
                {
                    xe.RemoveAttribute(attribute);
                }
            }
            catch { }
        }
        #endregion

        #region 读取XML资源到DataSet中
        /// <summary>
        /// 读取XML资源到DataSet中
        /// </summary>
        /// <param name="source">XML资源，文件为路径，否则为XML字符串</param>
        /// <param name="xmlType">XML资源类型</param>
        /// <returns>DataSet</returns>
        public  DataSet GetDataSet(string source, XmlType xmlType)
        {
            DataSet ds = new DataSet();
            if (xmlType == XmlType.File)
            {
                ds.ReadXml(source);
            }
            else
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(source);
                XmlNodeReader xnr = new XmlNodeReader(xd);
                ds.ReadXml(xnr);
            }
            return ds;
        }

        #endregion
        #region XML文档节点查询和读取
        /// <summary>
        /// 选择匹配XPath表达式的第一个节点XmlNode.
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
        /// <returns>返回XmlNode</returns>
        public  XmlNode GetXmlNodeByXpath(string xpath)
        {
            
            try
            {
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                return xmlNode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 选择匹配XPath表达式的节点列表XmlNodeList.
        /// </summary>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
        /// <returns>返回XmlNodeList</returns>
        public  XmlNodeList GetXmlNodeListByXpath( string xpath)
        {
            

            try
            {
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
                return xmlNodeList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute.
        /// </summary>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
        /// <returns>返回xmlAttributeName</returns>
        public  XmlAttribute GetXmlAttribute(string xpath, string xmlAttributeName)
        {
            string content = string.Empty;
            
            XmlAttribute xmlAttribute = null;
            try
            {
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                
                if (xmlNode != null)
                {
                    if (xmlNode.Attributes.Count > 0)
                    {
                        xmlAttribute = xmlNode.Attributes[xmlAttributeName];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; //这里可以定义你自己的异常处理
            }
            return xmlAttribute;
        }
        #endregion

        #region XML文档创建和节点或属性的添加、修改
        /// <summary>
        /// 创建一个XML文档
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="rootNodeName">XML文档根节点名称(须指定一个根节点名称)</param>
        /// <param name="version">XML文档版本号(必须为:"1.0")</param>
        /// <param name="encoding">XML文档编码方式</param>
        /// <param name="standalone">该值必须是"yes"或"no",如果为null,Save方法不在XML声明上写出独立属性</param>
        /// <returns>成功返回true,失败返回false</returns>
        public  bool CreateXmlDocument(string rootNodeName, string version, string encoding, string standalone)
        {
            bool isSuccess = false;
            try
            {
                xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration(version, encoding, standalone);
                XmlNode root = xmlDoc.CreateElement(rootNodeName);
                xmlDoc.AppendChild(xmlDeclaration);
                xmlDoc.AppendChild(root);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //这里可以定义你自己的异常处理
            }
            return isSuccess;
        }

        /// <summary>
        /// 依据匹配XPath表达式的第一个节点来创建它的子节点(如果此节点已存在则追加一个新的同名节点
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlNodeName">要匹配xmlNodeName的节点名称</param>
        /// <param name="innerText">节点文本值</param>
        /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
        /// <param name="value">属性值</param>
        /// <returns>成功返回true,失败返回false</returns>
        public  bool CreateXmlNodeByXPath(string xpath, string xmlNodeName, string innerText, string xmlAttributeName, string value)
        {
            bool isSuccess = false;
            
            try
            {
                
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //存不存在此节点都创建
                    XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
                    subElement.InnerXml = innerText;

                    //如果属性和值参数都不为空则在此新节点上新增属性
                    if (!string.IsNullOrEmpty(xmlAttributeName) && !string.IsNullOrEmpty(value))
                    {
                        XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
                        xmlAttribute.Value = value;
                        subElement.Attributes.Append(xmlAttribute);
                    }

                    xmlNode.AppendChild(subElement);
                }
                
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //这里可以定义你自己的异常处理
            }
            return isSuccess;
        }

        /// <summary>
        /// 依据匹配XPath表达式的第一个节点来创建或更新它的子节点(如果节点存在则更新,不存在则创建)
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlNodeName">要匹配xmlNodeName的节点名称</param>
        /// <param name="innerText">节点文本值</param>
        /// <returns>成功返回true,失败返回false</returns>
        public  bool CreateOrUpdateXmlNodeByXPath( string xpath, string xmlNodeName, string innerText)
        {
            bool isSuccess = false;
            bool isExistsNode = false;//标识节点是否存在
            
            try
            {
                
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //遍历xpath节点下的所有子节点
                    foreach (XmlNode node in xmlNode.ChildNodes)
                    {
                        if (node.Name.ToLower() == xmlNodeName.ToLower())
                        {
                            //存在此节点则更新
                            node.InnerXml = innerText;
                            isExistsNode = true;
                            break;
                        }
                    }
                    if (!isExistsNode)
                    {
                        //不存在此节点则创建
                        XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
                        subElement.InnerXml = innerText;
                        xmlNode.AppendChild(subElement);
                    }
                }                
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //这里可以定义你自己的异常处理
            }
            return isSuccess;
        }

        /// <summary>
        /// 依据匹配XPath表达式的第一个节点来创建或更新它的属性(如果属性存在则更新,不存在则创建)
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
        /// <param name="value">属性值</param>
        /// <returns>成功返回true,失败返回false</returns>
        public  bool CreateOrUpdateXmlAttributeByXPath(string xpath, string xmlAttributeName, string value)
        {
            bool isSuccess = false;
            bool isExistsAttribute = false;//标识属性是否存在
            //XmlDocument xmlDoc = new XmlDocument();
            try
            {
                
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //遍历xpath节点中的所有属性
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
                        {
                            //节点中存在此属性则更新
                            attribute.Value = value;
                            isExistsAttribute = true;
                            break;
                        }
                    }
                    if (!isExistsAttribute)
                    {
                        //节点中不存在此属性则创建
                        XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
                        xmlAttribute.Value = value;
                        xmlNode.Attributes.Append(xmlAttribute);
                    }
                }
                
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //这里可以定义你自己的异常处理
            }
            return isSuccess;
        }
        #endregion


        #region XML文档节点或属性的删除
        /// <summary>
        /// 删除匹配XPath表达式的第一个节点(节点中的子元素同时会被删除)
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <returns>成功返回true,失败返回false</returns>
        public  bool DeleteXmlNodeByXPath(string xpath)
        {
            bool isSuccess = false;
            //XmlDocument xmlDoc = new XmlDocument();
            try
            {
                
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //删除节点
                    xmlNode.ParentNode.RemoveChild(xmlNode);
                }
                
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //这里可以定义你自己的异常处理
            }
            return isSuccess;
        }

        /// <summary>
        /// 删除匹配XPath表达式的第一个节点中的匹配参数xmlAttributeName的属性
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlAttributeName">要删除的xmlAttributeName的属性名称</param>
        /// <returns>成功返回true,失败返回false</returns>
        public  bool DeleteXmlAttributeByXPath(string xpath, string xmlAttributeName)
        {
            bool isSuccess = false;
            bool isExistsAttribute = false;
            
            try
            {
                
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                XmlAttribute xmlAttribute = null;
                if (xmlNode != null)
                {
                    //遍历xpath节点中的所有属性
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
                        {
                            //节点中存在此属性
                            xmlAttribute = attribute;
                            isExistsAttribute = true;
                            break;
                        }
                    }
                    if (isExistsAttribute)
                    {
                        //删除节点中的属性
                        xmlNode.Attributes.Remove(xmlAttribute);
                    }
                }
                
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //这里可以定义你自己的异常处理
            }
            return isSuccess;
        }

        /// <summary>
        /// 删除匹配XPath表达式的第一个节点中的所有属性
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <returns>成功返回true,失败返回false</returns>
        public  bool DeleteAllXmlAttributeByXPath(string xpath)
        {
            bool isSuccess = false;
            //XmlDocument xmlDoc = new XmlDocument();
            try
            {

                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //遍历xpath节点中的所有属性
                    xmlNode.Attributes.RemoveAll();
                }
                
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //这里可以定义你自己的异常处理
            }
            return isSuccess;
        }
        #endregion

    }
}



//使用方法 //这是XML文档根节点名
//            string rootNodeName = "books";

//            //这是XML文档物理文件名（包含物理路径）
//            string xmlFileName = Application.StartupPath + @"\book.xml";

//            XMLHelper.CreateXmlDocument(xmlFileName, rootNodeName, "1.0", "utf-8", null);
//            MessageBox.Show("XML文档创建成功:" + xmlFileName);


//向XML文档中添加一个新节点

//            string xmlFileName = Application.StartupPath + @"\book.xml";
//            string xpath = "/books";  //这是新节点的父节点路径
//            string nodename = "book";　//这是新节点名称,在父节点下新增
//            string nodetext = "这是新节点中的文本值";

//            bool isSuccess = XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath, nodename, nodetext);
//            MessageBox.Show("XML节点添加或更新成功:" + isSuccess.ToString());



//向XML文档中的子节点中新增或修改（如果存在则修改）一个子节点,比如name,author,date节点等：

//            string xmlFileName = Application.StartupPath + @"\book.xml";
//            string xpath = "/books/book";  //这是新子节点的父节点路径
//            string nodename = "name";　//这是新子节点名称,在父节点下新增
//            string nodetext = "我的世界我的梦";

//            bool isSuccess = XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlFileName, xpath, nodename, nodetext);
//            MessageBox.Show("XML节点添加或更新成功:" + isSuccess.ToString());

//向XML文档中的子节点中新增或修改（如果存在则修改）一个子节点属性,比如id,ISDN属性等：

//            string xmlFileName = Application.StartupPath + @"\book.xml";
//            string xpath = "/books/book"; //要新增属性的节点
//            string attributeName = "id";　//新属性名称,ISDN号也是这么新增的
//            string attributeValue = "1";　//新属性值

//            bool isSuccess = XMLHelper.CreateOrUpdateXmlAttributeByXPath(xmlFileName, xpath, attributeName, attributeValue);
//            MessageBox.Show("XML属性添加或更新成功:" + isSuccess.ToString());

//删除XML文档中的子节点：

//            string xmlFileName = Application.StartupPath + @"\book.xml";
//            string xpath = "/books/book[@id='1']"; //要删除的id为1的book子节点

//            bool isSuccess = XMLHelper.DeleteXmlNodeByXPath(xmlFileName, xpath);
//            MessageBox.Show("XML节点删除成功:" + isSuccess.ToString());

//删除XML文档中子节点的属性：

//            string xmlFileName = Application.StartupPath + @"\book.xml";
//            //删除id为2的book子节点中的ISDN属性
//            string xpath = "/books/book[@id='2']";
//            string attributeName = "ISDN";

//            bool isSuccess = XMLHelper.DeleteXmlAttributeByXPath(xmlFileName, xpath, attributeName);
//            MessageBox.Show("XML属性删除成功:" + isSuccess.ToString());

//读取XML文档中的所有子节点：

//            string xmlFileName = Application.StartupPath + @"\book.xml";
//            //要读的id为1的book子节点
//            string xpath = "/books/book[@id='1']";

//            XmlNodeList nodeList = XMLHelper.GetXmlNodeListByXpath(xmlFileName, xpath);
//            string strAllNode = "";
//            //遍历节点中所有的子节点
//            foreach (XmlNode node in nodeList)
//            {
//                strAllNode += "\n name:" + node.Name + " InnerText:" + node.InnerText;
//            }

//            MessageBox.Show("XML节点中所有子节点有:" + strAllNode);