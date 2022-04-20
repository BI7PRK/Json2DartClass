using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json2DartClass
{
    public class JsonNull : JsonField
    {
        public override string dartTypeName => "Object";

        public override string DartMapAssignment(string name)
        {
            return $"\t\tdata['{name}'] = this.{name};";
        }

        public override string DartMapFetching(string name)
        {
            return $"\t\t{name} : map['{name}'],";
        }

        public override void toString(StringBuilder sb, int? indents)
        {
            sb.Append("null");
        }
    }
}
