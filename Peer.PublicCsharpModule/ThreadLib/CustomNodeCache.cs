using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web.Configuration;

namespace Peer.PublicCsharpModule.ThreadLib
{
    /// <summary>
    /// 所有节点对象管理
    /// </summary>
    public class CustomNodeCache
    {

        /// <summary>
        /// 禁用构造方法
        /// </summary>
        private CustomNodeCache() { }

        /// <summary>
        /// 新实例化节点对象锁
        /// </summary>
        private static readonly object lockObj = new object();

        /// <summary>
        /// 单例模式
        /// </summary>
        private static readonly CustomNodeCache cnc = new CustomNodeCache();

        /// <summary>
        /// 快捷访问
        /// </summary>
        public static CustomNodeCache Sections { get { return cnc; } }

        /// <summary>
        /// 已实例化节点对象列表
        /// </summary>
        private static readonly List<CustomNodeSection> list = new List<CustomNodeSection>();

        /// <summary>
        /// 获取节点对象
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public CustomNodeSection this[string nodeName]
        {
            get
            {
                var section = list.FirstOrDefault(p => p.NodeName == nodeName);
                if (section == null)
                {
                    lock (lockObj)
                    {
                        section = list.FirstOrDefault(p => p.NodeName == nodeName);
                        if (section == null)
                        {
                            section = new CustomNodeSection(nodeName);
                            list.Add(section);
                        }
                    }
                }
                return section;
            }
        }

    }

    /// <summary>
    /// 节点对象
    /// </summary>
    public class CustomNodeSection
    {

        /// <summary>
        /// 节点根目录名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 缓存的节点key-value键值对
        /// </summary>
        public Dictionary<string, string> kv = new Dictionary<string, string>();

        /// <summary>
        /// 初始化并缓存其数据
        /// </summary>
        /// <param name="nodeName"></param>
        public CustomNodeSection(string nodeName)
        {
            NodeName = nodeName;

            var coll = (NameValueCollection)WebConfigurationManager.GetSection(nodeName);
            if (coll != null && coll.Count > 0)
            {
                foreach (string item in coll.AllKeys)
                {
                    kv.Add(item, coll[item]);
                }
            }
        }

        /// <summary>
        /// 获取指定key的值，key不存在则为null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (!kv.ContainsKey(key))
                    return null;
                else
                    return kv[key];
            }
        }

    }
}
