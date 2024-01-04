using Behaviac.Design;
using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class ComputeErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is Compute;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is Compute compute))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_compute,", indent);

            stream.Write("{0}\t\tproperty = [{{compute,{{", indent);

            switch (compute.Operator)
            {
                case ComputeOperator.Add: 
                    stream.Write("'+'"); 
                    break;

                case ComputeOperator.Sub: 
                    stream.Write("'-'");
                    break;

                case ComputeOperator.Mul: 
                    stream.Write("'*'"); 
                    break;

                case ComputeOperator.Div: 
                    stream.Write("'/'"); 
                    break;

                default:
                    Debug.Check(false, "The operator is wrong!");
                    break;
            }

            stream.Write(",");

            if (compute.Opl != null)
            {
                VariableErlExporter.GenerateCode(compute.Opl, stream, indent);
            }

            stream.Write(",");

            if (compute.Opr1 != null)
            {
                RightValueErlExporter.GenerateCode(compute.Opr1, stream, indent);
            }

            stream.Write(",");

            if (compute.Opr2 != null)
            {
                RightValueErlExporter.GenerateCode(compute.Opr2, stream, indent);
            }

            stream.WriteLine("}}],");
        }
    }
}
