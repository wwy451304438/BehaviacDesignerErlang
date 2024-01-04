using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class DecoratorTimeErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is DecoratorTime;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is DecoratorTime decoratorTime))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_time,", indent);
            stream.Write("{0}\t\tproperty = [{{time,", indent);
            RightValueErlExporter.GenerateCode(decoratorTime.Time, stream, indent + "\t\t\t");
            stream.WriteLine("}],");
        }
    }
}
