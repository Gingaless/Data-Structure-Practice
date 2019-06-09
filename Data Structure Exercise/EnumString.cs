using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Data_Structure_Exercise
{
    public class EnumString : System.Attribute
    {
        private readonly string _value;

        public EnumString(string value) { _value = value; }

        public string Value { get { return _value; } }

        //SSLLStack class의 Node와 출동하는지 실험하기 위해서 맹글어놓은 internal class임. 결론은 충돌 안 함.
        public class Node
        {

        }
    }

    public static class StringEnum
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            EnumString[] attrs = fi.GetCustomAttributes(typeof(EnumString), false) as EnumString[];
            if (attrs.Length>0)
            {
                output = attrs[0].Value;
            }
            return output;
        }
    }
}
