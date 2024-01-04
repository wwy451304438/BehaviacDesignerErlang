using Behaviac.Design;
using System.IO;

namespace PluginBehaviac.DataExporters
{
    internal class ArrayErlExporter
    {
        public static void GenerateCode(object obj, StringWriter stream, string indent)
        {
            if (obj == null)
            {
                return;
            }
            
            var type = obj.GetType();

            if (!Behaviac.Design.Plugin.IsArrayType(type))
            {
                return;
            }
            var list = (System.Collections.IList)obj;

            stream.Write("[");
            if (list.Count > 0)
            {
                for (var i = 0; i < list.Count; ++i)
                {
                    if (i > 0)
                    {
                        stream.Write(",");
                    }
                    DataErlExporter.GenerateCode(list[i], stream, indent);
                }
            }
            stream.Write("]");
        }
    }
}
