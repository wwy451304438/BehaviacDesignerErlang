using Behaviac.Design;
using System.IO;

namespace PluginBehaviac.DataExporters
{
    public class RightValueErlExporter
    {
        public static void GenerateCode(RightValueDef rightValue, StringWriter stream, string indent)
        {
            if (rightValue.IsMethod)
            {
                MethodErlExporter.GenerateCode(rightValue.Method, stream, indent);
            }
            else
            {
                VariableErlExporter.GenerateCode(rightValue.Var, stream, indent);
            }
        }
    }
}
