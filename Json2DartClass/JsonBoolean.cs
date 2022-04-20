using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json2DartClass
{
    public class JsonBoolean : JsonField
    {
        public bool value;
        public JsonBoolean(bool value)
        {
            this.value = value;
        }

        public override string dartTypeName => "bool";

        public override string DartMapAssignment(string name)
        {
            return $"\t\tdata['{name}'] = this.{name};";
        }

        public override string DartMapFetching(string name)
        {
            return $"\t\t{name} : map['{name}'] ?? false,";
        }

        public override void toString(StringBuilder sb, int? indents)
        {
            sb.Append(value ? "true" : "false");
        }
    }
}
