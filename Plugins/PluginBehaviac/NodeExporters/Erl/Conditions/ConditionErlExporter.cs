using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using System.IO;
using Behaviac.Design;
using Condition = PluginBehaviac.Nodes.Condition;

namespace PluginBehaviac.NodeExporters
{
    public class ConditionErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is Condition;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is Condition))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_condition,", indent);
            stream.Write("{0}\t\tproperty = [", indent);
            GenerateCompare(node, stream);
            stream.WriteLine("],");
        }

        public static void GenerateCompare(Node node, StringWriter stream)
        {
            if (!(node is Condition condition))
            {
                return;
            }
            
            stream.Write("{compare,{");

            switch (condition.Operator)
            {
                case OperatorType.Equal:
                    stream.Write("'=='");
                    break;

                case OperatorType.NotEqual:
                    stream.Write("'!='");
                    break;

                case OperatorType.Greater:
                    stream.Write("'>'");
                    break;

                case OperatorType.GreaterEqual:
                    stream.Write("'>='");
                    break;

                case OperatorType.Less:
                    stream.Write("'<'");
                    break;

                case OperatorType.LessEqual:
                    stream.Write("'<='");
                    break;

                default:
                    Debug.Check(false, "invalid operator");
                    break;
            }

            stream.Write(",");

            if (condition.Opl != null)
            {
                RightValueErlExporter.GenerateCode(condition.Opl, stream, "");
            }

            stream.Write(",");

            if (condition.Opr != null)
            {
                RightValueErlExporter.GenerateCode(condition.Opr, stream, "");
            }

            stream.Write("}}");
        }
    }
}
