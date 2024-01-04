using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class DecoratorLoopUntilErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is DecoratorLoopUntil;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is DecoratorLoopUntil decoratorLoopUntil))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_loop_until,", indent);
            stream.Write("{0}\t\tproperty = [{{count,", indent);
            VariableErlExporter.GenerateCode(decoratorLoopUntil.Count, stream, indent + "\t\t\t");
            stream.WriteLine("}},{{until,{0}}}],", decoratorLoopUntil.Until ? "true" : "false");
        }
    }
}
