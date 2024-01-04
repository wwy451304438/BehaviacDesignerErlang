using Behaviac.Design.Nodes;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class SequenceStochasticErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is SequenceStochastic;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is SequenceStochastic))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_sequence_stochastic,", indent);
        }
    }
}
