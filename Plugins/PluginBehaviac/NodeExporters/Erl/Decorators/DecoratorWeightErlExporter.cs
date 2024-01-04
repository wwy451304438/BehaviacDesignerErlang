using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class DecoratorWeightErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is DecoratorWeight;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is DecoratorWeight decoratorWeight))
            {
                return;
            }

            if (decoratorWeight.Weight == null)
            {
                return;
            }
            
            stream.WriteLine("{0}\t\texecutor = ebt_weight,", indent);
            stream.Write("{0}\t\tproperty = [{{weight,", indent);

            VariableErlExporter.GenerateCode(decoratorWeight.Weight, stream, indent + "\t\t\t");

            stream.WriteLine("}],");
        }
    }
}
