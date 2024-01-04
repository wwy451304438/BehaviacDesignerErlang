using System;
using Behaviac.Design;
using Behaviac.Design.Attachments;
using Behaviac.Design.Nodes;
using PluginBehaviac.Events;
using PluginBehaviac.NodeExporters;
using PluginBehaviac.Properties;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms.VisualStyles;
using PluginBehaviac.DataExporters;

namespace PluginBehaviac.Exporters
{
    public class ExporterErl : Behaviac.Design.Exporters.Exporter
    {
        public ExporterErl(BehaviorNode node, string outputFolder, string filename, List<string> includedFilenames = null) 
            : base(node, outputFolder, filename+".erl", includedFilenames, "%%")
        {
            _outputFolder = Path.Combine(Path.GetFullPath(_outputFolder), "behaviac_generated");
        }

        public override Behaviac.Design.FileManagers.SaveResult Export(List<BehaviorNode> behaviors, bool exportBehaviors, bool exportMeta, int exportFileCount)
        {
            var behaviorFilename = "behaviors/ai_tree.erl";
            var typesFolder = string.Empty;
            var result = VerifyFilename(exportBehaviors, ref behaviorFilename, ref typesFolder);

            if (Behaviac.Design.FileManagers.SaveResult.Succeeded != result)
            {
                return result;
            }

            if (exportBehaviors)
            {
                ExportBehaviors(behaviors, behaviorFilename, exportFileCount);
            }

            if (exportMeta)
            {
                ExportAgents(typesFolder);
            }

            ExportEnumType();
            
            return result;
        }

        private Behaviac.Design.FileManagers.SaveResult VerifyFilename(bool exportBehaviors, ref string behaviorFilename, ref string typesFolder)
        {
            behaviorFilename = Path.Combine(_outputFolder, behaviorFilename);
            typesFolder = Path.Combine(_outputFolder, "types");

            // get the abolute folder of the file we want to export
            var folder = Path.GetDirectoryName(behaviorFilename);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (!Directory.Exists(typesFolder))
            {
                Directory.CreateDirectory(typesFolder);
            }

            if (exportBehaviors)
            {
                // verify it can be writable
                return Behaviac.Design.FileManagers.FileManager.MakeWritable(behaviorFilename, Resources.ExportFileWarning);
            }

            return Behaviac.Design.FileManagers.SaveResult.Succeeded;
        }

        private Behaviac.Design.FileManagers.SaveResult VerifyFilename(ref string filename)
        {
            filename = Path.Combine(_outputFolder, filename);
            var folder = Path.GetDirectoryName(filename);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return Behaviac.Design.FileManagers.SaveResult.Succeeded;
        }

        private void ExportBehaviors(List<BehaviorNode> behaviors, string filename, int exportFileCount)
        {
            if (behaviors.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(behaviors));
            using (var file = new StringWriter())
            {
                // 根据exportFileCount来决定是否覆盖文件
                // 目前已单文件的方式存储数据，后续太多则需要拆分
                if (exportFileCount != 1)
                {
                    ExportHead(file, filename);
                    foreach (var behavior in behaviors)
                    {
                        ExportBody(file, behavior, new List<TreeMethodContent>());
                    }
                    ExportTail(file);
                    UpdateFile(file, filename);
                }
                else
                {
                    ExportHead(file, filename);
                    List<TreeMethodContent> treeMethodContents = new List<TreeMethodContent>();
                    if (File.Exists(filename))
                    {
                        var treeMethodContent = new TreeMethodContent();
                        treeMethodContents = treeMethodContent.paramTree(filename);
                    }

                    foreach (var behavior in behaviors)
                    {
                        ExportBody(file, behavior, treeMethodContents);
                    }

                    foreach (var methodContent in treeMethodContents)
                    {
                        file.WriteLine("%% Source file: {0}", methodContent.source_file);
                        foreach (var line in methodContent.contentLines)
                        {
                            file.WriteLine(line);
                        }
                    }
                    
                    ExportTail(file);
                    UpdateFile(file, filename);
                    
                    //     var agentFolder = string.Empty;
                    //     foreach (var behavior in behaviors)
                    //     {
                    //         var behaviorFilename = behavior.RelativePath;
                    //         List<TreeMethodContent> treeMethodContents = new List<TreeMethodContent>();
                    //         behaviorFilename = behaviorFilename.Replace("\\", "/");
                    //         behaviorFilename = Path.ChangeExtension("bt_" + behaviorFilename, "erl");
                    //         behaviorFilename = Path.Combine("behaviors", behaviorFilename);
                    //
                    //
                    //         var result = VerifyFilename(true, ref behaviorFilename, ref agentFolder);
                    //
                    //         if (Behaviac.Design.FileManagers.SaveResult.Succeeded != result)
                    //         {
                    //             continue;
                    //         }
                    //
                    //         if (File.Exists(behaviorFilename))
                    //         {
                    //             var treeMethodContent = new TreeMethodContent();
                    //             treeMethodContents = treeMethodContent.paramTree(behaviorFilename);
                    //         }
                    //
                    //         using (var behaviorFile = new StringWriter())
                    //         {
                    //             ExportHead(behaviorFile, behaviorFilename);
                    //
                    //             ExportBody(behaviorFile, behavior, treeMethodContents);
                    //
                    //             ExportTail(behaviorFile);
                    //
                    //             UpdateFile(behaviorFile, behaviorFilename);
                    //         }
                    //     }
                    // } 
                }
            }
        }

