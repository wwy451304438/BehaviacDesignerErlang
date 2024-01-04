using Behaviac.Design;
using System.IO;

namespace PluginBehaviac.DataExporters
{
    public class PropertyErlExporter
    {
        public static void GenerateCode(PropertyDef property, MethodDef.Param arrayIndexElement, StringWriter stream, string indent)
        {
            if (property.IsPar || property.IsCustomized)
            {
                ParInfoErlExporter.GenerateCode(property, stream, indent);
                return;
            }

            var prop = property.BasicName;
            if (property.IsArrayElement && arrayIndexElement != null)
            {
                prop = prop.Replace("[]", "");
            }
            stream.Write("{{var,'{0}'", prop);
            if (arrayIndexElement != null && arrayIndexElement.IsArrayIndex)
            {
                stream.Write(",");
                ParameterErlExporter.GenerateCode(arrayIndexElement, stream, indent + "\t");
            }
            stream.Write("}");
        }
    }
}
