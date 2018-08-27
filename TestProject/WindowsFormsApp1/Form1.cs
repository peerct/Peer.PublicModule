using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (Peer.PublicCsharpModule.AppDomainLib pAppDomainProxy = new Peer.PublicCsharpModule.AppDomainLib())
                {
                    string sAssemblyName = "PluginA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string sTypeName = "PluginA.ClassA";
                    //插件和主程序都需要引入Interfaces.DLL作为通信接口
                    Interfaces.ISampleInterface pI = pAppDomainProxy.CreateInstance("PluginA", sAssemblyName,sTypeName);

                    if (pI != null)
                    {
                        txtOutput.Text = pI.GetValue(txtParameter.Text);
                        MessageBox.Show("执行完卸载！");
                    }
                    else
                    {
                        throw new ApplicationException("不能导入程序集： " + sAssemblyName);
                    }
                }
            }
            catch (Exception pException)
            {
                MessageBox.Show(pException.Message);
            }
        }
        //把所有插件放于private文件夹里面
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
