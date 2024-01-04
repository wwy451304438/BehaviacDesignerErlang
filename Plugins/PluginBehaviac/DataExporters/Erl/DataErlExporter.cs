using Behaviac.Design;
using System.IO;
using System;
using Behaviac.Design.Attributes;

namespace PluginBehaviac.DataExporters
{
    public class DataErlExporter
    {
        public static void GenerateCode(object obj, StringWriter stream, string indent)
        {
            if (obj == null)
            {
                return;
            }

            switch (obj)
            {
                case MethodDef def:
                {
                    MethodErlExporter.GenerateCode(def, stream, indent);
                    break;
                }
                case MethodDef.Param param1:
                {
                    ParameterErlExporter.GenerateCode(param1, stream, indent);
                    break;
                }
                case ParInfo info:
                {
                    ParInfoErlExporter.GenerateCode(info, stream, indent);
                    break;
                }
                case PropertyDef def:
                {
                    PropertyErlExporter.GenerateCode(def, null, stream, indent);
                    break;
                }
                case VariableDef def:
                {
                    VariableErlExporter.GenerateCode(def, stream, indent);
                    break;
                }
                // Array type
                case RightValueDef def:
                {
                    RightValueErlExporter.GenerateCode(def, stream, indent);
                    break;
                }
                default:
                {
                    GenerateValue(obj, stream, indent);
                    break;
                }
            }
        }

        public static void GenerateValue(object obj, StringWriter stream, string indent)
        {
            var type = obj.GetType();
                    
            if (Behaviac.Design.Plugin.IsArrayType(type))
            {
                ArrayErlExporter.GenerateCode(obj, stream, indent);
            }
            // Struct type
            else if (Behaviac.Design.Plugin.IsCustomClassType(type))
            {
                StructErlExporter.GenerateCode(obj, stream, indent);
            }
            // Enum type
            else if (Behaviac.Design.Plugin.IsEnumType(type)) 
            {
                EnumErlExporter.GeneratedCode(obj, stream);
            }
            // Other types
            else
            {
                var res = obj.ToString();

                if (Behaviac.Design.Plugin.IsStringType(type)) // string
                {
                    res = $"\"{res}\"";
                }
                else if (Behaviac.Design.Plugin.IsCharType(type)) // char
                {
                    var c = 'A';
                    if (res.Length >= 1)
                    {
                        c = res[0];
                    }
                    res = $"\'{c}\'";
                }
                else if (Behaviac.Design.Plugin.IsBooleanType(type)) // bool
                {
                    res = res.ToLowerInvariant();
                }
                stream.Write(res);
            }
        }
    }
}
