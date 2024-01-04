using Behaviac.Design;
using System.IO;

namespace PluginBehaviac.DataExporters
{
    public class VariableErlExporter
    {
        public static void GenerateCode(VariableDef variable, StringWriter stream, string indent)
        {
            if (variable.ValueClass == VariableDef.kConst)
            {
                DataErlExporter.GenerateCode(variable.Value, stream, indent);
            }
            else if (variable.Property != null)
            {
                PropertyErlExporter.GenerateCode(variable.Property, variable.ArrayIndexElement, stream, indent);
            }
        }
    }
}
