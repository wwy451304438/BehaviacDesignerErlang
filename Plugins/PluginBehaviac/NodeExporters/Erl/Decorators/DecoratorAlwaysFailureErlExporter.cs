using Behaviac.Design.Nodes;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class DecoratorAlwaysFailureErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is DecoratorAlwaysFailure;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is DecoratorAlwaysFailure))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_failure,", indent);
        }
    }
}