        private void ExportHead(StringWriter file, string exportFilename)
        {
            var wsfolder = Path.GetDirectoryName(Workspace.Current.FileName);
            exportFilename = Behaviac.Design.FileManagers.FileManager.MakeRelative(wsfolder, exportFilename);
            exportFilename = exportFilename.Replace("\\", "/");

            // write comments
            file.WriteLine("%% ---------------------------------------------------------------------");
            file.WriteLine("%% THIS FILE IS AUTO-GENERATED BY BEHAVIAC DESIGNER, SO PLEASE DON'T MODIFY IT BY YOURSELF!");
            file.WriteLine("%% Export file: {0}", exportFilename);
            file.WriteLine("%% ---------------------------------------------------------------------");
            file.WriteLine("%% --------tewtwetttttttt--------------------------");
            file.WriteLine("%% --------tewtwetttttttt--------------------------");
            file.WriteLine();

            file.WriteLine("-module({0}).", Path.GetFileNameWithoutExtension(exportFilename));
            file.WriteLine();

            file.WriteLine("-include_lib(\"ebt/include/ebt.hrl\").");
            file.WriteLine();

            file.WriteLine("-export([find/1]).");
            file.WriteLine();
        }

        private void ExportBody(StringWriter file, BehaviorNode behavior, List<TreeMethodContent> treeMethodContents)
        {
            behavior.PreExport();

            var filename = Path.ChangeExtension(behavior.RelativePath, "").Replace(".", "");
            filename = filename.Replace('\\', '/');

            treeMethodContents.RemoveAll(treeMethodContent => treeMethodContent.source_file == filename);
            
            var btClassName = $"bt_{getValidFilename(filename)}";
            var agentType = behavior.AgentType.Name;

            // write comments
            file.WriteLine("%% Source file: {0}", filename);

            file.WriteLine("find(\"{0}\") ->", filename);

            // create the class definition of its children
            foreach (Node child in ((Node)behavior).GetChildNodes())
            {
                ExportNodeClass(file, btClassName, agentType, behavior, child);
            }

            file.WriteLine("\t#ebt_tree{");
            file.WriteLine("\t\tid = \"{0}\",", filename);
            file.WriteLine("\t\tagent = '{0}':agent(),", SnakeCase(behavior.AgentType.BasicName));
            file.WriteLine("\t\tentry = {0},", ((Node)behavior.GenericChildren.GetChild(0)).Id);
            file.WriteLine("\t\tnodes = #{");
            foreach (Node child in ((Node)behavior).GetChildNodes())
            {
                ExportNode(file, btClassName, agentType, "bt", child, 3);
            }
            file.WriteLine("\t\t\tinvalid => undefined");
            file.WriteLine("\t\t}");
            file.WriteLine("\t};");
            behavior.PostExport();
            
        }

        private void ExportTail(StringWriter file)
        {
            file.WriteLine("find(_) ->");
            file.WriteLine("\tundefined.");
        }

        private void ExportAttachmentClass(StringWriter file, string btClassName, Node node)
        {
            foreach (var attach in node.Attachments)
            {
                if (!attach.Enable)
                {
                    continue;
                }

                var nodeName = $"attach{attach.Id}";

                var attachmentExporter = AttachmentErlExporter.CreateInstance(attach);
                attachmentExporter.GenerateClass(attach, file, "", nodeName, btClassName);
            }
        }

