using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class DecoratorLoopErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is DecoratorLoop;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is DecoratorLoop decoratorLoop))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_loop,", indent);
            stream.Write("{0}\t\tproperty = [{{count,", indent);
            VariableErlExporter.GenerateCode(decoratorLoop.Count, stream, indent + "\t\t\t");
            stream.WriteLine("}},{{within_frame,{0}}}],", decoratorLoop.DoneWithinFrame ? "true" : "false");
        }
    }
}
