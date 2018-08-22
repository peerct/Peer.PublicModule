using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Peer.PublicVBModule.VBLogToFile;
using Peer.PublicCsharpModule.xml;
using System.Xml;

namespace winformTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Peer.PublicVBModule.VBLogToFile.LogManager.WriteLog("测试正确信息", Peer.PublicVBModule.VBLogToFile.LogType.Info);
            string info= LogManager.getLogInfo(new StackFrame(true));
            Peer.PublicVBModule.VBLogToFile.LogManager.WriteLog("测试错误信息" + info, Peer.PublicVBModule.VBLogToFile.LogType.Wrong);

            Peer.PublicVBModule.VBLogToFile.LogManager.WriteLog("测试追踪信息", Peer.PublicVBModule.VBLogToFile.LogType.Trace);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //使用方法 //这是XML文档根节点名
            string rootNodeName = "books";

            //这是XML文档物理文件名（包含物理路径）
            string xmlFileName = Application.StartupPath + @"\book.xml";
            XmlCreateLib ins = new XmlCreateLib();
            ins.CreateXmlDocument(rootNodeName, "utf-8", null);
            //向XML文档中添加一个新节点
            string xpath = "/books";  //这是新节点的父节点路径
            string nodename = "book";　//这是新节点名称,在父节点下新增
            string nodetext = "这是新节点中的文本值";
            bool isSuccess = ins.CreateOrUpdateXmlNodeByXPath( xpath, nodename, nodetext);
            MessageBox.Show("XML节点添加或更新成功:" + isSuccess.ToString());
            //向XML文档中的子节点中新增或修改（如果存在则修改）一个子节点,比如name,author,date节点等：
             xpath = "/books/book";  //这是新子节点的父节点路径
             nodename = "name";　//这是新子节点名称,在父节点下新增
             nodetext = "我的世界我的梦";
            isSuccess = ins.CreateOrUpdateXmlNodeByXPath( xpath, nodename, nodetext);
            MessageBox.Show("XML节点添加或更新成功:" + isSuccess.ToString());

            //向XML文档中的子节点中新增或修改（如果存在则修改）一个子节点属性,比如id,ISDN属性等：
            xpath = "/books/book"; //要新增属性的节点
            string attributeName = "id";　//新属性名称,ISDN号也是这么新增的
            string attributeValue = "1";　//新属性值
            isSuccess = ins.CreateOrUpdateXmlAttributeByXPath( xpath, attributeName, attributeValue);
            MessageBox.Show("XML属性添加或更新成功:" + isSuccess.ToString());

            xpath = "/books/book"; //要新增属性的节点
            attributeName = "id";　//新属性名称,ISDN号也是这么新增的
            attributeValue = "2";　//新属性值
            isSuccess = ins.CreateOrUpdateXmlAttributeByXPath(xpath, attributeName, attributeValue);
            MessageBox.Show("XML属性添加或更新成功:" + isSuccess.ToString());

            xpath = "/books/book"; //要新增属性的节点
            attributeName = "ISDN";　//新属性名称,ISDN号也是这么新增的
            attributeValue = "";　//新属性值
            isSuccess = ins.CreateOrUpdateXmlAttributeByXPath(xpath, attributeName, attributeValue);
            MessageBox.Show("XML属性添加或更新成功:" + isSuccess.ToString());

            //删除XML文档中的子节点：
             xpath = "/books/book[@id='1']"; //要删除的id为1的book子节点
             isSuccess = ins.DeleteXmlNodeByXPath(xpath);
             MessageBox.Show("XML节点删除成功:" + isSuccess.ToString());

             //删除XML文档中子节点的属性：
             xpath = "/books/book[@id='2']";
             attributeName = "ISDN";
             isSuccess = ins.DeleteXmlAttributeByXPath(xpath, attributeName);
             MessageBox.Show("XML属性删除成功:" + isSuccess.ToString());

             //读取XML文档中的所有子节点：
             //要读的id为1的book子节点
             xpath = "/books/book[@id='2']";
             XmlNodeList nodeList = ins.GetXmlNodeListByXpath(xpath);
             string strAllNode = "";
             //遍历节点中所有的子节点
             foreach (XmlNode node in nodeList)
             {
                 strAllNode += "\n name:" + node.Name + " InnerText:" + node.InnerText;
             }
             MessageBox.Show("XML节点中所有子节点有:" + strAllNode);


             xmlFileName = ins.Save(xmlFileName, XmlCreateLib.XmlType.File);
            MessageBox.Show("XML文档创建成功:" + xmlFileName);


        }

        private void button3_Click(object sender, EventArgs e)
        {
            //使用方法 //这是XML文档根节点名
            string rootNodeName = "books";

            //这是XML文档物理文件名（包含物理路径）
            string xmlFileName = Application.StartupPath + @"\book.xml";
            XmlCreateLib ins = new XmlCreateLib();
            ins.CreateXmlDocument(rootNodeName, "utf-8", null);
            //向XML文档中添加一个新节点
            string xpath = "/books";  //这是新节点的父节点路径
            string nodename = "book";　//这是新节点名称,在父节点下新增
            string nodetext = "这是新节点中的文本值";
            bool isSuccess = ins.CreateOrUpdateXmlNodeByXPath(xpath, nodename, nodetext);
            MessageBox.Show("XML节点添加或更新成功:" + isSuccess.ToString());
            //向XML文档中的子节点中新增或修改（如果存在则修改）一个子节点,比如name,author,date节点等：
            xpath = "/books/book";  //这是新子节点的父节点路径
            nodename = "name";　//这是新子节点名称,在父节点下新增
            nodetext = "我的世界我的梦";
            isSuccess = ins.CreateOrUpdateXmlNodeByXPath(xpath, nodename, nodetext);
            MessageBox.Show("XML节点添加或更新成功:" + isSuccess.ToString());
            //向XML文档中的子节点中新增或修改（如果存在则修改）一个子节点属性,比如id,ISDN属性等：
            xpath = "/books/book"; //要新增属性的节点
            string attributeName = "id";　//新属性名称,ISDN号也是这么新增的
            string attributeValue = "1";　//新属性值
            isSuccess = ins.CreateOrUpdateXmlAttributeByXPath(xpath, attributeName, attributeValue);
            MessageBox.Show("XML属性添加或更新成功:" + isSuccess.ToString());

            xpath = "/books/book"; //要新增属性的节点
            attributeName = "id";　//新属性名称,ISDN号也是这么新增的
            attributeValue = "2";　//新属性值
            isSuccess = ins.CreateOrUpdateXmlAttributeByXPath(xpath, attributeName, attributeValue);
            MessageBox.Show("XML属性添加或更新成功:" + isSuccess.ToString());

            xpath = "/books/book"; //要新增属性的节点
            attributeName = "ISDN";　//新属性名称,ISDN号也是这么新增的
            attributeValue = "";　//新属性值
            isSuccess = ins.CreateOrUpdateXmlAttributeByXPath(xpath, attributeName, attributeValue);
            MessageBox.Show("XML属性添加或更新成功:" + isSuccess.ToString());


            //删除XML文档中的子节点：
            xpath = "/books/book[@id='1']"; //要删除的id为1的book子节点
            isSuccess = ins.DeleteXmlNodeByXPath(xpath);
            MessageBox.Show("XML节点删除成功:" + isSuccess.ToString());

            //删除XML文档中子节点的属性：
            xpath = "/books/book[@id='2']";
            attributeName = "ISDN";
            isSuccess = ins.DeleteXmlAttributeByXPath( xpath, attributeName);
            MessageBox.Show("XML属性删除成功:" + isSuccess.ToString());


            //读取XML文档中的所有子节点：
            //要读的id为1的book子节点
            xpath = "/books/book[@id='2']";
            XmlNodeList nodeList = ins.GetXmlNodeListByXpath( xpath);
            string strAllNode = "";
            //遍历节点中所有的子节点
            foreach (XmlNode node in nodeList)
            {
                strAllNode += "\n name:" + node.Name + " InnerText:" + node.InnerText;
            }
            MessageBox.Show("XML节点中所有子节点有:" + strAllNode);


            string xmlstr = ins.Save(xmlFileName, XmlCreateLib.XmlType.String);
            MessageBox.Show("XML文档创建成功:" + xmlstr);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Peer.PublicCsharpModule.CsharpCommon.DebugTimer ins = new Peer.PublicCsharpModule.CsharpCommon.DebugTimer();
            string str= ins.DebugStart();
            MessageBox.Show(str);
            str = ins.DebugStop();
            MessageBox.Show(str);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //使用方法 //这是XML文档根节点名
            string rootNodeName = "books";

            //这是XML文档物理文件名（包含物理路径）
            string xmlFileName = Application.StartupPath + @"\book.xml";
            XmlCreateLib ins = new XmlCreateLib();
            ins.CreateXmlDocument(rootNodeName, "utf-8", null);
            //向XML文档中添加一个新节点
            string xpath = "/books";  //这是新节点的父节点路径
            string nodename = "book";　//这是新节点名称,在父节点下新增
            string nodetext = "这是新节点中的文本值";
            bool isSuccess = ins.CreateOrUpdateXmlNodeByXPath(xpath, nodename, nodetext);
            MessageBox.Show("XML节点添加或更新成功:" + isSuccess.ToString());
            //向XML文档中的子节点中新增或修改（如果存在则修改）一个子节点,比如name,author,date节点等：
            xpath = "/books/book";  //这是新子节点的父节点路径
            nodename = "name";　//这是新子节点名称,在父节点下新增
            nodetext = "我的世界我的梦";
            isSuccess = ins.CreateOrUpdateXmlNodeByXPath(xpath, nodename, nodetext);
            MessageBox.Show("XML节点添加或更新成功:" + isSuccess.ToString());
            string xmlstr = ins.Save(xmlFileName, XmlCreateLib.XmlType.File);
            MessageBox.Show("XML文档创建成功:" + xmlstr);

            ClassicXML insXml = new ClassicXML();
            insXml.ImportXmlFile(xmlstr);
            //读取
            string str = insXml.ReadKey("book", "name", "");
            MessageBox.Show(str);
            //写入
            insXml.WriteKey("book", "name", "123");
            insXml.ImportXmlFile(xmlstr);
            str = insXml.ReadKey("book", "name", "");
            MessageBox.Show(str);

            //转换为dataset
            DataSet ds= insXml.XmlFile2DataSet(xmlstr);
            if (ds != null)
            {
                str = insXml.ConvertDataTableToXML(ds.Tables[0], "xml", "book");
                MessageBox.Show(str);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClassicXML insXml = new ClassicXML();
            DataSet ds = insXml.XmlString2DataSet("<books><book><name>123</name><name1>123</name1></book><book><name>1234</name><name1>123</name1></book></books>");
            if (ds != null)
            {
                string str= insXml.ConvertDataTableToXML(ds.Tables[0], "xml", "book");
                MessageBox.Show(str);
            }
        }
    }
}
