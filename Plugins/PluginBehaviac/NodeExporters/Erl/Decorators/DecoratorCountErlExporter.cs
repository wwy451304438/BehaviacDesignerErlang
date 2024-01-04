using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    internal class DecoratorCountErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is DecoratorCount;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is DecoratorCount decoratorCount))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_count,", indent);
            stream.Write("{0}\t\tproperty = [{{count,", indent);
            VariableErlExporter.GenerateCode(decoratorCount.Count, stream, indent + "\t\t\t");
            stream.WriteLine("}],");
        }
    }
}
