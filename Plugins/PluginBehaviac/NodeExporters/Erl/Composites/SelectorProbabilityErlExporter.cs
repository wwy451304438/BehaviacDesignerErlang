using Behaviac.Design.Nodes;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class SelectorProbabilityErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is SelectorProbability;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is SelectorProbability))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_random,", indent);
        }
    }
}
