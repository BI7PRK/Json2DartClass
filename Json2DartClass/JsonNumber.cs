using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Json2DartClass
{
    class JsonNumber : JsonField
    {
        public double value;
        public JsonNumber(double value)
        {
            this.value = value;
        }

        public override string dartTypeName => value % 1 == 0 ? "int" : "double";

        public override string DartMapAssignment(string name)
        {
            return $"\t\tdata['{name}'] = this.{name};";
        }

        public override string DartMapFetching(string name)
        {
            return $"\t\t{name} = map['{name}']?.to{ (dartTypeName == "int" ? "Int" : "Double")}() ?? 0;";
        }

        public override void toString(StringBuilder sb, int? indents)
		{
			sb.Append(value.ToString());
		}
	}
}
