using System;
using Behaviac.Design.Attachments;
using PluginBehaviac.Events;
using System.IO;
using Behaviac.Design;

namespace PluginBehaviac.NodeExporters
{
    public class PreconditionErlExporter : AttachmentErlExporter
    {
        protected override bool ShouldGenerateCode()
        {
            return true;
        }

        protected override void GenerateCode(Attachment attachment, StringWriter stream, string indent)
        {
            base.GenerateCode(attachment, stream, indent);

            if (!(attachment is Precondition precondition))
            {
                return;
            }

            stream.WriteLine("{0}\tAttach{1} = #ebt_precondition{{", indent, attachment.Id);
            stream.WriteLine("{0}\t\tid = {1},", indent, attachment.Id);

            var phase = "";
            switch (precondition.Phase)
            {
                case PreconditionPhase.Enter:
                    phase = "enter";
                    break;
                
                case PreconditionPhase.Update:
                    phase = "update"; 
                    break;
                
                case PreconditionPhase.Both: 
                    phase = "both"; 
                    break;
                
                default:
                    Debug.Check(false, "unknown phase");
                    break;
            }

            stream.WriteLine("{0}\t\tphase = {1},", indent, phase);
            stream.WriteLine("{0}\t\tis_and = {1},", indent, (precondition.BinaryOperator == BinaryOperator.And) ? "true" : "false");
            stream.Write("{0}\t\tcheck = ", indent);
            AttachActionErlExporter.GenerateCode(attachment, stream, indent);
            stream.WriteLine("{0}\t}},", indent);
        }
    }
}
