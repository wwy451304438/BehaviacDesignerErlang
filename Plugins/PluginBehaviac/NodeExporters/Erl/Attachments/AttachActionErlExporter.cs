using Behaviac.Design.Attachments;
using Behaviac.Design;
using PluginBehaviac.DataExporters;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class AttachActionErlExporter
    {
        public static void GenerateCode(Attachment attachment, StringWriter stream, string indent)
        {
            if (!(attachment is AttachAction attach))
            {
                return;
            }

            if (attach.IsAction())
            {
                stream.Write("{action,");
                MethodErlExporter.GenerateCode(attach.Opl.Method, stream, indent);
                stream.WriteLine("}");
            }
            else if (attach.IsAssign())
            {
                if (attach.Opl == null || attach.Opl.IsMethod || attach.Opl.Var == null || attach.Opr2 == null)
                {
                    return;
                }
                
                var prop = attach.Opl.Var.Property;

                if (prop == null) return;
                stream.Write("{assign,");

                RightValueErlExporter.GenerateCode(attach.Opl, stream, indent);

                stream.Write(",");

                RightValueErlExporter.GenerateCode(attach.Opr2, stream, indent);

                stream.WriteLine("}");
            }
            else if (attach.IsCompare())
            {
                stream.Write("{compare,");
                switch (attach.Operator)
                {
                    case OperatorTypes.Equal: 
                        stream.Write("'=='"); 
                        break;

                    case OperatorTypes.NotEqual:
                        stream.WriteLine("'!='"); 
                        break;

                    case OperatorTypes.Greater:
                        stream.WriteLine("'>'"); 
                        break;

                    case OperatorTypes.GreaterEqual:
                        stream.WriteLine("'>='"); 
                        break;

                    case OperatorTypes.Less: 
                        stream.WriteLine("'<'");
                        break;

                    case OperatorTypes.LessEqual:
                        stream.WriteLine("'<='"); 
                        break;
                    
                    default:
                        Debug.Check(false, "unknown operator");
                        break;
                }

                stream.Write(",");
                RightValueErlExporter.GenerateCode(attach.Opl, stream, indent);
                stream.Write(",");
                RightValueErlExporter.GenerateCode(attach.Opr2, stream, indent);
                stream.WriteLine("}");
            }
            else if (attach.IsCompute())
            {
                if (attach.Opl == null || attach.Opl.IsMethod || attach.Opl.Var == null || attach.Opr1 == null ||
                    attach.Opr2 == null)
                {
                    return;
                }
                
                var prop = attach.Opl.Var.Property;

                if (prop == null) return;
                
                stream.Write("{compute,");
                switch (attach.Operator)
                {
                    case OperatorTypes.Add:
                        stream.Write("'+'");
                        break;

                    case OperatorTypes.Sub:
                        stream.Write("'-'");
                        break;

                    case OperatorTypes.Mul:
                        stream.Write("'*'");
                        break;

                    case OperatorTypes.Div:
                        stream.Write("'/'");
                        break;

                    default:
                        Debug.Check(false, "The operator is wrong!");
                        break;
                }
                stream.Write(",");
                RightValueErlExporter.GenerateCode(attach.Opl, stream, indent);
                stream.Write(",");
                RightValueErlExporter.GenerateCode(attach.Opr1, stream, indent);
                stream.Write(",");
                RightValueErlExporter.GenerateCode(attach.Opr2, stream, indent);
                stream.WriteLine("}");
            }
        }
    }
}
