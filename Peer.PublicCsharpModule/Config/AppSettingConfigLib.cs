using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Peer.PublicCsharpModule.Config
{
   public class AppSettingConfigLib
    {

        ///<summary>  
        ///在＊.exe.config文件中appSettings配置节增加一对键、值对  
        ///</summary>  
        ///<param name="newKey"></param>  
        ///<param name="newValue"></param>  
        public static void UpdateAppConfig(string newKey, string newValue)
        {
                bool isModified = false;
                foreach (string key in ConfigurationManager.AppSettings)
                {
                    if (key == newKey)
                    {
                        isModified = true;
                        break;
                    }
                }
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (isModified)
                {
                    config.AppSettings.Settings.Remove(newKey);
                }
                config.AppSettings.Settings.Add(newKey, newValue);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
        }


        /// <summary>  
        /// 根据Key取Value值  
        /// </summary>  
        /// <param name="key"></param>  
        public static string GetValue(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigurationManager.AppSettings[key].ToString().Trim();
            }
            else
            {
                return "";
            }
        }

        /// <summary>  
        /// 根据Key修改Value  
        /// </summary>  
        /// <param name="key">要修改的Key</param>  
        /// <param name="value">要修改为的值</param>  
        public static void SetValue(string key, string value)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                ConfigurationManager.AppSettings.Set(key, value);
            }
        }

        /// <summary>  
        /// 添加新的Key ，Value键值对  
        /// </summary>  
        /// <param name="key">Key</param>  
        /// <param name="value">Value</param>  
        public static void Add(string key, string value)
        {
            if (ConfigurationManager.AppSettings[key] == null)
            {
                ConfigurationManager.AppSettings.Add(key, value);
            }
        }

        /// <summary>  
        /// 根据Key删除项  
        /// </summary>  
        /// <param name="key">Key</param>  
        public static void Remove(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                ConfigurationManager.AppSettings.Remove(key);
            }
        }  
    }
}
