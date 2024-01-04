using Behaviac.Design.Nodes;
using PluginBehaviac.DataExporters;
using PluginBehaviac.Nodes;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class DecoratorFramesErlExporter : NodeErlExporter
    {
        protected override bool ShouldGenerateCode(Node node)
        {
            return node is DecoratorFrames;
        }

        protected override void GenerateCode(Node node, StringWriter stream, string indent)
        {
            base.GenerateCode(node, stream, indent);

            if (!(node is DecoratorFrames decoratorFrames))
            {
                return;
            }

            stream.WriteLine("{0}\t\texecutor = ebt_frames,", indent);
            stream.Write("{0}\t\tproperty = [{{frames,", indent);
            RightValueErlExporter.GenerateCode(decoratorFrames.Frames, stream, indent + "\t\t\t");
            stream.WriteLine("}],");
        }
    }
}