        private void ExportNodeClass(StringWriter file, string btClassName, string agentType, BehaviorNode behavior, Node node)
        {
            if (!node.Enable)
            {
                return;
            }

            ExportAttachmentClass(file, btClassName, node);

            var nodeName = $"Node{node.Id}";

            var children = new List<string>();
            var interrupts = new List<string>();
            foreach (Node child in node.GetChildNodes())
            {
                if (AsChild(child))
                {
                    children.Add(child.Id.ToString());
                }
                else if(IskInterupt(child))
                {
                    interrupts.Add(child.Id.ToString());
                }
                else
                {
                    continue;
                }
            }

            var conditions = new List<string>();
            var effectors = new List<string>();
            var events = new List<string>();
            foreach (var attach in node.Attachments)
            {
                switch (attach)
                {
                    case Precondition _:
                        conditions.Add("Attach" + attach.Id);
                        continue;
                    case Effector _:
                        effectors.Add("Attach" + attach.Id);
                        continue;
                    case Event _:
                        events.Add("Attach" + attach.Id);
                        continue;
                }
            }

            file.WriteLine("\tNode{0} = #ebt_node{{", node.Id);
            file.WriteLine("\t\tid = {0},", node.Id);
            var nodeExporter = NodeErlExporter.CreateInstance(node);
            nodeExporter.GenerateClass(node, file, "", nodeName, agentType, btClassName);
            file.WriteLine("\t\tchildren = [{0}],", string.Join(",", children.ToArray()));
            if (interrupts.Count != 0)
            {
                file.WriteLine("\t\tinterrupt = [{0}],", string.Join(",", interrupts.ToArray()));
            }
            file.WriteLine("\t\tevents = [{0}],", string.Join(",", events.ToArray()));
            file.WriteLine("\t\tpreconditions = [{0}],", string.Join(",", conditions.ToArray()));
            file.WriteLine("\t\teffectors = [{0}]", string.Join(",", effectors.ToArray()));

            file.WriteLine("\t},");

            if (node is ReferencedBehavior)
            {
                return;
            }

            foreach (Node child in node.GetChildNodes())
            {
                ExportNodeClass(file, btClassName, agentType, behavior, child);
            }
        }

        private bool AsChild(BaseNode child)
        {
            var connector = child.Parent?.GetConnector(child);
            return connector == null || connector.IsAsChild;
        }

        private bool IskInterupt(BaseNode child)
        {
            var connector = child.Parent?.GetConnector(child);
            return connector != null && !connector.IsAsChild;
        }

        private string getValidFilename(string filename)
        {
            filename = filename.Replace('/', '_');
            filename = filename.Replace('-', '_');

            return filename;
        }

        private void ExportNode(StringWriter file, string btClassName, string agentType, string parentName, Node node, int indentDepth)
        {
            if (!node.Enable || (!AsChild(node) && !IskInterupt(node)))
            {
                return;
            }

            // generate the indent string
            var indent = string.Empty;

            for (var i = 0; i < indentDepth; ++i)
            {
                indent += '\t';
            }

            var nodeName = $"node{node.Id}";

            // open some brackets for a better formatting in the generated code
            file.WriteLine("{0}{1} => Node{1},", indent, node.Id);

            // export its instance and the properties
            var nodeExporter = NodeErlExporter.CreateInstance(node);
            nodeExporter.GenerateInstance(node, file, indent, nodeName, agentType, btClassName);

            // export the child nodes
            if (node.IsFSM || node is ReferencedBehavior)
            {
                return;
            }
            foreach (Node child in node.GetChildNodes())
            {
                ExportNode(file, btClassName, agentType, nodeName, child, indentDepth);
            }
        }

        private void ExportEnumType()
        {
            var enumFilePath = "behaviors/ai_enum.hrl";
            var result = VerifyFilename(ref enumFilePath);

            if (Behaviac.Design.FileManagers.SaveResult.Succeeded != result)
            {
                return;
            }
            using (var file = new StringWriter())
            {
                file.WriteLine("-ifndef(AI_ENUM_HRL).");
                file.WriteLine("-define(AI_ENUM_HRL, true).");
                foreach (var enumType in TypeManager.Instance.Enums)
                {
                    ExportEnumErlFile(enumType, file);
                }
                file.WriteLine("-endif.");
                UpdateFile(file, enumFilePath);
            }
        }

