using System;
using Behaviac.Design;
using System.IO;

namespace PluginBehaviac.DataExporters
{
    public class ParInfoErlExporter
    {
        public static void GenerateCode(PropertyDef property, StringWriter stream, string indent)
        {
            stream.Write("{{par,{0}}}", property.BasicName[property.BasicName.Length-1]);          
        }
    }
}
