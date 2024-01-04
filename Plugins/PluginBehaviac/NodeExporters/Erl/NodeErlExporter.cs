using Behaviac.Design.Nodes;
using System;
using System.IO;

namespace PluginBehaviac.NodeExporters
{
    public class NodeErlExporter : NodeExporter
    {
        public static NodeErlExporter CreateInstance(Node node)
        {
            if (node == null)
            {
                return new NodeErlExporter();
            }
            var exporterType = GetExporterType(node.GetType());

            if (exporterType != null)
            {
                return (NodeErlExporter)Activator.CreateInstance(exporterType);
            }

            return new NodeErlExporter();
        }

        private static Type GetExporterType(Type nodeType)
        {
            if (nodeType == null)
            {
                return null;
            }
            while (nodeType != typeof(Node))
            {
                var nodeExporter = "PluginBehaviac.NodeExporters." + nodeType.Name + "ErlExporter";
                var exporterType = Type.GetType(nodeExporter);

                if (exporterType != null)
                {
                    return exporterType;
                }

                foreach (var assembly in Behaviac.Design.Plugin.GetLoadedPlugins())
                {
                    var filename = Path.GetFileNameWithoutExtension(assembly.Location);
                    nodeExporter = filename + ".NodeExporters." + nodeType.Name + "ErlExporter";
                    exporterType = assembly.GetType(nodeExporter);

                    if (exporterType != null)
                    {
                        return exporterType;
                    }
                }

                nodeType = nodeType.BaseType;
            }

            return null;
        }

        public override void GenerateClass(Node node, StringWriter stream, string indent, string nodeName, string agentType, string btClassName)
        {
            if (ShouldGenerateCode(node))
            {
                GenerateCode(node, stream, indent);
            }
        }

        protected virtual bool ShouldGenerateCode(Node node)
        {
            return false;
        }

        protected virtual void GenerateCode(Node node, StringWriter stream, string indent)
        {
        }
    }
}
