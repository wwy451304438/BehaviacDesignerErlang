using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    internal class WaitFramesErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is WaitFrames;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is WaitFrames waitFrames))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_wait_frames,", indent);
            stream.Write("{0}\t\tproperty = [{{'frames',", indent);

            if (waitFrames.Frames != null)
            {
                RightValueErlExporter.GenerateCode(waitFrames.Frames, stream, indent);
            }

            stream.WriteLine("}],");
        }
    }
}
