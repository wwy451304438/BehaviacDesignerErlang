using Behaviac.Design.Nodes;
using Behaviac.Design;
using PluginBehaviac.DataExporters;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class ActionErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is Action;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is Action action))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_action,", indent);
            stream.Write("{0}\t\tproperty = [{{action,", indent);

            if (action.Method != null && !isNullMethod(action.Method))
            {
                MethodErlExporter.GenerateCode(action.Method, stream, indent);
            }

            stream.Write("},{result,");

            if (action.ResultFunctor != null)
            {
                MethodErlExporter.GenerateCode(action.ResultFunctor, stream, indent);
            }
            else
            {
                stream.Write("'{0}'", getResultOptionStr(action.ResultOption));
            }

            stream.WriteLine("}],");
        }

        private bool isNullMethod(MethodDef method)
        {
            return (method != null && method.BasicName == "null_method");
        }

        private string getResultOptionStr(EBTStatus status)
        {
            switch (status)
            {
                case EBTStatus.BT_SUCCESS: 
                    return "success";

                case EBTStatus.BT_FAILURE: 
                    return "failure";

                case EBTStatus.BT_RUNNING: 
                    return "running";
            }

            return "invalid";
        }
    }
}
