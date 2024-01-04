using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    internal class WaitErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is Wait;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is Wait wait))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_wait,", indent);
            stream.Write("{0}\t\tproperty = [{{'time',", indent);

            if (wait.Time != null)
            {
                RightValueErlExporter.GenerateCode(wait.Time, stream, indent);
            }

            stream.WriteLine("}],");
        }
    }
}
