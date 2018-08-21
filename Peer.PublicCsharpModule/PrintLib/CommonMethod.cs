
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
using System.Windows.Forms;

namespace Peer.PublicCsharpModule.PrintLib
{
    /// <summary>
    /// 公共方法
    /// </summary>
    public class CommonMethod
    {
        /// <summary>
        /// 显示普通 MessageBox 提示框
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowMessageBox(string msg)
        {
            MessageBox.Show(msg, "提示");
        }
        /// <summary>
        /// 毫米转为像素(注：dpi分水平和垂直，获取方法为得到 Graphics 的实例化对象 g，调用g.DpiX、g.DpiY)
        /// </summary>
        /// <param name="mm">毫米</param>
        /// <param name="fDPI">分辨率(水平/垂直)</param>
        /// <returns></returns>
        public static float MillimetersToPixel(float mm, float fDPI)
        {
            //毫米转像素：mm * dpi / 25.4 
            return (float)Math.Round((mm * fDPI / 25.4f), 2);
        }
        /// <summary>
        /// 像素转为毫米(注：dpi分水平和垂直，获取方法为得到 Graphics 的实例化对象 g，调用g.DpiX、g.DpiY)
        /// </summary>
        /// <param name="px">像素</param>
        /// <param name="fDPI">分辨率(水平/垂直)</param>
        /// <returns></returns>
        public static float PixelToMillimeters(float px, float fDPI)
        {
            //像素转毫米：px * 25.4 / dpi
            return (float)Math.Round(((px * 25.4f) / fDPI), 2); ;
        }
        /// <summary>
        /// 英寸到像素
        /// </summary>
        /// <param name="inches"></param>
        /// <returns></returns>
        public static float InchesToPixels(float inches, float fDPI)
        {
            return (float)Math.Round(inches * fDPI, 2);
        }
        /// <summary>
        /// 像素到英寸
        /// </summary>
        /// <param name="px"></param>
        /// <returns></returns>
        public static float PixelsToInches(float px, float fDPI)
        {
            return (float)Math.Round(px / fDPI, 2);
        }
        /// <summary>
        /// 毫米到英寸
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public static float MillimetersToInches(float mm)
        {
            return (float)Math.Round(mm / 25.4f, 2);
        }
        /// <summary>
        /// 英寸到毫米
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public static float InchesToMillimeters(float Inches)
        {
            return (float)Math.Round(Inches * 25.4f, 2);
        }
    }
}
