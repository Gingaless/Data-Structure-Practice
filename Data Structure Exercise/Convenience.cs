using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Structure_Exercise
{
    public static class Convenience
    {
        public static bool IsGenericTypeOf(Type genericType, object obj)
        {
            Type type = obj.GetType();
            if (type.IsGenericType && genericType.IsGenericType &&
                (type.GetGenericTypeDefinition() == genericType.GetGenericTypeDefinition()))
            {
                if (genericType == type.GetGenericTypeDefinition()) return true;
                Type[] genericArguments = genericType.GetGenericArguments();
                Type[] objGenArguments = type.GetGenericArguments();
                int l = genericArguments.Length;
                if (l == objGenArguments.Length)
                {
                    for (int i=0; i<l; i++)
                    {
                        bool ismatch = false;
                        Type gtype = objGenArguments[i];
                        while (gtype!=null)
                        {
                            if (gtype == genericArguments[i] || gtype.GetInterfaces().Contains<Type>(genericArguments[i]))
                            { ismatch = true; break; }
                            gtype = gtype.BaseType;
                        }
                        if (!(ismatch)) return false;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
