using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginA
{
    //需要动态加载和卸载的类需要继承MarshalByRefObject这个类
    public class ClassA : MarshalByRefObject, ISampleInterface
    {
        public string GetValue(string sParam)
        {
            return String.Format("{0} from ClassA", sParam);
        }
    }
}
