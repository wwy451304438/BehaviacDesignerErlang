using Behaviac.Design.Nodes;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class SelectorStochasticErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is SelectorStochastic;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is SelectorStochastic))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_selector_stochastic,", indent);
        }
    }
}
