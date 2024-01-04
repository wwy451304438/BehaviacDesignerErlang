using Behaviac.Design.Nodes;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class WithPreconditionErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is WithPrecondition;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is WithPrecondition))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_switch_case,", indent);
        }
    }
}