        private void ExportEnumErlFile(EnumType enumType, StringWriter file)
        {
            foreach (var member in enumType.Members)
            {
                file.WriteLine("-define({0}_{1}, {1}).", enumType.Name, member.Name);
            }
            file.WriteLine();
        }
        private void ExportAgents(string defaultAgentFolder)
        {
            foreach (var agent in Behaviac.Design.Plugin.AgentTypes)
            {
                if (agent.IsImplemented || agent.Name == "behaviac::Agent")
                {
                    continue;
                }
                var agentFolder = string.IsNullOrEmpty(agent.ExportLocation) ? defaultAgentFolder : Workspace.Current.MakeAbsolutePath(agent.ExportLocation);
                var filename = Path.Combine(agentFolder, SnakeCase(agent.BasicName) + ".erl");
                var oldFilename = "";

                if (!string.IsNullOrEmpty(agent.OldName) && agent.OldName != agent.Name)
                {
                    oldFilename = Path.Combine(agentFolder, SnakeCase(agent.BasicOldName) + ".erl");
                }

                Debug.Check(filename != oldFilename);
                agent.OldName = null;

                try
                {
                    if (!Directory.Exists(agentFolder))
                    {
                        Directory.CreateDirectory(agentFolder);
                    }

                    if (!File.Exists(filename))
                    {
                        ExportAgentErlFile(agent, filename, false);

                        if (File.Exists(oldFilename))
                        {
                            MergeFiles(oldFilename, filename, filename);
                        }
                    }
                    else
                    {
                        var behaviacAgentDir = GetBehaviacTypesDir();
                        var newFilename = Path.GetFileName(filename);
                        newFilename = Path.ChangeExtension(newFilename, ".new.erl");
                        newFilename = Path.Combine(behaviacAgentDir, newFilename);

                        ExportAgentErlFile(agent, newFilename, false);
                        Debug.Check(File.Exists(newFilename));

                        MergeFiles(filename, newFilename, filename);
                    }

                    if (File.Exists(oldFilename))
                    {
                        File.Delete(oldFilename);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Merge Erl Files Error : {0} {1}", filename, e.Message);
                }
            }
        }
        
        private void ExportAgentErlFile(AgentType agent, string filename, bool preview)
        {
            using (var file = new StringWriter())
            {
                ExportFileWarningHeader(file);

                file.WriteLine("-module('{0}').", SnakeCase(agent.BasicName));
                file.WriteLine();
                
                file.WriteLine("-compile([nowarn_unused_vars]).");
                file.WriteLine();
                
                file.WriteLine("-include_lib(\"ebt/include/ebt.hrl\").");
                file.WriteLine();
                
                file.WriteLine("-export([agent/0]).");
                file.WriteLine("-export([get_var/2, set_var/3]).");
                
                var methods = agent.GetMethods(true);
                
                foreach (var method in methods)
                {
                    if ((!preview && agent.IsImplemented) || method.IsNamedEvent || method.ClassName == "behaviac::Agent")
                    {
                        continue;
                    }
                    file.WriteLine("-export([{0}/2]).", SnakeCase(method.BasicName));
                }
                file.WriteLine();
                
                if (!preview)
                {
                    ExportBeginComment(file, "", file_init_part);
                    file.WriteLine();
                    ExportEndComment(file, "");
                    file.WriteLine();
                }

                file.WriteLine("-record('{0}', {{", agent.BasicName);
                file.WriteLine("object,");

                var properties = agent.GetProperties(true);
                for (var i = properties.Count - 1; i >= 0; i--)
                {
                    var prop = properties[i];
                    if (!preview && agent.IsImplemented || prop.IsPar || prop.IsArrayElement)
                    {
                        continue;
                    }
                    file.Write("\t'{0}' = ", prop.BasicName);
                    DataErlExporter.GenerateValue(prop.Variable.Value, file, "");
                    if (i > 0)
                    {
                        file.Write(",");
                    }
                    file.WriteLine();
                }

                file.WriteLine("}).");
                file.WriteLine();
                
                file.WriteLine("agent() ->");
                file.WriteLine("\t#'{0}'{{}}.", agent.BasicName);
                file.WriteLine();
                
                for (var i = properties.Count - 1; i >= 0; i--)
                {
                    var prop = properties[i];
                    if (!preview && agent.IsImplemented || prop.IsPar || prop.IsArrayElement)
                    {
                        continue;
                    }
                    file.WriteLine("get_var(Agent, '{0}') ->", prop.BasicName);
                    file.Write("\t");
                    file.WriteLine("Agent#'{0}'.'{1}';", agent.BasicName, prop.BasicName);
                }
                file.WriteLine("get_var(Agent, Key) ->");
                file.WriteLine("\tthrow({unknown_field, Agent, Key}).");
                file.WriteLine();
                
                for (var i = properties.Count - 1; i >= 0; i--)
                {
                    var prop = properties[i];
                    if (!preview && agent.IsImplemented || prop.IsPar || prop.IsArrayElement)
                    {
                        continue;
                    }
                    file.WriteLine("set_var(Agent, '{0}', Val) ->", prop.BasicName);
                    file.WriteLine("\tAgent#'{0}'{{'{1}' = Val}};", agent.BasicName, prop.BasicName);
                }
                file.WriteLine("set_var(Agent, Key, _Val) ->");
                file.WriteLine("\tthrow({unknown_field, Agent, Key}).");
                file.WriteLine();
                
                foreach (var method in methods)
                {
                    if ((!preview && agent.IsImplemented) || method.IsNamedEvent || method.ClassName == "behaviac::Agent")
                    {
                        continue;
                    }
                    
                    var allParams = "";

                    foreach (var param in method.Params)
                    {
                        if (!string.IsNullOrEmpty(allParams))
                        {
                            allParams += ", ";
                        }

                        allParams += param.Name;
                    }

                    ExportMethodComment(file, "");

                    file.WriteLine("%% {0}", method.DisplayName);
                    if (method.IsInherited && method.AgentType != null)
                    {
                        file.WriteLine("{0}(Agent, Args) ->", SnakeCase(method.BasicName));
                    }
                    else
                    {
                        file.WriteLine("{0}(Agent, [{1}]) ->", SnakeCase(method.BasicName), allParams);
                    }

                    if (!preview)
                    {
                        ExportBeginComment(file, "\t\t", method.BasicName);
                    }
                    
                    if (method.IsInherited && method.AgentType != null)
                    {
                        file.WriteLine("\t{0}:{1}(Agent, Args).", SnakeCase(method.AgentType.BasicName), SnakeCase(method.BasicName));
                    }
                    else
                    {
                        file.WriteLine("\tok.");
                    }
                    
                    if (!preview)
                    {
                        ExportEndComment(file, "\t\t");
                    }
                    

                    file.WriteLine();
                }

                UpdateFile(file, filename);
            }
        }
        
        private static string SnakeCase(string s)
        {
            var result = "";
            for (var i = 0; i < s.Length; i++)
            {
                if (i > 0 && s[i] >= 'A' && s[i] <= 'Z' && s[i-1] != '_')
                {
                    result += "_";
                }

                result += s[i];
            }

            return result.ToLower();
        }
        
        

        
    }

