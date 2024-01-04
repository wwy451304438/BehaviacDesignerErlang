using Behaviac.Design.Nodes;
using System.IO;
using Sequence = PluginBehaviac.Nodes.Sequence;

namespace PluginBehaviac.NodeExporters
{
    public class SequenceErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is Sequence;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is Sequence))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_sequence,", indent);
        }
    }
}
