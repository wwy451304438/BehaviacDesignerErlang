using Behaviac.Design.Nodes;
using System.IO;
using Selector = PluginBehaviac.Nodes.Selector;

namespace PluginBehaviac.NodeExporters
{
    public class SelectorErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is Selector;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is Selector))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_selector,", indent);
        }
    }
}
