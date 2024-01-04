using Behaviac.Design.Nodes;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class WaitforSignalErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return (node is WaitforSignal);
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is WaitforSignal))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_waitfor_signal,", indent);
        }
    }
}
