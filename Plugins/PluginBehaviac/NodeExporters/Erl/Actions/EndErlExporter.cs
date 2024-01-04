using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class EndErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is End;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is End end))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_end,", indent);
            stream.Write("{0}\t\tproperty = [{{result,", indent);

            if (end.EndStatus != null)
            {
                RightValueErlExporter.GenerateCode(end.EndStatus, stream, indent);
            }

            stream.WriteLine("}},{{quit,{0}}}],", end.EndOutside.ToString().ToLowerInvariant());
        }
    }
}
