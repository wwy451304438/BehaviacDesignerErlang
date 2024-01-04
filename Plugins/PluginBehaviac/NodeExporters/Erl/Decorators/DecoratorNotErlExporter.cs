using Behaviac.Design.Nodes;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class DecoratorNotErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is DecoratorNot;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is DecoratorNot))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_not,", indent);
        }
    }
}
