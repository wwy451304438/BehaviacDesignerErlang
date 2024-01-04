using Behaviac.Design.Attachments;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    internal class EventErlExporter : AttachmentErlExporter
    {
        protected override bool ShouldGenerateCode()
        {
            return true;
        }

        protected override void GenerateCode(Attachment attachment, StringWriter stream, string indent)
        {
            base.GenerateCode(attachment, stream, indent);

            if (!(attachment is Event evt))
            {
                return;
            }

            stream.WriteLine("{0}\tAttach{1} = #ebt_event{{", indent, attachment.Id);
            stream.WriteLine("{0}\t\tid = {1},", indent, attachment.Id);
            stream.WriteLine("{0}\t\ttask = '{1}',", indent, evt.Task.BasicName);
            stream.WriteLine("{0}\t\tonce = {1},", indent, evt.TriggeredOnce ? "true" : "false");
            stream.WriteLine("{0}\t\tmode = '{1}',", indent, evt.TriggerMode);
            stream.WriteLine("{0}\t\ttree = \"{1}\"", indent, evt.ReferenceFilename);
            stream.WriteLine("{0}\t}},", indent);
        }
    }
}
