using Behaviac.Design;
using System;
using System.IO;

namespace PluginBehaviac.DataExporters
{
    public class EnumErlExporter
    {
        public static void GeneratedCode(object obj, StringWriter stream)
        {
            Debug.Check(obj != null);

            if (obj == null)
            {
                return;
            }
            var type = obj.GetType();
            Debug.Check(type.IsEnum);

            var enumName = type.Name;
            var memberName = Enum.GetName(type, obj);

            if (enumName == "behaviac_EBTStatus" || enumName == "EBTStatus")
            {
                switch (memberName)
                {
                    case "BT_SUCCESS":
                        stream.Write("?SUCCESS");
                        return;
                    case "BT_FAILURE":
                        stream.Write("?FAILURE");
                        return;
                    case "BT_RUNNING":
                        stream.Write("?RUNNING");
                        return;
                    case "BT_INVALID":
                        stream.Write("?INVALID");
                        return;
                    default:
                        Debug.Check(false, "unknown status");
                        return;
                }
            }

            foreach (var member in Enum.GetValues(type))
            {
                if (member.Equals(obj))
                {
                    stream.Write(member);
                    return;
                }
            }

            Debug.Check(false, "unknown enum");
        }
    }
}
