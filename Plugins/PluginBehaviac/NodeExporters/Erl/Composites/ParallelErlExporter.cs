using Behaviac.Design.Nodes;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class ParallelErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is Parallel;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is Parallel))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_parallel,", indent);
            
            // stream.Write("{0}\t\tproperty = [{{'time',", indent);
            
        }
    }
}
