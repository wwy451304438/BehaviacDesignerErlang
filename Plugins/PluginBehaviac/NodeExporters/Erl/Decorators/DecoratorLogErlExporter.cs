using System.IO;
using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;

namespace PluginBehaviac.NodeExporters
{
    public class DecoratorLogErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is DecoratorLog;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is DecoratorLog decoratorLog))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_log,", indent);
            stream.WriteLine("{0}\t\tproperty = [{{msg,\"{1}\"}}],", indent, decoratorLog.Log);
        }
    }
}