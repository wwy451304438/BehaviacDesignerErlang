using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class AssignmentErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is Assignment;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is Assignment assignment))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_assign,", indent);
            stream.Write("{0}\t\tproperty = [{{assign,{{", indent);

            if (assignment.Opl != null)
            {
                VariableErlExporter.GenerateCode(assignment.Opl, stream, indent);
            }

            stream.Write(",");

            if (assignment.Opr != null)
            {
                RightValueErlExporter.GenerateCode(assignment.Opr, stream, indent);
            }

            stream.WriteLine("}}],");
        }
    }
}
