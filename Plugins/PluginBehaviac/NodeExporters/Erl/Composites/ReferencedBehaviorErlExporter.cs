using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class ReferencedBehaviorErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is ReferencedBehavior;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is ReferencedBehavior referencedBehavior))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_subtree,", indent);
            stream.Write("{0}\t\tproperty = [{{subtree,", indent);

            if (referencedBehavior.ReferenceBehavior != null)
            {
                RightValueErlExporter.GenerateCode(referencedBehavior.ReferenceBehavior, stream, indent);
            }
            
            stream.WriteLine("}],");
        }
    }
}