    class TreeMethodContent
    {
        public string source_file;
        public List<string> contentLines;
        
        private static char[] spaces = { ' ', '\t' };
        public static string method_start = "%% Source file: ";
        public static string method_end = "};";
        
        public List<TreeMethodContent> paramTree(string treeFilePath)
        {
            using (StreamReader file = new StreamReader(treeFilePath))
            {
                List<TreeMethodContent> treeMethodContents = new List<TreeMethodContent>();
                while (true)
                {
                    TreeMethodContent treeMethodContent = readTreeMethod(file);
                    if (treeMethodContent == null)
                    {
                        break;
                    }

                    treeMethodContents.Add(treeMethodContent);
                }

                return treeMethodContents;
            }
        }

        private TreeMethodContent readTreeMethod(StreamReader file)
        {
            List<string> contentLines = new List<string>();
            List<string> methodContentLines = new List<string>();
            TreeMethodContent treeMethodContent = new TreeMethodContent();
            string line = Readline(ref file, ref contentLines);
            while (line != null && !StartsWith(method_start, line))
            {
                line = file.ReadLine();
            }

            if (line != null && StartsWith(method_start, line))
            {   
                string fileName = SplitSourceFileName(line);
                treeMethodContent.source_file = fileName.Trim();
                while (line != null && !StartsWith(method_end, line))
                {
                    line = Readline(ref file, ref methodContentLines);
                }
                treeMethodContent.contentLines = methodContentLines;
                return treeMethodContent;
            }

            return null;
        }
        
        private static bool StartsWith(string token, string line)
        {
            string lineNoEmpty = line.TrimStart(spaces);

            return lineNoEmpty.StartsWith(token);
        }
        
        private static string Readline(ref StreamReader file, ref List<string> lines)
        {
            string line = file.ReadLine();
            if (line != null)
            {
                lines.Add(line);
            }

            return line;
        }

        private static string SplitSourceFileName(string sourcefilename)
        {
            string notTrimFilename = sourcefilename.Trim();
            int startLen = method_start.Length;
            return notTrimFilename.Substring(startLen);
        }
    }
    
    
}
