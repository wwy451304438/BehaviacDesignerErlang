using Behaviac.Design;
using System.IO;

namespace PluginBehaviac.DataExporters
{
    public class MethodErlExporter
    {
        public static void GenerateCode(MethodDef method, StringWriter stream, string indent)
        {
            var className = method.AgentType.BasicName;
            stream.Write("{{func,'{0}','{1}',[", className, method.BasicName);

            for (var i = 0; i < method.Params.Count; ++i)
            {
                if (method.Params[i].IsProperty || method.Params[i].IsLocalVar) // property
                {
                    if ((method.Params[i].Property != null && method.Params[i].Property.IsCustomized) || method.Params[i].IsLocalVar)
                    {
                        ParameterErlExporter.GenerateCode(method.Params[i], stream, indent);
                    }
                    else
                    {
                        ParameterErlExporter.GenerateCode(method.Params[i], stream, indent);
                    }
                }
                else // const value
                {
                    DataErlExporter.GenerateCode(method.Params[i].Value, stream, indent);
                }

                if (i < method.Params.Count-1)
                {
                    stream.Write(",");
                }
            }
            stream.Write("]}");
        }
    }
}
