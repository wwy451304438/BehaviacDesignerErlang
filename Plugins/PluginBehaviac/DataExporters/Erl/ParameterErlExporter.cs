using Behaviac.Design;
using System.IO;

namespace PluginBehaviac.DataExporters
{
    public class ParameterErlExporter
    {
        public static void GenerateCode(MethodDef.Param param, StringWriter stream, string indent)
        {
            if (param.Value is ParInfo par)
            {
                ParInfoErlExporter.GenerateCode(par, stream, indent);
            }
            DataErlExporter.GenerateCode(param.Value, stream, indent);
        }
    }
}
