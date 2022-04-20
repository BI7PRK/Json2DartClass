using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Json2DartClass
{
    class JsonString : JsonField
    {
        public string value;
        public JsonString(string value)
        {
            this.value = value;
        }

        public override string dartTypeName => "String";

        
        public override string DartMapAssignment(string name)
        {
            return $"\t\tdata['{name}'] = {name};";
        }

        public override string DartMapFetching(string name)
        {
            return $"\t\t{name} : map['{name}'] ?? '',";
        }

        public override void toString(StringBuilder sb, int? indents)
		{
			sb.AppendFormat("\"{0}\"", value.Replace("\"","\\\""));
		}
	}
}
