
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
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

namespace Peer.PublicCsharpModule.PrintLib
{

    /// <summary>
    /// 打印图片内容
    /// 使用示例:
    /// private ImagePrinter m_print = null;
    /// m_print = new ImagePrinter("");
    /// Bitmap _Bitmap = (Bitmap)Image.FromFile(@"D:\BLIMAGE\2017\4\13\1700005\Report_1700005_1.jpg");
    /// m_print.PrinterReady(_Bitmap);
    /// 打印预览： m_print.PrintView();
    /// 打印：m_print.PrintOut();
    /// </summary>
    public class ImagePrinter : IDisposable
    {
        public ImagePrinter(string printerName)
        {
            if (!string.IsNullOrEmpty(printerName))
                this.DefaultPrinterName = printerName;
            //打印事件设置
            m_printDoc.PrintPage += new PrintPageEventHandler(this.m_printDoc_PrintPage);
        }


        #region 变量
        /// <summary>
        /// 打印内容
        /// </summary>
        protected Bitmap m_printContent = null;
        /// <summary>
        /// 打印机名称
        /// </summary>
        protected string m_printerName = string.Empty;
        /// <summary>
        /// 打印预览
        /// </summary>
        PrintPreviewDialog m_printPreview = new PrintPreviewDialog();
        /// <summary>
        /// 待打印文档
        /// </summary>
        PrintDocument m_printDoc = new PrintDocument();
        #endregion


        #region 属性
        /// <summary>
        /// 待打印文档名，打印队列中的显示值
        /// </summary>
        /// <remarks>创建人员(日期):★草青工作室★(111216 12:13)</remarks>
        public string PrintDocName
        {
            set { m_printDoc.DocumentName = value; }
            get { return m_printDoc.DocumentName; }
        }
        /// <summary>
        /// 获取或设置默认纸张名称
        /// </summary>
        /// <remarks>创建人员(日期):★草青工作室★(111216 12:13)</remarks>
        public string DefaultPrinterName
        {
            set { m_printDoc.DefaultPageSettings.PrinterSettings.PrinterName = value; }
            get { return m_printDoc.DefaultPageSettings.PrinterSettings.PrinterName; }
        }
        /// <summary>
        /// 获取或设置默认纸张大小
        /// </summary>
        /// <remarks>创建人员(日期):★草青工作室★(111216 12:13)</remarks>
        public PaperSize DefaultPaperSize
        {
            set { m_printDoc.DefaultPageSettings.PaperSize = value; }
            get { return m_printDoc.DefaultPageSettings.PaperSize; }
        }
        /// <summary>
        /// 获取或设置默认页边距
        /// </summary>
        /// <remarks>创建人员(日期):★草青工作室★(111216 12:13)</remarks>
        public Margins DefaultMargins
        {
            set { m_printDoc.DefaultPageSettings.Margins = value; }
            get { return m_printDoc.DefaultPageSettings.Margins; }
        }

        #endregion


        #region 方法
        /// <summary>
        /// 准备打印
        /// </summary>
        /// <param name="printContent">打印内容</param>
        /// <returns></returns>
        /// <remarks>创建人员(日期):★草青工作室★(111217 16:45)</remarks>
        public bool PrinterReady(Bitmap printContent)
        {
            m_printContent = printContent;
            //打印事件设置
            m_printDoc.PrintPage += new PrintPageEventHandler(this.m_printDoc_PrintPage);
            return true;
        }
        /// <summary>
        /// 打印浏览
        /// </summary>
        /// <remarks>创建人员(日期):★草青工作室★(111215 21:50)</remarks>
        public DialogResult PrintView()
        {
            //打印预览
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = m_printDoc;
            return ppd.ShowDialog();
        }
        /// <summary>
        /// 开始打印
        /// </summary>
        /// <remarks>创建人员(日期):★草青工作室★(111215 21:50)</remarks>
        public void PrintOut()
        {
            try
            {
                m_printDoc.Print();
            }
            catch (Exception ex)
            {
                //RegisterLog.ExceptionsStack.RegisterError(ex);
                MessageBox.Show(ex.Message, "打印出错,详见异常日志！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_printDoc.PrintController.OnEndPrint(m_printDoc, new PrintEventArgs());
            }
        }
        /// <summary>
        /// 打印事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            //目标大小(注：打印区域使用的是‘百分之一英寸’为单位)
            int x = e.MarginBounds.X;
            int y = e.MarginBounds.Y;
            float width = CommonMethod.InchesToPixels((float)Math.Round(e.MarginBounds.Width / 100.0, 2), e.Graphics.DpiX);
            float height = CommonMethod.InchesToPixels((float)Math.Round(e.MarginBounds.Height / 100.0, 2), e.Graphics.DpiY);
            RectangleF destRectF = new RectangleF(x, y, width, height);//目标大小
            //原图大小
            Rectangle srcRectF = new Rectangle(0, 0, m_printContent.Width, m_printContent.Height);
            //绘制打印内容
            e.Graphics.DrawImage(m_printContent
              , destRectF, srcRectF
              , GraphicsUnit.Pixel);
        }


        #endregion




        public void Dispose()
        {
            if (m_printDoc != null)
                m_printDoc.PrintPage -= new PrintPageEventHandler(this.m_printDoc_PrintPage);
        }
    }
}
