using Behaviac.Design.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class TaskErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is Task;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is Task))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_task,", indent);
        }
    }
}
