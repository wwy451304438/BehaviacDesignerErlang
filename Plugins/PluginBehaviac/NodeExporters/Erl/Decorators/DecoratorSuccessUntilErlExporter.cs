using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class DecoratorSuccessUntilErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is DecoratorSuccessUntil;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is DecoratorSuccessUntil decoratorSuccessUntil))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_success_until,", indent);
            stream.Write("{0}\t\tproperty = [{{count,", indent);
            VariableErlExporter.GenerateCode(decoratorSuccessUntil.Count, stream, indent + "\t\t\t");
            stream.WriteLine("}],");
        }
    }
}
