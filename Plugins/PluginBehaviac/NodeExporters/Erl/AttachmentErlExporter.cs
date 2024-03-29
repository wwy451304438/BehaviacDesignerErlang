﻿/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Tencent is pleased to support the open source community by making behaviac available.
//
// Copyright (C) 2015-2017 THL A29 Limited, a Tencent company. All rights reserved.
//
// Licensed under the BSD 3-Clause License (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at http://opensource.org/licenses/BSD-3-Clause
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using Behaviac.Design.Attachments;

namespace PluginBehaviac.NodeExporters
{
    public class AttachmentErlExporter : AttachmentExporter
    {
        public static AttachmentErlExporter CreateInstance(Attachment attachment)
        {
            if (attachment == null)
            {
                return new AttachmentErlExporter();
            }
            
            var exporterType = GetExporterType(attachment.GetType());

            if (exporterType != null)
            {
                return (AttachmentErlExporter)Activator.CreateInstance(exporterType);
            }

            return new AttachmentErlExporter();
        }

        private static Type GetExporterType(Type attachmentType)
        {
            if (attachmentType == null)
            {
                return null;
            }
            while (attachmentType != typeof(Attachment))
            {
                var attachmentExporter = "PluginBehaviac.NodeExporters." + attachmentType.Name + "ErlExporter";
                var exporterType = Type.GetType(attachmentExporter);

                if (exporterType != null)
                {
                    return exporterType;
                }

                foreach (var assembly in Behaviac.Design.Plugin.GetLoadedPlugins())
                {
                    var filename = Path.GetFileNameWithoutExtension(assembly.Location);
                    attachmentExporter = filename + ".NodeExporters." + attachmentType.Name + "ErlExporter";
                    exporterType = assembly.GetType(attachmentExporter);

                    if (exporterType != null)
                    {
                        return exporterType;
                    }
                }

                attachmentType = attachmentType.BaseType;
            }

            return null;
        }

        public override void GenerateClass(Attachment attachment, StringWriter stream, string indent, string nodeName, string btClassName)
        {
            if (ShouldGenerateCode())
            {
                GenerateCode(attachment, stream, indent);
            }
        }

        protected virtual bool ShouldGenerateCode()
        {
            return false;
        }

        protected virtual void GenerateCode(Attachment attachment, StringWriter stream, string indent)
        {
        }
    }
}
