using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Peer.PublicCsharpModule
{
   public  class AppDomainLib : MarshalByRefObject, IDisposable
    {
        private AppDomain mpAppDomain = null;
        //缓存目录
        private string ApplicationNameStr = "";
        //插件所在子目录名称
        private string PluginDirNameStr = "";
        public AppDomainLib() { }

        ~AppDomainLib()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool bDisposing)
        {
            if (bDisposing)
            {
                if (mpAppDomain != null)
                {
                    AppDomain.Unload(mpAppDomain);
                    mpAppDomain = null;
                    if (!ApplicationNameStr.Equals(""))
                    {
                        string TmpDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ApplicationNameStr);
                        //移除缓存目录
                        if (Directory.Exists(@TmpDir))
                        {
                            Directory.Delete(@TmpDir, true);
                        }
                    }
                }
            }
        }
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public AppDomain AppDomainIns
        {
            get
            {
                if (mpAppDomain == null)
                {
                    string PrivateDirStr= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "private", PluginDirNameStr);
                    if (!Directory.Exists(PrivateDirStr))
                    {
                        Directory.CreateDirectory(PrivateDirStr);
                    }
                    ApplicationNameStr =  string.Format("{0}{1}","ApplicationLoader", Guid.NewGuid().ToString("B"));
                    AppDomainSetup setup = new AppDomainSetup();
                    setup.ApplicationName = ApplicationNameStr;
                    setup.ApplicationBase = PrivateDirStr;
                    setup.PrivateBinPath = setup.ApplicationBase;
                    setup.CachePath = AppDomain.CurrentDomain.BaseDirectory;
                    //AppDomain加载程序集的时候,如果没有ShadowCopyFiles,那就直接加载程序集,结果就是程序集被锁定,相反,如果启用了ShadowCopyFiles,则CLR会将准备加载的程序集拷贝一份至CachePath,再加载CachePath的这一份程序集,这样原程序集也就不会被锁定了.
                    //启用影像复制程序集
                    setup.ShadowCopyDirectories = setup.ApplicationBase;
                    setup.ShadowCopyFiles = "true"; 
                    //当前AppDomain也启用ShadowCopyFiles,在此,当前AppDomain也就是前面我们说过的那个默认域(Default Domain)
                    //主AppDomian在调用子AppDomain提供过来的类型,方法,属性的时候,也会将该程序集添加到自身程序集引用当中去,所以,"插件"程序集就被主AppDomain锁定
                    AppDomain.CurrentDomain.SetupInformation.ShadowCopyFiles = "true";
                    mpAppDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString("B"), null, setup);
                }
                return mpAppDomain;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pluginDirName">插件所在目录名称</param>
        /// <param name="sAssemblyName">PluginA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</param>
        /// <param name="sTypeName">PluginA.ClassA</param>
        /// <returns></returns>
        public dynamic CreateInstance(string pluginDirName,string sAssemblyName, string sTypeName)
        {
            dynamic pInstance = null;
            PluginDirNameStr = pluginDirName;
            try
            {
                return (dynamic)this.AppDomainIns.CreateInstanceAndUnwrap(sAssemblyName, sTypeName);
            }
            catch (Exception pException)
            {
                Console.WriteLine(pException.Message);
            }
            return pInstance;
        }

    }
}
