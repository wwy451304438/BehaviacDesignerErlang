using Behaviac.Design.Attachments;
using PluginBehaviac.Events;
using System.IO;
using Behaviac.Design;

namespace PluginBehaviac.NodeExporters
{
    public class EffectorErlExporter : AttachmentErlExporter
    {
        protected override bool ShouldGenerateCode()
        {
            return true;
        }

        protected override void GenerateCode(Attachment attachment, StringWriter stream, string indent)
        {
            base.GenerateCode(attachment, stream, indent);

            if (!(attachment is Effector effector))
            {
                return;
            }

            stream.WriteLine("{0}\tAttach{1} = #ebt_effector{{", indent, attachment.Id);
            stream.WriteLine("{0}\t\tid = {1},", indent, attachment.Id);

            var phase = "";
            switch (effector.Phase)
            {
                case EffectorPhase.Success:
                    phase = "success";
                    break;

                case EffectorPhase.Failure:
                    phase = "failure";
                    break;

                case EffectorPhase.Both:
                    phase = "both";
                    break;
                
                default:
                    Debug.Check(false, "unknown phase");
                    break;
            }

            stream.WriteLine("{0}\t\tphase = {1},", indent, phase);
            stream.Write("{0}\t\taction = ", indent);
            AttachActionErlExporter.GenerateCode(attachment, stream, indent);
            stream.WriteLine("{0}\t}},", indent);
        }
    }
}
